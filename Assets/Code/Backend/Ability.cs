using System.Threading.Tasks;

namespace Kamikaze.Backend
{
    public abstract class Ability
    {
        protected GameActions Actions { get; set; }
        public readonly Card card;
        public abstract Task Body();
        public abstract bool Condition();
        public abstract string Name { get; }
        public abstract string Description { get; }

        public Ability(Card card)
        {
            this.card = card;
            Actions = card.Actions;
        }
    }

    public class Move : Ability
    {
        public MovementStat movement;
        public Move(Card card, MovementStat movement) : base(card) { this.movement = movement; }
        public override bool Condition() { return true; }
        public override string Name => "Movement";
        public override string Description => "Regular movement.";

        public override async Task Body()
        {
            //card.FrontendCard.BeginMove();
        }
    }

    public class Attack : Ability
    {
        public AttackStat attack;
        public Attack(Card card, AttackStat attack) : base(card) { this.attack = attack; }
        public override bool Condition () => true;
        public override string Name => "Attack";
        public override string Description => "Regular attack.";

        public override async Task Body()
        {
            UnityEngine.Debug.Log($"Attacking the air with {attack.Mode} {attack.Value}");            
        }
    }
}
