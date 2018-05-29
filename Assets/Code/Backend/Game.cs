using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using static Kamikaze.Backend.GameEvents;

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

    public static class Game
    {
        public static bool isGameStarted = false;
        public static bool isGameOver = true;
        public static Player[] players;
        public static Player currentPlayer;
        public static List<Card> cardsInPlay;
        public static List<Card> cardsInField;

        public static void Initialize (List<Card> p1cards, List<Card> p2cards)
        {   
            var p1deck = new Stack<Card>();
            var p2deck = new Stack<Card>();

            foreach (var card in p1cards)            
                p1deck.Push(ScriptableObject.CreateInstance(card.GetType()) as Card);
            foreach (var card in p2cards)
                p2deck.Push(ScriptableObject.CreateInstance(card.GetType()) as Card);

            cardsInPlay = new List<Card>(p1deck);
            cardsInPlay.AddRange(p2deck);

            players = new Player[2];
            players[0] = new Player(p1deck);
            players[1] = new Player(p2deck);
            currentPlayer = players[0];
            isGameStarted = true;
            isGameOver = false;

            Run();
        }

        private static async void Run()
        {
            if (isGameStarted)
            {
                while (!isGameOver)
                {
                    currentPlayer = currentPlayer == players[0] ? players[1] : players[0];
                    await Turn(currentPlayer);
                }
            }
        }

        #region Phases

        private static async Task Turn(Player player)
        {
            await VictoryCheckPhase();
            await DrawPhase();
            await CrystalUnlockPhase();
            await ActionPointRestorePhase();
            await MainPhase();
            await EndPhase();
        }

        private static async Task VictoryCheckPhase()
        {
            //await CallEvent<OnVictoryCheckPhase>();
            // Check for Victory
        }

        private async static Task DrawPhase()
        {
            //await CallEvent<OnDrawPhase>(new OnDrawPhase(currentPlayer));
            await currentPlayer.DrawCard();
        }

        private static async Task CrystalUnlockPhase()
        {
            //currentPlayer.AddCrystal(/*await Choose crystal*/);
            //await GameEvents.OnCrystalUnlockPhase(currentPlayer);
        }

        private static async Task ActionPointRestorePhase()
        {
            //na real tô restaurando energia aqui também porque sei lá
            //foreach (var crystal in currentPlayer.crystals)
                //await currentPlayer.AddEnergy(crystal, 1);

            //restore ap 
        }

        private static async Task MainPhase()
        {
            //await GameEvents.OnMainPhase(currentPlayer);
        }

        private static async Task EndPhase()
        {
            //await GameEvents.OnEndPhase(currentPlayer);
        } 
        #endregion

    }
}