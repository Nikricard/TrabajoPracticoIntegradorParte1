namespace TrabajoPracticoIntegrador15_4
{
    partial class frmBitacora
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            btnRestaurar = new Button();
            btnSalir = new Button();
            button2 = new Button();
            button1 = new Button();
            cmbActividad = new ComboBox();
            cmbTipoEvento = new ComboBox();
            cmbUsuario = new ComboBox();
            dtpHasta = new DateTimePicker();
            dtpDesde = new DateTimePicker();
            lblActividad = new Label();
            lblUsuario = new Label();
            lblTipoDeEvento = new Label();
            lblHasta = new Label();
            lblDesde = new Label();
            tabControl = new TabControl();
            tabBitacora = new TabPage();
            dgvBitacora = new DataGridView();
            tabUsuario = new TabPage();
            dgvAudUsuario = new DataGridView();
            tabIdioma = new TabPage();
            dgvAudIdioma = new DataGridView();
            groupBox1.SuspendLayout();
            tabControl.SuspendLayout();
            tabBitacora.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBitacora).BeginInit();
            tabUsuario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAudUsuario).BeginInit();
            tabIdioma.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAudIdioma).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnRestaurar);
            groupBox1.Controls.Add(btnSalir);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(cmbActividad);
            groupBox1.Controls.Add(cmbTipoEvento);
            groupBox1.Controls.Add(cmbUsuario);
            groupBox1.Controls.Add(dtpHasta);
            groupBox1.Controls.Add(dtpDesde);
            groupBox1.Controls.Add(lblActividad);
            groupBox1.Controls.Add(lblUsuario);
            groupBox1.Controls.Add(lblTipoDeEvento);
            groupBox1.Controls.Add(lblHasta);
            groupBox1.Controls.Add(lblDesde);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(313, 385);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            // 
            // btnRestaurar
            // 
            btnRestaurar.Location = new Point(58, 294);
            btnRestaurar.Name = "btnRestaurar";
            btnRestaurar.Size = new Size(150, 28);
            btnRestaurar.TabIndex = 15;
            btnRestaurar.Tag = "btnRestaurar";
            btnRestaurar.Text = "Restaurar a este estado";
            btnRestaurar.UseVisualStyleBackColor = true;
            btnRestaurar.Click += btnRestaurar_Click;
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(93, 254);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(75, 23);
            btnSalir.TabIndex = 14;
            btnSalir.Tag = "btnSalir";
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // button2
            // 
            button2.Location = new Point(140, 212);
            button2.Name = "button2";
            button2.Size = new Size(103, 23);
            button2.TabIndex = 13;
            button2.Tag = "btnLimpiar";
            button2.Text = "Limpiar Filtros";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(45, 212);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 12;
            button1.Tag = "btnBuscar";
            button1.Text = "Buscar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // cmbActividad
            // 
            cmbActividad.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbActividad.FormattingEnabled = true;
            cmbActividad.Location = new Point(140, 158);
            cmbActividad.Name = "cmbActividad";
            cmbActividad.Size = new Size(150, 23);
            cmbActividad.TabIndex = 11;
            // 
            // cmbTipoEvento
            // 
            cmbTipoEvento.FormattingEnabled = true;
            cmbTipoEvento.Location = new Point(140, 111);
            cmbTipoEvento.Name = "cmbTipoEvento";
            cmbTipoEvento.Size = new Size(150, 23);
            cmbTipoEvento.TabIndex = 9;
            // 
            // cmbUsuario
            // 
            cmbUsuario.FormattingEnabled = true;
            cmbUsuario.Location = new Point(140, 134);
            cmbUsuario.Name = "cmbUsuario";
            cmbUsuario.Size = new Size(150, 23);
            cmbUsuario.TabIndex = 10;
            // 
            // dtpHasta
            // 
            dtpHasta.Location = new Point(58, 79);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(232, 23);
            dtpHasta.TabIndex = 6;
            // 
            // dtpDesde
            // 
            dtpDesde.Location = new Point(58, 46);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(232, 23);
            dtpDesde.TabIndex = 5;
            // 
            // lblActividad
            // 
            lblActividad.AutoSize = true;
            lblActividad.Location = new Point(70, 161);
            lblActividad.Name = "lblActividad";
            lblActividad.Size = new Size(57, 15);
            lblActividad.TabIndex = 4;
            lblActividad.Tag = "lblActividad";
            lblActividad.Text = "Actividad";
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(80, 137);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(47, 15);
            lblUsuario.TabIndex = 3;
            lblUsuario.Tag = "lblUsuario";
            lblUsuario.Text = "Usuario";
            // 
            // lblTipoDeEvento
            // 
            lblTipoDeEvento.AutoSize = true;
            lblTipoDeEvento.Location = new Point(35, 114);
            lblTipoDeEvento.Name = "lblTipoDeEvento";
            lblTipoDeEvento.Size = new Size(85, 15);
            lblTipoDeEvento.TabIndex = 2;
            lblTipoDeEvento.Tag = "lblTipoDeEvento";
            lblTipoDeEvento.Text = "Tipo de Evento";
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(16, 85);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(37, 15);
            lblHasta.TabIndex = 1;
            lblHasta.Tag = "lblHasta";
            lblHasta.Text = "Hasta";
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(16, 46);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(39, 15);
            lblDesde.TabIndex = 0;
            lblDesde.Tag = "lblDesde";
            lblDesde.Text = "Desde";
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabBitacora);
            tabControl.Controls.Add(tabUsuario);
            tabControl.Controls.Add(tabIdioma);
            tabControl.Location = new Point(331, 12);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(657, 385);
            tabControl.TabIndex = 2;
            tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
            // 
            // tabBitacora
            // 
            tabBitacora.Controls.Add(dgvBitacora);
            tabBitacora.Location = new Point(4, 24);
            tabBitacora.Name = "tabBitacora";
            tabBitacora.Padding = new Padding(3);
            tabBitacora.Size = new Size(649, 357);
            tabBitacora.TabIndex = 0;
            tabBitacora.Text = "Bitacora";
            tabBitacora.UseVisualStyleBackColor = true;
            // 
            // dgvBitacora
            // 
            dgvBitacora.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBitacora.Dock = DockStyle.Fill;
            dgvBitacora.Location = new Point(3, 3);
            dgvBitacora.Name = "dgvBitacora";
            dgvBitacora.Size = new Size(643, 351);
            dgvBitacora.TabIndex = 0;
            // 
            // tabUsuario
            // 
            tabUsuario.Controls.Add(dgvAudUsuario);
            tabUsuario.Location = new Point(4, 24);
            tabUsuario.Name = "tabUsuario";
            tabUsuario.Padding = new Padding(3);
            tabUsuario.Size = new Size(649, 357);
            tabUsuario.TabIndex = 1;
            tabUsuario.Text = "Auditoria Usuarios";
            tabUsuario.UseVisualStyleBackColor = true;
            // 
            // dgvAudUsuario
            // 
            dgvAudUsuario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAudUsuario.Dock = DockStyle.Fill;
            dgvAudUsuario.Location = new Point(3, 3);
            dgvAudUsuario.Name = "dgvAudUsuario";
            dgvAudUsuario.Size = new Size(643, 351);
            dgvAudUsuario.TabIndex = 0;
            // 
            // tabIdioma
            // 
            tabIdioma.Controls.Add(dgvAudIdioma);
            tabIdioma.Location = new Point(4, 24);
            tabIdioma.Name = "tabIdioma";
            tabIdioma.Padding = new Padding(3);
            tabIdioma.Size = new Size(649, 357);
            tabIdioma.TabIndex = 2;
            tabIdioma.Text = "Auditoria Idiomas";
            tabIdioma.UseVisualStyleBackColor = true;
            // 
            // dgvAudIdioma
            // 
            dgvAudIdioma.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAudIdioma.Dock = DockStyle.Fill;
            dgvAudIdioma.Location = new Point(3, 3);
            dgvAudIdioma.Name = "dgvAudIdioma";
            dgvAudIdioma.Size = new Size(643, 351);
            dgvAudIdioma.TabIndex = 0;
            // 
            // frmBitacora
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 409);
            Controls.Add(tabControl);
            Controls.Add(groupBox1);
            Name = "frmBitacora";
            Text = "frmBitacora";
            Load += frmBitacora_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabControl.ResumeLayout(false);
            tabBitacora.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvBitacora).EndInit();
            tabUsuario.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAudUsuario).EndInit();
            tabIdioma.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAudIdioma).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label lblDesde;
        private Label lblHasta;
        private Label lblTipoDeEvento;
        private Label lblUsuario;
        private Label lblActividad;
        private DateTimePicker dtpDesde;
        private DateTimePicker dtpHasta;
        private ComboBox cmbUsuario;
        private ComboBox cmbTipoEvento;
        private ComboBox cmbActividad;
        private Button button1;
        private Button button2;
        private Button btnSalir;
        private Button btnRestaurar;
        private TabControl tabControl;
        private TabPage tabBitacora;
        private DataGridView dgvBitacora;
        private TabPage tabUsuario;
        private DataGridView dgvAudUsuario;
        private TabPage tabIdioma;
        private DataGridView dgvAudIdioma;
    }
}
