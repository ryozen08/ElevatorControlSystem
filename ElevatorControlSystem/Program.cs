using ElevatorControlSystem.Service;
using ElevatorControlSystem.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ElevatorControlSystem
{
    public class ElevatorControlSystem
    {        
        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IElevatorService>(provider => new ElevatorService())
                .AddSingleton<IElevatorControlSystemService>(provider =>
                    {
                        var elevatorService = provider.GetService<IElevatorService>();
                        return new ElevatorControlSystemService(4, 10, elevatorService);
                    })
                .BuildServiceProvider();

            var elevatorControlSystemService = serviceProvider.GetService<IElevatorControlSystemService>();

            Timer timer = new Timer(_ => elevatorControlSystemService.GenerateRandomCall(), null, 0, 30000);
            elevatorControlSystemService.Run();
        }
    }
}
