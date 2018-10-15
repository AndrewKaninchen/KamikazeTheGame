using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.XR.WSA;

namespace Kamikaze.Frontend
{
    public class Token : MonoBehaviour, IPointerClickHandler
    {
        public FrontendController frontendController;
        
        public LayerMask layers; //temporário porque preguiça de pensar
        
        public float movementStat;
        
        [SerializeField] private GameObject abilityPanelPrefab;
        private AbilityPanel abilityPanel;
        [SerializeField] private Material friendlyMat, enemyMat, friendlyBaseMat, enemyBaseMat;
        
        [SerializeField] private ParticleSystemRenderer particleSystemRenderer;
        [SerializeField] private MeshRenderer meshRenderer;
        
        
        [SerializeField] private MoveState moveState = MoveState.Idle;
        [SerializeField] private InteractionState interactionState = InteractionState.Unlocked;
        private event Action OnUpdate;

        public TokenColor Color 
        {
            get => t_Color;
            set 
            {
                meshRenderer.material = value == TokenColor.Friendly ?  friendlyBaseMat : enemyBaseMat;
                particleSystemRenderer.material = value == TokenColor.Friendly ? friendlyMat : enemyMat;
                t_Color = value;
            }
        }

        private TokenColor t_Color;

        public enum TokenColor
        {
            Friendly,
            Enemy
        }

        public enum InteractionState
        {
            Locked,
            Unlocked
        }
        
        public enum MoveState
        {
            Idle,
            Moving,
            SelectingTargetPosition
        }


        private void Start()
        {
        }
        
        
        public async void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnPointerClick");

            if (interactionState == InteractionState.Unlocked)
            {
                switch (eventData.clickCount)
                {
                    case 2:
                        waitingForClickToDisplayAbilities = false;
                        switch (moveState)
                        {
                            case MoveState.Idle:
                                BeginMove();
                                moveState = MoveState.SelectingTargetPosition;
                                interactionState = InteractionState.Locked;
                                break;
                            case MoveState.Moving:
                                break;
                            case MoveState.SelectingTargetPosition:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    
                    case 1:
                        DisplayAbilities();
                        //interactionState = InteractionState.Locked;
                        break;
                    
                    default:
                        break;
                }
            }
        }
        
        private bool waitingForClickToDisplayAbilities = true;
        public async void DisplayAbilities()
        {
            waitingForClickToDisplayAbilities = true;
            await Task.Delay(150);
            if (waitingForClickToDisplayAbilities)
            {
                var canvas = frontendController.screenSpaceCanvas;
                var pos = Camera.main.WorldToScreenPoint(transform.position);
                Debug.Log(pos);
                var ap = Instantiate(abilityPanelPrefab, parent: canvas.transform, position: pos, rotation: Quaternion.identity);
            }
        }
        
        public async void BeginMove()
        {
            //Debug.Log("Waiting for Click");
            var pos = await SelectPosition();

            moveState = MoveState.Moving;
            var t = transform.DOMove(new Vector3(pos.x, transform.position.y, pos.z), 0.2f);
            //Debug.Log($"Moving to {pos}");
            t.onComplete += () =>
            {
                moveState = MoveState.Idle;
                interactionState = InteractionState.Unlocked;
                //Debug.Log($"Moved to {pos}");
            };
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
                        if (Vector3.Distance(hit.point, transform.position) > 0.1f)
                            transform.LookAt(hit.point, Vector3.up);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}