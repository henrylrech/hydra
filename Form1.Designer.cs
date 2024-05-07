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
            this.btnLinhas = new System.Windows.Forms.Button();
            this.btnPontos = new System.Windows.Forms.Button();
            this.btnProcessar = new System.Windows.Forms.Button();
            this.panelMap = new System.Windows.Forms.Panel();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLinhas
            // 
            this.btnLinhas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLinhas.Location = new System.Drawing.Point(619, 12);
            this.btnLinhas.Name = "btnLinhas";
            this.btnLinhas.Size = new System.Drawing.Size(169, 37);
            this.btnLinhas.TabIndex = 0;
            this.btnLinhas.Text = "colocar linhas";
            this.btnLinhas.UseVisualStyleBackColor = true;
            this.btnLinhas.Click += new System.EventHandler(this.btnLinhas_Click);
            // 
            // btnPontos
            // 
            this.btnPontos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPontos.Location = new System.Drawing.Point(619, 55);
            this.btnPontos.Name = "btnPontos";
            this.btnPontos.Size = new System.Drawing.Size(169, 37);
            this.btnPontos.TabIndex = 1;
            this.btnPontos.Text = "marcar pontos";
            this.btnPontos.UseVisualStyleBackColor = true;
            this.btnPontos.Click += new System.EventHandler(this.btnPontos_Click);
            // 
            // btnProcessar
            // 
            this.btnProcessar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProcessar.Location = new System.Drawing.Point(619, 98);
            this.btnProcessar.Name = "btnProcessar";
            this.btnProcessar.Size = new System.Drawing.Size(169, 37);
            this.btnProcessar.TabIndex = 2;
            this.btnProcessar.Text = "processar";
            this.btnProcessar.UseVisualStyleBackColor = true;
            this.btnProcessar.Click += new System.EventHandler(this.btnProcessar_Click);
            // 
            // panelMap
            // 
            this.panelMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMap.Location = new System.Drawing.Point(12, 12);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(601, 529);
            this.panelMap.TabIndex = 3;
            this.panelMap.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMap_Paint);
            this.panelMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseDown);
            this.panelMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseUp);
            // 
            // btnLimpar
            // 
            this.btnLimpar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLimpar.Location = new System.Drawing.Point(619, 504);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(169, 37);
            this.btnLimpar.TabIndex = 4;
            this.btnLimpar.Text = "limpar";
            this.btnLimpar.UseVisualStyleBackColor = true;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 553);
            this.Controls.Add(this.btnLimpar);
            this.Controls.Add(this.panelMap);
            this.Controls.Add(this.btnProcessar);
            this.Controls.Add(this.btnPontos);
            this.Controls.Add(this.btnLinhas);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.Text = "hydra";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLinhas;
        private System.Windows.Forms.Button btnPontos;
        private System.Windows.Forms.Button btnProcessar;
        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.Button btnLimpar;
    }
}

