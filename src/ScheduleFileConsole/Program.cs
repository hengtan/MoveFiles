using ScheduleFileConsole.Log;
using ScheduleFileConsole.Log.Interfaces;
using ScheduleFileConsole.Services;
using ScheduleFileConsole.Services.Interfaces;

ILogService logService = new LogService();
IFileService fileService = new FileService();
ISettingsFileProcessor settingsFileProcessor = new SettingsFileProcessor(fileService, logService);
ISettingsService settingsService = new SettingsService(fileService, logService, settingsFileProcessor);
IServiceFlow serviceFlow = new ServiceFlow(logService, fileService, settingsService);

serviceFlow.Run();