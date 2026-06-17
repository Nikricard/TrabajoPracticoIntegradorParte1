using BE;
using BLL;
using ABS;
using BLL_;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TrabajoPracticoIntegrador15_4
{
    public partial class frmPerfil : Form, IObservadorIdioma, IObservadorConjuntos
    {
        private readonly GestorIdioma gestor = GestorIdioma.Instancia;
        private Usuario usuarioSeleccionado = null;

        public frmPerfil()
        {
            InitializeComponent();
        }

        public void ActualizarIdioma(Idioma idioma)
            => TraductorUI.Traducir(this.Controls, idioma);

        private void frmPerfil_Load(object sender, EventArgs e)
        {
            gestor.Suscribir(this);
            PerfilBLL.Instancia.SuscribirConjuntos(this);   // escucha cambios de conjuntos
            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);

            CargarListas();
            CargarGrillaUsuarios();
        }

        private void frmPerfil_FormClosed(object sender, FormClosedEventArgs e)
        {
            gestor.Desuscribir(this);
            PerfilBLL.Instancia.DesuscribirConjuntos(this);
        }

        // Observer de conjuntos: lo dispara frmPermiso al crear/modificar/eliminar un conjunto.
        // Refresca las listas y, si hay un usuario seleccionado, reaplica sus marcas y el árbol.
        public void ActualizarConjuntos()
        {
            CargarListas();
            if (usuarioSeleccionado != null)
            {
                MarcarPermisosDelUsuario(usuarioSeleccionado.Id);
                MostrarArbolDelUsuario(usuarioSeleccionado.Id);
            }
        }

        // Carga inicial

        private void CargarListas()
        {
            // Atómicos
            clbAtomicos.Items.Clear();
            clbAtomicos.DisplayMember = "Nombre";
            foreach (PermisoAtomico p in PerfilBLL.Instancia.GetPermisosAtomicos())
                clbAtomicos.Items.Add(p);

            // Compuestos (conjuntos)
            clbCompuestos.Items.Clear();
            clbCompuestos.DisplayMember = "Nombre";
            foreach (PermisoBase p in PerfilBLL.Instancia.GetConjuntos())
                clbCompuestos.Items.Add(p);
        }

        private void CargarGrillaUsuarios()
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = UsuarioBLL.Instancia.GetAll();
            if (dgvUsuarios.Columns["Contraseña"] != null)
                dgvUsuarios.Columns["Contraseña"].Visible = false;
        }

        // Selección de usuario 

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count != 1) return;

            usuarioSeleccionado = (Usuario)dgvUsuarios.SelectedRows[0].DataBoundItem;
            txtNombre.Text      = usuarioSeleccionado.Nombre;

            MarcarPermisosDelUsuario(usuarioSeleccionado.Id);
            MostrarArbolDelUsuario(usuarioSeleccionado.Id);
        }

        //Tilda en ambas listas los permisos que tiene el usuario
        private void MarcarPermisosDelUsuario(int idUsuario)
        {
            // obtiene los códigos de permisos del usuario y tilda los que correspondan en las listas
            List<string> codigos = PerfilBLL.Instancia.GetCodigosDeUsuario(idUsuario);
            
            // recorre ambas listas y marca los permisos que el usuario tiene según los códigos obtenidos
            for (int i = 0; i < clbAtomicos.Items.Count; i++)
            {
                PermisoBase p = (PermisoBase)clbAtomicos.Items[i];
                clbAtomicos.SetItemChecked(i, codigos.Contains(p.Codigo));
            }
            // lo mismo pero para compuestos
            for (int i = 0; i < clbCompuestos.Items.Count; i++)
            {
                PermisoBase p = (PermisoBase)clbCompuestos.Items[i];
                clbCompuestos.SetItemChecked(i, codigos.Contains(p.Codigo));
            }
        }

        //Dibuja el árbol con los permisos efectivos del usuario
        private void MostrarArbolDelUsuario(int idUsuario)
        {
            treePermisos.Nodes.Clear();
            PermisoCompuesto raiz = PerfilBLL.Instancia.GetPermisosDeUsuario(idUsuario);

            if (raiz.Hijos.Count == 0) return;

            foreach (PermisoBase permiso in raiz.Hijos)
            {
                TreeNode nodo = new TreeNode($"[{permiso.Codigo}] {permiso.Nombre}");
                nodo.ForeColor = permiso is PermisoCompuesto
                    ? System.Drawing.Color.DarkBlue
                    : System.Drawing.Color.DarkGreen;
                AgregarNodosRecursivo(nodo, permiso);
                treePermisos.Nodes.Add(nodo);
            }
            treePermisos.ExpandAll();
        }

        private void AgregarNodosRecursivo(TreeNode nodo, PermisoBase permiso)
        {
            if (permiso is PermisoCompuesto compuesto)
            {
                // Si el permiso es un compuesto, recorre sus hijos y los agrega como nodos hijos del nodo actual.
                foreach (PermisoBase hijo in compuesto.Hijos)
                {
                    // Crea un nodo para el hijo y lo agrega al nodo actual,
                    // luego llama recursivamente para agregar los hijos del hijo.
                    TreeNode nodoHijo = new TreeNode($"[{hijo.Codigo}] {hijo.Nombre}");
                    //los compuestos en azul oscuro y los atómicos en verde oscuro
                    nodoHijo.ForeColor = hijo is PermisoCompuesto
                        ? System.Drawing.Color.DarkBlue
                        : System.Drawing.Color.DarkGreen;
                    // Agrega el nodo hijo al nodo actual
                    nodo.Nodes.Add(nodoHijo);
                    // Llama recursivamente para agregar los hijos del hijo
                    AgregarNodosRecursivo(nodoHijo, hijo);
                }
            }
        }

        // Guardar permisos del usuario

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (usuarioSeleccionado == null)
            {
                MessageBox.Show("Seleccione un usuario.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // junta los códigos tildados en las 2 listas
                var codigos = new List<string>();
                //lista de atomicos
                foreach (PermisoBase p in clbAtomicos.CheckedItems)
                    codigos.Add(p.Codigo);
                //lista de compuestos
                foreach (PermisoBase p in clbCompuestos.CheckedItems)
                    codigos.Add(p.Codigo);

                PerfilBLL.Instancia.GuardarPermisosDeUsuario(
                    usuarioSeleccionado.Id, usuarioSeleccionado.Nombre, codigos);

                MessageBox.Show($"Permisos actualizados para '{usuarioSeleccionado.Nombre}'.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // refresca las marcas y el árbol con los datos guardados
                MostrarArbolDelUsuario(usuarioSeleccionado.Id);
            }
            catch (Exception ex)
            {
                //mensaje personalizado
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this);
            PerfilBLL.Instancia.DesuscribirConjuntos(this);
            Close();
        }
    }
}