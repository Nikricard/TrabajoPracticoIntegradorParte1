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
            groupBox1 = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Location = new Point(253, 51);
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.Size = new Size(240, 256);
            dgvUsuarios.TabIndex = 2;
            dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(71, 313);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(120, 23);
            txtNombre.TabIndex = 5;
            // 
            // clbAtomicos
            // 
            clbAtomicos.CheckOnClick = true;
            clbAtomicos.FormattingEnabled = true;
            clbAtomicos.Location = new Point(499, 51);
            clbAtomicos.Name = "clbAtomicos";
            clbAtomicos.Size = new Size(150, 256);
            clbAtomicos.TabIndex = 3;
            // 
            // clbCompuestos
            // 
            clbCompuestos.CheckOnClick = true;
            clbCompuestos.FormattingEnabled = true;
            clbCompuestos.Location = new Point(655, 51);
            clbCompuestos.Name = "clbCompuestos";
            clbCompuestos.Size = new Size(150, 256);
            clbCompuestos.TabIndex = 4;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(197, 312);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(100, 23);
            btnGuardar.TabIndex = 6;
            btnGuardar.Tag = "btnGuardar";
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // treePermisos
            // 
            treePermisos.Location = new Point(17, 51);
            treePermisos.Name = "treePermisos";
            treePermisos.Size = new Size(230, 256);
            treePermisos.TabIndex = 1;
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(303, 313);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(100, 23);
            btnSalir.TabIndex = 7;
            btnSalir.Tag = "btnSalir";
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 317);
            label1.Name = "label1";
            label1.Size = new Size(51, 15);
            label1.TabIndex = 7;
            label1.Tag = "lblNombre";
            label1.Text = "Nombre";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(499, 33);
            label2.Name = "label2";
            label2.Size = new Size(107, 15);
            label2.TabIndex = 8;
            label2.Tag = "lblPermisosDisp";
            label2.Text = "Permisos atómicos";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(17, 33);
            label3.Name = "label3";
            label3.Size = new Size(55, 15);
            label3.TabIndex = 9;
            label3.Tag = "lblPermisos";
            label3.Text = "Permisos";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(655, 33);
            label4.Name = "label4";
            label4.Size = new Size(125, 15);
            label4.TabIndex = 10;
            label4.Tag = "lblConjuntos";
            label4.Text = "Permisos Compuestos";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(treePermisos);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(btnSalir);
            groupBox1.Controls.Add(dgvUsuarios);
            groupBox1.Controls.Add(btnGuardar);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtNombre);
            groupBox1.Controls.Add(clbAtomicos);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(clbCompuestos);
            groupBox1.Location = new Point(44, 26);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(832, 364);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Tag = "lblGestionPerfiles";
            groupBox1.Text = "Gestion de Perfiles";
            // 
            // frmPerfil
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1090, 446);
            Controls.Add(groupBox1);
            Name = "frmPerfil";
            Text = "frmPerfil";
            FormClosed += frmPerfil_FormClosed;
            Load += frmPerfil_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
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
        private GroupBox groupBox1;
    }
}
