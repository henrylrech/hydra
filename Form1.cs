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
        List<Caminhos> todosCaminhos = new List<Caminhos>();

        Point mousePos1;

        LinhasPonto pontoMaisProximoInic;
        LinhasPonto pontoMaisProximoFim;

        List<Linhas> linhasVisitadas = new List<Linhas>();
        List<Point> interseccoesEncontradas = new List<Point>();
        int linhapontoid;
        int indexPonto = 0;

        private float imgPositionX = 0;
        private float imgPositionY = 0;
        private int imgSpeed = 16;
        private int imgSize = 20;
        private Image pessoa;
        private LinhasPonto linhaAtual;
        private float passoX;
        private float passoY;
        private int indiceLinhaAtual = 0;
        private int indiceCaminhoAtual = 0;
        private bool percorreTodos = false;

        Func<double, double, double, double, double> calcularDistancia = (x1, y1, x2, y2) => Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        public Form1()
        {
            InitializeComponent();
            pessoa = Properties.Resources.pessoa;
            timerAnimacao.Enabled = false;
        }

        private void btnLinhas_Click(object sender, EventArgs e)
        {
            bpontos = false;
            blinhas = true;
            bprocessando = false;
            panelMap.Invalidate();
        }

        private void btnPontos_Click(object sender, EventArgs e)
        {
            bpontos = true;
            blinhas = false;
        }

        private void btnProcessar_Click(object sender, EventArgs e)
        {
            listBoxCaminhos.Items.Clear();
            todosCaminhos.Clear();
            bpontos = false;
            blinhas = false;
            bprocessando = true;
            linhapontoid = 1;
            for (int i=0; i<pontos.Count-1; i++)
            {
                indexPonto = i;
                linhapontoid = 0;
                linhasVisitadas.Clear();
                interseccoesEncontradas.Clear();
                List<LinhasPonto> caminho = CalcularRota();
                var distancia = CalcularDistanciaCaminho(caminho);
                labelDistancia.Text = distancia.ToString();
                todosCaminhos.Add(new Caminhos(caminho, distancia));
                listBoxCaminhos.Items.Add(todosCaminhos.Count);
            }
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
                pontos.Add(mousePos2);
            }
            panelMap.Invalidate();
        }

        private void panelMap_Paint(object sender, PaintEventArgs e)
        {
            DesenhaLinhas(e.Graphics);
            DesenhaProgresso(e.Graphics);
            if (pontos.Count >= 2)
            {
                DesenhaPontos(e.Graphics);
            }
            DesenhaImagem(e.Graphics);
        }
        private void DesenhaImagem(Graphics graphics)
        {
            graphics.DrawImage(pessoa, imgPositionX - (imgSize/2), imgPositionY - (imgSize / 2), imgSize, imgSize);
        }
        private void DesenhaLinhas(Graphics graphics)
        {
            Pen pen = new Pen(Color.Black, 1);
            Pen pen2 = new Pen(Color.LightGray, 1);

            if (!bprocessando)
            {
                foreach (var linha in linhas)
                {
                    graphics.DrawLine(pen, linha.Ponto1.X, linha.Ponto1.Y, linha.Ponto2.X, linha.Ponto2.Y);
                }
            }
            else
            {
                foreach (var linha in linhas)
                {
                    graphics.DrawLine(pen2, linha.Ponto1.X, linha.Ponto1.Y, linha.Ponto2.X, linha.Ponto2.Y);
                }
            }
        }

        private void DesenhaPontos(Graphics graphics)
        {

            for (int i = 0; i < pontos.Count; i++)
            {
                graphics.DrawEllipse(new Pen(Color.Gray, 2), new Rectangle(pontos[i].X - 1, pontos[i].Y - 1, 2, 2));
            }
            graphics.DrawEllipse(new Pen(Color.Green, 2), new Rectangle(pontos[indexPonto].X - 1, pontos[indexPonto].Y - 1, 2, 2));
            graphics.DrawEllipse(new Pen(Color.Red, 2), new Rectangle(pontos[indexPonto + 1].X - 1, pontos[indexPonto + 1].Y - 1, 2, 2));

        }

        private void DesenhaProgresso(Graphics graphics)
        {
            Pen pen = new Pen(Color.Black, 1);
            if (bprocessando)
            {
                foreach (var linha in todosCaminhos[indexPonto].Caminho)
                {
                    graphics.DrawLine(pen, linha.Linha.Ponto1.X, linha.Linha.Ponto1.Y, linha.Linha.Ponto2.X, linha.Linha.Ponto2.Y);
                }
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            indexPonto = 0;
            todosCaminhos.Clear();
            bprocessando = false;
            linhasVisitadas.Clear();
            listBoxCaminhos.Items.Clear();
            interseccoesEncontradas.Clear();
            linhas.Clear();
            pontos.Clear();
            panelMap.Invalidate();
        }

        private List<LinhasPonto> CalcularRota()
        {
            pontoMaisProximoInic = EncontrarPontoMaisProximo(indexPonto);
            pontoMaisProximoFim = EncontrarPontoMaisProximo(indexPonto+1);

            var caminho = ResolveLabirinto(new LinhasPonto(pontoMaisProximoInic.Ponto, new Linhas(pontos[indexPonto], pontoMaisProximoInic.Ponto)));

            panelMap.Invalidate();
            return caminho;
        }

        public LinhasPonto EncontrarPontoMaisProximo(int n)
        {
            Point ponto = pontos[n];
            double distanciamenor = 9999999;
            LinhasPonto result = new LinhasPonto();
            foreach (var linha in linhas)
            {
                Point pontoMaisProximo = ObterPontoMaisProximo(linha, ponto);
                if (calcularDistancia(ponto.X, ponto.Y, pontoMaisProximo.X, pontoMaisProximo.Y) < distanciamenor)
                {
                    distanciamenor = calcularDistancia(ponto.X, ponto.Y, pontoMaisProximo.X, pontoMaisProximo.Y);
                    result.Ponto = pontoMaisProximo;
                    result.Linha = linha;
                }
            }
            return result;
        }

        public static Point ObterPontoMaisProximo(Linhas linha, Point ponto)
        {
            var pontoA = linha.Ponto1;
            var pontoB = linha.Ponto2;

            var vetorAB = new Point(pontoB.X - pontoA.X, pontoB.Y - pontoA.Y);
            var vetorAP = new Point(ponto.X - pontoA.X, ponto.Y - pontoA.Y);

            var abDotab = Dot(vetorAB, vetorAB);
            var abDotap = Dot(vetorAB, vetorAP);

            // Evita a divisão para manter os valores como inteiros
            var tNumerator = abDotap;
            var tDenominator = abDotab;

            // Compara diretamente os numeradores para evitar conversão para float
            if (tNumerator < 0)
            {
                tNumerator = 0;
            }
            else if (tNumerator > tDenominator)
            {
                tNumerator = tDenominator;
            }

            // Calcula o ponto mais próximo usando apenas operações de inteiros
            return new Point(pontoA.X + tNumerator * vetorAB.X / tDenominator,
            pontoA.Y + tNumerator * vetorAB.Y / tDenominator);
        }

        private static int Dot(Point p1, Point p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
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

        public static Tuple<int, int> EncontrarInterseccao(Point p1, Point p2, Point p3, Point p4)
        {
            int x1 = p1.X;
            int y1 = p1.Y;
            int x2 = p2.X;
            int y2 = p2.Y;
            int x3 = p3.X;
            int y3 = p3.Y;
            int x4 = p4.X;
            int y4 = p4.Y;

            if (x1 == x3 && y1 == y3) // Pontos iniciais iguais não pode
            {
                return null;
            }

            // Verifica se algum dos segmentos é vertical
            bool segmento1Vertical = x1 == x2;
            bool segmento2Vertical = x3 == x4;

            if (segmento1Vertical && segmento2Vertical)
            {
                return null; // Ambos os segmentos são verticais e paralelos
            }

            int xInterseccao;
            int yInterseccao;

            if (segmento1Vertical)
            {
                xInterseccao = x1;
                yInterseccao = CalcularYInterseccao(xInterseccao, p3, p4);
            }
            else if (segmento2Vertical)
            {
                xInterseccao = x3;
                yInterseccao = CalcularYInterseccao(xInterseccao, p1, p2);
            }
            else
            {
                // Calcula as inclinações e interceptações dos segmentos não verticais
                double m1 = (double)(y2 - y1) / (x2 - x1);
                double m2 = (double)(y4 - y3) / (x4 - x3);
                double b1 = y1 - m1 * x1;
                double b2 = y3 - m2 * x3;

                // Verifica se as linhas são paralelas
                if (m1 == m2)
                {
                    if (p2 == p3)
                    {
                        return Tuple.Create(p2.X, p2.Y);
                    }
                    return null; // As linhas são paralelas e não se cruzam
                }

                // Encontra o ponto de intersecção das linhas infinitas
                double xInterseccaoDouble = (b2 - b1) / (m1 - m2);
                double yInterseccaoDouble = m1 * xInterseccaoDouble + b1;

                xInterseccao = (int)Math.Round(xInterseccaoDouble);
                yInterseccao = (int)Math.Round(yInterseccaoDouble);
            }

            // Verifica se o ponto de intersecção está dentro dos segmentos de linha
            if (VerificarDentroDosSegmentos(xInterseccao, yInterseccao, x1, y1, x2, y2, x3, y3, x4, y4))
            {
                return Tuple.Create(xInterseccao, yInterseccao);
            }

            return null; // O ponto de intersecção não está dentro dos segmentos
        }

        private static int CalcularYInterseccao(int xInterseccao, Point p1, Point p2)
        {
            double m = (double)(p2.Y - p1.Y) / (p2.X - p1.X);
            double b = p1.Y - m * p1.X;
            return (int)Math.Round(m * xInterseccao + b);
        }

        private static bool VerificarDentroDosSegmentos(int xInterseccao, int yInterseccao, int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            bool dentroSegmento1 = xInterseccao + 1 >= Math.Min(x1, x2) && xInterseccao - 1 <= Math.Max(x1, x2);
            bool dentroSegmento2 = xInterseccao + 1 >= Math.Min(x3, x4) && xInterseccao - 1 <= Math.Max(x3, x4);
            bool dentroSegmento3 = yInterseccao + 1 >= Math.Min(y1, y2) && yInterseccao - 1 <= Math.Max(y1, y2);
            bool dentroSegmento4 = yInterseccao + 1 >= Math.Min(y3, y4) && yInterseccao - 1 <= Math.Max(y3, y4);

            return dentroSegmento1 && dentroSegmento2 && dentroSegmento3 && dentroSegmento4;
        }

        private List<LinhasPonto> ResolveLabirinto(LinhasPonto lp)
        {
            List<LinhasPonto> percorridos = new List<LinhasPonto>();
            List<LinhasPonto> stack = new List<LinhasPonto>();
            List<LinhasPonto> todaslinhas = new List<LinhasPonto>();
            List<LinhasPonto> caminho = new List<LinhasPonto>();
            bool achou = false;
            stack.Add(new LinhasPonto(linhapontoid, lp.Ponto, lp.Linha));
            todaslinhas.Add(stack[0]);
            linhapontoid++;
            if (lp.Ponto == pontoMaisProximoFim.Ponto)
            {
                caminho.Add(new LinhasPonto(new Linhas(new Point(pontos[indexPonto + 1].X, pontos[indexPonto + 1].Y), new Point(pontoMaisProximoFim.Ponto.X, pontoMaisProximoFim.Ponto.Y))));
                caminho.Add(new LinhasPonto(new Linhas(new Point(pontos[indexPonto].X, pontos[indexPonto].Y), new Point(pontoMaisProximoInic.Ponto.X, pontoMaisProximoInic.Ponto.Y))));
                return caminho;
            }

            while (!achou)
            {
                stack = stack.OrderBy(p => calcularDistancia(p.Ponto.X, p.Ponto.Y, pontoMaisProximoFim.Ponto.X, pontoMaisProximoFim.Ponto.Y)).ToList();

                percorridos = RetornaInterseccoes(stack[0].Id, stack[0]);

                if (percorridos.Count > 0)
                {
                    if (percorridos[0].Linha.Ponto2 == pontoMaisProximoFim.Ponto) //achou
                    {
                        todaslinhas.AddRange(percorridos);
                        achou = true;
                        bool fimDaRua = false;
                        foreach (var linha in linhas)
                        {
                            if (pontoMaisProximoFim.Ponto == linha.Ponto1 || pontoMaisProximoFim.Ponto == linha.Ponto2)
                            {
                                fimDaRua = true; // pontomaispertofim esta no fim da rua             
                                break;
                            }
                        }

                        caminho.Add(percorridos[0]);

                        for (int i = 0; i < todaslinhas.Count; i++)
                        {
                            if (todaslinhas[i].Id == caminho.LastOrDefault().Parente)
                            {
                                Linhas templin = new Linhas(todaslinhas[i].Linha.Ponto1, caminho.LastOrDefault().Linha.Ponto1);
                                caminho.Add(new LinhasPonto(todaslinhas[i].Id, todaslinhas[i].Parente, todaslinhas[i].Ponto, templin));
                                //caminho.Add(todaslinhas[i]);
                                i = 0;
                            }
                        }
                        if (!fimDaRua)
                        {
                            caminho.RemoveAt(0);
                            caminho[0].Linha.Ponto2 = pontoMaisProximoFim.Ponto;
                        }
                        
                        caminho.Insert(0, new LinhasPonto(new Linhas(new Point(pontoMaisProximoFim.Ponto.X, pontoMaisProximoFim.Ponto.Y), new Point(pontos[indexPonto + 1].X, pontos[indexPonto + 1].Y))));
                        caminho.Add(new LinhasPonto(new Linhas(new Point(pontos[indexPonto].X, pontos[indexPonto].Y), new Point(pontoMaisProximoInic.Ponto.X, pontoMaisProximoInic.Ponto.Y))));
                        caminho.Reverse();
                        int j = caminho.Count;
                        for (int i=0; i<j; i++)
                        {
                            if (caminho[i].Linha.Ponto1 == caminho[i].Linha.Ponto2)
                            {
                                caminho.RemoveAt(i);
                                j--;
                            }
                        }
                        return caminho;
                    }
                }

                todaslinhas.AddRange(percorridos);
                stack.AddRange(percorridos);

                stack.RemoveAt(0);
                if (stack.Count == 0) //nao achou
                {
                    MessageBox.Show("Não achou solução");
                    break;
                }
            }
            return null;
        }

        private List<LinhasPonto> RetornaInterseccoes(int id, LinhasPonto linhaAtual)
        {
            List<LinhasPonto> interseccoes = new List<LinhasPonto>();
            //List<Point> pontosLinhaAtual = ObterPontosDaLinha(linhaAtual.Linha.Ponto1, linhaAtual.Linha.Ponto2);
            var teste = ObterPontoMaisProximo(linhaAtual.Linha, pontoMaisProximoFim.Ponto);
            teste = ChecaErroConversao(teste, pontoMaisProximoFim.Ponto);
            if (teste == pontoMaisProximoFim.Ponto)
            {
                interseccoes.Add(new LinhasPonto(linhapontoid, id, pontoMaisProximoFim.Ponto, new Linhas(linhaAtual.Ponto, pontoMaisProximoFim.Ponto)));
                linhapontoid++;
                return interseccoes;
            }

            foreach (var linha in linhas)
            {
                if ((linhaAtual.Linha.Ponto1 != linha.Ponto1 || linhaAtual.Linha.Ponto2 != linhaAtual.Linha.Ponto2) && (!linhasVisitadas.Contains(linha)))
                {
                    var tuplaInt = EncontrarInterseccao(linhaAtual.Linha.Ponto1, linhaAtual.Linha.Ponto2, linha.Ponto1, linha.Ponto2);
                    if (tuplaInt != null)
                    {
                        if (!interseccoesEncontradas.Contains(new Point(tuplaInt.Item1, tuplaInt.Item2)))
                        {
                            Point pontoInt = new Point(tuplaInt.Item1, tuplaInt.Item2);
                            pontoInt = ChecaErroConversao(pontoInt, linhaAtual.Linha.Ponto2);
                            linhasVisitadas.Add(linha);

                            interseccoesEncontradas.Add(pontoInt);

                            if (pontoInt != linha.Ponto1)
                            {
                                interseccoes.Add(new LinhasPonto(linhapontoid, id, linha.Ponto1, new Linhas(pontoInt, linha.Ponto1)));
                                linhapontoid++;
                            }
                            if (pontoInt != linha.Ponto2)
                            {
                                interseccoes.Add(new LinhasPonto(linhapontoid, id, linha.Ponto2, new Linhas(pontoInt, linha.Ponto2)));
                                linhapontoid++;
                            }

                            if (pontoInt != linhaAtual.Linha.Ponto1 && pontoInt != linhaAtual.Linha.Ponto2)
                            {
                                if (ObterPontosDaLinha(pontoInt, linhaAtual.Linha.Ponto1).Contains(linhaAtual.Ponto))
                                {
                                    interseccoes.Add(new LinhasPonto(linhapontoid, id, linhaAtual.Linha.Ponto1, new Linhas(pontoInt, linhaAtual.Linha.Ponto1)));
                                    linhapontoid++;
                                }
                                else
                                {
                                    interseccoes.Add(new LinhasPonto(linhapontoid, id, linhaAtual.Linha.Ponto2, new Linhas(pontoInt, linhaAtual.Linha.Ponto2)));
                                    linhapontoid++;
                                }
                            }
                        }
                    }
                }
            }
            return interseccoes;
        }

        private Point ChecaErroConversao(Point ponto, Point ponto2)
        {
            if (ponto.X + 1 == ponto2.X || ponto.X - 1 == ponto2.X)
            {
                ponto.X = ponto2.X;
            }

            if (ponto.Y + 1 == ponto2.Y || ponto.Y - 1 == ponto2.Y)
            {
                ponto.Y = ponto2.Y;
            }
            return ponto;
        }

        private double CalcularDistanciaCaminho(List<LinhasPonto> caminho)
        {
            double dist = 0;
            foreach (var rua in caminho)
            {
                dist += calcularDistancia(rua.Linha.Ponto1.X, rua.Linha.Ponto1.Y, rua.Linha.Ponto2.X, rua.Linha.Ponto2.Y);
            }
            return dist;
        }

        private void listBoxCaminhos_SelectedIndexChanged(object sender, EventArgs e)
        {
            indexPonto = listBoxCaminhos.SelectedIndex;
            labelDistancia.Text = todosCaminhos[listBoxCaminhos.SelectedIndex].Distancia.ToString();
            PercorreCaminho();
            panelMap.Invalidate();
        }

        private void timerAnimacao_Tick(object sender, EventArgs e)
        {
            if (indiceLinhaAtual >= todosCaminhos[indiceCaminhoAtual].Caminho.Count)
            {
                timerAnimacao.Enabled = false;
                return;
            }

            imgPositionX += passoX;
            imgPositionY += passoY;
            panelMap.Invalidate();

            if (Math.Abs(imgPositionX - linhaAtual.Linha.Ponto2.X) <= Math.Abs(passoX) && Math.Abs(imgPositionY - linhaAtual.Linha.Ponto2.Y) <= Math.Abs(passoY))
            {
                timerAnimacao.Enabled = false;
                indiceLinhaAtual++;
                if (indiceLinhaAtual < todosCaminhos[indiceCaminhoAtual].Caminho.Count)
                {
                    linhaAtual = todosCaminhos[indiceCaminhoAtual].Caminho[indiceLinhaAtual];
                    imgPositionX = linhaAtual.Linha.Ponto1.X;
                    imgPositionY = linhaAtual.Linha.Ponto1.Y;
                    float distancia = (float)Math.Sqrt(Math.Pow(linhaAtual.Linha.Ponto2.X - linhaAtual.Linha.Ponto1.X, 2) + Math.Pow(linhaAtual.Linha.Ponto2.Y - linhaAtual.Linha.Ponto1.Y, 2));
                    passoX = ((linhaAtual.Linha.Ponto2.X - linhaAtual.Linha.Ponto1.X) / (distancia / imgSpeed));
                    passoY = ((linhaAtual.Linha.Ponto2.Y - linhaAtual.Linha.Ponto1.Y) / (distancia / imgSpeed));
                    timerAnimacao.Enabled = true;
                }
            }
        }

        private void PercorreCaminho()
        {
            if (percorreTodos)
            {
                indiceCaminhoAtual = 0;
            }
            else
            {
                indiceCaminhoAtual = indexPonto;
            }

            indiceLinhaAtual = 0;
            if (todosCaminhos[indiceCaminhoAtual].Caminho.Count > 0)
            {
                linhaAtual = todosCaminhos[indiceCaminhoAtual].Caminho[indiceLinhaAtual];
                
                imgPositionX = linhaAtual.Linha.Ponto1.X;
                imgPositionY = linhaAtual.Linha.Ponto1.Y;
                float distancia = (float)Math.Sqrt(Math.Pow(linhaAtual.Linha.Ponto2.X - linhaAtual.Linha.Ponto1.X, 2) + Math.Pow(linhaAtual.Linha.Ponto2.Y - linhaAtual.Linha.Ponto1.Y, 2));
                passoX = ((linhaAtual.Linha.Ponto2.X - linhaAtual.Linha.Ponto1.X) / (distancia / imgSpeed));
                passoY = ((linhaAtual.Linha.Ponto2.Y - linhaAtual.Linha.Ponto1.Y) / (distancia / imgSpeed));
                timerAnimacao.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PercorreCaminho();
        }
    }
}
