using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.IO;
using ScheduleFileService.Log.Interfaces;
using ScheduleFileService.Log;

namespace ScheduleFileService
{
    public partial class Service1 : ServiceBase
    {
        private readonly ILogService _log;
        private readonly IFileService _fileService;
        private readonly ISettingsService _settingsService;
        private const string logFolderName = @"C:\ScheduleFile\log";
        private const string settingsFolderName = @"C:\ScheduleFile\configuration";
        public Service1(ILogService log, IFileService fileService, ISettingsService settingsService)
        {
            _log = log;
            _fileService = fileService;
            _settingsService = settingsService;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            FlowToRun();
        }

        public void FlowToRun()
        {
            //_fileService.CreateFolders(logFolderName);
            //_fileService.CreateFolders(settingsFolderName);
            _fileService.SystemFolders();
            _settingsService.CreateInitialSettingsFile();
            _log.CreateInitialLogFile();
            _settingsService.InsertingSettings();
            _log.InitialMessage();
            ReadTheSettings();
        }

        private void ReadTheSettings()
        {
            _settingsService.ReadSettings();

            //if (_fileService.GetFiles(settings.Origem).Length == 0)
            //{
            //    _log.CreateLog(settings);
            //}
            //else
            //{
            //    settings.DataInicial = DateTime.Now;
            //    settings.QTDOrigem = _fileService.GetFiles(settings.Origem).Length;

            //    var movedFiles = new List<Files>();
            //    var notMovedFiles = new List<Files>();

            //    string[] files = _fileService.GetFiles(settings.Origem);

            //    foreach (string file in files)
            //    {
            //        string fileName = Path.GetFileName(file);
            //        string destFile = Path.Combine(settings.Destino, fileName);
            //        if (_fileService.ExistFile(destFile))
            //        {
            //            _fileService.Delete(destFile);
            //        }
            //        _fileService.Move(file, destFile);
            //        movedFiles.Add(new Files { Nome = file });
            //    }
            //    settings.DataFinal = DateTime.Now;

            //    settings.ArquivosMovidos = movedFiles;
            //    settings.ArquivosNaoMovidos = notMovedFiles;

            //    _log.CreateLog(settings);
            //}
        }

        //private static void CreateFolders()
        //{
        //    string projectFolderPath = @"C:\ScheduleFile";
        //    string logFolder = Path.Combine(projectFolderPath, "configuration");
        //    string configFolder = Path.Combine(projectFolderPath, "log");

        //    bool configFolderExists = Directory.Exists(logFolder);
        //    bool logFolderExists = Directory.Exists(configFolder);

        //    if (!configFolderExists)
        //    {
        //        Directory.CreateDirectory(logFolder);
        //    }

        //    if (!logFolderExists)
        //    {
        //        Directory.CreateDirectory(configFolder);
        //    }
        //}

        //private static void CreateFiles()
        //{
        //    string projectFolderPath = @"C:\ScheduleFile";
        //    string confiFilePath = Path.Combine(projectFolderPath, "configuration", "settings.txt");
        //    string logFilePath = Path.Combine(projectFolderPath, "log", "log.txt");

        //    bool configFileExists = File.Exists(logFilePath);
        //    bool logFileExists = File.Exists(confiFilePath);

        //    if (!configFileExists)
        //    {
        //        using (File.Create(confiFilePath)) { }
        //    }

        //    if (!logFileExists)
        //    {
        //        using (File.Create(logFilePath)) { }
        //    }
        //}

        //private static void CreateSetting()
        //{
        //    string filePath = @"C:\ScheduleFile\configuration\settings.txt";
        //    bool settingsFileExists = File.Exists(filePath);

        //    if (settingsFileExists)
        //    {
        //        string content = File.ReadAllText(filePath);
        //        if (string.IsNullOrEmpty(content))
        //        {
        //            using (StreamWriter sw = File.AppendText(filePath))
        //            {
        //                var origem = "ORIGEM: C:\\origem";
        //                var destiny = "DESTINO: C:\\destino";

        //                sw.WriteLine(origem);
        //                sw.WriteLine(destiny);
        //            }
        //        }
        //    }
        //}

        //private void ReadTheSettings()
        //{
        //    FileExecuted fileExecuted = new FileExecuted();
        //    string filePath = @"C:\ScheduleFile\configuration\settings.txt";
        //    string[] lines = File.ReadAllLines(filePath);
        //    string[] strings = new string[3];
        //    var startIndex = 0;

        //    startIndex = lines[0].IndexOf("ORIGEM: ") + "ORIGEM: ".Length;
        //    strings[0] = lines[0].Substring(0);
        //    startIndex = lines[0].IndexOf("DESTINO: ") + "DESTINO: ".Length;
        //    strings[1] = lines[1].Substring(0);

        //    if (Directory.GetFiles(strings[0]).Length == 0)
        //    {
        //        log.CreateLog(fileExecuted);
        //    }
        //    else
        //    {
        //        fileExecuted.DataInicial = DateTime.Now;
        //        fileExecuted.Origem = strings[0];
        //        fileExecuted.Destino = strings[1];
        //        fileExecuted.QTDOrigem = Directory.GetFiles(strings[0]).Length;


        //        var a = new List<Files>();
        //        var e = new List<Files>();

        //        string source = strings[0];
        //        string destination = strings[1];
        //        string[] files = Directory.GetFiles(source);

        //        foreach (string file in files)
        //        {
        //            string fileName = Path.GetFileName(file);
        //            string destFile = Path.Combine(destination, fileName);
        //            if (File.Exists(destFile))
        //            {
        //                File.Delete(destFile);
        //            }
        //            File.Move(file, destFile);
        //            a.Add(new Files { Nome = file });
        //        }
        //        fileExecuted.DataFinal = DateTime.Now;

        //        fileExecuted.ArquivosMovidos = a;
        //        fileExecuted.ArquivosNaoMovidos = e;

        //        log.CreateLog(fileExecuted);
        //    }
        //}

        protected override void OnStop()
        {
        }
    }
}
