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
            var triggeredEffects = triggerEffects[typeof(T)] as List<TriggerEffect<T>>;

            var p1mandatory = new List<TriggerEffect<T>>();
            var p1optional = new List<TriggerEffect<T>>();
            var p2mandatory = new List<TriggerEffect<T>>();
            var p2optional = new List<TriggerEffect<T>>();

            var p1 = Game.currentPlayer;
            var p2 = p1.opponent;

            foreach (var eff in triggeredEffects)
            {
                if (eff.card.owner == p1)
                    (eff.Mandatory() ? p1mandatory : p1optional).Add(eff);
                else
                    (eff.Mandatory() ? p2mandatory : p2optional).Add(eff);
            }

            // Tell frontend to get the order of entry from the players

            var stack = p1mandatory.Concat(p2mandatory).Concat(p1optional).Concat(p2mandatory).ToList();

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

        public class OnBuildingSummoned
        {
            public BuildingCard building;
        }

        public class OnCardEffectActivated
        {
            public Card card;
            public TriggerEffect effect;
        }

        public class OnCardAbilityActivated
        {
            public FieldCard card;
            public Ability ability;
        }

        public class OnCardAttack
        {
            public UnitCard attacker;
            public IHealthStatHolder defender;
        }

        public class OnAddCrystal
        {
            public EnergyType energyType;
            public Player player;
        }

        public class OnAddEnergy
        {
            public EnergyType energyType;
            public Player player;
        }
    }
}
