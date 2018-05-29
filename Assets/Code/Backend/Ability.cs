using System.Threading.Tasks;

namespace Kamikaze.Backend
{
    public abstract class Ability
    {
        public Card card;
        public abstract Task Body();
        public abstract bool Condition();
        public string description;

        public Ability(Card card) { this.card = card; }
    }

    public class Move : Ability
    {
        public MovementStat movement;
        public Move(Card card, MovementStat movement) : base(card) { this.movement = movement; }
        public override bool Condition() { return true; }
        public override async Task Body()
        {
            card.FrontendCard.BeginMove();
        }
    }

    public class Attack : Ability
    {
        public AttackStat attack;
        public Attack(Card card, AttackStat attack) : base(card) { this.attack = attack; }
        public override bool Condition () => true;

        public override async Task Body()
        {
            UnityEngine.Debug.Log($"Attacking the air with {attack.Mode} {attack.Value}");            
        }
    }
}
