using ScheduleFileConsole.Log;
using ScheduleFileConsole.Log.Interfaces;
using ScheduleFileConsole.Services.Interfaces;

namespace ScheduleFileConsole.Services
{
    public class SettingsFileProcessor : ISettingsFileProcessor
    {
        private readonly IFileService _fileService;
        private readonly ILogService _logService;

        public SettingsFileProcessor(IFileService fileService, ILogService logService)
        {
            _fileService = fileService;
            _logService = logService;
        }

        public List<(string source, string destination)> ReadSettingsFromFile(string path)
        {
            var pathToRead = Path.Combine(path, "settings.txt");
            string[] lines = _fileService.ReadAllLines(pathToRead);

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
            Console.WriteLine("Processing files...");
            foreach (var settings in settingsList)
            {
                Console.WriteLine($"Processing files from {settings.source} to {settings.destination}...");

                FileExecuted fileExecuted = new FileExecuted()
                {
                    Origem = settings.source,
                    Destino = settings.destination,
                    ArquivosMovidos = new List<Files>(),  
                    ArquivosNaoMovidos = new List<Files>()  
                };
                
                var files = _fileService.GetFileSystemEntries(settings.source, "*", SearchOption.AllDirectories);

                Console.WriteLine($"Found {files.Length} files to process.");

                if (files.Length > 0)
                {
                    fileExecuted = MoveFiles(settings.source, settings.destination, _fileService);  
                    _logService.CreateLog(fileExecuted);
                }
                else
                {
                    _logService.CreateLog(fileExecuted);
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
                QTDOrigem = _fileService.GetFiles(source, "*.*", SearchOption.AllDirectories).Length,
                ArquivosMovidos = new List<Files>(),
                ArquivosNaoMovidos = new List<Files>()
            };

            DirectoryInfo sourceDir = new DirectoryInfo(source);

            // This recursive method will copy all files and directories
            CopyAll(sourceDir, destination, fileExecuted, fileService);  // Pass fileService here
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
                fileExecuted.ArquivosMovidos.Add(new Files { Nome = file.FullName });  // This should be safe now
            }

            foreach (DirectoryInfo subDir in source.GetDirectories())
            {
                string destDir = Path.Combine(destination, subDir.Name);
                CopyAll(subDir, destDir, fileExecuted, fileService);  // Pass fileService here
            }
        }
    }
}
