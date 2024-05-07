using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace hydra
{
    public partial class Form1 : Form
    {
        bool blinhas;
        bool bpontos;
        bool bprocessando;
        List<Linhas> linhas = new List<Linhas>();
        List<Point> pontos = new List<Point>();
        List<Point> progresso = new List<Point>();
        Point mousePos1;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLinhas_Click(object sender, EventArgs e)
        {
            bpontos = false;
            blinhas = true;
        }

        private void btnPontos_Click(object sender, EventArgs e)
        {
            bpontos = true;
            blinhas = false;
        }

        private void btnProcessar_Click(object sender, EventArgs e)
        {
            bpontos = false;
            blinhas = false;
            bprocessando = true;
            CalcularRota();
        }

        private void panelMap_MouseDown(object sender, MouseEventArgs e)
        {
            mousePos1 = new Point(e.X, e.Y);
        }

        private void panelMap_MouseUp(object sender, MouseEventArgs e)
        {
            Point mousePos2 = new Point(e.X, e.Y);
            if (blinhas == true)
            {
                linhas.Add(new Linhas(mousePos1, mousePos2));
            }
            else if (bpontos == true)
            {
                if (pontos.Count < 2)
                {
                    pontos.Add(mousePos2);
                }
            }
            panelMap.Invalidate();
        }

        private void panelMap_Paint(object sender, PaintEventArgs e)
        {
            DesenhaProgresso(e.Graphics);
            DesenhaLinhas(e.Graphics);
            if (pontos.Count == 2)
            {
                DesenhaPontos(e.Graphics);
            }
        }

        private void DesenhaLinhas(Graphics graphics)
        {
            Pen pen = new Pen(Color.Black, 1);
            
            
            foreach (var linha in linhas)
            {
                graphics.DrawLine(pen, linha.Ponto1.X, linha.Ponto1.Y, linha.Ponto2.X, linha.Ponto2.Y);
            }
            


            if (bprocessando)
            {
                Point pontoMaisProximo = EncontrarPontoMaisProximo();
                graphics.DrawLine(pen, pontos[0].X, pontos[0].Y, pontoMaisProximo.X, pontoMaisProximo.Y);
            }
        }
        
        private void DesenhaPontos(Graphics graphics)
        {
            graphics.DrawEllipse(new Pen(Color.Green, 1), new Rectangle(pontos[0].X - 1, pontos[0].Y - 1, 2, 2));
            graphics.DrawEllipse(new Pen(Color.Red, 1), new Rectangle(pontos[1].X - 1, pontos[1].Y - 1, 2, 2));
        }

        private void DesenhaProgresso(Graphics graphics)
        {
            foreach (var ponto in progresso)
            {
                graphics.DrawRectangle(new Pen(Color.Blue, 1), ponto.X, ponto.Y, 1, 1);
            }
            
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            bprocessando = false;
            linhas.Clear();
            pontos.Clear();
            panelMap.Invalidate();
        }

        private void CalcularRota()
        {
            /*Point pontoInicial = pontos[0];
            for(pontoInicial.Y = pontoInicial.Y-1; pontoInicial.Y >= 0; pontoInicial.Y--)
            {
                progresso.Add(pontoInicial);
                panelMap.Invalidate();
            }*/
            panelMap.Invalidate();
        }

        public Point EncontrarPontoMaisProximo()
        {
            Point temp = new Point();
            
            List<Point> listaDePontos = ObterPontosDaLinha(linhas[0].Ponto1, linhas[0].Ponto2);
            
            
            /*foreach (var linha in linhas)
            {
                temp.X = linha.Ponto1.X;
                temp.Y = linha.Ponto1.Y;
                listaDePontos.Add(temp);
                temp.X = linha.Ponto2.X;
                temp.Y = linha.Ponto2.Y;
                listaDePontos.Add(temp);
            }*/
            Point pontoInicial = pontos[0];
            return listaDePontos
            .OrderBy(p => Math.Sqrt(Math.Pow(p.X - pontoInicial.X, 2) + Math.Pow(p.Y - pontoInicial.Y, 2)))
            .FirstOrDefault();
        }
        public static List<Point> ObterPontosDaLinha(Point inicio, Point fim)
        {
            List<Point> pontos = new List<Point>();

            int x = inicio.X;
            int y = inicio.Y;
            int dx = fim.X - inicio.X;
            int dy = fim.Y - inicio.Y;

            int ix = dx > 0 ? 1 : -1;
            int iy = dy > 0 ? 1 : -1;
            dx = Math.Abs(dx);
            dy = Math.Abs(dy);

            pontos.Add(new Point(x, y));

            if (dx >= dy)
            {
                int erro = dx / 2;
                while (x != fim.X)
                {
                    x += ix;
                    erro -= dy;
                    if (erro < 0)
                    {
                        y += iy;
                        erro += dx;
                    }
                    pontos.Add(new Point(x, y));
                }
            }
            else
            {
                int erro = dy / 2;
                while (y != fim.Y)
                {
                    y += iy;
                    erro -= dx;
                    if (erro < 0)
                    {
                        x += ix;
                        erro += dy;
                    }
                    pontos.Add(new Point(x, y));
                }
            }

            return pontos;
        }

        private bool MovimentoValido(Point point)
        {


            return false;
        }
    }
}
