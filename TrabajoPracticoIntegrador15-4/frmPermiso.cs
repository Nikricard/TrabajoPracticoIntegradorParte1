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
        private PermisoBase conjuntoSeleccionado = null;

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

            // permisos compuestos, excluyendo el que se edita
            clbCompuestos.Items.Clear();
            clbCompuestos.DisplayMember = "Nombre";
            foreach (PermisoBase p in PerfilBLL.Instancia.GetConjuntos())
                if (p.Codigo != codigoExcluir)
                    clbCompuestos.Items.Add(p);
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

            conjuntoSeleccionado = (PermisoBase)dgvConjuntos.SelectedRows[0].DataBoundItem;
            txtNombre.Text = conjuntoSeleccionado.Nombre;

            // Recargar ambas listas excluyendo el propio conjunto
            CargarSeleccionables(conjuntoSeleccionado.Codigo);

            // Marca los hijos directos del conjunto seleccionado en las listas.
            var hijosDirectos = new HashSet<string>();
            if (conjuntoSeleccionado is PermisoCompuesto compuesto)
                foreach (PermisoBase hijo in compuesto.Hijos)
                    hijosDirectos.Add(hijo.Codigo);

            for (int i = 0; i < clbAtomicos.Items.Count; i++)
            {
                PermisoBase p = (PermisoBase)clbAtomicos.Items[i];
                clbAtomicos.SetItemChecked(i, hijosDirectos.Contains(p.Codigo));
            }

            for (int i = 0; i < clbCompuestos.Items.Count; i++)
            {
                PermisoBase p = (PermisoBase)clbCompuestos.Items[i];
                clbCompuestos.SetItemChecked(i, hijosDirectos.Contains(p.Codigo));
            }

            // Mostrar el árbol del conjunto seleccionado
            MostrarArbolDelConjunto(conjuntoSeleccionado);
        }

        // Dibuja el árbol completo del conjunto, bajando recursivamente
        // por todos los hijos (atómicos y compuestos).
        // Usa la misma lógica que frmPerfil.MostrarArbolDelUsuario.
        private void MostrarArbolDelConjunto(PermisoBase raiz)
        {
            treePermisos.Nodes.Clear();
            if (!(raiz is PermisoCompuesto compuesto) || compuesto.Hijos.Count == 0)
                return;

            foreach (PermisoBase hijo in compuesto.Hijos)
            {
                TreeNode nodo = new TreeNode($"[{hijo.Codigo}] {hijo.Nombre}");
                nodo.ForeColor = hijo is PermisoCompuesto
                    ? System.Drawing.Color.DarkBlue
                    : System.Drawing.Color.DarkGreen;
                AgregarNodosRecursivo(nodo, hijo);
                treePermisos.Nodes.Add(nodo);
            }
            treePermisos.ExpandAll();
        }

        private void AgregarNodosRecursivo(TreeNode nodo, PermisoBase permiso)
        {
            if (permiso is PermisoCompuesto compuesto)
            {
                foreach (PermisoBase hijo in compuesto.Hijos)
                {
                    TreeNode nodoHijo = new TreeNode($"[{hijo.Codigo}] {hijo.Nombre}");
                    nodoHijo.ForeColor = hijo is PermisoCompuesto
                        ? System.Drawing.Color.DarkBlue
                        : System.Drawing.Color.DarkGreen;
                    nodo.Nodes.Add(nodoHijo);
                    AgregarNodosRecursivo(nodoHijo, hijo);
                }
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

        // Junta los códigos marcados en las dos listas.
        private List<string> RecogerMarcados()
        {
            var codigos = new List<string>();
            foreach (PermisoBase p in clbAtomicos.CheckedItems)
                codigos.Add(p.Codigo);
            foreach (PermisoBase p in clbCompuestos.CheckedItems)
                codigos.Add(p.Codigo);
            return codigos;
        }

        // Desmarca ambas listas y limpia el árbol.
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Clear();
            for (int i = 0; i < clbAtomicos.Items.Count; i++)
                clbAtomicos.SetItemChecked(i, false);
            for (int i = 0; i < clbCompuestos.Items.Count; i++)
                clbCompuestos.SetItemChecked(i, false);
            treePermisos.Nodes.Clear();
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = string.Empty;
            conjuntoSeleccionado = null;
            CargarSeleccionables(null);
            treePermisos.Nodes.Clear();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this);
            Close();
        }
    }
}
