using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hydra
{
    public class Caminhos
    {
        public List<LinhasPonto> Caminho { get; set; }
        public double Distancia { get; set; }
        public Caminhos ()
        {

        }
        public Caminhos (List<LinhasPonto> caminho, double distancia)
        {
            this.Caminho = caminho;
            this.Distancia = distancia;
        }
    }
}
