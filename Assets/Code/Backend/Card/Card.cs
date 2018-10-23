using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

        public Frontend.Card FrontendCard { get; private set; }

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
      
        protected Card(Player owner, Player opponent, ICollection<Card> container, Frontend.Card front, GameController game, GameActions gameActions)
        {
            this.owner = owner;
            this.opponent = opponent;
            this.Container = container;
            this.Game = game;
            Actions = gameActions;
            FrontendCard = front;            
        }
        public static Card CreateCard(ClassTypeReference type, Player owner, Player opponent, ICollection<Card> container, Frontend_Old.Card front, GameController game, GameActions actions)
        {
            return (Card) Activator.CreateInstance(type,  args: 
                new object[] 
                {
                    owner, opponent, container, front, game, actions
                }
            );
        }

        public List<TriggerEffect> TriggerEffects { get; set; }


        public void SubscribeTriggerEffects()
        {
            foreach (var eff in TriggerEffects)            
                Game.Events.SubscribeTriggerEffect(eff);            
        }
    }

    
}