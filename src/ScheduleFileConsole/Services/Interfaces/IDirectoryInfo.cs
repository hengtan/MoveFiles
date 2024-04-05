namespace ScheduleFileConsole.Services.Interfaces
{
    public interface IDirectoryInfo
    {
        
        FileInfo[] GetFiles();
        IDirectoryInfo[] GetDirectories();
        string Name { get; }
    }
}
