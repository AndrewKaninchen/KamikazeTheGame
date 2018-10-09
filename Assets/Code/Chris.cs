using Kamikaze.Backend;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Kamikaze
{
    public class Chris : UnitCard
    {
        public override void Init(Player owner, Player opponent, IEnumerable container, Kamikaze.Frontend_Old.Card front, GameController game)
        {
            base.Init(owner, opponent, container, front, game);

            Attack = new AttackStat(AttackStat.AttackMode.Frontal, 3);
            Movement = new MovementStat(MovementStat.MovementMode.March, 10);

            Abilites = new List<Ability>
            {
                new Move (this, Movement),
                new Attack (this, Attack),
                new ChrisDrinksAbility(this)
            };

            TriggerEffects = new List<TriggerEffect>
            {
                new DrinkEffect()
            };
        }

        public class ChrisDrinksAbility : Ability
        {
            public ChrisDrinksAbility(Card card) : base(card) { }

            public override async Task Body()
            {
                UnityEngine.Debug.Log("Chris Drinks");
            }

            public override bool Condition() => true;
        }

        public class DrinkEffect : TriggerEffect<GameEvents.OnCardDrawn>
        {
            public override bool Condition()
            {
                return ((FieldCard) card).IsInField;
            }
            public override async Task Body()
            {
                UnityEngine.Debug.Log("Everybody Drinks");
            }
        }
    }
}