using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleFileService.Log.Interfaces
{
    public  interface IFileService
    {
        void CreateFolders(string folderPath);
        void CreateFiles(string filePath);
        void SystemFolders();
        string[] GetFiles(string path);
        bool ExistFile(string path);
        bool ExistFolder(string path);
        void Delete(string path);
        void Move(string source, string destination);
        string[] ReadAllLines(string path);
    }
}
