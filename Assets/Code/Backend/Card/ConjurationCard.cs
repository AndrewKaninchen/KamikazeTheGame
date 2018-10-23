using System.Collections.Generic;

namespace Kamikaze.Backend
{
    public abstract class ConjurationCard : Card
    {
        protected ConjurationCard(Player owner, Player opponent, ICollection<Card> container, Frontend.Card front, GameController game, GameActions actions)
            : base(owner, opponent, container, front, game, actions)
        {
        }
    }
}