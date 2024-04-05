//using ScheduleFileConsole.Log.Interfaces;
//using ScheduleFileConsole.Log;


//namespace ScheduleFileConsole.Services
//{
//    public class SettingsService : ISettingsService
//    {
//        private readonly string _path = @"C:\ScheduleFile\configuration";
//        private readonly IFileService _fileService;
//        private readonly ILogService _log;

//        public SettingsService()
//        {
//            _fileService = new FileService();
//            _log = new LogService();
//        }

//        public void CreateInitialSettingsFile()
//        {

//            string configFilePath = Path.Combine(_path, "settings.txt");

//            bool configFileExists = _fileService.ExistFile(configFilePath);

//            if (!configFileExists)
//            {
//                using (File.Create(configFilePath))
//                {}
//            }

//        }

//        public void InsertingSettings()
//        {

//            string _pathCombined = Path.Combine(_path, "settings.txt");

//            bool settingsFileExists = _fileService.ExistFile(_pathCombined);

//            if (settingsFileExists)
//            {
//                string content = File.ReadAllText(_pathCombined);
//                if (string.IsNullOrEmpty(content))
//                {
//                    using (StreamWriter sw = File.AppendText(_pathCombined))
//                    {


//                        var origem = "ORIGEM: C:\\origem";
//                        var destiny = "DESTINO: C:\\destino";

//                        sw.WriteLine(origem);
//                        sw.WriteLine(destiny);
//                    }
//                }
//            }
//        }

//        public void ReadSettings()
//        {
//            var settings = ReadSettingsFromFile();
//            ProcessFiles(settings);            
//        }

//        private List<(string source, string destination)> ReadSettingsFromFile()
//        {
//            var pathToRead = Path.Combine(_path, "settings.txt");
//            string[] lines = File.ReadAllLines(pathToRead);

//            var settingsList = new List<(string source, string destination)>();

//            foreach (var line in lines)
//            {
//                var parts = line.Split(',');
//                if (parts.Length == 2)
//                {
//                    string source = parts[0].Substring("ORIGEM: ".Length);
//                    string destination = parts[1].Substring("DESTINO: ".Length);
//                    settingsList.Add((source, destination));
//                }
//            }

//            return settingsList;
//        }

//        private void ProcessFiles(List<(string source, string destination)> settingsList)
//        {
//            foreach (var settings in settingsList)
//            {
//                FileExecuted fileExecuted = new FileExecuted()
//                {
//                    Origem = settings.source,
//                    Destino = settings.destination
//                };

//                var files = 
//                Directory.GetFileSystemEntries(settings.source, "*", SearchOption.AllDirectories);

//                if (files.Length > 0)
//                {
//                    fileExecuted = MoveFiles(settings.source, settings.destination);
//                    _log.CreateLog(fileExecuted);
//                }
//                else
//                {
//                    _log.CreateLog(fileExecuted);
//                }
//            }
//        }


//        private FileExecuted MoveFiles(string source, string destination)
//        {
//            FileExecuted fileExecuted = new FileExecuted
//            {
//                DataInicial = DateTime.Now,
//                Origem = source,
//                Destino = destination,
//                QTDOrigem = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories).Length,
//                ArquivosMovidos = new List<Files>(),
//                ArquivosNaoMovidos = new List<Files>()
//            };

//            DirectoryInfo sourceDir = new DirectoryInfo(source);

//            // This recursive method will copy all files and directories
//            CopyAll(sourceDir, destination, fileExecuted);            
//            fileExecuted.DataFinal = DateTime.Now;

//            return fileExecuted;
//        }

//        private void CopyAll(DirectoryInfo source, string destination, FileExecuted fileExecuted)
//        {
//            destination = destination.Trim();
//            Directory.CreateDirectory(destination);            

//            foreach (FileInfo file in source.GetFiles())
//            {
//                string destFile = Path.Combine(destination, file.Name);
//                if (_fileService.ExistFile(destFile))
//                {
//                    _fileService.Delete(destFile);
//                }
//                _fileService.Copy(file.FullName, destFile);
//                fileExecuted.ArquivosMovidos.Add(new Files { Nome = file.FullName });
//            }

//            foreach (DirectoryInfo subDir in source.GetDirectories())
//            {
//                string destDir = Path.Combine(destination, subDir.Name);
//                CopyAll(subDir, destDir, fileExecuted);
//            }
//        }
//    }
//}


using ScheduleFileConsole.Log.Interfaces;
using ScheduleFileConsole.Log;
using ScheduleFileConsole;

