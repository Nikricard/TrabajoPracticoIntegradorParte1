using BE;
using BLL;
using ABS;
using BLL_;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TrabajoPracticoIntegrador15_4
{
    public partial class frmPerfil : Form, IObservadorIdioma
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
            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);

            CargarListas();
            CargarGrillaUsuarios();
        }

        private void frmPerfil_FormClosed(object sender, FormClosedEventArgs e)
            => gestor.Desuscribir(this);

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
            foreach (IPermiso p in PerfilBLL.Instancia.GetConjuntos())
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
            List<string> codigos = PerfilBLL.Instancia.GetCodigosDeUsuario(idUsuario);

            for (int i = 0; i < clbAtomicos.Items.Count; i++)
            {
                IPermiso p = (IPermiso)clbAtomicos.Items[i];
                clbAtomicos.SetItemChecked(i, codigos.Contains(p.Codigo));
            }

            for (int i = 0; i < clbCompuestos.Items.Count; i++)
            {
                IPermiso p = (IPermiso)clbCompuestos.Items[i];
                clbCompuestos.SetItemChecked(i, codigos.Contains(p.Codigo));
            }
        }

        //Dibuja el árbol con los permisos efectivos del usuario
        private void MostrarArbolDelUsuario(int idUsuario)
        {
            treePermisos.Nodes.Clear();
            PermisoCompuesto raiz = PerfilBLL.Instancia.GetPermisosDeUsuario(idUsuario);

            if (raiz.Hijos.Count == 0) return;

            foreach (IPermiso permiso in raiz.Hijos)
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

        private void AgregarNodosRecursivo(TreeNode nodo, IPermiso permiso)
        {
            if (permiso is PermisoCompuesto compuesto)
            {
                foreach (IPermiso hijo in compuesto.Hijos)
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
                foreach (IPermiso p in clbAtomicos.CheckedItems)
                    codigos.Add(p.Codigo);
                foreach (IPermiso p in clbCompuestos.CheckedItems)
                    codigos.Add(p.Codigo);

                PerfilBLL.Instancia.GuardarPermisosDeUsuario(
                    usuarioSeleccionado.Id, usuarioSeleccionado.Nombre, codigos);

                MessageBox.Show(
                    $"Permisos actualizados para '{usuarioSeleccionado.Nombre}'.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                MostrarArbolDelUsuario(usuarioSeleccionado.Id);
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
            Close();
        }
    }
}
