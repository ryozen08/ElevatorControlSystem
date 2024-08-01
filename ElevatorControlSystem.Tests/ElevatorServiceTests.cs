using NUnit.Framework;
using ElevatorControlSystem.Service;
using System.Linq;
using ElevatorControlSystem.Model;
using ElevatorControlSystem.Model.Enums;

namespace ElevatorControlSystem.Tests
{
    [TestFixture]
    public class ElevatorServiceTests
    {
        private ElevatorService _elevatorService;

        [SetUp]
        public void Setup()
        {
            _elevatorService = new ElevatorService();
        }

        [Test]
        public void AddDestination_ShouldAddFloorToDestinations()
        {
            var elevator = new Elevator(1);
            _elevatorService.AddDestination(elevator, 5);

            Assert.Contains(5, elevator.Destinations);
        }

        [Test]
        public void Move_ShouldMoveElevatorUp()
        {
            var elevator = new Elevator(1) { CurrentFloor = 1 };
            _elevatorService.AddDestination(elevator, 5);
            _elevatorService.Move(elevator);

            Assert.AreEqual(2, elevator.CurrentFloor);
            Assert.AreEqual(Direction.Up, elevator.CurrentDirection);
        }

        [Test]
        public void Move_ShouldMoveElevatorDown()
        {
            var elevator = new Elevator(1) { CurrentFloor = 5 };
            _elevatorService.AddDestination(elevator, 1);
            _elevatorService.Move(elevator);

            Assert.AreEqual(4, elevator.CurrentFloor);
            Assert.AreEqual(Direction.Down, elevator.CurrentDirection);
        }

        [Test]
        public void Move_ShouldStopAtDestination()
        {
            var elevator = new Elevator(1) { CurrentFloor = 1 };
            _elevatorService.AddDestination(elevator, 2);
            _elevatorService.Move(elevator); // Move to 2

            Assert.AreEqual(2, elevator.CurrentFloor);
            Assert.AreEqual(Direction.Idle, elevator.CurrentDirection);
            Assert.IsEmpty(elevator.Destinations);
        }

        [Test]
        public void CanAcceptRequest_ShouldReturnTrue_WhenIdle()
        {
            var elevator = new Elevator(1);
            var result = _elevatorService.CanAcceptRequest(elevator, 5, Direction.Up);

            Assert.IsTrue(result);
        }

        [Test]
        public void CanAcceptRequest_ShouldReturnTrue_WhenSameDirection()
        {
            var elevator = new Elevator(1) { CurrentFloor = 3, CurrentDirection = Direction.Up };
            var result = _elevatorService.CanAcceptRequest(elevator, 5, Direction.Up);

            Assert.IsTrue(result);
        }

        [Test]
        public void CanAcceptRequest_ShouldReturnFalse_WhenDifferentDirection()
        {
            var elevator = new Elevator(1) { CurrentFloor = 5, CurrentDirection = Direction.Up };
            var result = _elevatorService.CanAcceptRequest(elevator, 3, Direction.Down);

            Assert.IsFalse(result);
        }
    }
}
