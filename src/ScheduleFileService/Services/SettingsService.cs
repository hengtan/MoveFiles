using ScheduleFileService.Log;
using ScheduleFileService;
using ScheduleFileService.Log.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace ScheduleFileService.Services
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

        //string filePath = @"C:\ScheduleFile\configuration\settings.txt";
        ////    bool settingsFileExists = File.Exists(filePath);

        ////    if (settingsFileExists)
        ////    {
        ////        string content = File.ReadAllText(filePath);
        ////        if (string.IsNullOrEmpty(content))
        ////        {
        ////            using (StreamWriter sw = File.AppendText(filePath))
        ////            {
        ////                var origem = "ORIGEM: C:\\origem";
        ////                var destiny = "DESTINO: C:\\destino";

        ////                sw.WriteLine(origem);
        ////                sw.WriteLine(destiny);
        ////            }
        ////        }
        ////    }

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
                QTDOrigem = Directory.GetFiles(source).Length,
                ArquivosMovidos = new List<Files>(),
                ArquivosNaoMovidos = new List<Files>()
            };

            string[] files = _fileService.GetFiles(source);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destination, fileName);
                if (_fileService.ExistFile(destFile))
                {
                    _fileService.Delete(destFile);
                }
                _fileService.Move(file, destFile);
                fileExecuted.ArquivosMovidos.Add(new Files { Nome = file });
            }

            fileExecuted.DataFinal = DateTime.Now;

            return fileExecuted;
        }

    }
}

//string filePath = @"C:\ScheduleFile\configuration\settings.txt";
//string[] lines = File.ReadAllLines(filePath);
//string[] strings = new string[3];
//var startIndex = 0;



//if (Directory.GetFiles(strings[0]).Length == 0)
//{
//    log.CreateLog(fileExecuted);
//}
//else
//{


//}