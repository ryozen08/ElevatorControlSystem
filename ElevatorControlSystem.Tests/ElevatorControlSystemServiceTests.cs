using NUnit.Framework;
using ElevatorControlSystem.Service;
using ElevatorControlSystem.Service.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Linq;
using ElevatorControlSystem.Model.Enums;
using ElevatorControlSystem.Model;

namespace ElevatorControlSystem.Tests
{
    [TestFixture]
    public class ElevatorControlSystemServiceTests
    {
        private ElevatorControlSystemService _elevatorControlSystemService;
        private Mock<IElevatorService> _mockElevatorService;
        private List<Elevator> _elevators;

        [SetUp]
        public void Setup()
        {
            _mockElevatorService = new Mock<IElevatorService>();
            _elevators = new List<Elevator>
            {
                new Elevator(1) { CurrentFloor = 1, CurrentDirection = Direction.Idle },
                new Elevator(2) { CurrentFloor = 5, CurrentDirection = Direction.Idle },
                new Elevator(3) { CurrentFloor = 10, CurrentDirection = Direction.Idle },
                new Elevator(4) { CurrentFloor = 2, CurrentDirection = Direction.Idle }
            };

            _elevatorControlSystemService = new ElevatorControlSystemService(_elevators.Count, 10, _mockElevatorService.Object);

            // Inject the mock elevators into the control system using reflection
            typeof(ElevatorControlSystemService)
                .GetField("elevators", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_elevatorControlSystemService, _elevators);
        }

        [Test]
        public void GenerateRandomCall_ShouldAssignElevator()
        {
            // Arrange
            _mockElevatorService.Setup(s => s.CanAcceptRequest(It.IsAny<Elevator>(), It.IsAny<int>(), It.IsAny<Direction>())).Returns(true);
            _mockElevatorService.Setup(s => s.AddDestination(It.IsAny<Elevator>(), It.IsAny<int>()));

            // Act
            _elevatorControlSystemService.GenerateRandomCall();

            // Assert
            _mockElevatorService.Verify(s => s.AddDestination(It.IsAny<Elevator>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void AssignElevator_ShouldAssignToBestElevator()
        {
            // Arrange
            var floor = 3;
            var direction = Direction.Up;

            _mockElevatorService.Setup(s => s.CanAcceptRequest(It.IsAny<Elevator>(), floor, direction)).Returns((Elevator e, int f, Direction d) => true);
            _mockElevatorService.Setup(s => s.AddDestination(It.IsAny<Elevator>(), floor)).Callback((Elevator e, int f) => e.Destinations.Add(f));

            // Act
            typeof(ElevatorControlSystemService)
                .GetMethod("AssignElevator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_elevatorControlSystemService, new object[] { floor, direction });

            // Assert
            var bestElevator = _elevators.OrderBy(e => Math.Abs(e.CurrentFloor - floor)).First();
            Assert.Contains(floor, bestElevator.Destinations);
        }
    }
}
