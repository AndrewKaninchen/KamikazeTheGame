﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Kamikaze.Frontend;
using Kamikaze.Frontend_Old;
using TypeReferences;
using UnityEngine;

namespace Kamikaze.Backend
{
    [Serializable]
    public abstract class Card
    {
        #region SerializedFields
        private CardAsset cardAsset;
        #endregion

        protected GameController Game { get; set; }
        public GameActions Actions { get; set; }

        public Frontend.Card FrontendCard { get; set; }
        public Frontend.Token FrontendToken => FrontendCard.Token;
        
        private ICollection<Card> container;
        public ICollection<Card> Container
        {
            get => container;
            set 
            {
                container?.Remove(this);
                container = value;
            }
        }
    
        public Player owner;
        public Player opponent;
      
        protected Card(Player owner, Player opponent, ICollection<Card> container, GameController game, GameActions gameActions)
        {
            this.owner = owner;
            this.opponent = opponent;
            this.Container = container;
            this.Game = game;
            Actions = gameActions;
            //FrontendCard = front;
            //FrontendToken = front.Token;
        }
        
        public static Card CreateCard(ClassTypeReference type, Player owner, Player opponent, ICollection<Card> container, GameController game, GameActions gameActions)
        {
            var instance = (Card) Activator.CreateInstance(type,  args: 
                new object[] 
                {
                    owner, opponent, container, game, gameActions
                }
            );

            var dummy = game.FrontendController.playerObjects[owner].hand.CreateDummy();
            var front = game.FrontendController.CreateCard(instance);
            front.dummy = dummy;
            instance.FrontendCard = front;
            return instance;
        }

        public List<TriggerEffect> TriggerEffects { get; set; }


        public void SubscribeTriggerEffects()
        {
            foreach (var eff in TriggerEffects)            
                Game.Events.SubscribeTriggerEffect(eff);            
        }
    }

    
}