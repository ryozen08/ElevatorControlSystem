using ElevatorControlSystem.Model;
using ElevatorControlSystem.Model.Enums;
using ElevatorControlSystem.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControlSystem.Service
{
    /// <summary>
    /// Responsible for managing the elevator control system.
    /// Performs the following:
    ///     Manages the elevators.
    ///     Controls the elevators' movements.
    ///     Assigns the best elevator to a given request.
    ///     Generates a random user request.
    /// </summary>
    public class ElevatorControlSystemService : IElevatorControlSystemService
    {
        private readonly List<Elevator> elevators;
        private readonly int numFloors;
        private readonly Random random;
        private readonly IElevatorService _elevatorService;

        public ElevatorControlSystemService(int numElevators, int numFloors, IElevatorService service)
        {
            this.numFloors = numFloors;
            elevators = new List<Elevator>();
            for (int i = 0; i < numElevators; i++)
            {
                elevators.Add(new Elevator(i + 1));
            }
            random = new Random();
            _elevatorService = service;
        }

        /// <summary>
        /// Generate random elevator requests
        /// </summary>
        public void GenerateRandomCall()
        {
            int floor = random.Next(2, numFloors + 1);
            Direction direction = (Direction)random.Next(0, 2);
            Console.WriteLine($"{direction} request on floor {floor} received");
            AssignElevator(floor, direction);
        }

        /// <summary>
        /// Assign a request to an elevator given a specified floor and direction by getting the best score.
        /// </summary>
        /// <param name="floor">The floor that gave the request.</param>
        /// <param name="direction">The direction of the request.</param>
        private void AssignElevator(int floor, Direction direction)
        {
            Elevator bestElevator = null;
            int bestScore = int.MaxValue;

            foreach (var elevator in elevators)
            {
                if (_elevatorService.CanAcceptRequest(elevator, floor, direction))
                {
                    int score = CalculateScore(elevator, floor, direction);
                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestElevator = elevator;
                    }
                }
            }

            if (bestElevator != null)
            {
                _elevatorService.AddDestination(bestElevator, floor);
            }
            else
            {
                Console.WriteLine("No suitable elevator found for the request.");
            }
        }

        /// <summary>
        /// Calculate an elevator's score based on a given floor and direction.
        /// The score is calculated by getting the distance (or difference) between the elevator's floor and the request's floor.
        /// The lower the difference means, the closer the elevator is to the request's floor, which allows the elevator to serve the request faster.        
        /// </summary>
        /// <param name="elevator">The elevator being calculated for a score.</param>
        /// <param name="floor">The floor that gave the request.</param>
        /// <param name="direction">The direction of the request.</param>
        /// <returns></returns>
        private int CalculateScore(Elevator elevator, int floor, Direction direction)
        {
            if (elevator.CurrentDirection == Direction.Idle)
            {
                return Math.Abs(elevator.CurrentFloor - floor);
            }
            if (elevator.CurrentDirection == direction)
            {
                if ((direction == Direction.Up && elevator.CurrentFloor <= floor) ||
                    (direction == Direction.Down && elevator.CurrentFloor >= floor))
                {
                    return Math.Abs(elevator.CurrentFloor - floor);
                }
            }
            return int.MaxValue;
        }

        public void Run()
        {
            while (true)
            {
                foreach (var elevator in elevators)
                {
                    _elevatorService.Move(elevator);
                    Console.WriteLine($"Elevator {elevator.Id} is on floor {elevator.CurrentFloor}");
                }
                Thread.Sleep(1000);
            }
        }
    }
}
