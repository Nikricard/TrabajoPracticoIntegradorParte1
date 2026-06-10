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

        //actualiza controles del frm
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

        // Llena las dos listas:
        // clbAtomicos  -> solo permisos atómicos
        // clbCompuestos-> solo permisos compuestos
        // En los compuestos excluye el conjunto que se esté editando
        // para evitar la auto-referencia (que un conjunto se contenga a sí mismo).
        private void CargarSeleccionables(string codigoExcluir)
        {
            // Permisos atómicos
            clbAtomicos.Items.Clear();
            clbAtomicos.DisplayMember = "Nombre";
            foreach (PermisoAtomico p in PerfilBLL.Instancia.GetPermisosAtomicos())
                clbAtomicos.Items.Add(p);
            //recorre lista de permisos y los agrega

            // permisos compuestos, excluyendo el que se edita
            clbCompuestos.Items.Clear();
            clbCompuestos.DisplayMember = "Nombre";
            foreach (IPermiso p in PerfilBLL.Instancia.GetConjuntos())
                if (p.Codigo != codigoExcluir)
                    clbCompuestos.Items.Add(p);
        }

        private void CargarConjuntos()
        {
            dgvConjuntos.DataSource = null;
            dgvConjuntos.DataSource = PerfilBLL.Instancia.GetConjuntos();
            if (dgvConjuntos.Columns["Hijos"] != null)
                dgvConjuntos.Columns["Hijos"].Visible = false;  // oculta la columna de hijos para no mostrar la estructura interna
            // recarga el DataGridView con los conjuntos disponibles
        }

        private void dgvConjuntos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvConjuntos.SelectedRows.Count != 1) return;

            conjuntoSeleccionado = (IPermiso)dgvConjuntos.SelectedRows[0].DataBoundItem;
            // Si se selecciona un conjunto, carga su nombre y marca sus hijos en las listas.
            txtNombre.Text = conjuntoSeleccionado.Nombre;
            // Carga el nombre del conjunto en el TextBox

            // Recargar ambas listas excluyendo el propio conjunto
            CargarSeleccionables(conjuntoSeleccionado.Codigo);

            // Marca los hijos directos del conjunto seleccionado en las listas.
            var hijosDirectos = new HashSet<string>();
            // Solo marca los hijos directos, no los permisos anidados dentro de otros compuestos.
            if (conjuntoSeleccionado is PermisoCompuesto compuesto)
                foreach (IPermiso hijo in compuesto.Hijos)
                    hijosDirectos.Add(hijo.Codigo);//

            for (int i = 0; i < clbAtomicos.Items.Count; i++)//un for clasico que no hacia desde 1er año
            {
                IPermiso p = (IPermiso)clbAtomicos.Items[i];// Recorre los permisos atómicos y marca los
                                                            // que son hijos directos del conjunto seleccionado.
                clbAtomicos.SetItemChecked(i, hijosDirectos.Contains(p.Codigo));
                // Si el código del permiso atómico está en el conjunto de hijos directos, se marca como seleccionado.
            }

            for (int i = 0; i < clbCompuestos.Items.Count; i++)
            {
                IPermiso p = (IPermiso)clbCompuestos.Items[i];
                // Recorre los permisos compuestos y marca los que son hijos directos del conjunto seleccionado.
                clbCompuestos.SetItemChecked(i, hijosDirectos.Contains(p.Codigo));
                // Si el código del permiso compuesto está en el conjunto de hijos directos, se marca como seleccionado.
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
                    //mensaje personalizado en caso de error
                    return;
                }

                var codigos = RecogerMarcados();
                if (codigos.Count == 0)
                {
                    MessageBox.Show("Seleccione al menos un permiso.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //mensaje personalizado en caso de error
                    return;
                }

                PerfilBLL.Instancia.CrearConjunto(nombre, codigos);
                //creamos el conjunto con el nombre y los códigos seleccionados
                MessageBox.Show($"Conjunto '{nombre}' creado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                //mensaje personalizado de éxito
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
                //verificaciones
                if (codigos.Count == 0)
                {
                    MessageBox.Show("Seleccione al menos un permiso.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //llamamos a actualizar conjunto del BLL con el código del conjunto seleccionado,
                //el nuevo nombre y los códigos seleccionados
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

        // Junta los códigos marcados en las dos listas.
        private List<string> RecogerMarcados()
        {   // Recorre ambas listas de permisos (atómicos y compuestos) y junta los códigos de los seleccionados en una sola lista.
            var codigos = new List<string>();
            foreach (IPermiso p in clbAtomicos.CheckedItems)
                codigos.Add(p.Codigo);
            foreach (IPermiso p in clbCompuestos.CheckedItems)
                codigos.Add(p.Codigo);
            return codigos;
        }

        // Desmarca ambas listas (deja el nombre y el conjunto seleccionado).
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbAtomicos.Items.Count; i++)
                clbAtomicos.SetItemChecked(i, false);
            for (int i = 0; i < clbCompuestos.Items.Count; i++)
                clbCompuestos.SetItemChecked(i, false);
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = string.Empty;
            conjuntoSeleccionado = null;
            CargarSeleccionables(null);  // recarga ambas listas sin marcas
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this);
            Close();
        }
    }
}
