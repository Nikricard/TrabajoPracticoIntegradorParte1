using ABS;
using BE;
using BLL;
using BLL_;
using System;
using System.Windows.Forms;

namespace TrabajoPracticoIntegrador15_4
{
    public partial class frmLogin : Form, IObservadorIdioma
    {
        private readonly GestorIdioma gestor = GestorIdioma.Instancia;
        public void ActualizarIdioma(Idioma idioma)
        => TraductorUI.Traducir(this.Controls, idioma);

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                string contraseña = txtContraseña.Text;
                //almacenamos valores

                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(contraseña))
                {//verificamos campos
                    MessageBox.Show("Debe completar todos los campos", "Atención",
                        //mensaje en caso de error
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UsuarioBLL.Instancia.Login(nombre, contraseña);
                //llamamos al Login de la instancia del usuarioBLL y le enviamos 
                //el nombre y contraseña 
                frmNav formPrincipal = new frmNav();
                formPrincipal.Show();
                //abrimos y mostramos un frmPrincipal
                this.Hide();//cerramos este
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al iniciar sesión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Limpiar();
                txtContraseña.Focus();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this);  
            Close();
            Application.Exit();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = new Usuario()
                {
                    Nombre = txtNombre.Text,
                    Contraseña = txtContraseña.Text
                };//almacenamos valores
                UsuarioBLL.Instancia.Add(usuario, txtContraseña.Text);
                //llamamos al metodo para añadir usuarios y enviamos
                MessageBox.Show($"Usuario {txtNombre.Text} registrado con éxito");
                Limpiar();//limpiamos campos
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Limpiar();
            }
        }

        private void Limpiar()
        {
            txtNombre.Text = string.Empty;
            txtContraseña.Text = string.Empty;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            gestor.Suscribir(this);    
            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);  // Aplica idioma actua
        }
    }
}
