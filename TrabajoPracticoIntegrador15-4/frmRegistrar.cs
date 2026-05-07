using BLL;
using BE;
using System;
using System.Windows.Forms;
using BLL_;

namespace TrabajoPracticoIntegrador15_4
{
    public partial class frmRegistrar : Form, IObservadorIdioma
    {
        private readonly GestorIdioma gestor = GestorIdioma.Instancia;

        public void ActualizarIdioma(Idioma idioma)
            => TraductorUI.Traducir(this.Controls, idioma);

        public frmRegistrar()
        {
            InitializeComponent();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = new Usuario()
                {//almacenamos nombres en variable
                    Nombre = txtNombre.Text,
                    Contraseña = txtContraseña.Text
                };
                UsuarioBLL.Instancia.Add(usuario, txtContraseña.Text);
                //enviamos el usuario y contraseña a bll para que lo agregue
                MessageBox.Show($"Usuario {txtNombre.Text} registrado con éxito");
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //en caso de error, atrapamos y enviamos msg
                Limpiar();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this);  
            Close();
        }

        private void frmRegistrar_Load(object sender, EventArgs e)
        {
            gestor.Suscribir(this);    
            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);  //Aplica idioma actual
        }

        public void Limpiar()
        {
            txtNombre.Text = string.Empty;
            txtContraseña.Text = string.Empty;
        }
    }
}
