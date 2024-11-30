using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TarefasAPI.Models
{
    public class Tarefas
    {
        public int idTarefa { get; set; }
        public string nomeTarefa { get; set; }
        public DateTime dataConclusaoTarefa { get; set; }
        public string descricaoTarefa { get; set; }
        public int IdUsuario { get; set; }
        public int IdCategoria { get; set; }
    }
}