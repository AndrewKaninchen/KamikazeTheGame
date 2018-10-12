using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        protected GameActions Actions { get; set; }

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
    
    public class AttackStat
    {
        public AttackStat(AttackMode mode, int value)
        {
            Mode = mode;
            Value = value;
        }

        public enum AttackMode
        {
            Frontal,
            Superior,
            Internal
        }

        public AttackMode Mode { get; set; }
        public int Value { get; set; }
    }

    public class MovementStat
    {
        public MovementStat(MovementMode mode, int value)
        {
            Mode = mode;
            Value = value;
        }

        public enum MovementMode
        {
            March,
            Fly
        }

        public MovementMode Mode { get; set; }
        public int Value { get; set; }
    }
    
    public interface IMovementStatHolder
    {
        MovementStat Movement { get; set; }
    }

    public interface IAttackStatHolder
    {
        AttackStat Attack { get; set; }
    }

    public interface IHealthStatHolder
    {
        int Health { get; set; }        
    }

    public abstract class TriggerEffect
    {
        public Card card;
        public virtual bool Mandatory() => true;
        public virtual bool Condition() => true;
        public abstract Task Body();
    }

    public abstract class TriggerEffect<T> : TriggerEffect where T : GameEvents.GameEvent
    {
        public T context;
    }
}