using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kamikaze.Frontend;
using UnityEngine;

namespace Kamikaze.Backend
{
    [Serializable]
    public abstract class Card : ScriptableObject
    {
        public Texture2D image;
        public string description;
        public Player owner;
        public Player opponent;
        public IEnumerable container;
        public Frontend.Card FrontendCard { get; private set; }

        public List<TriggerEffect> TriggerEffects { get; set; }
        
        public virtual void Init(Player owner, Player opponent, IEnumerable container, Frontend.Card front)
        {
            this.owner = owner;
            this.opponent = opponent;
            this.container = container;
            FrontendCard = front;            
        }

        public void SubscribeTriggerEffects()
        {
            foreach (var eff in TriggerEffects)            
                GameEvents.SubscribeTriggerEffect(eff);            
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
}