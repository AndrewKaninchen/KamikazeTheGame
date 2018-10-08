using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Kamikaze.Backend
{
    public class Player
    {
        public Player Opponent { get; }
        public Stack<Card> Deck { get; } = new Stack<Card>();
        public List<Card> Hand { get; } = new List<Card>();

        public uint maxCrystals;
        public List<EnergyType> crystals;

        public Player(Stack<Card> deck)
        {
            this.Deck = deck;
        }

        public Dictionary<EnergyType, int> Energy { get; set; }
        public int ActionPoints { get; set; }

        public async Task  DrawCards(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                //Do animation using await (possibly not calling DrawCard, for faster drawing)
                await DrawCard();
            }
        }

        public async Task DrawCard()
        {
            if (Deck.Count > 0)
            {
                var card = Deck.Pop();
                //Do animation using await
                Hand.Add(card);
            }
        //    GameEvents.OnCardDrawn.Invoke(opponent, (card, this));
        //    //foreach (var del in GameEvents.OnCardDrawn)
        //    //{
        //    //    await del(card, this);
        //    //}
        //    await card.OnDrawn();
        }

        public async Task AddCrystal(EnergyType type)
        {
            if (crystals.Count >= maxCrystals)
                return;
            //await GameEvents.OnAddCrystal(type, this);
        }

        public async Task AddEnergy(EnergyType type, int amount)
        {
            Energy[type] += amount;
            //await GameEvents.OnAddEnergy(type, this, amount);
        }

    }
}