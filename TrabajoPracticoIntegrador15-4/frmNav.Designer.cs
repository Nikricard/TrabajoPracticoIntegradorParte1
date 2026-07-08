namespace TrabajoPracticoIntegrador15_4
{
    partial class frmNav
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
            components = new System.ComponentModel.Container();
            menuStrip2 = new MenuStrip();
            registrarToolStripMenuItem = new ToolStripMenuItem();
            registrarToolStripMenuItem1 = new ToolStripMenuItem();
            modificarToolStripMenuItem = new ToolStripMenuItem();
            eliminarToolStripMenuItem = new ToolStripMenuItem();
            listarToolStripMenuItem = new ToolStripMenuItem();
            perfilesToolStripMenuItem = new ToolStripMenuItem();
            IdiomaMenuItem = new ToolStripMenuItem();
            bitacoraToolStripMenuItem = new ToolStripMenuItem();
            baseDeDatosToolStripMenuItem = new ToolStripMenuItem();
            backupToolStripMenuItem = new ToolStripMenuItem();
            restaurarToolStripMenuItem = new ToolStripMenuItem();
            recalcularDVToolStripMenuItem = new ToolStripMenuItem();
            salirToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1 = new ContextMenuStrip(components);
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            lblstatusUser = new ToolStripStatusLabel();
            permisosToolStripMenuItem = new ToolStripMenuItem();
            menuStrip2.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip2
            // 
            menuStrip2.Items.AddRange(new ToolStripItem[] { registrarToolStripMenuItem, listarToolStripMenuItem, permisosToolStripMenuItem, perfilesToolStripMenuItem, IdiomaMenuItem, bitacoraToolStripMenuItem, baseDeDatosToolStripMenuItem, salirToolStripMenuItem });
            menuStrip2.Location = new Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Size = new Size(800, 24);
            menuStrip2.TabIndex = 1;
            menuStrip2.Text = "menuStrip2";
            // 
            // registrarToolStripMenuItem
            // 
            registrarToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { registrarToolStripMenuItem1, modificarToolStripMenuItem, eliminarToolStripMenuItem });
            registrarToolStripMenuItem.Name = "registrarToolStripMenuItem";
            registrarToolStripMenuItem.Size = new Size(117, 20);
            registrarToolStripMenuItem.Tag = "menuGestionar";
            registrarToolStripMenuItem.Text = "Gestionar Usuarios";
            registrarToolStripMenuItem.Click += registrarToolStripMenuItem_Click;
            // 
            // registrarToolStripMenuItem1
            // 
            registrarToolStripMenuItem1.Name = "registrarToolStripMenuItem1";
            registrarToolStripMenuItem1.Size = new Size(125, 22);
            registrarToolStripMenuItem1.Tag = "btnRegistrar";
            registrarToolStripMenuItem1.Text = "Registrar";
            registrarToolStripMenuItem1.Click += registrarToolStripMenuItem1_Click;
            // 
            // modificarToolStripMenuItem
            // 
            modificarToolStripMenuItem.Name = "modificarToolStripMenuItem";
            modificarToolStripMenuItem.Size = new Size(125, 22);
            modificarToolStripMenuItem.Tag = "btnModificar";
            modificarToolStripMenuItem.Text = "Modificar";
            modificarToolStripMenuItem.Click += modificarToolStripMenuItem_Click;
            // 
            // eliminarToolStripMenuItem
            // 
            eliminarToolStripMenuItem.Name = "eliminarToolStripMenuItem";
            eliminarToolStripMenuItem.Size = new Size(125, 22);
            eliminarToolStripMenuItem.Tag = "btnEliminar";
            eliminarToolStripMenuItem.Text = "Eliminar";
            eliminarToolStripMenuItem.Click += eliminarToolStripMenuItem_Click;
            // 
            // listarToolStripMenuItem
            // 
            listarToolStripMenuItem.Name = "listarToolStripMenuItem";
            listarToolStripMenuItem.Size = new Size(47, 20);
            listarToolStripMenuItem.Tag = "menuListar";
            listarToolStripMenuItem.Text = "Listar";
            listarToolStripMenuItem.Click += listarToolStripMenuItem_Click;
            // 
            // perfilesToolStripMenuItem
            // 
            perfilesToolStripMenuItem.Name = "perfilesToolStripMenuItem";
            perfilesToolStripMenuItem.Size = new Size(57, 20);
            perfilesToolStripMenuItem.Tag = "menuPerfiles";
            perfilesToolStripMenuItem.Text = "Perfiles";
            perfilesToolStripMenuItem.Click += perfilesToolStripMenuItem_Click;
            // 
            // IdiomaMenuItem
            // 
            IdiomaMenuItem.Name = "IdiomaMenuItem";
            IdiomaMenuItem.Size = new Size(104, 20);
            IdiomaMenuItem.Tag = "menuIdioma";
            IdiomaMenuItem.Text = "Cambiar Idioma";
            // 
            // bitacoraToolStripMenuItem
            // 
            bitacoraToolStripMenuItem.Name = "bitacoraToolStripMenuItem";
            bitacoraToolStripMenuItem.Size = new Size(62, 20);
            bitacoraToolStripMenuItem.Tag = "menuBitacora";
            bitacoraToolStripMenuItem.Text = "Bitacora";
            bitacoraToolStripMenuItem.Click += bitacoraToolStripMenuItem_Click;
            // 
            // baseDeDatosToolStripMenuItem
            // 
            baseDeDatosToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { backupToolStripMenuItem, restaurarToolStripMenuItem, recalcularDVToolStripMenuItem });
            baseDeDatosToolStripMenuItem.Name = "baseDeDatosToolStripMenuItem";
            baseDeDatosToolStripMenuItem.Size = new Size(96, 20);
            baseDeDatosToolStripMenuItem.Tag = "menuBaseDatos";
            baseDeDatosToolStripMenuItem.Text = "Base de datos";
            // 
            // backupToolStripMenuItem
            // 
            backupToolStripMenuItem.Name = "backupToolStripMenuItem";
            backupToolStripMenuItem.Size = new Size(220, 22);
            backupToolStripMenuItem.Tag = "menuBackup";
            backupToolStripMenuItem.Text = "Hacer backup...";
            backupToolStripMenuItem.Click += backupToolStripMenuItem_Click;
            // 
            // restaurarToolStripMenuItem
            // 
            restaurarToolStripMenuItem.Name = "restaurarToolStripMenuItem";
            restaurarToolStripMenuItem.Size = new Size(220, 22);
            restaurarToolStripMenuItem.Tag = "menuRestore";
            restaurarToolStripMenuItem.Text = "Restaurar backup...";
            restaurarToolStripMenuItem.Click += restaurarToolStripMenuItem_Click;
            // 
            // recalcularDVToolStripMenuItem
            // 
            recalcularDVToolStripMenuItem.Name = "recalcularDVToolStripMenuItem";
            recalcularDVToolStripMenuItem.Size = new Size(220, 22);
            recalcularDVToolStripMenuItem.Tag = "menuRecalcularDV";
            recalcularDVToolStripMenuItem.Text = "Recalcular dígitos verificadores";
            recalcularDVToolStripMenuItem.Click += recalcularDVToolStripMenuItem_Click;
            // 
            // salirToolStripMenuItem
            // 
            salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            salirToolStripMenuItem.Size = new Size(57, 20);
            salirToolStripMenuItem.Tag = "menuCerrar";
            salirToolStripMenuItem.Text = "Logout";
            salirToolStripMenuItem.Click += salirToolStripMenuItem_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, lblstatusUser });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.TabIndex = 6;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(118, 17);
            toolStripStatusLabel1.Tag = "lblLogeado";
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // lblstatusUser
            // 
            lblstatusUser.Name = "lblstatusUser";
            lblstatusUser.Size = new Size(118, 17);
            lblstatusUser.Text = "toolStripStatusLabel2";
            // 
            // permisosToolStripMenuItem
            // 
            permisosToolStripMenuItem.Name = "permisosToolStripMenuItem";
            permisosToolStripMenuItem.Size = new Size(67, 20);
            permisosToolStripMenuItem.Text = "Permisos";
            permisosToolStripMenuItem.Click += permisosToolStripMenuItem_Click;
            // 
            // frmNav
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip2);
            IsMdiContainer = true;
            Name = "frmNav";
            Text = "frmNav";
            Load += frmNav_Load;
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip2;
        private ToolStripMenuItem registrarToolStripMenuItem;
        private ToolStripMenuItem listarToolStripMenuItem;
        private ToolStripMenuItem salirToolStripMenuItem;
        private ToolStripMenuItem registrarToolStripMenuItem1;
        private ToolStripMenuItem modificarToolStripMenuItem;
        private ToolStripMenuItem eliminarToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem IdiomaMenuItem;
        private ToolStripMenuItem bitacoraToolStripMenuItem;
        private ToolStripMenuItem perfilesToolStripMenuItem;
        private ToolStripMenuItem baseDeDatosToolStripMenuItem;
        private ToolStripMenuItem backupToolStripMenuItem;
        private ToolStripMenuItem restaurarToolStripMenuItem;
        private ToolStripMenuItem recalcularDVToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel lblstatusUser;
        private ToolStripMenuItem permisosToolStripMenuItem;
    }
}
