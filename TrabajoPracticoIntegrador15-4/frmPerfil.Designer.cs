namespace TrabajoPracticoIntegrador15_4
{
    partial class frmPerfil
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dgvUsuarios = new DataGridView();
            txtNombre = new TextBox();
            clbAtomicos = new CheckedListBox();
            clbCompuestos = new CheckedListBox();
            btnGuardar = new Button();
            treePermisos = new TreeView();
            btnSalir = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Location = new Point(284, 89);
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.Size = new Size(240, 256);
            dgvUsuarios.TabIndex = 0;
            dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(52, 27);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(120, 23);
            txtNombre.TabIndex = 1;
            // 
            // clbAtomicos
            // 
            clbAtomicos.CheckOnClick = true;
            clbAtomicos.FormattingEnabled = true;
            clbAtomicos.Location = new Point(530, 89);
            clbAtomicos.Name = "clbAtomicos";
            clbAtomicos.Size = new Size(150, 256);
            clbAtomicos.TabIndex = 2;
            // 
            // clbCompuestos
            // 
            clbCompuestos.CheckOnClick = true;
            clbCompuestos.FormattingEnabled = true;
            clbCompuestos.Location = new Point(686, 89);
            clbCompuestos.Name = "clbCompuestos";
            clbCompuestos.Size = new Size(150, 256);
            clbCompuestos.TabIndex = 3;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(178, 26);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(100, 23);
            btnGuardar.TabIndex = 4;
            btnGuardar.Tag = "btnGuardar";
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // treePermisos
            // 
            treePermisos.Location = new Point(48, 89);
            treePermisos.Name = "treePermisos";
            treePermisos.Size = new Size(230, 256);
            treePermisos.TabIndex = 5;
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(284, 27);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(100, 23);
            btnSalir.TabIndex = 6;
            btnSalir.Tag = "btnSalir";
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(52, 9);
            label1.Name = "label1";
            label1.Size = new Size(51, 15);
            label1.TabIndex = 7;
            label1.Tag = "lblNombre";
            label1.Text = "Nombre";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(530, 71);
            label2.Name = "label2";
            label2.Size = new Size(107, 15);
            label2.TabIndex = 8;
            label2.Tag = "lblPermisosDisp";
            label2.Text = "Permisos atómicos";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(48, 71);
            label3.Name = "label3";
            label3.Size = new Size(55, 15);
            label3.TabIndex = 9;
            label3.Tag = "lblPermisos";
            label3.Text = "Permisos";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(686, 71);
            label4.Name = "label4";
            label4.Size = new Size(125, 15);
            label4.TabIndex = 10;
            label4.Tag = "lblConjuntos";
            label4.Text = "Permisos Compuestos";
            // 
            // frmPerfil
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(975, 450);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnSalir);
            Controls.Add(treePermisos);
            Controls.Add(btnGuardar);
            Controls.Add(clbCompuestos);
            Controls.Add(clbAtomicos);
            Controls.Add(txtNombre);
            Controls.Add(dgvUsuarios);
            Name = "frmPerfil";
            Text = "frmPerfil";
            FormClosed += frmPerfil_FormClosed;
            Load += frmPerfil_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvUsuarios;
        private TextBox txtNombre;
        private CheckedListBox clbAtomicos;
        private CheckedListBox clbCompuestos;
        private Button btnGuardar;
        private TreeView treePermisos;
        private Button btnSalir;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}
