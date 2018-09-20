using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kamikaze.Frontend
{
    public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public enum DragState
        {
            NotDragging,
            Dragging,
            ReturningFromDrag,
            Summoning
        }

        //public Material transparentMaterial;
        public GameObject tokenPrefab;
        public DummyCard dummy;
        public float distanceFromCameraWhenDraggingCards;
        public DragState dragState;
        private GameObject token;
        public LayerMask layers;


        private void LateUpdate()
        {
            switch (dragState)
            {
                case DragState.NotDragging:
                    MoveToPositionImmediately();
                    break;
                case DragState.Dragging:
                {
                    var pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f);
                    pos.z *= distanceFromCameraWhenDraggingCards;
                    pos = Camera.main.ScreenToWorldPoint(pos);

                    transform.DOKill();
                    transform.position = pos;
                    break;
                }
                case DragState.ReturningFromDrag:
                {
                    transform.position = Vector3.Lerp(transform.position, dummy.transform.position, 0.5f);
                    if (Vector3.Distance(transform.position, dummy.transform.position) < 0.01f)
                    {
                        transform.position = dummy.transform.position;
                        dragState = DragState.NotDragging;
                    }

                    break;
                }
                case DragState.Summoning:
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

        public void MoveToPosition(Vector3 position, Quaternion rotation)
        {
            transform.DOKill();
            transform.DOMove(position, 0.05f);
            transform.DORotate(rotation.eulerAngles, 0.2f);
        }

        public void MoveToPositionImmediately(Vector3 position, Quaternion rotation)
        {
            transform.DOKill();
            transform.position = position;
            transform.rotation = rotation;
        }
    
        public void MoveToPosition() => MoveToPosition(dummy.transform.position, dummy.transform.rotation);
        public void MoveToPositionImmediately() => MoveToPositionImmediately(dummy.transform.position, dummy.transform.rotation);

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log($"OnBeginDrag: {this}");

            dragState = DragState.Dragging;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log($"OnDrag: {this}");
            //transform.DOMove(pos, 0.1f);

            Debug.Log(Input.mousePosition.y);
            if (Input.mousePosition.y / Screen.height > .3f)
            {
                if (dragState != DragState.Summoning)
                {
                    dragState = DragState.Summoning;
                    Debug.Log("Summoning");
                    GetComponent<MeshRenderer>().enabled = false;
                    
                    if(token == null) token = Instantiate(tokenPrefab);
                }
                //dragState = DragState.Summonning;
            }
            else
            {
                if (dragState == DragState.Summoning)
                {
                    dragState = DragState.Dragging;
                    Debug.Log("Stap Summoning");
                    Destroy(token);
                    GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log($"OnEndDrag: {this}");

            //var t = transform.DOMove(dummy.transform.position, 20f);
            dragState = DragState.ReturningFromDrag;

            //t.SetSpeedBased();
            ////t.SetRelative();
            ////t.onUpdate += () => t.ChangeEndValue(dummy.transform.position, true);
            //t.onComplete += () => 
            //{
            //    dragState = DragState.NotDragging;
            //    Debug.Log("FOI PORRA");
            //};
        }

        public void OnPointerClick(PointerEventData eventData)
        {
           // Debug.Log($"OnPointerClick: {this}");
            //MoveToPosition();
            //MoveToPositionImmediately();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
           // Debug.Log($"OnPointerEnter: {this}");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log($"OnPointerExit: {this}");
        }
    }
}