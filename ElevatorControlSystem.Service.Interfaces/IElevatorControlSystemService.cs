using ElevatorControlSystem.Model;
using ElevatorControlSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControlSystem.Service.Interfaces
{
    public interface IElevatorControlSystemService
    {
        void GenerateRandomCall();
        void Run();
    }
}
