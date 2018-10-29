using System.Collections.Generic;

namespace Kamikaze.Backend
{
    public abstract class EnchantmentCard : FieldCard
    {
        protected EnchantmentCard(Player owner, Player opponent, ICollection<Card> container, GameController game, GameActions actions)
            : base(owner, opponent, container, game, actions)
        {
        }
    }
}