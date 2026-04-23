using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrabajoPracticoIntegrador15_4
{
    public partial class frmNav : Form
    {
        public frmNav()
        {
            InitializeComponent();
            lblUser.Text = UsuarioBLL.Instancia.UsuarioActivo?.Nombre;
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
            this.Close();
        }
    }
}
