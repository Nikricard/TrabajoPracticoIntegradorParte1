namespace TrabajoPracticoIntegrador15_4
{
    partial class frmModificar
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
            dgvUsuarios = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            txtId = new TextBox();
            txtNombre = new TextBox();
            btnModificar = new Button();
            btnSalir = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Location = new Point(236, 73);
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.Size = new Size(414, 150);
            dgvUsuarios.TabIndex = 0;
            dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(35, 74);
            label1.Name = "label1";
            label1.Size = new Size(18, 15);
            label1.TabIndex = 1;
            label1.Tag = "lblId";
            label1.Text = "ID";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(35, 105);
            label2.Name = "label2";
            label2.Size = new Size(51, 15);
            label2.TabIndex = 2;
            label2.Tag = "lblNombre";
            label2.Text = "Nombre";
            // 
            // txtId
            // 
            txtId.Location = new Point(103, 73);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(100, 23);
            txtId.TabIndex = 3;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(103, 105);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(100, 23);
            txtNombre.TabIndex = 4;
            // 
            // btnModificar
            // 
            btnModificar.Location = new Point(236, 242);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(75, 23);
            btnModificar.TabIndex = 5;
            btnModificar.Tag = "btnModificar";
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(328, 242);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(75, 23);
            btnSalir.TabIndex = 6;
            btnSalir.Tag = "btnSalir";
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // frmModificar
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSalir);
            Controls.Add(btnModificar);
            Controls.Add(txtNombre);
            Controls.Add(txtId);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dgvUsuarios);
            Name = "frmModificar";
            Text = "frmModificar";
            Load += frmModificar_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvUsuarios;
        private Label label1;
        private Label label2;
        private TextBox txtId;
        private TextBox txtNombre;
        private Button btnModificar;
        private Button btnSalir;
    }
}