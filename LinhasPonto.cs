using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hydra
{
    public class LinhasPonto
    {
        public int Id { get; set; }
        public int Parente { get; set; }
        public Point Ponto { get; set; }
        public Linhas Linha { get; set; }
        public LinhasPonto()
        {
        }

        public LinhasPonto(Linhas linhas)
        {
            this.Linha = linhas;
        }

        public LinhasPonto(Point point, Linhas linhas)
        {
            this.Ponto = point;
            this.Linha = linhas;
        }
        public LinhasPonto(int id, Point point, Linhas linhas)
        {
            this.Id = id;
            this.Ponto = point;
            this.Linha = linhas;
        }
        public LinhasPonto(int id, int parente, Point point, Linhas linhas)
        {
            this.Id = id;
            this.Parente = parente;
            this.Ponto = point;
            this.Linha = linhas;
        }

    }
}
