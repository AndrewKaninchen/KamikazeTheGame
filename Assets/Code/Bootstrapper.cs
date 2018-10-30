using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kamikaze
{
    public class Bootstrapper : MonoBehaviour
    {
        [HideInInspector] public Frontend.FrontendController frontendController;
        [HideInInspector] public Backend.GameController gameController;
        [HideInInspector] public Backend.GameEvents gameEvents;

        [Header("Decks")]
        public List<Backend.CardAsset> cardsP1;
        public List<Backend.CardAsset> cardsP2;

        private void Start()
        {
            frontendController = GetComponent<Frontend.FrontendController>();
            if (frontendController == null)
                gameObject.AddComponent<Frontend.FrontendController>();
        }
        
        public void Begin ()
        {
            gameController = new Backend.GameController(cardsP1, cardsP2, frontendController);
            gameEvents = new Backend.GameEvents(gameController);

            foreach (var player in gameController.Players)
                foreach (var c in player.Deck)
                    gameController.FrontendController.playerObjects[player].deck.AddCard(c.FrontendCard);
            
            
#pragma warning disable 4014
            gameController.Run();
#pragma warning restore 4014
        }
    }
}