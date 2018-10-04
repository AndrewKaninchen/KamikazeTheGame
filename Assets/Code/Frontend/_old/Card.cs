using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kamikaze.Frontend_Old
{
    public class Card : Selectable,
        IBeginDragHandler, IDragHandler, IEndDragHandler, 
        IPointerEnterHandler, IPointerExitHandler, IDropHandler, 
        IInitializePotentialDragHandler, IPointerClickHandler
    {
        [NonSerialized]
        public Backend.FieldCard BackendCard;
        public Backend.FieldCard BackendCardTemplate;
        private Animator anim;
        private LayoutElement layoutElement;
        private bool isSummoning;
        private bool isOnField = false;


        //private static Card currentHovered;
        //private static Card currentDragged;
        //private static bool isDraggingAny;
        //private static bool isHoveringAny;
        private Vector3 originalPosition;

        private AbilityPanel abilityPanel;
        public HandPanel handPanel;

        public bool isHighlighted;

        protected override void Awake()
        {
            base.Awake();
            abilityPanel = UIManager.Instance.abilityPanel;
            anim = GetComponent<Animator>();
            layoutElement = GetComponent<LayoutElement>();
            
        }

        protected override void Start()
        {
            //BackendCard = ScriptableObject.CreateInstance(BackendCardTemplate.GetType()) as Backend.FieldCard;
            //BackendCard.Init(null, null, null, this);
            BackendCard.SubscribeTriggerEffects();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (!interactable) return;

            if (handPanel.currentHoveredCard == null && handPanel.currentDraggedCard == null)
            {
                anim.SetTrigger("Hovering");
                handPanel.currentHoveredCard = this;
                //isHoveringAny = true;
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (handPanel.currentHoveredCard == this && handPanel.currentDraggedCard != this)
            {
                handPanel.currentHoveredCard = null;
                anim.SetTrigger("Normal");
            }
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (!interactable) return;

            if (!isOnField)
            {
                Debug.Log("Dragging");
                anim.SetTrigger("Dragging");
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isOnField)
            {
                if (handPanel.currentDraggedCard == null)
                {
                    layoutElement.ignoreLayout = true;
                    Camera.main.ScreenToViewportPoint(transform.position);
                    originalPosition = transform.position;
                    handPanel.currentDraggedCard = this;
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isOnField)
            {
                var vp = Camera.main.ScreenToViewportPoint(transform.position);
                var wasSummoning = isSummoning;

                isSummoning = vp.y > 0.3f;

                if (isSummoning != wasSummoning)
                {
                    if (isSummoning)
                    {
                        Debug.Log("Summoning");
                    }
                    else
                    {
                        Debug.Log("Dragging");
                        anim.SetTrigger("Dragging");
                    }

                    anim.SetBool("Summoning", isSummoning);
                }

                transform.SetPositionAndRotation(Input.mousePosition, Quaternion.identity);
                transform.SetAsLastSibling();
            }

            else
            {

            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isOnField)
            {
                if (isSummoning)
                {
                    isOnField = true;
                    isSummoning = false;
                    handPanel.currentDraggedCard = null;
                    handPanel.currentHoveredCard = null;


                    anim.SetBool("OnField", isOnField);
                    anim.SetBool("Summoning", isSummoning);
                    return;
                }
                if (handPanel.currentDraggedCard == this)
                {
                    isSummoning = false;
                    handPanel.currentDraggedCard = null;
                    handPanel.currentHoveredCard = null;

                    anim.SetBool("Summoning", isSummoning);
                    StartCoroutine(ReturnToOriginalPosition());
                }
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
        }
        
        private IEnumerator MoveTo (Vector3 target)
        {
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                yield return new WaitForEndOfFrame();
                transform.position = Vector3.MoveTowards(transform.position, target, 50f);
                //Debug.Log(Vector3.Distance(transform.position, target));
            }
        }

        private IEnumerator ReturnToOriginalPosition()
        {
            yield return MoveTo(originalPosition);
            anim.SetTrigger("Normal");
            layoutElement.ignoreLayout = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable) return;

            if (isOnField)
            {
                if (eventData.clickCount == 2)
                {
                    BeginMove();
                }
                else if (eventData.clickCount == 1)
                {
                    UIManager.Instance.abilityPanel.Show(this);
                }
            }
        }

        public async void BeginMove ()
        {            
            var result = await Move();
            if (!result)
                abilityPanel.Show(this);
        }

        public Task<bool> Move()
        {
            abilityPanel.Hide();
            isMoving = true;
            var completionSource = new TaskCompletionSource<bool>();
            Action end = () =>
            {
                isMoving = false;
                completionSource.SetResult(true);
            };
            Action cancel = () =>
            {
                isMoving = false;
                completionSource.SetResult(false);
            };
            
            end += () => { cancelMove -= cancel; endMove -= end; };
            cancel += () => { cancelMove -= cancel; endMove -= end; };

            cancelMove += cancel;
            endMove += end;

            return completionSource.Task;
        }
        
        private bool isMoving;

        private Action cancelMove;
        private Action endMove;

        private void Update()
        {
            if (isMoving)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(MoveTo(Input.mousePosition));
                    endMove();
                }
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
                {
                    cancelMove();
                }
            }
        }
    }
}
