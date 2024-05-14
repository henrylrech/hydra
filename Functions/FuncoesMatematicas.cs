using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hydra.Functions
{
    class FuncoesMatematicas
    {
        static Func<float, float, float, float, float> calcularDistancia = (x1, y1, x2, y2) => (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        public static Point ObterInterseccaoMaisProxima(LinhasPonto linhaAtual, List<Linhas> linhas)
        {
            float menordist = 99999999;
            Point ponto = new Point();
            foreach (var linha in linhas)
            {
                var tuplaInt = EncontrarInterseccao(linhaAtual.Linha.Ponto1, linhaAtual.Linha.Ponto2, linha.Ponto1, linha.Ponto2);
                if (tuplaInt != null)
                {
                    float dist1 = calcularDistancia(linha.Ponto1.X, linha.Ponto1.Y, linhaAtual.Linha.Ponto1.X, linhaAtual.Linha.Ponto1.Y);
                    if (menordist > dist1)
                    {
                        menordist = dist1;
                        ponto.X = tuplaInt.Item1;
                        ponto.Y = tuplaInt.Item2;
                    }
                }
            }
            return ponto;
        }

        public static bool EstaDentroDaLinha(Linhas linha, Linhas linhaAtual)
        {
            Point p1 = ObterPontoMaisProximo(linha, linhaAtual.Ponto1);
            p1 = ChecaErroConversao(p1, linhaAtual.Ponto1);
            Point p2 = ObterPontoMaisProximo(linha, linhaAtual.Ponto2);
            p2 = ChecaErroConversao(p2, linhaAtual.Ponto2);
            if (p1 == linhaAtual.Ponto1 && p2 == linhaAtual.Ponto2)
            {
                return true;
            }
            return false;
        }

        public static List<Point> ObterInterseccoesDaLinhaFinal(LinhasPonto pontoMaisProximoFim, List<Linhas> linhas)
        {
            Linhas linhaFinal = pontoMaisProximoFim.Linha;
            List<Point> inter = new List<Point>();

            foreach (var linha in linhas)
            {
                var tuplaInt = EncontrarInterseccao(linhaFinal.Ponto1, linhaFinal.Ponto2, linha.Ponto1, linha.Ponto2);
                if (tuplaInt != null)
                {
                    Point pontoInt = new Point(tuplaInt.Item1, tuplaInt.Item2);
                    pontoInt = ChecaErroConversao(pontoInt, linhaFinal.Ponto2);
                    inter.Add(pontoInt);
                }
            }
            return inter;
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

            if (x1 == x3 && y1 == y3) 
            {
                return null;
            }

            bool segmento1Vertical = x1 == x2;
            bool segmento2Vertical = x3 == x4;

            if (segmento1Vertical && segmento2Vertical)
            {
                return null; 
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
                double m1 = (double)(y2 - y1) / (x2 - x1);
                double m2 = (double)(y4 - y3) / (x4 - x3);
                double b1 = y1 - m1 * x1;
                double b2 = y3 - m2 * x3;

                if (m1 == m2)
                {
                    return null;
                }

                double xInterseccaoDouble = (b2 - b1) / (m1 - m2);
                double yInterseccaoDouble = m1 * xInterseccaoDouble + b1;

                xInterseccao = (int)Math.Round(xInterseccaoDouble);
                yInterseccao = (int)Math.Round(yInterseccaoDouble);
            }

            if (VerificarDentroDosSegmentos(xInterseccao, yInterseccao, x1, y1, x2, y2, x3, y3, x4, y4))
            {
                return Tuple.Create(xInterseccao, yInterseccao);
            }

            return null; 
        }

        public static int CalcularYInterseccao(int xInterseccao, Point p1, Point p2)
        {
            double m = (double)(p2.Y - p1.Y) / (p2.X - p1.X);
            double b = p1.Y - m * p1.X;
            return (int)Math.Round(m * xInterseccao + b);
        }

        public static bool VerificarDentroDosSegmentos(int xInterseccao, int yInterseccao, int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            bool dentroSegmento1 = xInterseccao + 1 >= Math.Min(x1, x2) && xInterseccao - 1 <= Math.Max(x1, x2);
            bool dentroSegmento2 = xInterseccao + 1 >= Math.Min(x3, x4) && xInterseccao - 1 <= Math.Max(x3, x4);
            bool dentroSegmento3 = yInterseccao + 1 >= Math.Min(y1, y2) && yInterseccao - 1 <= Math.Max(y1, y2);
            bool dentroSegmento4 = yInterseccao + 1 >= Math.Min(y3, y4) && yInterseccao - 1 <= Math.Max(y3, y4);

            return dentroSegmento1 && dentroSegmento2 && dentroSegmento3 && dentroSegmento4;
        }

        public static Point ChecaErroConversao(Point ponto, Point ponto2)
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

        public static int Dot(Point p1, Point p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }

        public static float CalcularDistanciaCaminho(List<LinhasPonto> caminho)
        {
            if (caminho == null)
            {
                return 0;
            }

            float dist = 0;
            foreach (var rua in caminho)
            {
                dist += calcularDistancia(rua.Linha.Ponto1.X, rua.Linha.Ponto1.Y, rua.Linha.Ponto2.X, rua.Linha.Ponto2.Y);
            }
            return dist;
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

        public static LinhasPonto EncontrarPontoMaisProximo(int n, List<Point> pontos, List<Linhas> linhas)
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
    }
}
