using ABS;
using BE;
using BLL;
using BLL_;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace TrabajoPracticoIntegrador15_4
{
    public partial class frmNav : Form, IObservadorIdioma
    {
        private readonly GestorIdioma gestor = GestorIdioma.Instancia;

        public frmNav()
        {
            InitializeComponent();
            lblstatusUser.Text = UsuarioBLL.Instancia.UsuarioActivo?.Nombre;
        }

        private void frmNav_Load(object sender, EventArgs e)
        {
            gestor.Suscribir(this);
            gestor.CargarIdiomas();
            CargarMenuIdiomas();
            AplicarPermisos();

            if (gestor.IdiomaActivo != null)
            {
                ActualizarIdioma(gestor.IdiomaActivo);
            }
        }

        // GestorIdioma llama a este método cuando el idioma cambia o cuando
        // cambia la lista de idiomas (alta/baja desde frmIdioma).
        public void ActualizarIdioma(Idioma idioma)
        {
            TraductorUI.Traducir(this.Controls, idioma);
            CargarMenuIdiomas();
        }


        private void AplicarPermisos()
        {
            var p = PerfilBLL.Instancia;

            // Gestión de usuarios
            registrarToolStripMenuItem1.Enabled =
                p.TienePermiso(PerfilBLL.Permisos.CrearUsuario);
            modificarToolStripMenuItem.Enabled =
                p.TienePermiso(PerfilBLL.Permisos.ModificarUsuario);
            eliminarToolStripMenuItem.Enabled =
                p.TienePermiso(PerfilBLL.Permisos.EliminarUsuario);
            listarToolStripMenuItem.Enabled =
                p.TienePermiso(PerfilBLL.Permisos.ListarUsuarios);

            // Bitácora — solo Admin
            bitacoraToolStripMenuItem.Enabled =
                p.TienePermiso(PerfilBLL.Permisos.VerBitacora);

            // Perfiles y Permisos
            perfilesToolStripMenuItem.Enabled =
                p.TienePermiso(PerfilBLL.Permisos.GestionarPerfiles);
            permisosToolStripMenuItem.Enabled =
                p.TienePermiso(PerfilBLL.Permisos.GestionarPerfiles);

            // Idiomas
            IdiomaMenuItem.Enabled =
                p.TienePermiso(PerfilBLL.Permisos.AgregarIdioma) ||
                p.TienePermiso(PerfilBLL.Permisos.ListarUsuarios);

            // Backup / Restore / Recalcular DVH — quien tenga ADM001
            bool puedeBackup = p.TienePermiso(PerfilBLL.Permisos.Backup);
            baseDeDatosToolStripMenuItem.Enabled = puedeBackup;
            backupToolStripMenuItem.Enabled       = puedeBackup;
            restaurarToolStripMenuItem.Enabled    = puedeBackup;
            recalcularDVToolStripMenuItem.Enabled = puedeBackup;
        }

        private void CargarMenuIdiomas()
        {
            IdiomaMenuItem.DropDownItems.Clear();

            foreach (Idioma idioma in gestor.IdiomasDisponibles)
            {
                var item = new ToolStripMenuItem(idioma.Nombre);
                item.Tag = idioma;
                item.Click += (s, e) =>
                {
                    gestor.CambiarIdioma((Idioma)((ToolStripMenuItem)s).Tag);
                };
                IdiomaMenuItem.DropDownItems.Add(item);
                item.Text = idioma.Nombre;
            }

            IdiomaMenuItem.DropDownItems.Add(new ToolStripSeparator());

            var agregar = new ToolStripMenuItem();
            agregar.Tag = "menuAgregar";
            agregar.Text = gestor.IdiomaActivo != null
                             ? gestor.IdiomaActivo.Traducir("menuAgregar")
                             : "Gestionar idiomas...";
            agregar.Enabled = PerfilBLL.Instancia.TienePermiso(PerfilBLL.Permisos.AgregarIdioma);

            agregar.Click += (s, e) =>
            {
                frmIdioma frmIdioma = new frmIdioma();
                frmIdioma.MdiParent = this;
                frmIdioma.Show();
            };
            IdiomaMenuItem.DropDownItems.Add(agregar);
        }

        // Backup / Restore / Recalcular DVH

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.Title = "Guardar backup de la base";
                dlg.Filter = "Archivo de backup SQL (*.bak)|*.bak";
                dlg.FileName = $"Usuarios_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                if (dlg.ShowDialog() != DialogResult.OK) return;

                try
                {
                    Cursor = Cursors.WaitCursor;
                    BackupBLL.Instancia.HacerBackup(dlg.FileName);
                    Cursor = Cursors.Default;

                    MessageBox.Show($"Backup creado en:\n{dlg.FileName}",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show($"No se pudo crear el backup:\n{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void restaurarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Restaurar backup";
                dlg.Filter = "Archivo de backup SQL (*.bak)|*.bak";
                if (dlg.ShowDialog() != DialogResult.OK) return;

                var resp = MessageBox.Show(
                    "Restaurar reemplazará TODA la base de datos actual.\n" +
                    "Se cerrarán todas las sesiones activas.\n\n" +
                    "Después de restaurar deberá iniciar sesión nuevamente.\n\n" +
                    "¿Desea continuar?",
                    "Confirmar restauración",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resp != DialogResult.Yes) return;

                try
                {
                    Cursor = Cursors.WaitCursor;
                    BackupBLL.Instancia.RestaurarBackup(dlg.FileName);
                    Cursor = Cursors.Default;

                    MessageBox.Show(
                        "Base restaurada correctamente.\n" +
                        "El sistema volverá a la pantalla de login.",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Tras el restore la sesión actual ya no es válida
                    UsuarioBLL.Instancia.Logout();
                    gestor.Desuscribir(this);
                    new frmLogin().Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show($"No se pudo restaurar:\n{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Recalcula todos los DVH y el DVV desde cero, tomando el estado
        // actual de la tabla Usuarios como válido.
        // Uso previsto:
        //   • Inicializar el sistema por primera vez (DVH en NULL).
        //   • Aceptar el estado tras un restore de backup.
        //   • Aceptar el estado actual tras detectar adulteración
        //     (con criterio — legítima los cambios detectados).
        private void recalcularDVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show(
                "Recalcular los dígitos verificadores tomará el estado ACTUAL " +
                "de la tabla Usuarios como válido.\n\n" +
                "Usalo cuando:\n" +
                "  • Inicializás el control de integridad por primera vez.\n" +
                "  • Acabás de restaurar un backup confiable.\n" +
                "  • Verificaste el detalle de una adulteración y aceptás " +
                "el estado actual.\n\n" +
                "¿Desea continuar?",
                "Recalcular dígitos verificadores",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                IntegridadBLL.Instancia.RecalcularTodo();
                Cursor = Cursors.Default;

                MessageBox.Show(
                    "Dígitos verificadores recalculados correctamente.\n" +
                    "La integridad de la tabla Usuarios quedó validada.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(
                    "Error al recalcular: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Resto de handlers (sin cambios)

        private void registrarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListar frmListar = new frmListar();
            frmListar.MdiParent = this;
            frmListar.Show();
        }

        private void registrarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmRegistrar frmRegistrar = new frmRegistrar();
            frmRegistrar.MdiParent = this;
            frmRegistrar.Show();
        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmModificar frmModificar = new frmModificar();
            frmModificar.MdiParent = this;
            frmModificar.Show();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEliminar frmEliminar = new frmEliminar();
            frmEliminar.MdiParent = this;
            frmEliminar.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UsuarioBLL.Instancia.Logout();
            new frmLogin().Show();
            gestor.Desuscribir(this);
            this.Close();
        }

        private void IdiomaMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem itemSeleccionado = (ToolStripMenuItem)sender;
            Idioma idiomaSeleccionado = (Idioma)itemSeleccionado.Tag;

            gestor.CambiarIdioma(idiomaSeleccionado);
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBitacora frmBitacora = new frmBitacora();
            frmBitacora.MdiParent = this;
            frmBitacora.Show();
        }

        private void perfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPerfil frmPerfil = new frmPerfil();
            frmPerfil.MdiParent = this;
            frmPerfil.Show();
        }

        private void permisosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPermiso frmPermisos = new frmPermiso();
            frmPermisos.MdiParent = this;
            frmPermisos.Show();
        }
    }
}
