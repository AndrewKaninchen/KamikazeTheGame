using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
#pragma warning disable 1998

namespace Kamikaze.Backend
{
    public class GameActions
    {
        private GameController GameController { get; set; }

        public GameActions(GameController controller)
        {
            GameController = controller;
        }

        public async Task GetCardFromDeck(Card card)
        {
        }
        
        public async Task CreateCard(Card template)
        {
        }
        
        public async Task Summon(Card card, Vector2 position)
        {
        }

        public async Task Damage(Card card)
        {
        }

        public async Task Heal(Card card)
        {
        }

        public async Task ApplyStatus(Card card /*, Buf*/)
        {
        }

        public async Task ChooseCardFromList(IEnumerable<Card> list)
        {
        }
        
        public async Task ChooseCardFromField()
        {
        }
        
        public async Task ChooseCardFromHand()
        {
        }
        
        public async Task ChooseCardFromDeck()
        {
        }
        
        public async Task ChooseOptionFromList(Player player, IEnumerable<string> options)
        {
        }
        
        public async Task ChooseCardFromList(Player player, IEnumerable<Card> options)
        {
        }
    }
}