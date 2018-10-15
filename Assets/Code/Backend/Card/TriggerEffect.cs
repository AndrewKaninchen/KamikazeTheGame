using System.Threading.Tasks;

namespace Kamikaze.Backend
{
    public abstract class TriggerEffect
    {
        public Card effectOwnerCard;
        protected GameActions Actions { get; set; }

        protected TriggerEffect(Card effectOwnerCard)
        {
            this.effectOwnerCard = effectOwnerCard;
            Actions = effectOwnerCard.Actions;
        }

        public virtual bool Mandatory() => true;
        public virtual bool Condition() => true;
        public abstract Task Body();
    }
    
    public abstract class TriggerEffect<T> : TriggerEffect
    {
        public T context;

        protected TriggerEffect(Card effectOwnerCard) : base(effectOwnerCard)
        {
        }
    }
}