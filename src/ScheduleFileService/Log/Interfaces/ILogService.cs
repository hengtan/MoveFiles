using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleFileService.Log
{
    public interface ILogService 
    {
        void InitialMessage();
        void CreateLog(FileExecuted fileExecuted);
        void CreateInitialLogFile();
    }
}
