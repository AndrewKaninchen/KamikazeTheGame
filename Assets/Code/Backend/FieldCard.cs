using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kamikaze.Backend
{
    public abstract class FieldCard : Card
    {
        public List<Ability> Abilites { get; set; }

        public Vector2 Position { get; set; }
        public bool IsExiled { get; set; }
        public bool IsInField { get; set; }

        protected FieldCard(Player owner, Player opponent, IEnumerable container, Frontend_Old.Card front, GameController game, GameActions actions) 
            : base (owner, opponent, container, front, game, actions)
        {
        }
    }
}