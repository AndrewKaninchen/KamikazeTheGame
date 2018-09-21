using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Kamikaze.Frontend
{
    public class Token : MonoBehaviour, IPointerClickHandler
    {
        public float movementStat;
        public event Action OnUpdate;
        public MoveState moveState = MoveState.Idle;
        [SerializeField] private LayerMask layers; //temporário porque preguiça de pensar

        public enum MoveState
        {
            Idle,
            Moving,
            SelectingTargetPosition
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnPointerClick");

            switch (moveState)
            {
                case MoveState.Idle:
                    BeginMove();
                    moveState = MoveState.SelectingTargetPosition;
                    break;
            }
        }

        public async void BeginMove()
        {
            Debug.Log("Waiting for Click");
            var pos = await SelectPosition();

            moveState = MoveState.Moving;
            var t = transform.DOMove(new Vector3(pos.x, transform.position.y, pos.z), 0.2f);
            Debug.Log($"Moving to {pos}");
            t.onComplete += () =>
            {
                moveState = MoveState.Idle;
                Debug.Log($"Moved to {pos}");
            };
        }

        public void DisplayActions()
        {

        }

        public Task<Vector3> SelectPosition()
        {
            var completionSource = new TaskCompletionSource<Vector3>();

            OnUpdate += Handler;
            void Handler()
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out var hit, 1000f, layers))
                    {
                        completionSource.SetResult(hit.point);
                        OnUpdate -= Handler;
                    }
                }
            }

            return completionSource.Task;
        }

        private void Update()
        {
            OnUpdate?.Invoke();

            switch (moveState)
            {
                case MoveState.Idle:
                    break;
                case MoveState.Moving:
                    break;
                case MoveState.SelectingTargetPosition:
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out var hit, 1000f, layers))
                    {
                        transform.LookAt(hit.point, Vector3.up);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}