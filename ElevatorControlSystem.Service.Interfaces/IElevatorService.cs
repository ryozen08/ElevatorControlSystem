using ElevatorControlSystem.Model;
using ElevatorControlSystem.Model.Enums;

namespace ElevatorControlSystem.Service.Interfaces

{
    public interface IElevatorService
    {
        void AddDestination(Elevator elevator, int floor);
        void Move(Elevator elevator);
        bool CanAcceptRequest(Elevator elevator, int floor, Direction direction);
    }
}
