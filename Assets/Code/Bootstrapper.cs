using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kamikaze
{
    public class Bootstrapper : MonoBehaviour
    {
        private Frontend.FrontendController frontendController;
        private Backend.GameController gameController;
        private Backend.GameEvents gameEvents;

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

            frontendController.Init(gameController);
            gameController.Run();
        }
    }
}