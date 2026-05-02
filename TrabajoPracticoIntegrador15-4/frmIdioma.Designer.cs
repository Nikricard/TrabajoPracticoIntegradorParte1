namespace TrabajoPracticoIntegrador15_4
{
    partial class frmIdioma
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
            dgvIdiomas = new DataGridView();
            txtValor = new TextBox();
            chkDefecto = new CheckBox();
            btnAgregar = new Button();
            btnEliminar = new Button();
            btnGuardar = new Button();
            dgvTraducciones = new DataGridView();
            txtNombre = new TextBox();
            txtClave = new TextBox();
            btnSalir = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvIdiomas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvTraducciones).BeginInit();
            SuspendLayout();
            // 
            // dgvIdiomas
            // 
            dgvIdiomas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvIdiomas.Location = new Point(336, 71);
            dgvIdiomas.Name = "dgvIdiomas";
            dgvIdiomas.Size = new Size(442, 150);
            dgvIdiomas.TabIndex = 0;
            dgvIdiomas.SelectionChanged += dgvIdiomas_SelectionChanged;
            // 
            // txtValor
            // 
            txtValor.Location = new Point(262, 380);
            txtValor.Name = "txtValor";
            txtValor.Size = new Size(100, 23);
            txtValor.TabIndex = 1;
            // 
            // chkDefecto
            // 
            chkDefecto.AutoSize = true;
            chkDefecto.Location = new Point(190, 110);
            chkDefecto.Name = "chkDefecto";
            chkDefecto.Size = new Size(87, 19);
            chkDefecto.TabIndex = 2;
            chkDefecto.Text = "Por defecto";
            chkDefecto.UseVisualStyleBackColor = true;
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(143, 198);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(75, 23);
            btnAgregar.TabIndex = 3;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = true;
            btnAgregar.Click += btnAgregar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(224, 198);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(75, 23);
            btnEliminar.TabIndex = 4;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(405, 460);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(75, 23);
            btnGuardar.TabIndex = 5;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // dgvTraducciones
            // 
            dgvTraducciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTraducciones.Location = new Point(405, 294);
            dgvTraducciones.Name = "dgvTraducciones";
            dgvTraducciones.Size = new Size(453, 150);
            dgvTraducciones.TabIndex = 6;
            dgvTraducciones.SelectionChanged += dgvTraducciones_SelectionChanged;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(190, 71);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(100, 23);
            txtNombre.TabIndex = 7;
            // 
            // txtClave
            // 
            txtClave.Location = new Point(262, 344);
            txtClave.Name = "txtClave";
            txtClave.Size = new Size(100, 23);
            txtClave.TabIndex = 8;
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(486, 460);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(75, 23);
            btnSalir.TabIndex = 9;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(126, 74);
            label1.Name = "label1";
            label1.Size = new Size(51, 15);
            label1.TabIndex = 10;
            label1.Text = "Nombre";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(196, 347);
            label2.Name = "label2";
            label2.Size = new Size(36, 15);
            label2.TabIndex = 11;
            label2.Text = "Clave";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(198, 376);
            label3.Name = "label3";
            label3.Size = new Size(33, 15);
            label3.TabIndex = 12;
            label3.Text = "Valor";
            // 
            // frmIdioma
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1047, 606);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnSalir);
            Controls.Add(txtClave);
            Controls.Add(txtNombre);
            Controls.Add(dgvTraducciones);
            Controls.Add(btnGuardar);
            Controls.Add(btnEliminar);
            Controls.Add(btnAgregar);
            Controls.Add(chkDefecto);
            Controls.Add(txtValor);
            Controls.Add(dgvIdiomas);
            Name = "frmIdioma";
            Text = "frmIdioma";
            Load += frmIdioma_Load;
            ((System.ComponentModel.ISupportInitialize)dgvIdiomas).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvTraducciones).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvIdiomas;
        private TextBox txtValor;
        private CheckBox chkDefecto;
        private Button btnAgregar;
        private Button btnEliminar;
        private Button btnGuardar;
        private DataGridView dgvTraducciones;
        private TextBox txtNombre;
        private TextBox txtClave;
        private Button btnSalir;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}