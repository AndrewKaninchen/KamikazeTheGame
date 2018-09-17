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
        public DummyCard dummy;

        private void LateUpdate()
        {
            MoveToPositionImmediately();
        }

        public void MoveToPosition() => MoveToPosition(dummy.transform.position, dummy.transform.rotation);
        public void MoveToPosition(Vector3 position, Quaternion rotation)
        {
            transform.DOKill();
            transform.DOMove(position, 0.2f);
            transform.DORotate(rotation.eulerAngles, 0.2f);
        }
    
        public void MoveToPositionImmediately() => MoveToPositionImmediately(dummy.transform.position, dummy.transform.rotation);
        public void MoveToPositionImmediately(Vector3 position, Quaternion rotation)
        {
            transform.DOKill();
            transform.position = position;
            transform.rotation = rotation;
        }
    
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log($"OnBeginDrag: {this}");
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log($"OnDrag: {this}"); 
        }
    
        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log($"OnEndDrag: {this}");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"OnPointerClick: {this}");
            //MoveToPosition();
            MoveToPositionImmediately();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log($"OnPointerEnter: {this}");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log($"OnPointerExit: {this}");
        }
    }
}