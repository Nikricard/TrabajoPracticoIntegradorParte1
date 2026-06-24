using ABS;
using BE;
using BLL;
using BLL_;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TrabajoPracticoIntegrador15_4
{
    public partial class frmBitacora : Form, IObservadorIdioma, IObservadorBitacora
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
            // ComboBox de tipo de evento
            cmbTipoEvento.Items.Clear();
            cmbTipoEvento.Items.Add("Todos");
            cmbTipoEvento.Items.Add("Exito");
            cmbTipoEvento.Items.Add("Error");
            cmbTipoEvento.Items.Add("Excepcion");
            cmbTipoEvento.SelectedIndex = 0;

            // ComboBox de usuarios — opción "Todos" + todos los usuarios
            CargarComboUsuarios();

            // Fechas por defecto: última semana
            dtpDesde.Value = DateTime.Today.AddDays(-7);
            dtpHasta.Value = DateTime.Today;

            ConfigurarGrillas();

            Buscar();
            CargarAuditorias();

            gestor.Suscribir(this);
            BitacoraBLL.Instancia.Suscribir(this);  // escucha nuevos eventos

            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);
        }

        // Observer de bitácora: lo dispara cualquier BLL cuando registra un evento.
        // Refresca respetando los filtros actuales que tenga el usuario aplicados.
        public void ActualizarBitacora()
        {
            // Si el form se cerró pero quedó algún observer huérfano, no hacer nada.
            if (IsDisposed || !IsHandleCreated) return;

            // Las llamadas pueden venir de otro thread? Aseguro UI thread con Invoke.
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ActualizarBitacora));
                return;
            }

            Buscar();
            // Si está en una pestaña de auditoría, también la refresco.
            if (tabControl.SelectedTab == tabUsuario ||
                tabControl.SelectedTab == tabIdioma)
                CargarAuditorias();
        }


        // Llena el ComboBox con "Todos" + los nombres de todos los usuarios.
        private void CargarComboUsuarios()
        {
            cmbUsuario.Items.Clear();
            cmbUsuario.Items.Add("Todos");

            foreach (Usuario u in UsuarioBLL.Instancia.GetAll())
                cmbUsuario.Items.Add(u.Nombre);

            cmbUsuario.SelectedIndex = 0;  // por defecto "Todos"
        }

        private void ConfigurarGrillas()
        {
            // Bitácora
            dgvBitacora.AutoGenerateColumns = false;
            dgvBitacora.Columns.Clear();
            dgvBitacora.Columns.Add(ColTexto("Fecha", "Fecha", 120));
            dgvBitacora.Columns.Add(ColTexto("Usuario", "Usuario", 100));
            dgvBitacora.Columns.Add(ColTexto("Actividad", "Actividad", 160));
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
            dgvAudUsuario.Columns.Add(ColTexto("Operacion", "Operación", 140));
            dgvAudUsuario.Columns.Add(ColTexto("IdUsuario", "ID usuario", 80));
            dgvAudUsuario.Columns.Add(ColTexto("NombreAnterior", "Nombre anterior", 140));
            dgvAudUsuario.Columns.Add(ColTexto("NombreNuevo", "Nombre nuevo", 140));

            // Auditoría idiomas
            dgvAudIdioma.AutoGenerateColumns = false;
            dgvAudIdioma.Columns.Clear();
            dgvAudIdioma.Columns.Add(ColTexto("Fecha", "Fecha", 120));
            dgvAudIdioma.Columns.Add(ColTexto("UsuarioAccion", "Quién", 100));
            dgvAudIdioma.Columns.Add(ColTexto("Operacion", "Operación", 160));
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
                ? null
                : cmbTipoEvento.SelectedItem?.ToString();

            // Si el usuario seleccionado es "Todos", no filtramos por usuario
            string usuario = cmbUsuario.SelectedItem?.ToString() == "Todos"
                ? null
                : cmbUsuario.SelectedItem?.ToString();

            List<RegistroBitacora> resultados = BitacoraBLL.Instancia.Buscar(
                dtpDesde.Value.Date,
                dtpHasta.Value.Date,
                usuario,
                txtActividad.Text.Trim(),
                tipoEvento
            );

            dgvBitacora.DataSource = null;
            dgvBitacora.DataSource = resultados;

            foreach (DataGridViewRow row in dgvBitacora.Rows)
            {
                if (row.DataBoundItem is RegistroBitacora r)
                {
                    row.DefaultCellStyle.BackColor = r.TipoEvento switch
                    {
                        TipoEvento.Error     => System.Drawing.Color.FromArgb(255, 220, 220),
                        TipoEvento.Excepcion => System.Drawing.Color.FromArgb(255, 200, 200),
                        _                    => System.Drawing.Color.White
                    };
                }
            }
        }

        private void CargarAuditorias()
        {
            dgvAudUsuario.DataSource = null;
            dgvAudUsuario.DataSource = BitacoraBLL.Instancia.GetAuditoriaUsuario();

            dgvAudIdioma.DataSource = null;
            dgvAudIdioma.DataSource = BitacoraBLL.Instancia.GetAuditoriaIdioma();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Today.AddDays(-7);
            dtpHasta.Value = DateTime.Today;
            cmbUsuario.SelectedIndex    = 0;   // vuelve a "Todos"
            txtActividad.Text           = string.Empty;
            cmbTipoEvento.SelectedIndex = 0;
            Buscar();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabUsuario ||
                tabControl.SelectedTab == tabIdioma)
                CargarAuditorias();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this);
            BitacoraBLL.Instancia.Desuscribir(this);
            Close();
        }
    }
}
