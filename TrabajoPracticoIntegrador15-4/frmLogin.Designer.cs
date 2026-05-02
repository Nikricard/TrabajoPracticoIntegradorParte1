namespace TrabajoPracticoIntegrador15_4
{
    partial class frmLogin
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
            label1 = new Label();
            label2 = new Label();
            txtNombre = new TextBox();
            txtContraseña = new TextBox();
            btnIngresar = new Button();
            btnSalir = new Button();
            btnRegistrar = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(144, 84);
            label1.Name = "label1";
            label1.Size = new Size(51, 15);
            label1.TabIndex = 0;
            label1.Tag = "lblNombre";
            label1.Text = "Nombre";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(140, 118);
            label2.Name = "label2";
            label2.Size = new Size(67, 15);
            label2.TabIndex = 1;
            label2.Tag = "lblContrasena";
            label2.Text = "Contraseña";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(213, 76);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(150, 23);
            txtNombre.TabIndex = 2;
            // 
            // txtContraseña
            // 
            txtContraseña.Location = new Point(213, 110);
            txtContraseña.Name = "txtContraseña";
            txtContraseña.PasswordChar = '*';
            txtContraseña.Size = new Size(150, 23);
            txtContraseña.TabIndex = 3;
            // 
            // btnIngresar
            // 
            btnIngresar.Location = new Point(280, 139);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(75, 23);
            btnIngresar.TabIndex = 4;
            btnIngresar.Tag = "btnIngresar";
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = true;
            btnIngresar.Click += btnIngresar_Click;
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(361, 139);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(75, 23);
            btnSalir.TabIndex = 5;
            btnSalir.Tag = "btnSalir";
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // btnRegistrar
            // 
            btnRegistrar.Location = new Point(199, 139);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(75, 23);
            btnRegistrar.TabIndex = 6;
            btnRegistrar.Tag = "btnRegistrar";
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnRegistrar);
            Controls.Add(btnSalir);
            Controls.Add(btnIngresar);
            Controls.Add(txtContraseña);
            Controls.Add(txtNombre);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Name = "frmLogin";
            Text = "frmLogin";
            Load += frmLogin_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtNombre;
        private TextBox txtContraseña;
        private Button btnIngresar;
        private Button btnSalir;
        private Button btnRegistrar;
    }
}