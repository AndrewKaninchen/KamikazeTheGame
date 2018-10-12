using System.Threading.Tasks;

namespace Kamikaze.Backend
{
    public abstract class TriggerEffect
    {
        public Card card;
        public virtual bool Mandatory() => true;
        public virtual bool Condition() => true;
        public abstract Task Body();
    }
}