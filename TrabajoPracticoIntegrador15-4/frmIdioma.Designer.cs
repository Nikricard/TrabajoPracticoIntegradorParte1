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
            btnLimpiarSeleccion = new Button();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            groupBox3 = new GroupBox();
            btnEliminarTag = new Button();
            txtTag = new TextBox();
            dgvTags = new DataGridView();
            btnAgregarTag = new Button();
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvIdiomas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvTraducciones).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTags).BeginInit();
            SuspendLayout();
            // 
            // dgvIdiomas
            // 
            dgvIdiomas.AllowUserToAddRows = false;
            dgvIdiomas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvIdiomas.Location = new Point(276, 22);
            dgvIdiomas.MultiSelect = false;
            dgvIdiomas.Name = "dgvIdiomas";
            dgvIdiomas.ReadOnly = true;
            dgvIdiomas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIdiomas.Size = new Size(327, 150);
            dgvIdiomas.TabIndex = 0;
            dgvIdiomas.SelectionChanged += dgvIdiomas_SelectionChanged;
            // 
            // txtValor
            // 
            txtValor.Location = new Point(105, 76);
            txtValor.Name = "txtValor";
            txtValor.Size = new Size(100, 23);
            txtValor.TabIndex = 1;
            // 
            // chkDefecto
            // 
            chkDefecto.AutoSize = true;
            chkDefecto.Location = new Point(105, 75);
            chkDefecto.Name = "chkDefecto";
            chkDefecto.Size = new Size(87, 19);
            chkDefecto.TabIndex = 2;
            chkDefecto.Tag = "checkDefecto";
            chkDefecto.Text = "Por defecto";
            chkDefecto.UseVisualStyleBackColor = true;
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(105, 115);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(75, 23);
            btnAgregar.TabIndex = 3;
            btnAgregar.Tag = "btnAgregar";
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = true;
            btnAgregar.Click += btnAgregar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(186, 115);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(75, 23);
            btnEliminar.TabIndex = 4;
            btnEliminar.Tag = "btnEliminar";
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(105, 131);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(75, 23);
            btnGuardar.TabIndex = 5;
            btnGuardar.Tag = "btnGuardar";
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // dgvTraducciones
            // 
            dgvTraducciones.AllowUserToAddRows = false;
            dgvTraducciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTraducciones.Location = new Point(276, 40);
            dgvTraducciones.MultiSelect = false;
            dgvTraducciones.Name = "dgvTraducciones";
            dgvTraducciones.ReadOnly = true;
            dgvTraducciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTraducciones.Size = new Size(327, 150);
            dgvTraducciones.TabIndex = 6;
            dgvTraducciones.SelectionChanged += dgvTraducciones_SelectionChanged;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(105, 36);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(100, 23);
            txtNombre.TabIndex = 7;
            // 
            // txtClave
            // 
            txtClave.Location = new Point(105, 40);
            txtClave.Name = "txtClave";
            txtClave.ReadOnly = true;
            txtClave.Size = new Size(100, 23);
            txtClave.TabIndex = 8;
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(619, 487);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(75, 23);
            btnSalir.TabIndex = 9;
            btnSalir.Tag = "btnSalir";
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(41, 39);
            label1.Name = "label1";
            label1.Size = new Size(51, 15);
            label1.TabIndex = 10;
            label1.Tag = "lblNombre";
            label1.Text = "Nombre";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(41, 43);
            label2.Name = "label2";
            label2.Size = new Size(25, 15);
            label2.TabIndex = 11;
            label2.Tag = "Tag";
            label2.Text = "Tag";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(41, 84);
            label3.Name = "label3";
            label3.Size = new Size(33, 15);
            label3.TabIndex = 12;
            label3.Tag = "lblTraduccion";
            label3.Text = "Valor";
            // 
            // btnLimpiarSeleccion
            // 
            btnLimpiarSeleccion.Location = new Point(24, 115);
            btnLimpiarSeleccion.Name = "btnLimpiarSeleccion";
            btnLimpiarSeleccion.Size = new Size(75, 23);
            btnLimpiarSeleccion.TabIndex = 13;
            btnLimpiarSeleccion.Tag = "btnLimpiar";
            btnLimpiarSeleccion.Text = "Limpiar Seleccion";
            btnLimpiarSeleccion.UseVisualStyleBackColor = true;
            btnLimpiarSeleccion.Click += btnLimpiarSeleccion_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtClave);
            groupBox1.Controls.Add(txtValor);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(dgvTraducciones);
            groupBox1.Controls.Add(btnGuardar);
            groupBox1.Location = new Point(664, 258);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(633, 223);
            groupBox1.TabIndex = 14;
            groupBox1.TabStop = false;
            groupBox1.Tag = "boxTraducciones";
            groupBox1.Text = "Gestion de traducciones";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dgvIdiomas);
            groupBox2.Controls.Add(chkDefecto);
            groupBox2.Controls.Add(btnLimpiarSeleccion);
            groupBox2.Controls.Add(btnAgregar);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(btnEliminar);
            groupBox2.Controls.Add(txtNombre);
            groupBox2.Location = new Point(664, 28);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(633, 204);
            groupBox2.TabIndex = 15;
            groupBox2.TabStop = false;
            groupBox2.Tag = "boxIdioma";
            groupBox2.Text = "Gestion de Idioma";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(btnEliminarTag);
            groupBox3.Controls.Add(txtTag);
            groupBox3.Controls.Add(dgvTags);
            groupBox3.Controls.Add(btnAgregarTag);
            groupBox3.Controls.Add(label5);
            groupBox3.Location = new Point(27, 28);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(618, 453);
            groupBox3.TabIndex = 16;
            groupBox3.TabStop = false;
            groupBox3.Tag = "boxTags";
            groupBox3.Text = "Gestion de Tags";
            // 
            // btnEliminarTag
            // 
            btnEliminarTag.Location = new Point(115, 81);
            btnEliminarTag.Name = "btnEliminarTag";
            btnEliminarTag.Size = new Size(75, 23);
            btnEliminarTag.TabIndex = 23;
            btnEliminarTag.Tag = "btnEliminar";
            btnEliminarTag.Text = "Eliminar";
            btnEliminarTag.UseVisualStyleBackColor = true;
            btnEliminarTag.Click += btnEliminarTag_Click;
            // 
            // txtTag
            // 
            txtTag.Location = new Point(90, 39);
            txtTag.Name = "txtTag";
            txtTag.Size = new Size(100, 23);
            txtTag.TabIndex = 20;
            // 
            // dgvTags
            // 
            dgvTags.AllowUserToAddRows = false;
            dgvTags.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTags.Location = new Point(261, 39);
            dgvTags.MultiSelect = false;
            dgvTags.Name = "dgvTags";
            dgvTags.ReadOnly = true;
            dgvTags.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTags.Size = new Size(327, 381);
            dgvTags.TabIndex = 19;
            dgvTags.SelectionChanged += dgvTags_SelectionChanged;
            // 
            // btnAgregarTag
            // 
            btnAgregarTag.Location = new Point(34, 81);
            btnAgregarTag.Name = "btnAgregarTag";
            btnAgregarTag.Size = new Size(75, 23);
            btnAgregarTag.TabIndex = 18;
            btnAgregarTag.Tag = "btnAgregar";
            btnAgregarTag.Text = "Agregar";
            btnAgregarTag.UseVisualStyleBackColor = true;
            btnAgregarTag.Click += btnAgregarTag_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(26, 42);
            label5.Name = "label5";
            label5.Size = new Size(25, 15);
            label5.TabIndex = 21;
            label5.Text = "Tag";
            // 
            // frmIdioma
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1347, 763);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(btnSalir);
            Name = "frmIdioma";
            Text = "frmIdioma";
            Load += frmIdioma_Load;
            ((System.ComponentModel.ISupportInitialize)dgvIdiomas).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvTraducciones).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTags).EndInit();
            ResumeLayout(false);
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
        private Button btnLimpiarSeleccion;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private TextBox txtTag;
        private DataGridView dgvTags;
        private Button btnAgregarTag;
        private Label label5;
        private Button btnEliminarTag;
    }
}