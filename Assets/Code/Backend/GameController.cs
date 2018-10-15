using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoroutineAsync;
using Kamikaze.Frontend;

namespace Kamikaze.Backend
{
    public class GameController
    {
        public FrontendController FrontendController { get; }
        public Player[] Players { get; }
        public Player CurrentPlayer { get; set; }
        public GameEvents Events { get; private set; }
        public GameActions Actions { get; private set; }

        public List<Card> CardsInPlay { get; private set; }
        public List<Card> CardsInField { get; private set; }

        public bool IsGameStarted { get; private set; }
        public bool IsGameOver { get; private set; }

        public GameController (IEnumerable<CardAsset> p1Cards, IEnumerable<CardAsset> p2Cards, FrontendController frontendController)
        {
            FrontendController = frontendController;
            Events = new GameEvents(this);
            Actions = new GameActions(this, frontendController);

            var p1Deck = new Stack<Card>();
            var p2Deck = new Stack<Card>();

            foreach (var card in p1Cards)            
                p1Deck.Push(Card.CreateCard(card.associatedType));
            foreach (var card in p2Cards)
                p2Deck.Push(Card.CreateCard(card.associatedType));

            CardsInPlay = new List<Card>(p1Deck);
            CardsInPlay.AddRange(p2Deck);
            CardsInField = new List<Card>();

            Players = new Player[2];
            Players[0] = new Player(p1Deck);
            Players[1] = new Player(p2Deck);
            CurrentPlayer = Players[0];            
            IsGameOver = false;
        }

        public async Task Run()
        {
            IsGameStarted = true;
            if (IsGameStarted)
            {
                while (!IsGameOver)
                {
                    CurrentPlayer = CurrentPlayer == Players[0] ? Players[1] : Players[0];
                    await ChangePlayer();
                    await Turn(CurrentPlayer);
                }
            }
        }

        private async Task ChangePlayer()
        {
            await FrontendController.StartCoroutineAsync(FrontendController.ChangePlayer());
            Debug.Log("O CARALHO DO MEU AVÔ");
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
            Debug.Log("VictoryCheckPhase");

            //await CallEvent<OnVictoryCheckPhase>();
            // Check for Victory
        }

        private async Task DrawPhase()
        {
            Debug.Log("DrawPhase");
            //await CallEvent<OnDrawPhase>(new OnDrawPhase(currentPlayer));
            await CurrentPlayer.DrawCard();
        }

        private async Task CrystalUnlockPhase()
        {
            Debug.Log("CrystalUnlockPhase");

            //currentPlayer.AddCrystal(/*await Choose crystal*/);
            //await GameEvents.OnCrystalUnlockPhase(currentPlayer);
        }

        private async Task ActionPointRestorePhase()
        {
            Debug.Log("ActionPointRestorePhase");

            //na real tô restaurando energia aqui também porque sei lá
            //foreach (var crystal in currentPlayer.crystals)
            //await currentPlayer.AddEnergy(crystal, 1);

            //restore ap 
        }

        private async Task MainPhase()
        {
            Debug.Log("NÃO CONTÉM CARTAS PATÉTICAS");

            //await frontendController.ShowMainPhaseText();

            //await Events.CallEvent(new OnMainPhase());

            Debug.Log("MainPhase");
            
            FrontendController.DisplayEndTurnButton();
            
            var completionSource = new TaskCompletionSource<int>();



            void OnTurnEnded()
            {
                completionSource.SetResult(1);
                FrontendController.OnTurnEnded -= OnTurnEnded;
            }

            FrontendController.OnTurnEnded += OnTurnEnded;

            await completionSource.Task;
        }

        private async Task EndPhase()
        {
            Debug.Log("EndPhase");
            //await GameEvents.OnEndPhase(currentPlayer);
        }
        #endregion
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

    }
}