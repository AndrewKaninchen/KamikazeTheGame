using System.Threading.Tasks;
using static Kamikaze.Backend.GameEvents;

namespace Kamikaze.Backend
{
    public abstract class TriggerEffect
    {
        public Card card;
        public virtual bool Mandatory() => true;
        public virtual bool Condition() => true;
        public abstract Task Body();
    }

    public abstract class TriggerEffect<T> : TriggerEffect where T : GameEvent
    {
        public T context;
    }
}