namespace ScheduleFileConsole.Services
{
    public interface ISettingsService
    {
        void CreateInitialSettingsFile();
        void InsertingSettings();
        void ReadSettings();
    }

    public class SettingsService : ISettingsService
    {
        private readonly string _path = @"C:\ScheduleFile\configuration";
        private readonly IFileService _fileService;
        private readonly ILogService _log;
        private readonly ISettingsFileProcessor _settingsFileProcessor;

        public SettingsService(IFileService fileService, ILogService log, ISettingsFileProcessor settingsFileProcessor)
        {
            _fileService = fileService;
            _log = log;
            _settingsFileProcessor = settingsFileProcessor;
        }

        public void CreateInitialSettingsFile()
        {
            string configFilePath = Path.Combine(_path, "settings.txt");

            if (!_fileService.ExistFile(configFilePath))
            {
                using (File.Create(configFilePath)) { }
            }
        }

        public void InsertingSettings()
        {
            string _pathCombined = Path.Combine(_path, "settings.txt");

            if (_fileService.ExistFile(_pathCombined))
            {
                string content = File.ReadAllText(_pathCombined);
                if (string.IsNullOrEmpty(content))
                {
                    using (StreamWriter sw = File.AppendText(_pathCombined))
                    {
                        var origem = "ORIGEM: C:\\origem";
                        var destiny = "DESTINO: C:\\destino";

                        sw.WriteLine(origem);
                        sw.WriteLine(destiny);
                    }
                }
            }
        }

        public void ReadSettings()
        {
            var settings = _settingsFileProcessor.ReadSettingsFromFile(_path);
            _settingsFileProcessor.ProcessFiles(settings, _fileService, _log);
        }
    }
}

public interface ISettingsFileProcessor
{
    List<(string source, string destination)> ReadSettingsFromFile(string path);
    void ProcessFiles(List<(string source, string destination)> settingsList, IFileService fileService, ILogService log);
}

public class SettingsFileProcessor : ISettingsFileProcessor
{
    public List<(string source, string destination)> ReadSettingsFromFile(string path)
    {
        var pathToRead = Path.Combine(path, "settings.txt");
        string[] lines = File.ReadAllLines(pathToRead);

        var settingsList = new List<(string source, string destination)>();

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length == 2)
            {
                string source = parts[0].Substring("ORIGEM: ".Length);
                string destination = parts[1].Substring("DESTINO: ".Length);
                settingsList.Add((source, destination));
            }
        }

        return settingsList;
    }

    public void ProcessFiles(List<(string source, string destination)> settingsList, IFileService fileService, ILogService log)
    {
        foreach (var settings in settingsList)
        {
            FileExecuted fileExecuted = new FileExecuted()
            {
                Origem = settings.source,
                Destino = settings.destination
            };

            var files =
            Directory.GetFileSystemEntries(settings.source, "*", SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                fileExecuted = MoveFiles(settings.source, settings.destination, fileService);
                log.CreateLog(fileExecuted);
            }
            else
            {
                log.CreateLog(fileExecuted);
            }
        }
    }

    private FileExecuted MoveFiles(string source, string destination, IFileService fileService)
    {
        FileExecuted fileExecuted = new FileExecuted
        {
            DataInicial = DateTime.Now,
            Origem = source,
            Destino = destination,
            QTDOrigem = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories).Length,
            ArquivosMovidos = new List<Files>(),
            ArquivosNaoMovidos = new List<Files>()
        };

        DirectoryInfo sourceDir = new DirectoryInfo(source);

        // This recursive method will copy all files and directories
        CopyAll(sourceDir, destination, fileExecuted, fileService);
        fileExecuted.DataFinal = DateTime.Now;

        return fileExecuted;
    }

    private void CopyAll(DirectoryInfo source, string destination, FileExecuted fileExecuted, IFileService fileService)
    {
        destination = destination.Trim();
        Directory.CreateDirectory(destination);

        foreach (FileInfo file in source.GetFiles())
        {
            string destFile = Path.Combine(destination, file.Name);
            if (fileService.ExistFile(destFile))
            {
                fileService.Delete(destFile);
            }
            fileService.Copy(file.FullName, destFile);
            fileExecuted.ArquivosMovidos.Add(new Files { Nome = file.FullName });
        }

        foreach (DirectoryInfo subDir in source.GetDirectories())
        {
            string destDir = Path.Combine(destination, subDir.Name);
            CopyAll(subDir, destDir, fileExecuted, fileService);
        }
    }
}