using BE;
using BLL_;
using ABS;
using BLL;
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
    public partial class frmIdioma : Form, IObservadorIdioma
    {

        private readonly GestorIdioma gestor = GestorIdioma.Instancia;
        private Idioma idiomaSeleccionado = null;

        public frmIdioma()
        {
            InitializeComponent();
        }

        public void ActualizarIdioma(Idioma idioma)
        {
            // Este formulario también se traduce si cambia el idioma
            foreach (Control c in this.Controls)
                if (c.Tag != null)
                    c.Text = idioma.Traducir(c.Tag.ToString());
        }

        private void frmIdioma_Load(object sender, EventArgs e)
        {
            gestor.Suscribir(this);
            CargarGrillaIdiomas();

            // Columnas de la grilla de traducciones
            dgvTraducciones.Columns.Clear();
            dgvTraducciones.Columns.Add("Clave", "Clave (Tag)");
            dgvTraducciones.Columns.Add("Traduccion", "Traducción");
            dgvTraducciones.Columns[0].Width = 160;
            dgvTraducciones.Columns[1].Width = 220;
            dgvTraducciones.Columns[0].ReadOnly = true;

        }

        private void CargarGrillaIdiomas()
        {
            dgvIdiomas.DataSource = null;
            dgvIdiomas.DataSource = gestor.IdiomasDisponibles;
            dgvIdiomas.Columns["Traducciones"].Visible = false; // ocultar diccionario
        }

        private void dgvIdiomas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvIdiomas.SelectedRows.Count != 1) return;

            idiomaSeleccionado = (Idioma)dgvIdiomas.SelectedRows[0].DataBoundItem;

            // Autocompleta los textboxes del idioma
            txtNombre.Text = idiomaSeleccionado.Nombre;
            chkDefecto.Checked = idiomaSeleccionado.Defecto;

            // Carga sus traducciones en la grilla inferior
            CargarGrillaTraducciones();

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                if (string.IsNullOrEmpty(nombre))
                {
                    MessageBox.Show("Ingrese un nombre para el idioma.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Recoge las traducciones de la grilla inferior
                var traducciones = new List<(string clave, string valor)>();
                foreach (DataGridViewRow row in dgvTraducciones.Rows)
                {
                    if (row.IsNewRow) continue;
                    string clave = row.Cells["Clave"].Value?.ToString() ?? "";
                    string valor = row.Cells["Traduccion"].Value?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(clave) && !string.IsNullOrEmpty(valor))
                        traducciones.Add((clave, valor));
                }

                gestor.AgregarIdioma(nombre, chkDefecto.Checked, traducciones);
                MessageBox.Show($"Idioma '{nombre}' agregado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarIdioma();
                CargarGrillaIdiomas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idiomaSeleccionado == null) return;

            var resp = MessageBox.Show(
                $"¿Eliminar el idioma '{idiomaSeleccionado.Nombre}' y todas sus traducciones?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp != DialogResult.Yes) return;

            try
            {
                gestor.EliminarIdioma(idiomaSeleccionado.IdIdioma);
                LimpiarIdioma();
                CargarGrillaIdiomas();
                dgvTraducciones.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void LimpiarIdioma()
        {
            txtNombre.Text = string.Empty;
            chkDefecto.Checked = false;
            idiomaSeleccionado = null;
        }

        private void CargarGrillaTraducciones()
        {
            dgvTraducciones.Rows.Clear();
            if (idiomaSeleccionado == null) return;

            foreach (var kv in idiomaSeleccionado.Traducciones)
                dgvTraducciones.Rows.Add(kv.Key, kv.Value);
        }

        private void dgvTraducciones_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTraducciones.SelectedRows.Count != 1) return;
            var row = dgvTraducciones.SelectedRows[0];
            txtClave.Text = row.Cells["Clave"].Value?.ToString() ?? "";
            txtValor.Text = row.Cells["Traduccion"].Value?.ToString() ?? "";

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (idiomaSeleccionado == null)
            {
                MessageBox.Show("Seleccione primero un idioma.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string clave = txtClave.Text.Trim();
            string valor = txtValor.Text.Trim();

            if (string.IsNullOrEmpty(clave) || string.IsNullOrEmpty(valor))
            {
                MessageBox.Show("Complete clave y traducción.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                gestor.GuardarTraduccion(idiomaSeleccionado.IdIdioma, clave, valor);
                MessageBox.Show("Traducción guardada.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresca la grilla de traducciones
                idiomaSeleccionado = gestor.IdiomasDisponibles.Find(i => i.IdIdioma == idiomaSeleccionado.IdIdioma);
                CargarGrillaTraducciones();
                txtClave.Text = string.Empty;
                txtValor.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this);
            this.Close();
        }
    }
}
