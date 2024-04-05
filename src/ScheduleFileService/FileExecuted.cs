using System;
using System.Collections.Generic;

namespace ScheduleFileService
{
    public class FileExecuted
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Origem { get; set; }
        public string Destino { get; set; }
        public int QTDOrigem { get; set; }
        public List<Files> ArquivosMovidos { get; set; }
        public List<Files> ArquivosNaoMovidos { get; set; }
    }

    public class Files
    {
        public string Nome { get; set; }
        public string Erro { get; set; }
    }
}
