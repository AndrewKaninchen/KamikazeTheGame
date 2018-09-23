﻿using System;
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
        //public Material transparentMaterial;
        public GameObject tokenPrefab;
        public DummyCard dummy;
        public float distanceFromCameraWhenDraggingCards;
        private GameObject token;
        [SerializeField] private LayerMask layers; //temporário porque preguiça de pensar

        public DragState dragState = DragState.Tweening;
        public enum DragState
        {
            NotDragging,
            Dragging,
            Previewing,
            Tweening
        }

        public event Action OnSummon;
        
        private bool isVisible;
        private bool Visible 
        {
            get => isVisible;
            set 
            {
                if (value)
                    Destroy(token);
                else
                    token = Instantiate(tokenPrefab);

                GetComponent<MeshRenderer>().enabled = value;
                //GetComponent<MeshCollider>().enabled = value;
                isVisible = value;
            }
        }

        public IEnumerator GetDrawn()
        {
            yield return new WaitForEndOfFrame();
            transform.DOMove(dummy.transform.position, 0.1f).onComplete += () => dragState = DragState.NotDragging;;
            transform.DORotate(dummy.transform.rotation.eulerAngles, 0.2f);
        }

        private void LateUpdate()
        {
            switch (dragState)
            {
                case DragState.Dragging:
                {
                    var pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f);
                    pos.z *= distanceFromCameraWhenDraggingCards;
                    pos = Camera.main.ScreenToWorldPoint(pos);

                    transform.DOKill();
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

            Debug.Log(Input.mousePosition.y);
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
                    //dragState = DragState.ReturningFromDrag;
                    break;
                case DragState.Previewing:
                    OnSummon?.Invoke();
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