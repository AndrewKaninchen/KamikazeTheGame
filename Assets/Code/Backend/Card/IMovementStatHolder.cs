namespace Kamikaze.Backend
{
    public enum MovementMode
    {
        March,
        Fly
    }
    
    public class MovementStat
    {
        public MovementStat(MovementMode mode, int value)
        {
            Mode = mode;
            Value = value;
        }
       
        public MovementMode Mode { get; set; }
        public int Value { get; set; }
    }
    
    public interface IMovementStatHolder
    {
        MovementStat Movement { get; set; }
    }
}