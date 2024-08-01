using ElevatorControlSystem.Model.Enums;

namespace ElevatorControlSystem.Model
{
    public class Elevator
    {
        public int Id { get; }
        public int CurrentFloor { get; set; }
        public Direction CurrentDirection { get; set; }
        public List<int> Destinations { get; }

        public Elevator(int id, int initialFloor = 1)
        {
            Id = id;
            CurrentFloor = initialFloor;
            CurrentDirection = Direction.Idle;
            Destinations = new List<int>();
        }        
    }
}
