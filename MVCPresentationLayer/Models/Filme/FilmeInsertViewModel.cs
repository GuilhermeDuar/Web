using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPresentationLayer.Models.Filme
{
    public class FilmeInsertViewModel
    {
        public string Nome { get; set; }
        public int Duracao { get; set; }
        public int AnoLancamento { get; set; }
        public int GeneroID { get; set; }
    }
}
