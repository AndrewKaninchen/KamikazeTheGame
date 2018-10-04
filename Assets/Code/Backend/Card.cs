using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kamikaze.Frontend_Old;
using UnityEngine;

namespace Kamikaze.Backend
{
    [Serializable]
    public class CardAsset : ScriptableObject
    {
        public Texture2D image;
        public string description;
        public Type associatedType;
    }

    public abstract class Card
    {
        #region SerializedFields
        private CardAsset cardAsset;
        private GameController game;
        #endregion

        public Frontend_Old.Card FrontendCard { get; private set; }

        public IEnumerable container;
        public Player owner;
        public Player opponent;

        public static Card CreateCard(Type type)
        {
            return (Card) Activator.CreateInstance(type);
        }

        public List<TriggerEffect> TriggerEffects { get; set; }
        
        public virtual void Init(Player owner, Player opponent, IEnumerable container, Frontend_Old.Card front, GameController game)
        {
            this.owner = owner;
            this.opponent = opponent;
            this.container = container;
            this.game = game;
            FrontendCard = front;            
        }

        public void SubscribeTriggerEffects()
        {
            foreach (var eff in TriggerEffects)            
                game.Events.SubscribeTriggerEffect(eff);            
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

    public abstract class FieldCard : Card
    {
        public List<Ability> Abilites { get; set; }

        public Vector2 Position { get; set; }
        public bool IsExiled { get; set; }
        public bool IsInField { get; set; }

        public override void Init(Player owner, Player opponent, IEnumerable container, Frontend_Old.Card front, GameController game)
        {
            base.Init(owner, opponent, container, front, game);
        }
    }

    public abstract class UnitCard : FieldCard, IAttackStatHolder, IMovementStatHolder, IHealthStatHolder
    {
        public AttackStat Attack { get; set; }
        public MovementStat Movement { get; set; }
        public int Health { get; set; }

        public override void Init(Player owner, Player opponent, IEnumerable container, Frontend_Old.Card front, GameController game)
        {
            base.Init(owner, opponent, container, front, game);
            Abilites = new List<Ability>
            {
                new Move(this, Movement),
                new Attack(this, Attack)
            };
        }
    }

    public abstract class BuildingCard : FieldCard, IHealthStatHolder
    {
        public int Health { get; set; }
    }

    public abstract class EnchantmentCard : FieldCard
    {
    }

    public abstract class ConjurationCard : Card
    {
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