using System.ServiceProcess;
using ScheduleFileService.Services.Interfaces;

namespace ScheduleFileService
{
    public partial class Service1 : ServiceBase
    {
        private readonly IServiceFlow _serviceFlow;

        public Service1(IServiceFlow serviceFlow)
        {
            _serviceFlow = serviceFlow;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _serviceFlow.Run();
        }

        protected override void OnStop()
        {
            // Code to stop the service
        }
    }
}
