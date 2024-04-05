// See https://aka.ms/new-console-template for more information
using ScheduleFileConsole.Log;
using ScheduleFileConsole.Log.Interfaces;
using ScheduleFileConsole.Services;
using ScheduleFileConsole.Services.Interfaces;



ILogService logService = new LogService();
IFileService fileService = new FileService();
ISettingsService settingsService = new SettingsService();
IServiceFlow serviceFlow = new ServiceFlow(logService, fileService, settingsService);

serviceFlow.Run();
