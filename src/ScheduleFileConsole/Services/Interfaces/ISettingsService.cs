namespace ScheduleFileConsole.Log.Interfaces
{
    public interface ISettingsService
    {
        void CreateInitialSettingsFile();
        void InsertingSettings();
        void ReadSettings();
    }
}
