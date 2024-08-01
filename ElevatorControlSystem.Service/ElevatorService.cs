using ElevatorControlSystem.Model;
using ElevatorControlSystem.Model.Enums;
using ElevatorControlSystem.Service.Interfaces;

namespace ElevatorControlSystem.Service
{
    public class ElevatorService : IElevatorService
    {
        public void AddDestination(Elevator elevator, int floor)
        {
            if (!elevator.Destinations.Contains(floor))
            {
                elevator.Destinations.Add(floor);
                elevator.Destinations.Sort();
            }
        }

        /// <summary>
        /// Move a given elevator to a destination from the top of its list of destinations.
        /// </summary>
        /// <param name="elevator"></param>
        public void Move(Elevator elevator)
        {
            if (!elevator.Destinations.Any())
            {
                elevator.CurrentDirection = Direction.Idle;
                return;
            }

            int nextFloor = elevator.Destinations.First();
            if (nextFloor > elevator.CurrentFloor)
            {
                elevator.CurrentDirection = Direction.Up;
                elevator.CurrentFloor++;
            }
            else if (nextFloor < elevator.CurrentFloor)
            {
                elevator.CurrentDirection = Direction.Down;
                elevator.CurrentFloor--;
            }

            Console.WriteLine($"Elevator {elevator.Id} is moving to floor {elevator.CurrentFloor}");
            Thread.Sleep(10000); // Simulate movement time between floors

            if (elevator.CurrentFloor == nextFloor)
            {
                elevator.Destinations.RemoveAt(0);
                elevator.CurrentDirection = elevator.Destinations.Count > 0 ? elevator.CurrentDirection : Direction.Idle;
                Console.WriteLine($"Elevator {elevator.Id} has arrived at floor {elevator.CurrentFloor}");
                Thread.Sleep(10000); // Passengers enter/leave
            }
        }

        public bool CanAcceptRequest(Elevator elevator, int floor, Direction direction)
        {
            if (elevator.CurrentDirection == Direction.Idle)
                return true;

            if (elevator.CurrentDirection == direction)
            {
                if (direction == Direction.Up && floor >= elevator.CurrentFloor)
                    return true;

                if (direction == Direction.Down && floor <= elevator.CurrentFloor)
                    return true;
            }

            return false;
        }
    }
}
