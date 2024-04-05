using ScheduleFileService.Log;
using ScheduleFileService.Log.Interfaces;
using ScheduleFileService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleFileService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if (!DEBUG)
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
			{
				new Service1()
			};
            ServiceBase.Run(ServicesToRun);
#else
            // Debug code: Permite debugar um código sem se passar por um Windows Service.
            // Defina qual método deseja chamar no inicio do Debug (ex. MetodoRealizaFuncao)
            // Depois de debugar basta compilar em Release e instalar para funcionar normalmente.
            ILogService log = new LogService();
            IFileService fileService = new FileService(); // Assuming you have a FileService class that implements IFileService
            ISettingsService settingsService = new SettingsService(); // Assuming you have a SettingsService class that implements ISettingsService

            Service1 service = new Service1(log, fileService, settingsService);
            //Service1 service = new Service1();
            // Chamada do seu método para Debug.
            service.FlowToRun();
            // Coloque sempre um breakpoint para o ponto de parada do seu código.
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#endif

        }
    }
}
