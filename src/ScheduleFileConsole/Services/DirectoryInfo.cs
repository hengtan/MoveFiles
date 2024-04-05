using ScheduleFileConsole.Services.Interfaces;

namespace ScheduleFileConsole.Services
{
    public class DirectoryInfoWrapper(string path) : IDirectoryInfo
    {
        private readonly DirectoryInfo _directoryInfo = new DirectoryInfo(path);

        public FileInfo[] GetFiles()
        {
            return _directoryInfo.GetFiles();
        }

        public IDirectoryInfo[] GetDirectories()
        {
            return _directoryInfo.GetDirectories()
                .Select(dir => new DirectoryInfoWrapper(dir.FullName))
                .ToArray();
        }

        public string Name => _directoryInfo.Name;
    }
}
