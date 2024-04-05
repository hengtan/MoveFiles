using ScheduleFileConsole.Log;
using ScheduleFileConsole.Log.Interfaces;

namespace ScheduleFileConsole.Services.Interfaces
{
    public interface ISettingsFileProcessor
    {
        List<(string source, string destination)> ReadSettingsFromFile(string path);
        void ProcessFiles(List<(string source, string destination)> settingsList, IFileService fileService, ILogService log);
    }
}
