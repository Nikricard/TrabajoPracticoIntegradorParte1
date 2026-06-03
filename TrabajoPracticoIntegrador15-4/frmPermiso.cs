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
    public partial class frmPermiso : Form, IObservadorIdioma
    {
        private readonly GestorIdioma gestor = GestorIdioma.Instancia;
        private Perfil conjuntoSeleccionado = null;

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

            CargarPermisosAtomicos();
            CargarConjuntos();

        }

        // Carga de datos
        private void CargarPermisosAtomicos()
        {
            clbPermisos.Items.Clear();
            clbPermisos.DisplayMember = "Nombre";   // muestra el Nombre del PermisoAtomico
            foreach (PermisoAtomico p in PerfilBLL.Instancia.GetPermisosAtomicos())
                clbPermisos.Items.Add(p);
        }

        private void CargarConjuntos()
        {
            dgvConjuntos.DataSource = null;
            dgvConjuntos.DataSource = PerfilBLL.Instancia.GetAllPerfiles();

            // Ocultar la columna del árbol
            if (dgvConjuntos.Columns["Permiso"] != null)
                dgvConjuntos.Columns["Permiso"].Visible = false;
        }

        private void dgvConjuntos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvConjuntos.SelectedRows.Count != 1) return;

            conjuntoSeleccionado = (Perfil)dgvConjuntos.SelectedRows[0].DataBoundItem;
            txtNombre.Text = conjuntoSeleccionado.Nombre;

            // Marca en el CheckedListBox los permisos que tiene el conjunto
            for (int i = 0; i < clbPermisos.Items.Count; i++)
            {
                PermisoAtomico p = (PermisoAtomico)clbPermisos.Items[i];
                clbPermisos.SetItemChecked(i, conjuntoSeleccionado.TienePermiso(p.Codigo));
            }

        }
        // Crear conjunto 

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

                // Recoge los códigos de los permisos tildados
                var codigos = new List<string>();
                foreach (PermisoAtomico p in clbPermisos.CheckedItems)
                    codigos.Add(p.Codigo);

                if (codigos.Count == 0)
                {
                    MessageBox.Show("Seleccione al menos un permiso.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PerfilBLL.Instancia.CrearConjunto(nombre, codigos);

                MessageBox.Show($"Conjunto '{nombre}' creado y disponible para asignar.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarFormulario();
                CargarConjuntos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // Eliminar conjunto

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
                "Los usuarios que lo tengan asignado quedarán sin perfil.",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp != DialogResult.Yes) return;

            try
            {
                PerfilBLL.Instancia.EliminarConjunto(
                    conjuntoSeleccionado.IdPerfil,
                    conjuntoSeleccionado.Nombre,
                    conjuntoSeleccionado.Permiso?.Codigo
                );

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

        // Limpiar

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = string.Empty;
            conjuntoSeleccionado = null;
            for (int i = 0; i < clbPermisos.Items.Count; i++)
                clbPermisos.SetItemChecked(i, false);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
            gestor.Desuscribir(this);
        }
    }
}
