using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Threading.Tasks;
using Kamikaze.Backend;
using UnityEngine.Serialization;

namespace Kamikaze.Frontend
{
    [Serializable]
    public class PlayerObjects
    {
        public CameraController cam;
        public HandPanel hand;
        public List<Token> tokens;
    }
    
    public partial class FrontendController : MonoBehaviour
    {
        public Dictionary<Backend.Player, PlayerObjects> playerObjects = new Dictionary<Player, PlayerObjects>();
        
        #region Backend Objects
        private Backend.GameController gameController;
        public PlayerObjects p1Objects;
        public PlayerObjects p2Objects;
        public PlayerObjects currentPlayerObjects;
        public PlayerObjects otherPlayerObjects;
        #endregion

        #region Components & Objects
        public Animator suaVez;
        public GameObject endTurnButton;
        public Canvas screenSpaceCanvas;
        #endregion

        #region Events
        public event Action OnTurnEnded;
        #endregion
        
        public void Initialize(Backend.GameController gc)
        {
            gameController = gc;
            playerObjects.Add(gc.Players[0], p1Objects);
            playerObjects.Add(gc.Players[1], p2Objects);
        }

        public void Start()
        {
            p1Objects.hand.frontendController = this;
            p2Objects.hand.frontendController = this;
            
            currentPlayerObjects = p1Objects;
            otherPlayerObjects = p2Objects;

            p1Objects.hand.playerObjects = p1Objects;
            p2Objects.hand.playerObjects = p2Objects;
        }

        public void DisplayEndTurnButton()
        {
            endTurnButton.SetActive(true);
        }

        public void HideEndTurnButton()
        {
            endTurnButton.SetActive(false);
        }

        public void EndTurn()
        {
            OnTurnEnded?.Invoke();
        }

        public IEnumerator ChangePlayer()
        {
            HideEndTurnButton();

            otherPlayerObjects = currentPlayerObjects;
            currentPlayerObjects = currentPlayerObjects == p1Objects ? p2Objects : p1Objects;

            suaVez.SetTrigger("VAI");
            yield return new WaitForSeconds(1.5f);

            otherPlayerObjects.cam.gameObject.SetActive(false);
            currentPlayerObjects.cam.gameObject.SetActive(true);
            yield return new WaitForSeconds(.3f);

            foreach (var t in currentPlayerObjects.tokens)
                t.Color = Token.TokenColor.Friendly;

            foreach (var t in otherPlayerObjects.tokens)
                t.Color = Token.TokenColor.Enemy;

            suaVez.SetTrigger("DESCE");
            yield return new WaitForSeconds(.7f);
        }

        public Token CreateToken()
        {
            
            throw new NotImplementedException();
        }
        
        public Token CreateCard()
        {
            throw new NotImplementedException();
        }
    }

    
}