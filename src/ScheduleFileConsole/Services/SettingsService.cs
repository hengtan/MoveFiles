using ScheduleFileConsole.Log.Interfaces;
using ScheduleFileConsole.Log;
using ScheduleFileConsole.Services.Interfaces;

namespace ScheduleFileConsole.Services
{
    public class SettingsService(IFileService fileService, ILogService log, ISettingsFileProcessor settingsFileProcessor) : ISettingsService
    {
        private readonly string _path = @"C:\ScheduleFile\configuration";
        private readonly IFileService _fileService = fileService;
        private readonly ILogService _log = log;
        private readonly ISettingsFileProcessor _settingsFileProcessor = settingsFileProcessor;

        public void CreateInitialSettingsFile()
        {
            string configFilePath = Path.Combine(_path, "settings.txt");

            if (!_fileService.ExistFile(configFilePath))
            {
                using (File.Create(configFilePath)) { }
            }
        }

        public void InsertingSettings()
        {
            string _pathCombined = Path.Combine(_path, "settings.txt");

            if (_fileService.ExistFile(_pathCombined))
            {
                string content = File.ReadAllText(_pathCombined);
                if (string.IsNullOrEmpty(content))
                {
                    using (StreamWriter sw = File.AppendText(_pathCombined))
                    {
                        var origem = "ORIGEM: C:\\origem";
                        var destiny = "DESTINO: C:\\destino";

                        sw.WriteLine(origem);
                        sw.WriteLine(destiny);
                    }
                }
            }
        }

        public void ReadSettings()
        {
            var settings = _settingsFileProcessor.ReadSettingsFromFile(_path);
            _settingsFileProcessor.ProcessFiles(settings, _fileService, _log);
        }
    }
}