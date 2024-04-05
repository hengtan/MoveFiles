using System.ServiceProcess;
using ScheduleFileService.Log;
using ScheduleFileService.Log.Interfaces;
using ScheduleFileService.Services;
using ScheduleFileService.Services.Interfaces;

namespace ScheduleFileService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ILogService logService = new LogService();
            IFileService fileService = new FileService();
            ISettingsService settingsService = new SettingsService();
            IServiceFlow serviceFlow = new ServiceFlow(logService, fileService, settingsService);

#if (!DEBUG)
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1(serviceFlow) // Pass serviceFlow to the constructor
            };
            ServiceBase.Run(ServicesToRun);
#else
            // Debug code: Allows you to debug code without passing through a Windows Service.
            // Set which method you want to call at the start of Debug (e.g. MethodPerformsFunction)
            // After debugging, just compile in Release and install to work normally.
            Service1 service = new Service1(serviceFlow);
            // Call your method for Debug.
            serviceFlow.Run();
            // Always put a breakpoint for the stopping point of your code.
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#endif
        }
    }
}