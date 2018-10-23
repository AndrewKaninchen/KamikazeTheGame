using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Kamikaze.Backend
{
    public class Player
    {
        public GameController GameController { get; private set; }
        public Player Opponent { get; }
        public List<Card> Deck { get; }
        public List<Card> Hand { get; }

        public uint maxCrystals;
        public List<EnergyType> crystals;

        public Player(List<Card> deck, GameController gameController)
        {
            GameController = gameController;
            Deck = deck;
            Hand = new List<Card>();
        }

        public Dictionary<EnergyType, int> Energy { get; set; }
        public int ActionPoints { get; set; }

        public async Task DrawCards(int amount)
        {
            for (var i = 0; i < amount; i++)
                await DrawCard();
        }

        public async Task DrawCard()
        {
            if (Deck.Count > 0)
            {
                var card = Deck.First();
                await GameController.Actions.AddCardToPlayersHand(this, card);
            }
        }

        public async Task AddCrystal(EnergyType type)
        {
            if (crystals.Count >= maxCrystals)
                return;
            await GameController.Events.CallEvent(new GameEvents.OnAddCrystal(type, this));
        }

        public async Task AddEnergy(EnergyType type, int amount)
        {
            Energy[type] += amount;
            await GameController.Events.CallEvent(new GameEvents.OnAddEnergy(type, this, amount));
        }

    }
}