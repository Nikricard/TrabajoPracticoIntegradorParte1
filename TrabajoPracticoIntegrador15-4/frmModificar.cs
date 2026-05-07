using BLL;
using BE;
using System;
using System.Windows.Forms;
using BLL_;

namespace TrabajoPracticoIntegrador15_4
{
    public partial class frmModificar : Form, IObservadorIdioma
    {
        private readonly GestorIdioma gestor = GestorIdioma.Instancia;
        public void ActualizarIdioma(Idioma idioma)
        => TraductorUI.Traducir(this.Controls, idioma);

        public frmModificar()
        {
            InitializeComponent();
        }

        private void frmModificar_Load(object sender, EventArgs e)
        {
            ActualizarGrid();

            gestor.Suscribir(this);    
            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);  // Aplica idioma actual

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = new Usuario()
                {//nuevo usuario para guardar atributos dentro
                    Nombre = txtNombre.Text,
                    Id = int.Parse(txtId.Text)
                };

                UsuarioBLL.Instancia.Modify(usuario);
                //enviamos el usuario para que lo trabaje la BLL en Modify
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
        {//esto nos permite actualizar los textbox al usuario seleccionado con el cursor
            if (dgvUsuarios.SelectedRows.Count == 1)
            {
                Usuario usuario = (Usuario)dgvUsuarios.SelectedRows[0].DataBoundItem;
                txtId.Text = usuario.Id.ToString();
                txtNombre.Text = usuario.Nombre;
            }
        }

        private void ActualizarGrid()
        {
            dgvUsuarios.DataSource = UsuarioBLL.Instancia.GetAll();
        }

        private void Limpiar()
        {
            txtId.Text = string.Empty;
            txtNombre.Text = string.Empty;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this); 
            Close();
        }
    }
}
