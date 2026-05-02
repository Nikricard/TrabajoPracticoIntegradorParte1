using BLL;
using BE;
using System;
using System.Windows.Forms;
using BLL_;

namespace TrabajoPracticoIntegrador15_4
{
    public partial class frmEliminar : Form, IObservadorIdioma
    {

        private readonly GestorIdioma gestor = GestorIdioma.Instancia;

        // 1. Implementar el método del Observer 
        public void ActualizarIdioma(Idioma idioma)
            => TraductorUI.Traducir(this.Controls, idioma);

        public frmEliminar()
        {
            InitializeComponent();
        }

        private void frmEliminar_Load(object sender, EventArgs e)
        {
            gestor.Suscribir(this);    // 2. Suscribirse
            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);  // 3. Aplicar idioma actual

            ActualizarGrid();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = new Usuario()
                {
                    Nombre = txtNombre.Text,
                    Id = int.Parse(txtId.Text)
                };//almacenamos en un usuario los datos ingresados
                UsuarioBLL.Instancia.Delete(usuario);
                //enviamos el usuario a la BLL
                //usando el singleton
                MessageBox.Show($"Usuario {usuario.Nombre} eliminado con éxito");
                Limpiar();
                ActualizarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Limpiar();
            }
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 1)
            {
                Usuario usuario = (Usuario)dgvUsuarios.SelectedRows[0].DataBoundItem;
                txtId.Text = usuario.Id.ToString();
                txtNombre.Text = usuario.Nombre;
                //Actualizamos los campos al clickear en el dgv
            }
        }

        private void ActualizarGrid()
        {
            dgvUsuarios.DataSource = UsuarioBLL.Instancia.GetAll();
            //la lista retornada por el GetAll se asigna como el DataSource del dgv
        }

        private void Limpiar()
        {
            txtId.Text = string.Empty;
            txtNombre.Text = string.Empty;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this);  // 4. Desuscribirse al cerrar
            Close();
        }
    }
}
