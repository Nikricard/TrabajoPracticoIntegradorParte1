namespace TrabajoPracticoIntegrador15_4
{
    partial class frmBitacora
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
            dtpDesde = new DateTimePicker();
            dtpHasta = new DateTimePicker();
            txtUsuario = new TextBox();
            txtActividad = new TextBox();
            cmbTipoEvento = new ComboBox();
            button1 = new Button();
            button2 = new Button();
            tabControl = new TabControl();
            tabBitacora = new TabPage();
            dgvBitacora = new DataGridView();
            tabUsuario = new TabPage();
            dgvAudUsuario = new DataGridView();
            tabIdioma = new TabPage();
            dgvAudIdioma = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            btnSalir = new Button();
            tabControl.SuspendLayout();
            tabBitacora.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBitacora).BeginInit();
            tabUsuario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAudUsuario).BeginInit();
            tabIdioma.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAudIdioma).BeginInit();
            SuspendLayout();
            // 
            // dtpDesde
            // 
            dtpDesde.Location = new Point(71, 36);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(200, 23);
            dtpDesde.TabIndex = 0;
            // 
            // dtpHasta
            // 
            dtpHasta.Location = new Point(71, 80);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(200, 23);
            dtpHasta.TabIndex = 1;
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(115, 173);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(100, 23);
            txtUsuario.TabIndex = 2;
            // 
            // txtActividad
            // 
            txtActividad.Location = new Point(115, 211);
            txtActividad.Name = "txtActividad";
            txtActividad.Size = new Size(100, 23);
            txtActividad.TabIndex = 3;
            // 
            // cmbTipoEvento
            // 
            cmbTipoEvento.FormattingEnabled = true;
            cmbTipoEvento.Location = new Point(115, 125);
            cmbTipoEvento.Name = "cmbTipoEvento";
            cmbTipoEvento.Size = new Size(121, 23);
            cmbTipoEvento.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(28, 252);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 5;
            button1.Tag = "btnBuscar";
            button1.Text = "Buscar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(115, 252);
            button2.Name = "button2";
            button2.Size = new Size(121, 23);
            button2.TabIndex = 6;
            button2.Tag = "btnLimpiar";
            button2.Text = "Limpiar Filtros";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabBitacora);
            tabControl.Controls.Add(tabUsuario);
            tabControl.Controls.Add(tabIdioma);
            tabControl.Location = new Point(322, 12);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(466, 401);
            tabControl.TabIndex = 7;
            // 
            // tabBitacora
            // 
            tabBitacora.Controls.Add(dgvBitacora);
            tabBitacora.Location = new Point(4, 24);
            tabBitacora.Name = "tabBitacora";
            tabBitacora.Padding = new Padding(3);
            tabBitacora.Size = new Size(458, 373);
            tabBitacora.TabIndex = 0;
            tabBitacora.Text = "Bitacora";
            tabBitacora.UseVisualStyleBackColor = true;
            // 
            // dgvBitacora
            // 
            dgvBitacora.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBitacora.Location = new Point(6, 6);
            dgvBitacora.Name = "dgvBitacora";
            dgvBitacora.Size = new Size(446, 361);
            dgvBitacora.TabIndex = 13;
            // 
            // tabUsuario
            // 
            tabUsuario.Controls.Add(dgvAudUsuario);
            tabUsuario.Location = new Point(4, 24);
            tabUsuario.Name = "tabUsuario";
            tabUsuario.Padding = new Padding(3);
            tabUsuario.Size = new Size(458, 373);
            tabUsuario.TabIndex = 1;
            tabUsuario.Text = "Auditoria Usuarios";
            tabUsuario.UseVisualStyleBackColor = true;
            // 
            // dgvAudUsuario
            // 
            dgvAudUsuario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAudUsuario.Location = new Point(3, 6);
            dgvAudUsuario.Name = "dgvAudUsuario";
            dgvAudUsuario.Size = new Size(446, 361);
            dgvAudUsuario.TabIndex = 0;
            // 
            // tabIdioma
            // 
            tabIdioma.Controls.Add(dgvAudIdioma);
            tabIdioma.Location = new Point(4, 24);
            tabIdioma.Name = "tabIdioma";
            tabIdioma.Size = new Size(458, 373);
            tabIdioma.TabIndex = 2;
            tabIdioma.Text = "Auditoria Idiomas";
            tabIdioma.UseVisualStyleBackColor = true;
            // 
            // dgvAudIdioma
            // 
            dgvAudIdioma.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAudIdioma.Location = new Point(6, 6);
            dgvAudIdioma.Name = "dgvAudIdioma";
            dgvAudIdioma.Size = new Size(449, 351);
            dgvAudIdioma.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(53, 181);
            label1.Name = "label1";
            label1.Size = new Size(47, 15);
            label1.TabIndex = 8;
            label1.Tag = "lblUsuario";
            label1.Text = "Usuario";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(43, 214);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 9;
            label2.Tag = "lblActividad";
            label2.Text = "Actividad";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 128);
            label3.Name = "label3";
            label3.Size = new Size(85, 15);
            label3.TabIndex = 10;
            label3.Tag = "lblTipoDeEvento";
            label3.Text = "Tipo de evento";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(26, 44);
            label4.Name = "label4";
            label4.Size = new Size(39, 15);
            label4.TabIndex = 11;
            label4.Tag = "lblDesde";
            label4.Text = "Desde";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(27, 86);
            label5.Name = "label5";
            label5.Size = new Size(37, 15);
            label5.TabIndex = 12;
            label5.Tag = "lblHasta";
            label5.Text = "Hasta";
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(71, 293);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(75, 23);
            btnSalir.TabIndex = 13;
            btnSalir.Tag = "btnSalir";
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // frmBitacora
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSalir);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(tabControl);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(cmbTipoEvento);
            Controls.Add(txtActividad);
            Controls.Add(txtUsuario);
            Controls.Add(dtpHasta);
            Controls.Add(dtpDesde);
            Name = "frmBitacora";
            Text = "frmBitacora";
            Load += frmBitacora_Load;
            tabControl.ResumeLayout(false);
            tabBitacora.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvBitacora).EndInit();
            tabUsuario.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAudUsuario).EndInit();
            tabIdioma.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAudIdioma).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dtpDesde;
        private DateTimePicker dtpHasta;
        private TextBox txtUsuario;
        private TextBox txtActividad;
        private ComboBox cmbTipoEvento;
        private Button button1;
        private Button button2;
        private TabControl tabControl;
        private TabPage tabBitacora;
        private DataGridView dgvBitacora;
        private TabPage tabUsuario;
        private TabPage tabIdioma;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private DataGridView dgvAudUsuario;
        private DataGridView dgvAudIdioma;
        private Button btnSalir;
    }
}