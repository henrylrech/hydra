namespace hydra
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnLinhas = new System.Windows.Forms.Button();
            this.btnPontos = new System.Windows.Forms.Button();
            this.btnProcessar = new System.Windows.Forms.Button();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.labelDistancia = new System.Windows.Forms.Label();
            this.listBoxCaminhos = new System.Windows.Forms.ListBox();
            this.timerAnimacao = new System.Windows.Forms.Timer(this.components);
            this.labelMPos = new System.Windows.Forms.Label();
            this.panelMap = new hydra.PanelMap();
            this.labelDistanciaTotal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLinhas
            // 
            this.btnLinhas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLinhas.Location = new System.Drawing.Point(464, 10);
            this.btnLinhas.Margin = new System.Windows.Forms.Padding(2);
            this.btnLinhas.Name = "btnLinhas";
            this.btnLinhas.Size = new System.Drawing.Size(127, 30);
            this.btnLinhas.TabIndex = 0;
            this.btnLinhas.Text = "colocar linhas";
            this.btnLinhas.UseVisualStyleBackColor = true;
            this.btnLinhas.Click += new System.EventHandler(this.btnLinhas_Click);
            // 
            // btnPontos
            // 
            this.btnPontos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPontos.Location = new System.Drawing.Point(464, 45);
            this.btnPontos.Margin = new System.Windows.Forms.Padding(2);
            this.btnPontos.Name = "btnPontos";
            this.btnPontos.Size = new System.Drawing.Size(127, 30);
            this.btnPontos.TabIndex = 1;
            this.btnPontos.Text = "marcar pontos";
            this.btnPontos.UseVisualStyleBackColor = true;
            this.btnPontos.Click += new System.EventHandler(this.btnPontos_Click);
            // 
            // btnProcessar
            // 
            this.btnProcessar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProcessar.Location = new System.Drawing.Point(464, 80);
            this.btnProcessar.Margin = new System.Windows.Forms.Padding(2);
            this.btnProcessar.Name = "btnProcessar";
            this.btnProcessar.Size = new System.Drawing.Size(127, 30);
            this.btnProcessar.TabIndex = 2;
            this.btnProcessar.Text = "processar";
            this.btnProcessar.UseVisualStyleBackColor = true;
            this.btnProcessar.Click += new System.EventHandler(this.btnProcessar_Click);
            // 
            // btnLimpar
            // 
            this.btnLimpar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLimpar.Location = new System.Drawing.Point(464, 410);
            this.btnLimpar.Margin = new System.Windows.Forms.Padding(2);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(127, 30);
            this.btnLimpar.TabIndex = 4;
            this.btnLimpar.Text = "limpar";
            this.btnLimpar.UseVisualStyleBackColor = true;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // labelDistancia
            // 
            this.labelDistancia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDistancia.AutoSize = true;
            this.labelDistancia.Location = new System.Drawing.Point(461, 382);
            this.labelDistancia.Name = "labelDistancia";
            this.labelDistancia.Size = new System.Drawing.Size(57, 13);
            this.labelDistancia.TabIndex = 5;
            this.labelDistancia.Text = "Distância: ";
            // 
            // listBoxCaminhos
            // 
            this.listBoxCaminhos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxCaminhos.FormattingEnabled = true;
            this.listBoxCaminhos.Location = new System.Drawing.Point(464, 115);
            this.listBoxCaminhos.Name = "listBoxCaminhos";
            this.listBoxCaminhos.Size = new System.Drawing.Size(127, 82);
            this.listBoxCaminhos.TabIndex = 6;
            this.listBoxCaminhos.SelectedIndexChanged += new System.EventHandler(this.listBoxCaminhos_SelectedIndexChanged);
            // 
            // timerAnimacao
            // 
            this.timerAnimacao.Interval = 20;
            this.timerAnimacao.Tick += new System.EventHandler(this.timerAnimacao_Tick);
            // 
            // labelMPos
            // 
            this.labelMPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMPos.AutoSize = true;
            this.labelMPos.Location = new System.Drawing.Point(461, 200);
            this.labelMPos.Name = "labelMPos";
            this.labelMPos.Size = new System.Drawing.Size(63, 13);
            this.labelMPos.TabIndex = 8;
            this.labelMPos.Text = "MousePos: ";
            // 
            // panelMap
            // 
            this.panelMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMap.Location = new System.Drawing.Point(12, 10);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(443, 434);
            this.panelMap.TabIndex = 7;
            this.panelMap.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMap_Paint);
            this.panelMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseDown);
            this.panelMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseMove);
            this.panelMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseUp);
            // 
            // labelDistanciaTotal
            // 
            this.labelDistanciaTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDistanciaTotal.AutoSize = true;
            this.labelDistanciaTotal.Location = new System.Drawing.Point(461, 395);
            this.labelDistanciaTotal.Name = "labelDistanciaTotal";
            this.labelDistanciaTotal.Size = new System.Drawing.Size(84, 13);
            this.labelDistanciaTotal.TabIndex = 9;
            this.labelDistanciaTotal.Text = "Distância Total: ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 456);
            this.Controls.Add(this.labelDistanciaTotal);
            this.Controls.Add(this.labelMPos);
            this.Controls.Add(this.panelMap);
            this.Controls.Add(this.listBoxCaminhos);
            this.Controls.Add(this.labelDistancia);
            this.Controls.Add(this.btnLimpar);
            this.Controls.Add(this.btnProcessar);
            this.Controls.Add(this.btnPontos);
            this.Controls.Add(this.btnLinhas);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(604, 495);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "hydra";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLinhas;
        private System.Windows.Forms.Button btnPontos;
        private System.Windows.Forms.Button btnProcessar;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.Label labelDistancia;
        private System.Windows.Forms.ListBox listBoxCaminhos;
        private System.Windows.Forms.Timer timerAnimacao;
        private PanelMap panelMap;
        private System.Windows.Forms.Label labelMPos;
        private System.Windows.Forms.Label labelDistanciaTotal;
    }
}

