using ABS;
using BE;
using BLL;
using BLL_;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TrabajoPracticoIntegrador15_4
{
    public partial class frmPermiso : Form, IObservadorIdioma
    {
        private readonly GestorIdioma gestor = GestorIdioma.Instancia;
        private IPermiso conjuntoSeleccionado = null;

        public frmPermiso()
        {
            InitializeComponent();
        }

        public void ActualizarIdioma(Idioma idioma)
            => TraductorUI.Traducir(this.Controls, idioma);

        private void frmPermiso_Load(object sender, EventArgs e)
        {
            gestor.Suscribir(this);
            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);

            CargarSeleccionables(null);
            CargarConjuntos();
        }

        private void frmPermiso_FormClosed(object sender, FormClosedEventArgs e)
            => gestor.Desuscribir(this);

        // Carga de datos 

        // Llena el CheckedListBox con atómicos + compuestos.
        // Excluye el conjunto que se esté editando (evita auto-referencia).

        private void CargarSeleccionables(string codigoExcluir)
        {
            clbPermisos.Items.Clear();
            clbPermisos.DisplayMember = "Nombre";
            foreach (IPermiso p in PerfilBLL.Instancia.GetSeleccionablesParaConjunto(codigoExcluir))
                clbPermisos.Items.Add(p);
        }

        private void CargarConjuntos()
        {
            dgvConjuntos.DataSource = null;
            dgvConjuntos.DataSource = PerfilBLL.Instancia.GetConjuntos();
            if (dgvConjuntos.Columns["Hijos"] != null)
                dgvConjuntos.Columns["Hijos"].Visible = false;
        }

        private void dgvConjuntos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvConjuntos.SelectedRows.Count != 1) return;

            conjuntoSeleccionado = (IPermiso)dgvConjuntos.SelectedRows[0].DataBoundItem;
            txtNombre.Text = conjuntoSeleccionado.Nombre;

            // Recargar seleccionables excluyendo el propio conjunto
            CargarSeleccionables(conjuntoSeleccionado.Codigo);

            // Marcar los permisos que ya tiene este conjunto
            for (int i = 0; i < clbPermisos.Items.Count; i++)
            {
                IPermiso p = (IPermiso)clbPermisos.Items[i];
                clbPermisos.SetItemChecked(i, conjuntoSeleccionado.TienePermiso(p.Codigo));
            }
        }

        // Crear 

        private void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                if (string.IsNullOrEmpty(nombre))
                {
                    MessageBox.Show("Ingrese un nombre para el conjunto.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var codigos = RecogerMarcados();
                if (codigos.Count == 0)
                {
                    MessageBox.Show("Seleccione al menos un permiso.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PerfilBLL.Instancia.CrearConjunto(nombre, codigos);
                MessageBox.Show($"Conjunto '{nombre}' creado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarFormulario();
                CargarConjuntos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Actualizar

        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
            if (conjuntoSeleccionado == null)
            {
                MessageBox.Show("Seleccione un conjunto para actualizar.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string nombre = txtNombre.Text.Trim();
                var codigos = RecogerMarcados();
                if (codigos.Count == 0)
                {
                    MessageBox.Show("Seleccione al menos un permiso.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PerfilBLL.Instancia.ActualizarConjunto(
                    conjuntoSeleccionado.Codigo, nombre, codigos);

                MessageBox.Show($"Conjunto '{nombre}' actualizado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarFormulario();
                CargarConjuntos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Eliminar 

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (conjuntoSeleccionado == null)
            {
                MessageBox.Show("Seleccione un conjunto para eliminar.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resp = MessageBox.Show(
                $"¿Eliminar el conjunto '{conjuntoSeleccionado.Nombre}'?\n" +
                "Se quitará de los usuarios y conjuntos que lo usen.",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp != DialogResult.Yes) return;

            try
            {
                PerfilBLL.Instancia.EliminarConjunto(
                    conjuntoSeleccionado.Codigo, conjuntoSeleccionado.Nombre);

                MessageBox.Show("Conjunto eliminado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarFormulario();
                CargarConjuntos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Auxiliares

        private List<string> RecogerMarcados()
        {
            var codigos = new List<string>();
            foreach (IPermiso p in clbPermisos.CheckedItems)
                codigos.Add(p.Codigo);
            return codigos;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbPermisos.Items.Count; i++)
                clbPermisos.SetItemChecked(i, false);
        }


        private void LimpiarFormulario()
        {
            txtNombre.Text = string.Empty;
            conjuntoSeleccionado = null;
            CargarSeleccionables(null);  // recarga la lista completa sin marcas
        }


        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this);
            Close();
        }

        
    }
}
