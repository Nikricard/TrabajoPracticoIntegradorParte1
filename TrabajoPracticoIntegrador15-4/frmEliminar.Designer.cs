namespace TrabajoPracticoIntegrador15_4
{
    partial class frmEliminar
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
            btnSalir = new Button();
            btnEliminar = new Button();
            txtNombre = new TextBox();
            txtId = new TextBox();
            label2 = new Label();
            label1 = new Label();
            dgvUsuarios = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(340, 248);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(75, 23);
            btnSalir.TabIndex = 13;
            btnSalir.Tag = "btnSalir";
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(248, 248);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(75, 23);
            btnEliminar.TabIndex = 12;
            btnEliminar.Tag = "btnEliminar";
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(115, 111);
            txtNombre.Name = "txtNombre";
            txtNombre.ReadOnly = true;
            txtNombre.Size = new Size(100, 23);
            txtNombre.TabIndex = 11;
            // 
            // txtId
            // 
            txtId.Location = new Point(115, 79);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(100, 23);
            txtId.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(47, 111);
            label2.Name = "label2";
            label2.Size = new Size(51, 15);
            label2.TabIndex = 9;
            label2.Tag = "lblNombre";
            label2.Text = "Nombre";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(47, 80);
            label1.Name = "label1";
            label1.Size = new Size(18, 15);
            label1.TabIndex = 8;
            label1.Tag = "lblId";
            label1.Text = "ID";
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Location = new Point(248, 79);
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.Size = new Size(414, 150);
            dgvUsuarios.TabIndex = 7;
            dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
            // 
            // frmEliminar
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSalir);
            Controls.Add(btnEliminar);
            Controls.Add(txtNombre);
            Controls.Add(txtId);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dgvUsuarios);
            Name = "frmEliminar";
            Text = "frmEliminar";
            Load += frmEliminar_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSalir;
        private Button btnEliminar;
        private TextBox txtNombre;
        private TextBox txtId;
        private Label label2;
        private Label label1;
        private DataGridView dgvUsuarios;
    }
}