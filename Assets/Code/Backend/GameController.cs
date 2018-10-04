using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using static Kamikaze.Backend.GameEvents;
using CoroutineAsync;


namespace Kamikaze.Backend
{
    public enum EnergyType
    {
        PrimaMateria,
        Vitae,
        Glamour,
        Ether,
        Quintessence
    }

    public class GameController
    {
        public bool isGameStarted = false;
        public bool isGameOver = true;
        public Player[] players;
        public Player currentPlayer;
        public List<Card> cardsInPlay;
        public List<Card> cardsInField;

        public GameEvents Events { get; private set; }

        private Frontend.FrontendController frontendController;

        public GameController (List<CardAsset> p1cards, List<CardAsset> p2cards, Frontend.FrontendController frontendController)
        {
            this.frontendController = frontendController;
            this.Events = new GameEvents(this);

            var p1deck = new Stack<Card>();
            var p2deck = new Stack<Card>();

            foreach (var card in p1cards)            
                p1deck.Push(Card.CreateCard(card.associatedType));
            foreach (var card in p2cards)
                p2deck.Push(Card.CreateCard(card.associatedType));

            cardsInPlay = new List<Card>(p1deck);
            cardsInPlay.AddRange(p2deck);

            players = new Player[2];
            players[0] = new Player(p1deck);
            players[1] = new Player(p2deck);
            currentPlayer = players[0];
            isGameStarted = true;
            isGameOver = false;
        }

        public async void Run()
        {
            if (isGameStarted)
            {
                while (!isGameOver)
                {
                    currentPlayer = currentPlayer == players[0] ? players[1] : players[0];
                    await ChangePlayer();
                    await Turn(currentPlayer);
                }
            }
        }

        public async Task ChangePlayer()
        {
            await frontendController.StartCoroutineAsync(frontendController.ChangePlayer());
        }

        #region Phases

        private async Task Turn(Player player)
        {
            await VictoryCheckPhase();
            await DrawPhase();
            await CrystalUnlockPhase();
            await ActionPointRestorePhase();
            await MainPhase();
            await EndPhase();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task VictoryCheckPhase()
        {
            //await CallEvent<OnVictoryCheckPhase>();
            // Check for Victory
        }

        private async Task DrawPhase()
        {
            //await CallEvent<OnDrawPhase>(new OnDrawPhase(currentPlayer));
            await currentPlayer.DrawCard();
        }

        private async Task CrystalUnlockPhase()
        {
            //currentPlayer.AddCrystal(/*await Choose crystal*/);
            //await GameEvents.OnCrystalUnlockPhase(currentPlayer);
        }

        private async Task ActionPointRestorePhase()
        {
            //na real tô restaurando energia aqui também porque sei lá
            //foreach (var crystal in currentPlayer.crystals)
                //await currentPlayer.AddEnergy(crystal, 1);

            //restore ap 
        }

        private async Task MainPhase()
        {
            //await GameEvents.OnMainPhase(currentPlayer);
        }

        private async Task EndPhase()
        {
            //await GameEvents.OnEndPhase(currentPlayer);
        }
        #endregion
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

    }
}