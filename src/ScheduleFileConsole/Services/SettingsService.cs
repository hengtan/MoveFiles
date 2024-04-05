﻿
using ScheduleFileConsole.Log.Interfaces;
using ScheduleFileConsole.Log;


namespace ScheduleFileConsole.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly string _path = @"C:\ScheduleFile\configuration";
        private readonly IFileService _fileService;
        private readonly ILogService _log;

        public SettingsService()
        {
            _fileService = new FileService();
            _log = new LogService();
        }

        public void CreateInitialSettingsFile()
        {

            string configFilePath = Path.Combine(_path, "settings.txt");

            bool configFileExists = _fileService.ExistFile(configFilePath);

            if (!configFileExists)
            {
                using (File.Create(configFilePath))
                {}
            }

        }

        public void InsertingSettings()
        {

            string _pathCombined = Path.Combine(_path, "settings.txt");

            bool settingsFileExists = _fileService.ExistFile(_pathCombined);

            if (settingsFileExists)
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
            var settings = ReadSettingsFromFile();
            ProcessFiles(settings);            
        }

        private (string source, string destination) ReadSettingsFromFile()
        {
            var pathToRead = Path.Combine(_path, "settings.txt");
            string[] lines = File.ReadAllLines(pathToRead);
            string source = lines[0].Substring("ORIGEM: ".Length);
            string destination = lines[1].Substring("DESTINO: ".Length);
            return (source, destination);
        }

        private void ProcessFiles((string source, string destination) settings)
        {
            FileExecuted fileExecuted = new FileExecuted(){
                Origem = settings.source,
                Destino= settings.destination
            };           

            var teste = _fileService.GetFiles(settings.source);

            if(teste.Length > 0)
            {
                fileExecuted = MoveFiles(settings.source, settings.destination);
                _log.CreateLog(fileExecuted);
            }
            else
            {
                _log.CreateLog(fileExecuted);
            }            
        }

        private FileExecuted MoveFiles(string source, string destination)
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
            CopyAll(sourceDir, destination, fileExecuted);

            fileExecuted.DataFinal = DateTime.Now;

            return fileExecuted;
        }

        private void CopyAll(DirectoryInfo source, string destination, FileExecuted fileExecuted)
        {
            Directory.CreateDirectory(destination);

            foreach (FileInfo file in source.GetFiles())
            {
                string destFile = Path.Combine(destination, file.Name);
                if (_fileService.ExistFile(destFile))
                {
                    _fileService.Delete(destFile);
                }
                _fileService.Copy(file.FullName, destFile);
                fileExecuted.ArquivosMovidos.Add(new Files { Nome = file.FullName });
            }

            foreach (DirectoryInfo subDir in source.GetDirectories())
            {
                string destDir = Path.Combine(destination, subDir.Name);
                CopyAll(subDir, destDir, fileExecuted);
            }
        }
    }
}