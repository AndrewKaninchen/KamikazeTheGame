using System.Collections.Generic;

namespace Kamikaze.Backend
{
    public abstract class BuildingCard : FieldCard
    {
        protected BuildingCard(Player owner, Player opponent, ICollection<Card> container, GameController game, GameActions actions) 
            : base(owner, opponent, container, game, actions)
        {
        }
    }
}