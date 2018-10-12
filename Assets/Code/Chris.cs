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
        public Chris(Player owner, Player opponent, IEnumerable container, Frontend_Old.Card front, GameController game, GameActions actions) 
            : base(owner, opponent, container, front, game, actions)
        {
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
                await Actions.Damage(card as IHealthStatHolder);
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