using System;
using System.Collections;
using System.Collections.Generic;
using Kamikaze.Frontend_Old;

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

        public Frontend_Old.Card FrontendCard { get; private set; }

        public IEnumerable container;
        public Player owner;
        public Player opponent;

        public static Card CreateCard(Type type)
        {
            return (Card) Activator.CreateInstance(type);
        }

        public List<TriggerEffect> TriggerEffects { get; set; }

        protected Card(Player owner, Player opponent, IEnumerable container, Frontend_Old.Card front, GameController game, GameActions gameActions)
        {
            this.owner = owner;
            this.opponent = opponent;
            this.container = container;
            this.Game = game;
            Actions = gameActions;
            FrontendCard = front;            
        }

        public void SubscribeTriggerEffects()
        {
            foreach (var eff in TriggerEffects)            
                Game.Events.SubscribeTriggerEffect(eff);            
        }
    }

    public abstract class TriggerEffect<T> : TriggerEffect where T : GameEvents.GameEvent
    {
        public T context;
    }
}