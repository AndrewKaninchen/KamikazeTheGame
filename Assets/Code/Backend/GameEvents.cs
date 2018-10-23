using System;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Kamikaze.Backend
{
    public class GameEvents
    {
        public GameController Game { get; private set; }
        public GameEvents(GameController game) { Game = game; }

        private Dictionary<Type, List<TriggerEffect>> triggerEffects = new Dictionary<Type, List<TriggerEffect>>();

        public void SubscribeTriggerEffect(TriggerEffect eff)
        {
            var T = eff.GetType().BaseType.GetGenericArguments()[0];
            if (!triggerEffects.ContainsKey(T))
                triggerEffects.Add(T, new List<TriggerEffect>() { eff });
            else
                triggerEffects[T].Add(eff);
        }
        
        public async Task CallEvent<T>(T context) where T : GameEvent
        {
            var triggeredEffects = triggerEffects[typeof(T)];

            var p1Mandatory = new List<TriggerEffect<T>>();
            var p1Optional = new List<TriggerEffect<T>>();
            var p2Mandatory = new List<TriggerEffect<T>>();
            var p2Optional = new List<TriggerEffect<T>>();

            var p1 = Game.CurrentPlayer;
            var p2 = p1.Opponent;

            if (triggeredEffects != null)
            {
                foreach (var eff in triggeredEffects)
                {
                    if (eff.effectOwnerCard.owner == p1)
                        (eff.Mandatory() ? p1Mandatory : p1Optional).Add(eff as TriggerEffect<T>);
                    else
                        (eff.Mandatory() ? p2Mandatory : p2Optional).Add(eff as TriggerEffect<T>);
                }
            }
            // Tell frontend to get the order of entry from the players

            var stack = p1Mandatory.Concat(p2Mandatory).Concat(p1Optional).Concat(p2Mandatory).ToList();

            foreach (var eff in stack)
                await eff.Body();

            //http://yugioh.wikia.com/wiki/Simultaneous_Effects_Go_On_Chain
        }

        public abstract class GameEvent { }

        public class OnGameStart : GameEvent {}

        public class OnCardDrawn : GameEvent
        {
            public Card cardDrawn;
            public Player player;
        }

        public class OnUnitSummoned : GameEvent
        {
            public UnitCard unit;
        }

        public class OnBuildingSummoned : GameEvent
        {
            public BuildingCard building;
        }

        public class OnCardEffectActivated : GameEvent
        {
            public Card card;
            public TriggerEffect effect;
        }

        public class OnCardAbilityActivated : GameEvent
        {
            public Card card;
            public Ability ability;
        }

        public class OnCardAttacked : GameEvent
        {
            public Card attacker;
            public Card defender;
            public Attack attackUsed;
        }

        public class OnCardDamaged : GameEvent
        {
            public FieldCard damagedCard;
            //info about damage taken
        }


        public class OnCardHealed : GameEvent
        {
            public FieldCard healedCard;
        }
        
        public class OnCardDestroyed : GameEvent
        {
            public FieldCard destroyedCard;
        }

        public class OnCardRessurrected : GameEvent
        {
            public FieldCard ressurrectedCard;
        }
         
        public class OnAddCrystal : GameEvent
        {
            public OnAddCrystal(EnergyType energyType, Player player)
            {
                this.energyType = energyType;
                this.player = player;
            }

            public EnergyType energyType;
            public Player player;
        }

        public class OnAddEnergy : GameEvent
        {
            public OnAddEnergy(EnergyType energyType, Player player, int amountAdded)
            {
                this.energyType = energyType;
                this.player = player;
                this.amountAdded = amountAdded;
            }

            public EnergyType energyType;
            public Player player;
            public int amountAdded;
        }
    }
}
