using System;
using DG.Tweening;
using Kamikaze.Backend;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kamikaze.Frontend
{
    public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region Enum Types

        public enum DragState
        {
            NotDragging,
            Dragging,
            Previewing,
        }
        
        public enum LocationState
        {
            Hand,
            Field,
            Deck,
        }
       
        #endregion
        
        #region Card Object & Game Logic References
        public PlayerObjects owner;
        [HideInInspector] public FrontendController frontendController;
        public Backend.Card BackendCard { get; private set; }
        public DummyCard dummy;

        // ReSharper disable once InconsistentNaming
        private Token _token;
        public Token Token
        {
            get
            {
                if(_token != null) return _token; 
                _token = Instantiate(tokenPrefab).GetComponent<Token>();
                _token.Initialize(BackendCard as FieldCard);
                _token.gameObject.SetActive(false);
                Debug.Log(_token);
                return _token;
            }

            private set => _token = value;
        }

        #endregion        

        #region Prefabs & Assets
        public GameObject tokenPrefab;
        #endregion
        
        #region Value Fields & Properties
        public float distanceFromCameraWhenDraggingCards;
        [SerializeField] private LayerMask layers; //TODO: tirar essa bosta? Talvez valha a pena deixar como read only de alguma forma? Mostrar só no prefab?
        public DragState dragState = DragState.NotDragging;
        public LocationState locationState = LocationState.Deck;
        private bool isVisible;
        private bool Visible 
        {
            get => isVisible;
            set
            {
                Token.gameObject.SetActive(!value);

                GetComponent<MeshRenderer>().enabled = value;
                isVisible = value;
            }
        }
        #endregion

        #region Events
        public event Action OnSummon;
        #endregion

        public void Initialize(PlayerObjects owner, FrontendController frontendController, Backend.Card backendCard, DummyCard dummy)
        {
            this.owner = owner;
            this.frontendController = frontendController;
            this.BackendCard = backendCard;
            this.dummy = dummy;
            
            Debug.Log(this.BackendCard);
            dummy.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }
        
        private void LateUpdate()
        {
            switch (locationState)
            {
                case LocationState.Deck:
                    break;
                case LocationState.Hand:
                    switch (dragState)
                    {
                        case DragState.Dragging:
                        {
                            var pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f);
                            pos.z *= distanceFromCameraWhenDraggingCards;
                            if (Camera.main != null) pos = Camera.main.ScreenToWorldPoint(pos);

                            transform.position = pos;
                            break;
                        }
                        case DragState.NotDragging:
                        {
                            transform.SetPositionAndRotation
                            (
                                Vector3.Lerp(transform.position, dummy.transform.position, 0.5f),
                                Quaternion.Lerp(transform.rotation, dummy.transform.rotation, 0.5f)
                            );

                            if (Vector3.Distance(transform.position, dummy.transform.position) < 0.01f &&
                                Quaternion.Angle(transform.rotation, dummy.transform.rotation) < 0.01f)
                            {
                                transform.position = dummy.transform.position;
                                transform.rotation = dummy.transform.rotation;
                                dragState = DragState.NotDragging;
                            }

                            break;
                        }
                        case DragState.Previewing:
                        {
                            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                            if (Physics.Raycast(ray, out var hit, 1000f, layers))
                            {
                                var objectHit = hit.transform;
                            }

                            var pos = hit.point;
                            Token.transform.position = pos;
                            break;
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                case LocationState.Field:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            switch (locationState)
            {
                case LocationState.Hand:
                    Debug.Log($"OnBeginDrag: {this}");
                    if (dragState == DragState.NotDragging)
                        dragState = DragState.Dragging;
                    break;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log($"OnDrag: {this}");
            //transform.DOMove(pos, 0.1f);

//            Debug.Log(Input.mousePosition.y);
            if (Input.mousePosition.y / Screen.height > .3f)
            {
                if (dragState != DragState.Previewing)
                {
                    Debug.Log("Summoning");
                    dragState = DragState.Previewing;
                    Visible = false;
                }
            }
            else
            {
                if (dragState == DragState.Previewing)
                {
                    Debug.Log("Stap Summoning");
                    dragState = DragState.Dragging;
                    Visible = true;
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log($"OnEndDrag: {this}");

            switch (dragState)
            {
                case DragState.Dragging:
                    dragState = DragState.NotDragging;
                    break;
                case DragState.Previewing:
                    OnSummon?.Invoke();
                    
                    owner.tokens.Add(Token);
                    Token.Color = Token.TokenColor.Friendly;
                    Token.frontendController = frontendController;
                    dummy.gameObject.SetActive(false);
                    this.gameObject.SetActive(false);
                    //Destroy(dummy.gameObject);
                    //Destroy(this.gameObject);
                    break;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (locationState)
            {
                case LocationState.Hand:
                    break;
                case LocationState.Field:
                    break;
                case LocationState.Deck:
                    locationState = LocationState.Hand;
                    owner.hand.AddCard(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}