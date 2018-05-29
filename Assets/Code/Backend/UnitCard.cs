using System.Collections;
using System.Collections.Generic;

namespace Kamikaze.Backend
{
    public abstract class UnitCard : FieldCard, IAttackStatHolder, IMovementStatHolder, IHealthStatHolder
    {
        public AttackStat Attack { get; set; }
        public MovementStat Movement { get; set; }
        public int Health { get; set; }

        public override void Init(Player owner, Player opponent, IEnumerable container, Frontend.Card front)
        {
            base.Init(owner, opponent, container, front);
            Abilites = new List<Ability>
            {
                new Move(this, Movement),
                new Attack(this, Attack)
            };
        }
    }
}
