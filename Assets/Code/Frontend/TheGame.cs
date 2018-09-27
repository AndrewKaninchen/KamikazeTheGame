using UnityEngine;
using System.Collections.Generic;
using System;
using CoroutineAsync;
using System.Collections;

namespace Kamikaze.Frontend
{
    public class TheGame : MonoBehaviour
    {
        public Player p1, p2, currentPlayer, otherPlayer;
        public Animator SUAVEZ;


        public void Start()
        {
            currentPlayer = p1;
            otherPlayer = p2;

            p1.hand.player = p1;
            p2.hand.player = p2;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                StartCoroutine(ChangePlayer());
        }

        public IEnumerator ChangePlayer()
        {
            otherPlayer = currentPlayer;
            currentPlayer = currentPlayer == p1 ? p2 : p1;

            SUAVEZ.SetTrigger("VAI");
            yield return new WaitForSeconds(1.5f);

            otherPlayer.cam.gameObject.SetActive(false);
            currentPlayer.cam.gameObject.SetActive(true);
            yield return new WaitForSeconds(.3f);



            foreach (Token t in currentPlayer.tokens)
                t.Color = Token.TokenColor.Friendly;

            foreach (Token t in otherPlayer.tokens)
                t.Color = Token.TokenColor.Enemy;

            SUAVEZ.SetTrigger("DESCE");
            yield return new WaitForSeconds(1.1f);
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