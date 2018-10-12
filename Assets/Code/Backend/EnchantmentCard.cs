using System.Collections;

namespace Kamikaze.Backend
{
    public abstract class EnchantmentCard : FieldCard
    {
        protected EnchantmentCard(Player owner, Player opponent, IEnumerable container, Frontend_Old.Card front, GameController game, GameActions actions)
            : base(owner, opponent, container, front, game, actions)
        {
        }
    }
}