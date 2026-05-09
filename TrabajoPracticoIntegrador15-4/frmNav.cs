using ABS;
using BE;
using BLL;
using BLL_;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            lblUser.Text = UsuarioBLL.Instancia.UsuarioActivo?.Nombre;
        }

        private void frmNav_Load(object sender, EventArgs e)
        {
            gestor.Suscribir(this);
            gestor.CargarIdiomas();
            CargarMenuIdiomas();

            if (gestor.IdiomaActivo != null)
            {
                ActualizarIdioma(gestor.IdiomaActivo);
            }

        }


        // GestorIdioma llama a este método cuando el idioma cambia.
        // Actualiza controles normales e ítems del MenuStrip.
        public void ActualizarIdioma(Idioma idioma)
                => TraductorUI.Traducir(this.Controls, idioma);

        private void CargarMenuIdiomas()
        {
            IdiomaMenuItem.DropDownItems.Clear();
            gestor.CargarIdiomas();

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
            agregar.Tag = "menuAgregar";   // misma clave que está en la tabla Palabra
            agregar.Text = gestor.IdiomaActivo != null
                             ? gestor.IdiomaActivo.Traducir("menuAgregar")
                             : "Gestionar idiomas...";
            agregar.Click += (s, e) =>
            {
                new frmIdioma().ShowDialog();
                CargarMenuIdiomas();
            };
            IdiomaMenuItem.DropDownItems.Add(agregar);
        }

        private void registrarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListar frmListar = new frmListar();
            frmListar.ShowDialog();
        }

        private void registrarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmRegistrar frmRegistrar = new frmRegistrar();
            frmRegistrar.ShowDialog();
        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmModificar frmModificar = new frmModificar();
            frmModificar.ShowDialog();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEliminar frmEliminar = new frmEliminar();
            frmEliminar.ShowDialog();
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

            // Un solo llamado actualiza TODOS los formularios registrados como observadores
            gestor.CambiarIdioma(idiomaSeleccionado);
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBitacora frmBitacora = new frmBitacora();
            frmBitacora.ShowDialog();
        }
    }
}
