using System.Collections;
using System.Collections.Generic;

namespace Kamikaze.Backend
{
    public abstract class UnitCard : FieldCard, IAttackStatHolder, IMovementStatHolder, IHealthStatHolder
    {
        public AttackStat Attack { get; set; }
        public MovementStat Movement { get; set; }
        public int Health { get; set; }

        public UnitCard(Player owner, Player opponent, IEnumerable container, Frontend_Old.Card front, GameController game, GameActions actions)
            : base (owner, opponent, container, front, game, actions)
        {
            Abilites = new List<Ability>
            {
                new Move(this, Movement),
                new Attack(this, Attack)
            };
        }
    }
}