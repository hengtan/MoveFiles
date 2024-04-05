using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleFileService.Log.Interfaces
{
    public interface ISettingsService
    {
        void CreateInitialSettingsFile();
        void InsertingSettings();
        void ReadSettings();
    }
}
