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

        // Flag que se enciende si la verificación de integridad al iniciar
        // el form detecta inconsistencias en la tabla Usuarios.
        // Si está en true, solo el Administrador puede loguearse.
        private bool _integridadComprometida = false;

        public void ActualizarIdioma(Idioma idioma)
        => TraductorUI.Traducir(this.Controls, idioma);

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            gestor.Suscribir(this);
            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);

            VerificarIntegridadAlIniciar();
        }

        // Ejecuta la verificación DVH+DVV. Si falla, deja el sistema en modo
        // bloqueado — solo el admin podrá loguearse a continuación.
        private void VerificarIntegridadAlIniciar()
        {
            if (IntegridadBLL.Instancia.VerificarIntegridad())
                return;

            _integridadComprometida = true;

            MessageBox.Show(
                "ADVERTENCIA — Integridad de datos comprometida.\n\n" +
                "Se detectó que la tabla Usuarios fue modificada por afuera del sistema.\n\n" +
                "El acceso está restringido: solo el Administrador puede iniciar sesión " +
                "para decidir cómo proceder.",
                "Verificación de integridad",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                string contraseña = txtContraseña.Text;

                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(contraseña))
                {
                    MessageBox.Show("Debe completar todos los campos", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UsuarioBLL.Instancia.Login(nombre, contraseña);

                bool esAdmin = PerfilBLL.Instancia.TienePermiso(
                    PerfilBLL.Permisos.GestionarPerfiles);

                // Caso 1: integridad comprometida → solo admin puede seguir.
                if (_integridadComprometida)
                {
                    if (!esAdmin)
                    {
                        UsuarioBLL.Instancia.Logout();
                        MessageBox.Show(
                            "El sistema está bloqueado por una falla de integridad.\n" +
                            "Solo el Administrador puede ingresar en este momento.",
                            "Acceso denegado",
                            MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        Limpiar();
                        txtNombre.Focus();
                        return;
                    }

                    // Admin entra: mostrar el detalle y NO recalcular por default.
                    // El admin decide qué hacer desde el menú (restaurar backup
                    // o recalcular manualmente, con criterio).
                    MostrarDetalleAdulteracion();
                }
                // Caso 2: integridad OK pero los dígitos nunca se inicializaron.
                // Si entra el admin, se inicializan automáticamente sin preguntar.
                else if (esAdmin && IntegridadBLL.Instancia.NecesitaInicializacion())
                {
                    try
                    {
                        IntegridadBLL.Instancia.RecalcularTodo();
                    }
                    catch
                    {
                        // Si falla, no bloqueamos; el admin puede reintentar
                        // desde el menú "Base de datos".
                    }
                }

                frmNav formPrincipal = new frmNav();
                formPrincipal.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al iniciar sesión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Limpiar();
                txtContraseña.Focus();
            }
        }

        // Muestra al admin el detalle de la adulteración detectada
        // y le recuerda las acciones disponibles en el menú.
        private void MostrarDetalleAdulteracion()
        {
            string detalle = string.Join("\n • ", IntegridadBLL.Instancia.UltimasInconsistencias);
            if (string.IsNullOrEmpty(detalle))
                detalle = "(sin detalle adicional)";
            else
                detalle = " • " + detalle;

            MessageBox.Show(
                "Bienvenido, " + UsuarioBLL.Instancia.UsuarioActivo?.Nombre + ".\n\n" +
                "Se detectaron estas inconsistencias:\n\n" +
                detalle + "\n\n" +
                "Opciones disponibles desde el menú 'Base de datos':\n\n" +
                "  • RESTAURAR BACKUP: volver a un estado confiable anterior.\n" +
                "  • RECALCULAR: aceptar el estado actual como válido (usar con criterio).",
                "Integridad comprometida",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this);
            Close();
            Application.Exit();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Si la integridad está comprometida, no permitir registrar
            // nuevos usuarios: no sabemos si la base es confiable.
            if (_integridadComprometida)
            {
                MessageBox.Show(
                    "No se pueden registrar usuarios mientras la integridad " +
                    "esté comprometida. Debe iniciar sesión como Administrador " +
                    "y decidir cómo proceder desde el menú 'Base de datos'.",
                    "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            try
            {
                Usuario usuario = new Usuario()
                {
                    Nombre = txtNombre.Text,
                    Contraseña = txtContraseña.Text
                };
                UsuarioBLL.Instancia.Add(usuario, txtContraseña.Text);
                MessageBox.Show($"Usuario {txtNombre.Text} registrado con éxito");
                Limpiar();
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
    }
}
