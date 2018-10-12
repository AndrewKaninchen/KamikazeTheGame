using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.Serialization;

namespace Kamikaze.Frontend
{
    public class FrontendController : MonoBehaviour
    {
        public Player p1, p2, currentPlayer, otherPlayer;
        [FormerlySerializedAs("SUA_VEZ")] public Animator suaVez;
        public GameObject endTurnButton;


        public event Action OnTurnEnded;

        private Backend.GameController gameController;

        public void Init(Backend.GameController gc)
        {
            gameController = gc;
        }

        public void Start()
        {
            currentPlayer = p1;
            otherPlayer = p2;

            p1.hand.player = p1;
            p2.hand.player = p2;
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

            otherPlayer = currentPlayer;
            currentPlayer = currentPlayer == p1 ? p2 : p1;

            suaVez.SetTrigger("VAI");
            yield return new WaitForSeconds(1.5f);

            otherPlayer.cam.gameObject.SetActive(false);
            currentPlayer.cam.gameObject.SetActive(true);
            yield return new WaitForSeconds(.3f);

            foreach (Token t in currentPlayer.tokens)
                t.Color = Token.TokenColor.Friendly;

            foreach (Token t in otherPlayer.tokens)
                t.Color = Token.TokenColor.Enemy;

            suaVez.SetTrigger("DESCE");
            yield return new WaitForSeconds(.7f);
        }

        public Token CreateToken(Card card)
        {
            throw  new NotImplementedException();
        }        
    }

    [Serializable]
    public class Player
    {
        public CameraController cam;
        public HandPanel hand;
        public List<Token> tokens;
    }
}