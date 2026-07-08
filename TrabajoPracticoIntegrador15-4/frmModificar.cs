using BLL;
using ABS;
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

        private Usuario usuarioAnterior = null;

        public frmModificar()
        {
            InitializeComponent();
        }

        private void frmModificar_Load(object sender, EventArgs e)
        {
            ActualizarGrid();

            gestor.Suscribir(this);
            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                // El usuario nuevo copia la contraseña hasheada del anterior
                // para no perderla (frmModificar no la edita).
                // Aunque UsuarioBLL.Modify ahora relee desde BD para calcular
                // el DVH, mantenemos el objeto coherente por buenas prácticas.
                Usuario usuarioNuevo = new Usuario()
                {
                    Id = int.Parse(txtId.Text),
                    Nombre = txtNombre.Text,
                    Contraseña = usuarioAnterior?.Contraseña   // preserva el hash
                };

                UsuarioBLL.Instancia.Modify(usuarioAnterior, usuarioNuevo);
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

                // Preservamos la contraseña actual en el objeto anterior
                // para poder pasarla al nuevo si la operación de Modify
                // llegara a necesitarla.
                usuarioAnterior = new Usuario()
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Contraseña = usuario.Contraseña
                };
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
