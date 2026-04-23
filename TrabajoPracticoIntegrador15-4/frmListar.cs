using BE;
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
    public partial class frmListar : Form
    {
        public frmListar()
        {
            InitializeComponent();
        }

        private void frmListar_Load(object sender, EventArgs e)
        {
            dgvUsuarios.DataSource = UsuarioBLL.Instancia.GetAll();
            //llamada al metodo GetAll usando la instancia del UsuarioBLL
            //y establecemos la lista devuelta como datasource
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            /*if (dgvUsuarios.SelectedRows.Count == 1)
            {
                Usuario usuario = (Usuario)dgvUsuarios.SelectedRows[0].DataBoundItem;

                //txtNombre.Text = usuario.Nombre;
            }*/
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
