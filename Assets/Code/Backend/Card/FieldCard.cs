using System.Collections.Generic;
using UnityEngine;

namespace Kamikaze.Backend
{
    public abstract class FieldCard : Card, IHealthStatHolder
    {
        public List<Ability> Abilites { get; set; }

        public Vector2 Position { get; set; }
        public bool IsExiled { get; set; }
        public bool IsInField { get; set; }
        public int Health { get; set; }

        protected FieldCard(Player owner, Player opponent, ICollection<Card> container, GameController game, GameActions actions) 
            : base (owner, opponent, container, game, actions)
        {
        }

    }
}