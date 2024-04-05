using ScheduleFileConsole.Log.Interfaces;

namespace ScheduleFileConsole.Log
{
    public class FileService : IFileService
    {
        private const string ProjectFolderPath = @"C:\ScheduleFile";
        private const string LogFolderName = "configuration";
        private const string ConfigFolderName = "log";

        public void SystemFolders()
        {
            CreateFolders(LogFolderName);
            CreateFolders(ConfigFolderName);
        }

        public void CreateFolders(string folderPath)
        {
            string combineFolderPath = Path.Combine(ProjectFolderPath, folderPath);
            if (!Directory.Exists(combineFolderPath))
            {
                Directory.CreateDirectory(combineFolderPath);
            }
        }

        public void CreateFiles(string filePath)
        {
            string combineFilePath = Path.Combine(ProjectFolderPath, filePath);
            if (!File.Exists(combineFilePath))
            {
                File.Create(combineFilePath);
            }
        }

        public void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public bool ExistFile(string path)
        {
            return File.Exists(path);
        }

        public bool ExistFolder(string path)
        {
            return Directory.Exists(path);
        }

        public string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        public void Move(string source, string destination)
        {
            if (File.Exists(source))
            {
                File.Move(source, destination);
            }
        }

        public string[] ReadAllLines(string path)
        {
            return File.ReadAllLines(path);
        }

        public void Copy(string source, string destination)
        {
            if (File.Exists(source))
            {
                File.Copy(source, destination, true);
            }
        }

        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(path, searchPattern, searchOption);
        }

        public string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFileSystemEntries(path, searchPattern, searchOption);
        }
    }
}
