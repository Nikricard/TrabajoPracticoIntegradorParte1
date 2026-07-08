using ABS;
using BE;
using BLL;
using BLL_;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TrabajoPracticoIntegrador15_4
{
    public partial class frmListar : Form, IObservadorIdioma
    {
        private readonly GestorIdioma gestor = GestorIdioma.Instancia;

        public void ActualizarIdioma(Idioma idioma)
            => TraductorUI.Traducir(this.Controls, idioma);

        public frmListar()
        {
            InitializeComponent();
        }

        private void frmListar_Load(object sender, EventArgs e)
        {
            gestor.Suscribir(this);
            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);

            CargarUsuarios();
        }

        // Carga los usuarios directamente desde la BD para poder mostrar
        // la columna DVH (que no está en la entidad Usuario para no
        // filtrarla a las capas superiores como si fuera un campo de negocio).
        private void CargarUsuarios()
        {
            string cs = "Server=DESKTOP-FD6Q6GG\\SQLEXPRESS;Database=Usuarios;Integrated Security=True";

            var tabla = new System.Data.DataTable();
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT Id, Nombre, ISNULL(DVH, '(sin calcular)') AS DVH " +
                    "FROM Usuarios ORDER BY Id", con);
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(tabla);
            }

            dgvUsuarios.AutoGenerateColumns = true;
            dgvUsuarios.DataSource = tabla;

            // Ajustes visuales de la columna DVH
            if (dgvUsuarios.Columns["DVH"] != null)
            {
                dgvUsuarios.Columns["DVH"].HeaderText = "DVH (hash)";
                dgvUsuarios.Columns["DVH"].DefaultCellStyle.Font =
                    new System.Drawing.Font("Consolas", 8);
                dgvUsuarios.Columns["DVH"].Width = 400;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this);
            Close();
        }
    }
}
