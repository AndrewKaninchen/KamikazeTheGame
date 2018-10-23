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
        #endregion
        
        #region Card Object & Game Logic References
        public PlayerObjects owner;
        [HideInInspector] public FrontendController frontendController;
        private Backend.Card backendCard;
        public DummyCard dummy;
        private Token token;
        #endregion        

        #region Prefabs & Assets
        public GameObject tokenPrefab;
        #endregion
        
        #region Value Fields & Properties
        public float distanceFromCameraWhenDraggingCards;
        [SerializeField] private LayerMask layers; //TODO: tirar essa bosta? Talvez valha a pena deixar como read only de alguma forma? Mostrar só no prefab?
        public DragState dragState = DragState.NotDragging;
        private bool isVisible;
        private bool Visible 
        {
            get => isVisible;
            set 
            {
                if (value)
                    Destroy(token);
                else
                {
                    token = Instantiate(tokenPrefab).GetComponent<Token>();
                    token.Initialize(backendCard as FieldCard);
                }

                GetComponent<MeshRenderer>().enabled = value;
                //GetComponent<MeshCollider>().enabled = value;
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
            this.backendCard = backendCard;
            this.dummy = dummy;
            dummy.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }
        
        private void LateUpdate()
        {
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
                        Quaternion.Angle(transform.rotation, dummy.transform.rotation) < 0.01f )
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
                    token.transform.position = pos;
                    break;
                }
            }
        }

        public Tweener MoveToPosition(Vector3 position, Quaternion rotation)
        {
            transform.DOKill();
            var t = transform.DOMove(position, 0.05f);
            transform.DORotate(rotation.eulerAngles, 0.2f);
            return t;
        }
        
        public void MoveToPositionImmediately(Vector3 position, Quaternion rotation)
        {
            transform.DOKill();
            transform.position = position;
            transform.rotation = rotation;
        }
    
        public Tweener MoveToPosition() => MoveToPosition(dummy.transform.position, dummy.transform.rotation);
        public void MoveToPositionImmediately() => MoveToPositionImmediately(dummy.transform.position, dummy.transform.rotation);

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log($"OnBeginDrag: {this}");
            if (dragState == DragState.NotDragging)
                dragState = DragState.Dragging;
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
                    
                    owner.tokens.Add(token);
                    token.Color = Token.TokenColor.Friendly;
                    token.frontendController = frontendController;
                    Destroy(dummy.gameObject);
                    Destroy(this.gameObject);
                    break;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}