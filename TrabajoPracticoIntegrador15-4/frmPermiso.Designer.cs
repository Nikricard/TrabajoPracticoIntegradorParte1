namespace TrabajoPracticoIntegrador15_4
{
    partial class frmPermiso
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
            clbPermisos = new CheckedListBox();
            txtNombre = new TextBox();
            btnCrear = new Button();
            lblConjuntos = new Label();
            btnEliminar = new Button();
            btnLimpiar = new Button();
            dgvConjuntos = new DataGridView();
            btnSalir = new Button();
            lblPermisosDisp = new Label();
            label1 = new Label();
            groupBox1 = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)dgvConjuntos).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // clbPermisos
            // 
            clbPermisos.FormattingEnabled = true;
            clbPermisos.Location = new Point(25, 136);
            clbPermisos.Name = "clbPermisos";
            clbPermisos.Size = new Size(135, 148);
            clbPermisos.TabIndex = 0;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(24, 49);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(121, 23);
            txtNombre.TabIndex = 1;
            // 
            // btnCrear
            // 
            btnCrear.Location = new Point(158, 49);
            btnCrear.Name = "btnCrear";
            btnCrear.Size = new Size(75, 23);
            btnCrear.TabIndex = 2;
            btnCrear.Tag = "btnCrear";
            btnCrear.Text = "Crear";
            btnCrear.UseVisualStyleBackColor = true;
            btnCrear.Click += btnCrear_Click;
            // 
            // lblConjuntos
            // 
            lblConjuntos.AutoSize = true;
            lblConjuntos.Location = new Point(214, 108);
            lblConjuntos.Name = "lblConjuntos";
            lblConjuntos.Size = new Size(75, 15);
            lblConjuntos.TabIndex = 3;
            lblConjuntos.Tag = "lblConjuntos";
            lblConjuntos.Text = "lblConjuntos";
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(321, 49);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(75, 23);
            btnEliminar.TabIndex = 4;
            btnEliminar.Tag = "btnEliminar";
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(239, 49);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(75, 23);
            btnLimpiar.TabIndex = 5;
            btnLimpiar.Tag = "btnLimpiar";
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // dgvConjuntos
            // 
            dgvConjuntos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvConjuntos.Location = new Point(214, 136);
            dgvConjuntos.Name = "dgvConjuntos";
            dgvConjuntos.Size = new Size(289, 148);
            dgvConjuntos.TabIndex = 6;
            dgvConjuntos.SelectionChanged += dgvConjuntos_SelectionChanged;
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(402, 49);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(75, 23);
            btnSalir.TabIndex = 7;
            btnSalir.Tag = "btnSalir";
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // lblPermisosDisp
            // 
            lblPermisosDisp.AutoSize = true;
            lblPermisosDisp.Location = new Point(25, 108);
            lblPermisosDisp.Name = "lblPermisosDisp";
            lblPermisosDisp.Size = new Size(91, 15);
            lblPermisosDisp.TabIndex = 8;
            lblPermisosDisp.Tag = "lblPermisosDisp";
            lblPermisosDisp.Text = "lblPermisosDisp";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 31);
            label1.Name = "label1";
            label1.Size = new Size(64, 15);
            label1.TabIndex = 9;
            label1.Tag = "lblNombre";
            label1.Text = "lblNombre";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(clbPermisos);
            groupBox1.Controls.Add(lblPermisosDisp);
            groupBox1.Controls.Add(txtNombre);
            groupBox1.Controls.Add(btnSalir);
            groupBox1.Controls.Add(btnCrear);
            groupBox1.Controls.Add(dgvConjuntos);
            groupBox1.Controls.Add(lblConjuntos);
            groupBox1.Controls.Add(btnLimpiar);
            groupBox1.Controls.Add(btnEliminar);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(526, 302);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Tag = "lblGestionPermisos";
            groupBox1.Text = "lblGestionPermisos";
            // 
            // frmPermiso
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox1);
            Name = "frmPermiso";
            Text = "frmPermiso";
            Load += frmPermiso_Load;
            ((System.ComponentModel.ISupportInitialize)dgvConjuntos).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private CheckedListBox clbPermisos;
        private TextBox txtNombre;
        private Button btnCrear;
        private Label lblConjuntos;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvConjuntos;
        private Button btnSalir;
        private Label lblPermisosDisp;
        private Label label1;
        private GroupBox groupBox1;
    }
}