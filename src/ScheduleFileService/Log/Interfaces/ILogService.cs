namespace ScheduleFileService.Log
{
    public interface ILogService 
    {
        void InitialMessage();
        void CreateLog(FileExecuted fileExecuted);
        void CreateInitialLogFile();
    }
}
