using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kamikaze.Backend
{
    [Serializable]
    public abstract class FieldCard : Card
    {
        public List<Ability> Abilites { get; set; }

        public Vector2 Position { get; set; }
        public bool IsExiled { get; set; }
        public bool IsInField { get; set; }

        public override void Init(Player owner, Player opponent, IEnumerable container, Frontend.Card front)
        {
            base.Init(owner, opponent, container, front);
        }
    }
}
