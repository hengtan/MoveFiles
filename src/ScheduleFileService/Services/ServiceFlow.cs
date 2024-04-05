using ScheduleFileService.Log.Interfaces;
using ScheduleFileService.Log;
using ScheduleFileService.Services.Interfaces;

namespace ScheduleFileService.Services
{
    public class ServiceFlow : IServiceFlow
    {
        private readonly ILogService _log;
        private readonly IFileService _fileService;
        private readonly ISettingsService _settingsService;

        public ServiceFlow(ILogService log, IFileService fileService, ISettingsService settingsService)
        {
            _log = log;
            _fileService = fileService;
            _settingsService = settingsService;
        }

        public void Run()
        {
            _fileService.SystemFolders();
            _settingsService.CreateInitialSettingsFile();
            _log.CreateInitialLogFile();
            _settingsService.InsertingSettings();
            _log.InitialMessage();
            _settingsService.ReadSettings();
        }
    }
}
