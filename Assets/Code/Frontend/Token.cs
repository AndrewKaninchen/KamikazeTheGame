using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Kamikaze.Backend;
using UnityEngine.XR.WSA;

namespace Kamikaze.Frontend
{
    public class Token : MonoBehaviour, IPointerClickHandler
    {
        #region EnumTypes

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
        
        public enum AbilityState
        {
            None,
            WaitingForSecondClickNotToHappenToThenDisplayAbilities,
            DisplayingPanel,
            UsingAbility
        }

        #endregion
        
        #region Card Object & Game Logic References
        [HideInInspector] public FrontendController frontendController;
        private Backend.FieldCard card;
        #endregion

        #region Prefabs & Assets
        [SerializeField] private GameObject abilityPanelPrefab;
        [SerializeField] private Material friendlyMat, enemyMat, friendlyBaseMat, enemyBaseMat;
        #endregion
         
        #region Component References           
        [SerializeField] private ParticleSystemRenderer particleSystemRenderer;
        [SerializeField] private MeshRenderer meshRenderer;
        private AbilityPanel abilityPanel;
        #endregion
        
        #region Value Fields & Properties
        [SerializeField] private LayerMask layers; //temporário porque preguiça de pensar        
        
        private TokenColor tokenColor;
        
        public TokenColor Color 
        {
            get => tokenColor;
            set 
            {
                meshRenderer.material = value == TokenColor.Friendly ?  friendlyBaseMat : enemyBaseMat;
                particleSystemRenderer.material = value == TokenColor.Friendly ? friendlyMat : enemyMat;
                tokenColor = value;
            }
        }
        
        [SerializeField] private MoveState moveState = MoveState.Idle;
        [SerializeField] private InteractionState interactionState = InteractionState.Unlocked;
        [SerializeField] private AbilityState abilityState = AbilityState.None;
        #endregion

        #region Events
        private event Action OnUpdate;
        #endregion

        public void Initialize(FieldCard card)
        {
            this.card = card;
        }
        
        public async void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnPointerClick");

            if (interactionState == InteractionState.Unlocked)
            {
                switch (eventData.clickCount)
                {
                    case 2:
                        abilityState = AbilityState.None;
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
        
        
        public async void DisplayAbilities()
        {
            abilityState = AbilityState.WaitingForSecondClickNotToHappenToThenDisplayAbilities;
            await Task.Delay(150);
            if (abilityState == AbilityState.WaitingForSecondClickNotToHappenToThenDisplayAbilities)
            {
                var canvas = frontendController.screenSpaceCanvas;
                var pos = Camera.main.WorldToScreenPoint(transform.position);
                Debug.Log(pos);
                var ap = Instantiate(abilityPanelPrefab, parent: canvas.transform, position: pos, rotation: Quaternion.identity).GetComponent<AbilityPanel>();
                var selectedAbility = await ap.Display(card);
                
                if (selectedAbility != null)
                    if (selectedAbility.Condition())
                        await selectedAbility.Body();
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