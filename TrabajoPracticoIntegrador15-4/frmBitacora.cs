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

namespace TrabajoPracticoIntegrador15_4
{
    public partial class frmBitacora : Form , IObservadorIdioma
    {

        private readonly GestorIdioma gestor = GestorIdioma.Instancia;

        public void ActualizarIdioma(Idioma idioma)
            => TraductorUI.Traducir(this.Controls, idioma);

        public frmBitacora()
        {
            InitializeComponent();
        }

        private void frmBitacora_Load(object sender, EventArgs e)
        {
            // Configurar ComboBox de tipo de evento
            cmbTipoEvento.Items.Clear();
            cmbTipoEvento.Items.Add("Todos");
            cmbTipoEvento.Items.Add("Exito");
            cmbTipoEvento.Items.Add("Error");
            cmbTipoEvento.Items.Add("Excepcion");
            cmbTipoEvento.SelectedIndex = 0;

            // Fechas por defecto: última semana
            dtpDesde.Value = DateTime.Today.AddDays(-7);
            dtpHasta.Value = DateTime.Today;

            // Configurar columnas legibles de los DataGridViews
            ConfigurarGrillas();

            // Cargar todo sin filtros al abrir
            Buscar();
            CargarAuditorias();

            gestor.Suscribir(this);
            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);  //Aplica idioma actual

        }

        private void ConfigurarGrillas()
        {
            // creamos columnas con nombres en el dgv para mostrar datos
            // Ajustar tamaño de columnas para mejor visualización

            // Bitácora
            dgvBitacora.AutoGenerateColumns = false;
            dgvBitacora.Columns.Clear();
            dgvBitacora.Columns.Add(ColTexto("Fecha", "Fecha", 120));
            dgvBitacora.Columns.Add(ColTexto("Usuario", "Usuario", 100));
            dgvBitacora.Columns.Add(ColTexto("Actividad", "Actividad", 130));
            dgvBitacora.Columns.Add(ColTexto("TipoEvento", "Tipo", 80));
            dgvBitacora.Columns.Add(ColTexto("Entidad", "Entidad", 80));
            dgvBitacora.Columns.Add(ColTexto("Descripcion", "Descripción", 200));
            dgvBitacora.Columns.Add(ColTexto("ValorAnterior", "Valor anterior", 160));
            dgvBitacora.Columns.Add(ColTexto("ValorNuevo", "Valor nuevo", 160));

            // Auditoría usuarios
            dgvAudUsuario.AutoGenerateColumns = false;
            dgvAudUsuario.Columns.Clear();
            dgvAudUsuario.Columns.Add(ColTexto("Fecha", "Fecha", 120));
            dgvAudUsuario.Columns.Add(ColTexto("UsuarioAccion", "Quién", 100));
            dgvAudUsuario.Columns.Add(ColTexto("Operacion", "Operación", 80));
            dgvAudUsuario.Columns.Add(ColTexto("IdUsuario", "ID usuario", 80));
            dgvAudUsuario.Columns.Add(ColTexto("NombreAnterior", "Nombre anterior", 140));
            dgvAudUsuario.Columns.Add(ColTexto("NombreNuevo", "Nombre nuevo", 140));

            // Auditoría idiomas
            dgvAudIdioma.AutoGenerateColumns = false;
            dgvAudIdioma.Columns.Clear();
            dgvAudIdioma.Columns.Add(ColTexto("Fecha", "Fecha", 120));
            dgvAudIdioma.Columns.Add(ColTexto("UsuarioAccion", "Quién", 100));
            dgvAudIdioma.Columns.Add(ColTexto("Operacion", "Operación", 100));
            dgvAudIdioma.Columns.Add(ColTexto("IdIdioma", "ID idioma", 80));
            dgvAudIdioma.Columns.Add(ColTexto("NombreAnterior", "Nombre anterior", 120));
            dgvAudIdioma.Columns.Add(ColTexto("NombreNuevo", "Nombre nuevo", 120));
            dgvAudIdioma.Columns.Add(ColTexto("ClaveTraduccion", "Clave", 120));
            dgvAudIdioma.Columns.Add(ColTexto("ValorAnterior", "Valor anterior", 140));
            dgvAudIdioma.Columns.Add(ColTexto("ValorNuevo", "Valor nuevo", 140));
        }

        private DataGridViewTextBoxColumn ColTexto(string prop, string header, int w)
        {
            return new DataGridViewTextBoxColumn
            {
                DataPropertyName = prop,
                HeaderText = header,
                Width = w,
                ReadOnly = true
            };
        }

        private void Buscar()
        {
            string tipoEvento = cmbTipoEvento.SelectedItem?.ToString() == "Todos"
                ? null  // Si es "Todos", no filtramos por tipo
                : cmbTipoEvento.SelectedItem?.ToString();   // Si no es "Todos", usamos el valor seleccionado como filtro

            List<RegistroBitacora> resultados = BitacoraBLL.Instancia.Buscar( // llamamos al método de búsqueda de la BLL con los filtros
                dtpDesde.Value.Date,
                dtpHasta.Value.Date,
                txtUsuario.Text.Trim(), 
                txtActividad.Text.Trim(),
                tipoEvento
            );

            dgvBitacora.DataSource = null;
            dgvBitacora.DataSource = resultados;

            // Colorear filas según tipo de evento
            foreach (DataGridViewRow row in dgvBitacora.Rows)
            {
                if (row.DataBoundItem is RegistroBitacora r)
                {
                    row.DefaultCellStyle.BackColor = r.TipoEvento switch
                    {
                        TipoEvento.Error => System.Drawing.Color.FromArgb(255, 220, 220),
                        TipoEvento.Excepcion => System.Drawing.Color.FromArgb(255, 200, 200),
                        _ => System.Drawing.Color.White
                    };
                }
            }
        }

        private void CargarAuditorias()
        {
            dgvAudUsuario.DataSource = null;
            dgvAudUsuario.DataSource = BitacoraBLL.Instancia.GetAuditoriaUsuario();
            // Cargar auditoría de idiomas solo si se selecciona la pestaña correspondiente
            dgvAudIdioma.DataSource = null;
            dgvAudIdioma.DataSource = BitacoraBLL.Instancia.GetAuditoriaIdioma();
        }

        private void button1_Click(object sender, EventArgs e) //me olvide el nombre
        {
            Buscar(); //buscamos con los filtros ingresados
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Today.AddDays(-7);
            dtpHasta.Value = DateTime.Today;
            txtUsuario.Text = string.Empty;
            txtActividad.Text = string.Empty;
            cmbTipoEvento.SelectedIndex = 0;
            Buscar();

        }
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabUsuario ||
                tabControl.SelectedTab == tabIdioma)
                CargarAuditorias(); // recarga auditorías al cambiar de pestaña para mostrar datos actualizados
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this);
            Close();
        }
    }
}
