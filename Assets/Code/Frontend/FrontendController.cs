using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace Kamikaze.Frontend
{
    public class FrontendController : MonoBehaviour
    {
        public Player p1, p2, currentPlayer, otherPlayer;
        public Animator SUA_VEZ;
        public GameObject endTurnButton;


        public event Action OnTurnEnded;

        private Backend.GameController gameController;

        public void Init(Backend.GameController  gameController)
        {
            this.gameController = gameController;
        }

        public void Start()
        {
            currentPlayer = p1;
            otherPlayer = p2;

            p1.hand.player = p1;
            p2.hand.player = p2;
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
                //gameController.ChangePlayer();
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

            SUA_VEZ.SetTrigger("VAI");
            yield return new WaitForSeconds(1.5f);

            otherPlayer.cam.gameObject.SetActive(false);
            currentPlayer.cam.gameObject.SetActive(true);
            yield return new WaitForSeconds(.3f);

            foreach (Token t in currentPlayer.tokens)
                t.Color = Token.TokenColor.Friendly;

            foreach (Token t in otherPlayer.tokens)
                t.Color = Token.TokenColor.Enemy;

            SUA_VEZ.SetTrigger("DESCE");
            yield return new WaitForSeconds(.7f);
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