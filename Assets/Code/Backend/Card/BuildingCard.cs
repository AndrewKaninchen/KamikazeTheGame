using System.Collections;

namespace Kamikaze.Backend
{
    public abstract class BuildingCard : FieldCard, IHealthStatHolder
    {
        public int Health { get; set; }

        protected BuildingCard(Player owner, Player opponent, IEnumerable container, Frontend_Old.Card front, GameController game, GameActions actions) 
            : base(owner, opponent, container, front, game, actions)
        {
        }
    }
}