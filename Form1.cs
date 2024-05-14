using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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

        Func<float, float, float, float, float> calcularDistancia = (x1, y1, x2, y2) => (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
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
            indexPonto = 0;
            listBoxCaminhos.Items.Clear();
            todosCaminhos.Clear();
            bpontos = false;
            blinhas = false;
            bprocessando = true;
            linhapontoid = 1;
            for (int i = 0; i < pontos.Count - 1; i++)
            {
                indexPonto = i;
                linhapontoid = 0;
                linhasVisitadas.Clear();
                interseccoesEncontradas.Clear();
                List<LinhasPonto> caminho = CalcularRota();
                if (caminho != null)
                {
                    float distancia = Functions.FuncoesMatematicas.CalcularDistanciaCaminho(caminho);
                    labelDistancia.Text = "Distancia: " + distancia.ToString();
                    todosCaminhos.Add(new Caminhos(caminho, distancia));
                    listBoxCaminhos.Items.Add(todosCaminhos.Count);
                }
            }
            if (todosCaminhos.Count > 0)
            {
                percorreTodos = true;
                float distanciatotal = 0;
                foreach (var caminho in todosCaminhos)
                {
                    distanciatotal += caminho.Distancia;
                }
                labelDistanciaTotal.Text = "Distancia Total: " + distanciatotal.ToString();
                PercorreCaminho();
                panelMap.Invalidate();
            }
            else
            {
                bprocessando = false;
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
                if (mousePos1 != mousePos2)
                {
                    linhas.Add(new Linhas(mousePos1, mousePos2));
                }
            }
            else if (bpontos == true)
            {
                pontos.Add(mousePos2);
            }
            panelMap.Invalidate();
        }

        private void panelMap_MouseMove(object sender, MouseEventArgs e)
        {
            labelMPos.Text = "MousePos: " + e.X + ", " + e.Y;
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
            graphics.DrawImage(pessoa, imgPositionX - (imgSize / 2), imgPositionY - (imgSize / 2), imgSize, imgSize);
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
            pontoMaisProximoInic = Functions.FuncoesMatematicas.EncontrarPontoMaisProximo(indexPonto, pontos, linhas);
            pontoMaisProximoFim = Functions.FuncoesMatematicas.EncontrarPontoMaisProximo(indexPonto + 1, pontos, linhas);
            double dist2pts = calcularDistancia(pontos[indexPonto].X, pontos[indexPonto].Y, pontos[indexPonto + 1].X, pontos[indexPonto + 1].Y);
            double distptlin = calcularDistancia(pontos[indexPonto].X, pontos[indexPonto].Y, pontoMaisProximoInic.Ponto.X, pontoMaisProximoInic.Ponto.Y);

            List<LinhasPonto> caminho = new List<LinhasPonto>();
            if (dist2pts < distptlin)
            {
                caminho.Add(new LinhasPonto(pontos[indexPonto + 1], new Linhas(pontos[indexPonto], pontos[indexPonto + 1])));
                panelMap.Invalidate();
                return caminho;
            }

            caminho = ResolveLabirinto(new LinhasPonto(pontoMaisProximoInic.Ponto, new Linhas(pontos[indexPonto], pontoMaisProximoInic.Ponto)));
            panelMap.Invalidate();
            return caminho;
        }

        private List<LinhasPonto> ResolveLabirinto(LinhasPonto lp)
        {
            List<LinhasPonto> percorridos = new List<LinhasPonto>();
            List<LinhasPonto> stack = new List<LinhasPonto>();
            List<LinhasPonto> todaslinhas = new List<LinhasPonto>();
            List<LinhasPonto> caminho = new List<LinhasPonto>();
            List<Point> interseccoesFinal = Functions.FuncoesMatematicas.ObterInterseccoesDaLinhaFinal(pontoMaisProximoFim, linhas);
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
                    if (percorridos[0].Linha.Ponto2 == pontoMaisProximoFim.Ponto && percorridos.Count == 1) //achou
                    {
                        todaslinhas.AddRange(percorridos);
                        achou = true;
                        bool fimDaRua = false;
                        foreach (var linha in linhas)
                        {
                            if (pontoMaisProximoFim.Ponto == linha.Ponto1 || pontoMaisProximoFim.Ponto == linha.Ponto2)
                            {
                                fimDaRua = true; // pontomaisproximofim esta no fim da rua             
                                break;
                            }
                        }

                        caminho.Add(percorridos[0]);

                        for (int i = 0; i < todaslinhas.Count; i++)
                        {
                            if (todaslinhas[i].Id == caminho.LastOrDefault().Parente)
                            {
                                caminho.Add(new LinhasPonto(todaslinhas[i].Id, todaslinhas[i].Parente, todaslinhas[i].Ponto, new Linhas(todaslinhas[i].Linha.Ponto1, caminho.LastOrDefault().Linha.Ponto1)));
                                i = 0;
                            }

                            if (caminho.LastOrDefault().Parente == 0)
                            {
                                break;
                            }
                        }

                        if (!fimDaRua)
                        {
                            caminho.RemoveAt(0);
                            caminho[0].Linha.Ponto2 = pontoMaisProximoFim.Ponto;
                        }

                        bool existe = false;
                        foreach (var linha in caminho)
                        {
                            if (linha.Linha.Ponto1.X == pontoMaisProximoFim.Ponto.X && linha.Linha.Ponto1.Y == pontoMaisProximoFim.Ponto.Y)
                            {
                                if (linha.Linha.Ponto2.X == pontos[indexPonto + 1].X && linha.Linha.Ponto2.Y == pontos[indexPonto + 1].Y)
                                {
                                    existe = true;
                                }
                            }
                        }

                        if (!existe)
                        {
                            caminho.Insert(0, new LinhasPonto(new Linhas(new Point(pontoMaisProximoFim.Ponto.X, pontoMaisProximoFim.Ponto.Y), new Point(pontos[indexPonto + 1].X, pontos[indexPonto + 1].Y))));
                        }

                        caminho.Add(new LinhasPonto(new Linhas(new Point(pontos[indexPonto].X, pontos[indexPonto].Y), new Point(pontoMaisProximoInic.Ponto.X, pontoMaisProximoInic.Ponto.Y))));
                        caminho.Reverse();
                        int j = caminho.Count;
                        for (int i = 0; i < j; i++)
                        {
                            if (caminho[i].Linha.Ponto1 == caminho[i].Linha.Ponto2)
                            {
                                caminho.RemoveAt(i);
                                i--;
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
            

            var teste = Functions.FuncoesMatematicas.ObterPontoMaisProximo(linhaAtual.Linha, pontoMaisProximoFim.Ponto);
            teste = Functions.FuncoesMatematicas.ChecaErroConversao(teste, pontoMaisProximoFim.Ponto);
            if (teste == pontoMaisProximoFim.Ponto)
            {
                interseccoes.Add(new LinhasPonto(linhapontoid, id, pontoMaisProximoFim.Ponto, new Linhas(linhaAtual.Linha.Ponto1, pontoMaisProximoFim.Ponto)));
                linhapontoid++;
                return interseccoes;
            }

            foreach (var linha in linhas)
            {
                if ((linhaAtual.Linha.Ponto1 != linha.Ponto1 || linhaAtual.Linha.Ponto2 != linha.Ponto2) && (!linhasVisitadas.Contains(linha) || (interseccoesEncontradas.Contains(linhaAtual.Linha.Ponto1) && linhaAtual.Linha.Ponto2 != linha.Ponto1)))
                {
                    var tuplaInt = Functions.FuncoesMatematicas.EncontrarInterseccao(linhaAtual.Linha.Ponto1, linhaAtual.Linha.Ponto2, linha.Ponto1, linha.Ponto2);
                    if (tuplaInt != null)
                    {
                        if (!interseccoesEncontradas.Contains(new Point(tuplaInt.Item1, tuplaInt.Item2)) && (!Functions.FuncoesMatematicas.EstaDentroDaLinha(linha, linhaAtual.Linha) || interseccoesEncontradas.Contains(linhaAtual.Linha.Ponto1)))
                        {
                            Point pontoInt = new Point(tuplaInt.Item1, tuplaInt.Item2);
                            pontoInt = Functions.FuncoesMatematicas.ChecaErroConversao(pontoInt, linhaAtual.Linha.Ponto2);
                            linhasVisitadas.Add(linha);

                            /*if (interseccoesFinal.Contains(pontoInt))
                            {
                                interseccoes.Add(new LinhasPonto(linhapontoid, id, pontoMaisProximoFim.Ponto, new Linhas(pontoInt, pontoMaisProximoFim.Ponto)));
                                linhapontoid++;
                                return interseccoes;
                            }*/

                            if (pontoInt != linha.Ponto1)
                            {
                                interseccoes.Add(new LinhasPonto(linhapontoid, id, linha.Ponto1, new Linhas(pontoInt, linha.Ponto1), linha));
                                linhapontoid++;
                            }
                            if (pontoInt != linha.Ponto2)
                            {
                                interseccoes.Add(new LinhasPonto(linhapontoid, id, linha.Ponto2, new Linhas(pontoInt, linha.Ponto2), linha));
                                linhapontoid++;
                            }

                            if (pontoInt != linhaAtual.Linha.Ponto2)
                            {
                                if (Functions.FuncoesMatematicas.ObterPontosDaLinha(pontoInt, linhaAtual.Linha.Ponto1).Contains(linhaAtual.Ponto))
                                {
                                    interseccoes.Add(new LinhasPonto(linhapontoid, id, linhaAtual.Linha.Ponto1, new Linhas(pontoInt, linhaAtual.Linha.Ponto1), linha));
                                    linhapontoid++;
                                }
                                else
                                {
                                    interseccoes.Add(new LinhasPonto(linhapontoid, id, linhaAtual.Linha.Ponto2, new Linhas(pontoInt, linhaAtual.Linha.Ponto2), linha));
                                    linhapontoid++;
                                }
                            }

                            if (interseccoes.Count > 3)
                            {
                                Point ponto = Functions.FuncoesMatematicas.ObterInterseccaoMaisProxima(linhaAtual, linhas);
                                ponto = Functions.FuncoesMatematicas.ChecaErroConversao(ponto, linhaAtual.Linha.Ponto2);
                                for (int i = 0; i < interseccoes.Count; i++)
                                {
                                    LinhasPonto interseccao = interseccoes[i];
                                    ponto = Functions.FuncoesMatematicas.ChecaErroConversao(ponto, interseccao.Linha.Ponto1);
                                    if (interseccao.Linha.Ponto1 != ponto)
                                    {
                                        linhasVisitadas.Remove(interseccao.LinhaPai);
                                        interseccoes.Remove(interseccao);
                                        try { interseccoesEncontradas.Remove(interseccao.Linha.Ponto1); } catch { }
                                        i--;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return interseccoes;
        }

        private void listBoxCaminhos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxCaminhos.SelectedIndex != -1)
            {
                indexPonto = listBoxCaminhos.SelectedIndex;
                labelDistancia.Text = "Distancia: " + todosCaminhos[listBoxCaminhos.SelectedIndex].Distancia;
                percorreTodos = false;
                if (todosCaminhos.Count > 0)
                {
                    PercorreCaminho();
                }
                panelMap.Invalidate();
            }
        }

        private void PercorreCaminho()
        {
            if (percorreTodos)
            {
                indexPonto = 0;
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
                else if (percorreTodos)
                {
                    indiceCaminhoAtual++;
                    if (indiceCaminhoAtual < todosCaminhos.Count)
                    {
                        indexPonto = indiceCaminhoAtual;
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
                }
            }
        }
    }
}
