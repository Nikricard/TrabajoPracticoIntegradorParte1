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
            clbAtomicos = new CheckedListBox();
            clbCompuestos = new CheckedListBox();
            txtNombre = new TextBox();
            btnCrear = new Button();
            lblConjuntos = new Label();
            btnEliminar = new Button();
            btnLimpiar = new Button();
            dgvConjuntos = new DataGridView();
            btnSalir = new Button();
            lblAtomicos = new Label();
            lblConjuntosDisp = new Label();
            label1 = new Label();
            groupBox1 = new GroupBox();
            btnActualizar = new Button();
            lblArbol = new Label();
            treePermisos = new TreeView();
            ((System.ComponentModel.ISupportInitialize)dgvConjuntos).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // clbAtomicos
            // 
            clbAtomicos.FormattingEnabled = true;
            clbAtomicos.Location = new Point(24, 136);
            clbAtomicos.Name = "clbAtomicos";
            clbAtomicos.Size = new Size(130, 148);
            clbAtomicos.TabIndex = 7;
            // 
            // clbCompuestos
            // 
            clbCompuestos.FormattingEnabled = true;
            clbCompuestos.Location = new Point(160, 136);
            clbCompuestos.Name = "clbCompuestos";
            clbCompuestos.Size = new Size(130, 148);
            clbCompuestos.TabIndex = 8;
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
            lblConjuntos.Location = new Point(300, 108);
            lblConjuntos.Name = "lblConjuntos";
            lblConjuntos.Size = new Size(62, 15);
            lblConjuntos.TabIndex = 4;
            lblConjuntos.Tag = "lblConjuntos";
            lblConjuntos.Text = "Conjuntos";
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(401, 48);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(75, 23);
            btnEliminar.TabIndex = 5;
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
            btnLimpiar.TabIndex = 3;
            btnLimpiar.Tag = "btnLimpiar";
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // dgvConjuntos
            // 
            dgvConjuntos.AllowUserToAddRows = false;
            dgvConjuntos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvConjuntos.Location = new Point(300, 136);
            dgvConjuntos.MultiSelect = false;
            dgvConjuntos.Name = "dgvConjuntos";
            dgvConjuntos.ReadOnly = true;
            dgvConjuntos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvConjuntos.Size = new Size(255, 148);
            dgvConjuntos.TabIndex = 9;
            dgvConjuntos.SelectionChanged += dgvConjuntos_SelectionChanged;
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(482, 48);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(75, 23);
            btnSalir.TabIndex = 6;
            btnSalir.Tag = "btnSalir";
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // lblAtomicos
            // 
            lblAtomicos.AutoSize = true;
            lblAtomicos.Location = new Point(24, 108);
            lblAtomicos.Name = "lblAtomicos";
            lblAtomicos.Size = new Size(107, 15);
            lblAtomicos.TabIndex = 9;
            lblAtomicos.Tag = "lblAtomicos";
            lblAtomicos.Text = "Permisos atómicos";
            // 
            // lblConjuntosDisp
            // 
            lblConjuntosDisp.AutoSize = true;
            lblConjuntosDisp.Location = new Point(160, 108);
            lblConjuntosDisp.Name = "lblConjuntosDisp";
            lblConjuntosDisp.Size = new Size(125, 15);
            lblConjuntosDisp.TabIndex = 10;
            lblConjuntosDisp.Tag = "lblConjuntosDisp";
            lblConjuntosDisp.Text = "Conjuntos disponibles";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 31);
            label1.Name = "label1";
            label1.Size = new Size(64, 15);
            label1.TabIndex = 11;
            label1.Tag = "lblNombre";
            label1.Text = "lblNombre";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnActualizar);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(clbAtomicos);
            groupBox1.Controls.Add(clbCompuestos);
            groupBox1.Controls.Add(lblAtomicos);
            groupBox1.Controls.Add(lblConjuntosDisp);
            groupBox1.Controls.Add(txtNombre);
            groupBox1.Controls.Add(btnSalir);
            groupBox1.Controls.Add(btnCrear);
            groupBox1.Controls.Add(dgvConjuntos);
            groupBox1.Controls.Add(lblConjuntos);
            groupBox1.Controls.Add(btnLimpiar);
            groupBox1.Controls.Add(btnEliminar);
            groupBox1.Controls.Add(lblArbol);
            groupBox1.Controls.Add(treePermisos);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(835, 302);
            groupBox1.TabIndex = 12;
            groupBox1.TabStop = false;
            groupBox1.Tag = "lblGestionPermisos";
            groupBox1.Text = "lblGestionPermisos";
            // 
            // btnActualizar
            // 
            btnActualizar.Location = new Point(320, 49);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(75, 23);
            btnActualizar.TabIndex = 4;
            btnActualizar.Tag = "btnActualizar";
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click_1;
            // 
            // lblArbol
            // 
            lblArbol.AutoSize = true;
            lblArbol.Location = new Point(575, 108);
            lblArbol.Name = "lblArbol";
            lblArbol.Size = new Size(113, 15);
            lblArbol.TabIndex = 13;
            lblArbol.Tag = "lblArbolConjunto";
            lblArbol.Text = "Detalle del conjunto";
            // 
            // treePermisos
            // 
            treePermisos.Location = new Point(575, 136);
            treePermisos.Name = "treePermisos";
            treePermisos.Size = new Size(240, 148);
            treePermisos.TabIndex = 14;
            // 
            // frmPermiso
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(870, 340);
            Controls.Add(groupBox1);
            Name = "frmPermiso";
            Text = "frmPermiso";
            FormClosed += frmPermiso_FormClosed;
            Load += frmPermiso_Load;
            ((System.ComponentModel.ISupportInitialize)dgvConjuntos).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private CheckedListBox clbAtomicos;
        private CheckedListBox clbCompuestos;
        private TextBox txtNombre;
        private Button btnCrear;
        private Label lblConjuntos;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvConjuntos;
        private Button btnSalir;
        private Label lblAtomicos;
        private Label lblConjuntosDisp;
        private Label label1;
        private GroupBox groupBox1;
        private Button btnActualizar;
        private TreeView treePermisos;
        private Label lblArbol;
    }
}
