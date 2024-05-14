using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hydra
{
    public class Linhas
    {
        public Point Ponto1 { get; set; }
        public Point Ponto2 { get; set; }
        public Linhas(Point ponto1, Point ponto2)
        {
            Ponto1 = ponto1;
            Ponto2 = ponto2;
        }
    }
}
