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
        private Palabra tagSeleccionada = null;
        private bool cargando = false;


        public frmIdioma()
        {
            InitializeComponent();
        }

        /*public void ActualizarIdioma(Idioma idioma)
        {
            // Este formulario también se traduce si cambia el idioma
            foreach (Control c in this.Controls)
            {
                if (c.Tag != null)
                {
                    c.Text = idioma.Traducir(c.Tag.ToString());
                }
            }
        }*/

        public void ActualizarIdioma(Idioma idioma)
            => TraductorUI.Traducir(this.Controls, idioma);


        private void frmIdioma_Load(object sender, EventArgs e)
        {
            gestor.Suscribir(this);

            // Columnas de traducciones
            dgvTraducciones.Columns.Clear();
            dgvTraducciones.Columns.Add("Clave", "Clave (Tag)");
            dgvTraducciones.Columns.Add("Traduccion", "Traducción");
            dgvTraducciones.Columns[0].Width = 160;
            dgvTraducciones.Columns[1].Width = 220;
            dgvTraducciones.Columns[0].ReadOnly = true;

            btnAgregarTag.Enabled = PerfilBLL.Instancia.TienePermiso(PerfilBLL.Permisos.AgregarTags);
            btnEliminarTag.Enabled = PerfilBLL.Instancia.TienePermiso(PerfilBLL.Permisos.EliminarTags);

            CargarGrillaTags();
            CargarGrillaIdiomas();

            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);

        }

        // Seccion Tags

        private void CargarGrillaTags()
        {
            cargando = true;
            dgvTags.DataSource = null;
            dgvTags.DataSource = gestor.GetPalabras();
            cargando = false;
        }

        private void dgvTags_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            if (dgvTags.SelectedRows.Count != 1) return;

            tagSeleccionada = (Palabra)dgvTags.SelectedRows[0].DataBoundItem;
            txtTag.Text = tagSeleccionada.Texto;

        }

        private void btnAgregarTag_Click(object sender, EventArgs e)
        {
            try
            {
                string texto = txtTag.Text.Trim();
                if (string.IsNullOrEmpty(texto))
                {
                    MessageBox.Show("Ingrese el nombre de la tag.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Crea la tag y la asigna a todos los idiomas automáticamente
                gestor.AgregarPalabra(texto);

                MessageBox.Show($"Tag '{texto}' agregada a todos los idiomas.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtTag.Text = string.Empty;
                CargarGrillaTags();

                // Refrescar traducciones si hay un idioma seleccionado
                if (idiomaSeleccionado != null) CargarGrillaTraducciones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnEliminarTag_Click(object sender, EventArgs e)
        {
            if (tagSeleccionada == null)
            {
                MessageBox.Show("Seleccione una tag para eliminar.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resp = MessageBox.Show(
                $"¿Eliminar la tag '{tagSeleccionada.Texto}' y sus traducciones en todos los idiomas?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp != DialogResult.Yes) return;

            try
            {
                gestor.EliminarPalabra(tagSeleccionada.IdPalabra);
                tagSeleccionada = null;
                txtTag.Text = string.Empty;
                CargarGrillaTags();
                if (idiomaSeleccionado != null) CargarGrillaTraducciones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // Seccion Idiomas


        private void CargarGrillaIdiomas()
        {
            cargando = true;
            dgvIdiomas.DataSource = null;
            dgvIdiomas.DataSource = gestor.IdiomasDisponibles;
            if (dgvIdiomas.Columns["Traducciones"] != null)
                dgvIdiomas.Columns["Traducciones"].Visible = false;
            cargando = false;
        }


        private void dgvIdiomas_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            if (dgvIdiomas.SelectedRows.Count != 1) return;

            idiomaSeleccionado = (Idioma)dgvIdiomas.SelectedRows[0].DataBoundItem;
            txtNombre.Text = idiomaSeleccionado.Nombre;
            chkDefecto.Checked = idiomaSeleccionado.Defecto;

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

                // Crea el idioma y le asigna TODAS las tags automáticamente
                gestor.AgregarIdioma(nombre, chkDefecto.Checked);

                MessageBox.Show($"Idioma '{nombre}' agregado con todas las tags.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarIdioma();
                CargarGrillaIdiomas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idiomaSeleccionado == null)
            {
                MessageBox.Show("Seleccione un idioma para eliminar.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnLimpiarSeleccion_Click(object sender, EventArgs e)
        {
            LimpiarIdioma();
            dgvTraducciones.Rows.Clear();
        }

        private void LimpiarIdioma()
        {
            txtNombre.Text = string.Empty;
            chkDefecto.Checked = false;
            idiomaSeleccionado = null;
        }
        
        // Seccion Traducciones

        private void CargarGrillaTraducciones()
        {
            dgvTraducciones.Rows.Clear();
            if (idiomaSeleccionado == null) return;

            // Muestra todas las tags; si no hay traducción, queda vacía
            foreach (Palabra tag in gestor.GetPalabras())
            {
                idiomaSeleccionado.Traducciones.TryGetValue(tag.Texto, out string valor);
                dgvTraducciones.Rows.Add(tag.Texto, valor ?? string.Empty);
            }
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

                idiomaSeleccionado = gestor.IdiomasDisponibles
                    .Find(i => i.IdIdioma == idiomaSeleccionado.IdIdioma);
                CargarGrillaTraducciones();
                txtClave.Text = string.Empty;
                txtValor.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this);
            this.Close();
        }

    }
}
