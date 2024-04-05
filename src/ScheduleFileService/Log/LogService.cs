//using Microsoft.Build.Framework.XamlTypes;
//using System;
//using System.IO;

//namespace ScheduleFileService.Log
//{
//    public class Log : ILog
//    {
//        //readonly string path = @"C:\ScheduleFile\log\log.txt";

//        //public void Dispose()
//        //{
//        //    throw new NotImplementedException();
//        //}

//        //public bool CheckFileExists()
//        //{
//        //    bool logPathExists = File.Exists(this.path);
//        //    return logPathExists;
//        //}

//        //public void InitialMessage()
//        //{
//        //    if (CheckFileExists())
//        //    {
//        //        using (StreamWriter sw = File.AppendText(path))
//        //        {
//        //            sw.WriteLine("------------------------------------------------");
//        //            sw.WriteLine("O serviço foi iniciado as: " + DateTime.Now);
//        //            sw.WriteLine("------------------------------------------------");
//        //        }
//        //    }
//        //}

//        //public void CreateLog(FileExecuted fileExecuted)
//        //{
//        //    using (StreamWriter sw = File.AppendText(path))
//        //    {
//        //        if (CheckFileExists() && fileExecuted.QTDOrigem > 0)
//        //        {
//        //            sw.WriteLine("Executando.. ");
//        //            //sw.WriteLine(DateTime.Now);
//        //            sw.WriteLine("Iniciado: " + fileExecuted.DataInicial);
//        //            sw.WriteLine("Finalizado: " + fileExecuted.DataFinal);
//        //            sw.WriteLine("Origem: " + fileExecuted.Origem);
//        //            sw.WriteLine("Destino: " + fileExecuted.Destino);
//        //            sw.WriteLine("Quantidade de itens movidos: " + fileExecuted.QTDOrigem);
//        //            sw.WriteLine("Lista dos itens movidos: ");
//        //            foreach (var item in fileExecuted.ArquivosMovidos)
//        //            {
//        //                sw.WriteLine("Nome: " + item.Nome);
//        //                if (item.Erro != null)
//        //                    sw.WriteLine("Erro: " + item.Erro);
//        //            }
//        //            if (fileExecuted.ArquivosNaoMovidos != null)
//        //            {
//        //                sw.WriteLine("Arquivos Não Movidos: ");
//        //                foreach (var item in fileExecuted.ArquivosNaoMovidos)
//        //                {
//        //                    sw.WriteLine("Nome: " + item.Nome);
//        //                    sw.WriteLine("Erro: " + item.Erro);
//        //                }
//        //            }
//        //            else
//        //            {
//        //                sw.WriteLine("Arquivos que não foram movidos: 0");
//        //            }
//        //            sw.WriteLine("\n");
//        //        }
//        //        else
//        //        {
//        //            sw.WriteLine("Nenhum arquivo encontrado para mover.");
//        //        }
//        //    }
//        //}
//        public void CreateLog(FileExecuted fileExecuted)
//        {
//            throw new NotImplementedException();
//        }

//        public void InitialMessage()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
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
            sw.WriteLine("List of moved items: ");
            WriteFileDetails(sw, fileExecuted.ArquivosMovidos, "Moved");
            WriteFileDetails(sw, fileExecuted.ArquivosNaoMovidos, "Not Moved");
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