namespace Kamikaze.Backend
{    
    public class AttackStat
    {
        public AttackStat(AttackMode mode, int value)
        {
            Mode = mode;
            Value = value;
        }

        public enum AttackMode
        {
            Frontal,
            Superior,
            Internal
        }

        public AttackMode Mode { get; set; }
        public int Value { get; set; }
    }

    public interface IAttackStatHolder
    {
        AttackStat Attack { get; set; }
    }
}