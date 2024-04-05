using ScheduleFileConsole.Log.Interfaces;
using ScheduleFileConsole.Log;
using ScheduleFileConsole.Services.Interfaces;

namespace ScheduleFileConsole.Services
{
    public class ServiceFlow(ILogService log, IFileService fileService, ISettingsService settingsService) : IServiceFlow
    {
        private readonly ILogService _log = log;
        private readonly IFileService _fileService = fileService;
        private readonly ISettingsService _settingsService = settingsService;

        public void Run()
        {
            //show message start
            Console.WriteLine("Starting the service...");

            _fileService.SystemFolders();
            _settingsService.CreateInitialSettingsFile();
            _log.CreateInitialLogFile();
            _settingsService.InsertingSettings();
            _log.InitialMessage();
            _settingsService.ReadSettings();

            //show message end
            Console.WriteLine("Finishing the service...");
        }
    }
}
