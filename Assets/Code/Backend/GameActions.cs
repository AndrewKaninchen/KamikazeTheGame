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
        private Frontend.FrontendController FrontendController{ get; set; }

        public GameActions(GameController controller, Frontend.FrontendController frontend)
        {
            GameController = controller;
            FrontendController = frontend;
        }

        public async Task ForcePlayerToDrawCards(Player player, int amount)
        {
            await player.DrawCards(amount);
        }
        
        public async Task ForcePlayerToDrawCard(Player player)
        {
            await player.DrawCard();
        }
        
        public async Task AddCardToPlayersHand(Player player, Card card)
        {
            player.Hand.Add(card);
            card.Container = player.Hand;
            FrontendController.playerObjects[player].hand.AddCard(card.FrontendCard);
        }
        
        public async Task<Card> GetCardFromDeck(Card card)
        {
            throw new NotImplementedException();
        }
        
        public async Task<Card> CreateCard(Card template)
        {
            throw new NotImplementedException();
        }
        
        public async Task Summon(Card card, Vector2 position)
        {
            throw new NotImplementedException();
        }

        public async Task Damage(IHealthStatHolder card)
        {
            throw new NotImplementedException();
        }

        public async Task Heal(IHealthStatHolder card)
        {
            throw new NotImplementedException();
        }

        public async Task ApplyStatus(Card card /*, Buf*/)
        {
            throw new NotImplementedException();
        }

        public async Task ChooseCardFromList(IEnumerable<Card> list)
        {
            throw new NotImplementedException();
        }
        
        public async Task<Card> ChooseCardFromField()
        {
            throw new NotImplementedException();
        }
        
        public async Task<Card> ChooseCardFromHand()
        {
            throw new NotImplementedException();
        }
        
        public async Task<Card> ChooseCardFromDeck()
        {
            throw new NotImplementedException();
        }
        
        public async Task<Card> ChooseCardFromList(Player player, IEnumerable<Card> options)
        {
            throw new NotImplementedException();
        }
        
        public async Task<int> ChooseOptionFromList(Player player, IEnumerable<string> options)
        {
            throw new NotImplementedException();
        }

        public async Task<string> DisplayMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}