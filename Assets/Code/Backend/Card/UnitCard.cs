using System.Collections.Generic;

namespace Kamikaze.Backend
{
    public abstract class UnitCard : FieldCard, IAttackStatHolder, IMovementStatHolder
    {
        public AttackStat Attack { get; set; }
        public MovementStat Movement { get; set; }

        protected UnitCard(Player owner, Player opponent, ICollection<Card> container, Frontend.Card front, GameController game, GameActions actions)
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