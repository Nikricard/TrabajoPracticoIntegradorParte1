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

        // Lista fija de actividades registradas por BitacoraBLL. Se muestra en
        // el ComboBox de filtro. La primera entrada "Todas" desactiva el filtro.
        private static readonly List<string> ActividadesDelSistema = new List<string>
        {
            "Todas",
            "Inicio de sesión",
            "Cierre de sesión",
            "Alta de usuario",
            "Modificación de usuario",
            "Baja de usuario",
            "Rollback de usuario",
            "Asignación de perfil",
            "Alta de conjunto",
            "Alta de idioma",
            "Baja de idioma",
            "Modificación de traducción",
            "Backup de base de datos",
            "Restore de base de datos"
        };

        public void ActualizarIdioma(Idioma idioma)
            => TraductorUI.Traducir(this.Controls, idioma);

        public frmBitacora()
        {
            InitializeComponent();
        }

        private void frmBitacora_Load(object sender, EventArgs e)
        {
            cmbTipoEvento.Items.Clear();
            cmbTipoEvento.Items.Add("Todos");
            cmbTipoEvento.Items.Add("Exito");
            cmbTipoEvento.Items.Add("Error");
            cmbTipoEvento.Items.Add("Excepcion");
            cmbTipoEvento.SelectedIndex = 0;

            // Cargar ComboBox de actividades (reemplaza al TextBox anterior).
            cmbActividad.Items.Clear();
            foreach (string a in ActividadesDelSistema)
                cmbActividad.Items.Add(a);
            cmbActividad.SelectedIndex = 0;

            CargarComboUsuarios();

            dtpDesde.Value = DateTime.Today.AddDays(-7);
            dtpHasta.Value = DateTime.Today;

            ConfigurarGrillas();
            AplicarPermisos();

            Buscar();
            CargarAuditorias();

            gestor.Suscribir(this);
            BitacoraBLL.Instancia.Suscribir(this);

            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);
        }

        public void ActualizarBitacora()
        {
            if (IsDisposed || !IsHandleCreated) return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ActualizarBitacora));
                return;
            }
            Buscar();
            if (tabControl.SelectedTab == tabUsuario ||
                tabControl.SelectedTab == tabIdioma)
                CargarAuditorias();
        }

        private void CargarComboUsuarios()
        {
            cmbUsuario.Items.Clear();
            cmbUsuario.Items.Add("Todos");
            foreach (Usuario u in UsuarioBLL.Instancia.GetAll())
                cmbUsuario.Items.Add(u.Nombre);
            cmbUsuario.SelectedIndex = 0;
        }

        private void AplicarPermisos()
        {
            bool puedeRestaurar = PerfilBLL.Instancia.TienePermiso(
                PerfilBLL.Permisos.RestaurarAuditoria);
            btnRestaurar.Enabled = puedeRestaurar;
            btnRestaurar.Visible = puedeRestaurar;
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

            // Auditoría usuarios — usamos "Valor anterior / Valor nuevo" en
            // lugar de "Nombre anterior / Nombre nuevo" porque los cambios
            // de permisos también viajan por estas columnas (el "valor"
            // puede ser un nombre o una lista de códigos de permiso).
            dgvAudUsuario.AutoGenerateColumns = false;
            dgvAudUsuario.Columns.Clear();
            dgvAudUsuario.Columns.Add(ColTexto("Fecha", "Fecha", 120));
            dgvAudUsuario.Columns.Add(ColTexto("UsuarioAccion", "Quién", 100));
            dgvAudUsuario.Columns.Add(ColTexto("Operacion", "Operación", 140));
            dgvAudUsuario.Columns.Add(ColTexto("IdUsuario", "ID usuario", 80));

            // Columnas anchas con wrap para leer permisos concatenados.
            var colAnt = ColTexto("NombreAnterior", "Valor anterior", 260);
            colAnt.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvAudUsuario.Columns.Add(colAnt);

            var colNue = ColTexto("NombreNuevo", "Valor nuevo", 260);
            colNue.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvAudUsuario.Columns.Add(colNue);

            // Columna virtual que indica si la fila tiene snapshot (restaurable).
            var colRestaurable = new DataGridViewTextBoxColumn
            {
                HeaderText = "Restaurable",
                Width = 90,
                ReadOnly = true,
                Name = "colRestaurable"
            };
            dgvAudUsuario.Columns.Add(colRestaurable);

            // Las filas se ajustan en altura para mostrar el contenido completo.
            dgvAudUsuario.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

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
                ? null : cmbTipoEvento.SelectedItem?.ToString();
            string usuario = cmbUsuario.SelectedItem?.ToString() == "Todos"
                ? null : cmbUsuario.SelectedItem?.ToString();
            string actividad = cmbActividad.SelectedItem?.ToString() == "Todas"
                ? null : cmbActividad.SelectedItem?.ToString();

            List<RegistroBitacora> resultados = BitacoraBLL.Instancia.Buscar(
                dtpDesde.Value.Date, dtpHasta.Value.Date,
                usuario, actividad, tipoEvento);

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
            var audUsuarios = BitacoraBLL.Instancia.GetAuditoriaUsuario();
            dgvAudUsuario.DataSource = null;
            dgvAudUsuario.DataSource = audUsuarios;

            // Marcar la columna "Restaurable" y setear tooltip con el snapshot.
            foreach (DataGridViewRow row in dgvAudUsuario.Rows)
            {
                if (row.DataBoundItem is AuditoriaUsuario a)
                {
                    bool tieneSnapshot = !string.IsNullOrEmpty(a.SnapshotJson);
                    row.Cells["colRestaurable"].Value = tieneSnapshot ? "Sí" : "—";
                    if (tieneSnapshot)
                    {
                        row.Cells["colRestaurable"].Style.ForeColor =
                            System.Drawing.Color.DarkGreen;
                        // Tooltip con el JSON del snapshot al pasar el mouse
                        row.Cells["colRestaurable"].ToolTipText = a.SnapshotJson;
                    }
                }
            }

            dgvAudIdioma.DataSource = null;
            dgvAudIdioma.DataSource = BitacoraBLL.Instancia.GetAuditoriaIdioma();
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            if (dgvAudUsuario.SelectedRows.Count != 1)
            {
                MessageBox.Show("Seleccione una fila de la auditoría para restaurar.",
                    "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var fila = dgvAudUsuario.SelectedRows[0].DataBoundItem as AuditoriaUsuario;
            if (fila == null) return;

            if (string.IsNullOrEmpty(fila.SnapshotJson))
            {
                MessageBox.Show(
                    "Esta entrada no tiene un snapshot asociado (es anterior a la " +
                    "versión con control de cambios completo). Solo se pueden " +
                    "restaurar entradas con la columna 'Restaurable' en Sí.",
                    "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UsuarioSnapshot snapshot;
            try { snapshot = UsuarioSnapshot.FromJson(fila.SnapshotJson); }
            catch (Exception ex)
            {
                MessageBox.Show("El snapshot está corrupto: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var resp = MessageBox.Show(
                $"¿Restaurar el usuario Id {snapshot.Id} al estado del " +
                $"{fila.Fecha:g}?\n\n" +
                snapshot.DescripcionCorta() + "\n\n" +
                "La contraseña actual NO se modificará.\n" +
                "Esta operación quedará registrada como un rollback en la auditoría.",
                "Confirmar restauración",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp != DialogResult.Yes) return;

            try
            {
                UsuarioBLL.Instancia.Rollback(snapshot, fila.Fecha);
                MessageBox.Show(
                    "Rollback aplicado correctamente.\n" +
                    "El usuario fue restaurado al estado seleccionado.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarAuditorias();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo aplicar el rollback:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e) => Buscar();

        private void button2_Click(object sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Today.AddDays(-7);
            dtpHasta.Value = DateTime.Today;
            cmbUsuario.SelectedIndex = 0;
            cmbActividad.SelectedIndex = 0;
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
