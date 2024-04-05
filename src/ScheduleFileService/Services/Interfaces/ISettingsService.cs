namespace ScheduleFileService.Log.Interfaces
{
    public interface ISettingsService
    {
        void CreateInitialSettingsFile();
        void InsertingSettings();
        void ReadSettings();
    }
}
