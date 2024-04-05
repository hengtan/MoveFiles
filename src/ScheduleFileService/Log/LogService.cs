using ScheduleFileService.Log.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace ScheduleFileService.Log
{
    public class LogService : ILogService
    {
        private readonly IFileService _fileService;
        private readonly string _path = @"C:\ScheduleFile\log\log.txt";

        public LogService()
        {
            _fileService = new FileService();
        }

        public void InitialMessage()
        {
            WriteToLog(sw =>
            {
                sw.WriteLine("------------------------------------------------");
                sw.WriteLine("The service started at: " + DateTime.Now);
                sw.WriteLine("------------------------------------------------");
            });
        }

        public void CreateLog(FileExecuted fileExecuted)
        {
            WriteToLog(sw =>
            {
                if (fileExecuted.QTDOrigem > 0)
                {
                    WriteExecutionDetails(sw, fileExecuted);
                }
                else
                {
                    sw.WriteLine("No files found to move.");
                }
            });
        }

        private void WriteToLog(Action<StreamWriter> writeAction)
        {
            using (StreamWriter sw = File.AppendText(_path))
            {
                writeAction(sw);
            }
        }

        private void WriteExecutionDetails(StreamWriter sw, FileExecuted fileExecuted)
        {
            //sw.WriteLine("Executing.. ");
            sw.WriteLine("Started: " + fileExecuted.DataInicial);
            sw.WriteLine("Finished: " + fileExecuted.DataFinal);
            sw.WriteLine("Source: " + fileExecuted.Origem);
            sw.WriteLine("Destination: " + fileExecuted.Destino);
            sw.WriteLine("Number of items moved: " + fileExecuted.QTDOrigem);            
            if(fileExecuted.ArquivosMovidos.Count > 0)
            {
                sw.WriteLine("List of moved items: ");
                WriteFileDetails(sw, fileExecuted.ArquivosMovidos, "Moved");
            }            
            if (fileExecuted.ArquivosNaoMovidos.Count > 0)
            {
                WriteFileDetails(sw, fileExecuted.ArquivosNaoMovidos, "Moved");
            }            
            sw.WriteLine("\n");
        }

        private void WriteFileDetails(StreamWriter sw, IEnumerable<Files> fileDetails, string status)
        {
            if (fileDetails != null)
            {
                sw.WriteLine($"Files {status}: ");
                foreach (var item in fileDetails)
                {
                    sw.WriteLine(">>> " + item.Nome);
                    if (item.Erro != null)
                        sw.WriteLine("Error: " + item.Erro);
                }
            }
            else
            {
                sw.WriteLine($"Files {status}: 0");
            }
        }

        public void CreateInitialLogFile()
        {
            bool logFileExists = _fileService.ExistFile(_path);
            if (!logFileExists)
            {
                using (File.Create(_path)) { }
            }

        }
    }
}