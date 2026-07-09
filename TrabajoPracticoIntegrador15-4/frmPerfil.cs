using ABS;
using BE;
using BLL;
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

        // El observador de conjuntos: cuando desde frmPermiso se crea, actualiza
        // o elimina un conjunto, frmPerfil se refresca automáticamente.
        public void ActualizarConjuntos()
        {
            CargarSeleccionables();
            if (usuarioSeleccionado != null)
                MarcarPermisosDelUsuario(usuarioSeleccionado.Id);
        }

        private void frmPerfil_Load(object sender, EventArgs e)
        {
            gestor.Suscribir(this);
            PerfilBLL.Instancia.SuscribirConjuntos(this);

            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);

            CargarUsuarios();
            CargarSeleccionables();
        }

        private void frmPerfil_FormClosed(object sender, FormClosedEventArgs e)
        {
            gestor.Desuscribir(this);
            PerfilBLL.Instancia.DesuscribirConjuntos(this);
        }

        private void CargarUsuarios()
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = UsuarioBLL.Instancia.GetAll();
        }

        private void CargarSeleccionables()
        {
            // Atómicos
            clbAtomicos.Items.Clear();
            clbAtomicos.DisplayMember = "Nombre";
            foreach (PermisoAtomico p in PerfilBLL.Instancia.GetPermisosAtomicos())
                clbAtomicos.Items.Add(p);

            // Compuestos (conjuntos disponibles)
            clbCompuestos.Items.Clear();
            clbCompuestos.DisplayMember = "Nombre";
            foreach (PermisoBase p in PerfilBLL.Instancia.GetConjuntos())
                clbCompuestos.Items.Add(p);
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 1)
            {
                usuarioSeleccionado = (Usuario)dgvUsuarios.SelectedRows[0].DataBoundItem;
                if (usuarioSeleccionado != null)
                    MarcarPermisosDelUsuario(usuarioSeleccionado.Id);
            }
        }

        // Marca en las dos listas los permisos que el usuario ya tiene
        // asignados directamente, y dibuja el árbol completo (con recursión
        // en los compuestos) en el TreeView.
        private void MarcarPermisosDelUsuario(int idUsuario)
        {
            var codigosDirectos = new HashSet<string>(
                new DAL.UsuarioDAL().GetPermisosCodigos(idUsuario));

            for (int i = 0; i < clbAtomicos.Items.Count; i++)
            {
                var p = (PermisoBase)clbAtomicos.Items[i];
                clbAtomicos.SetItemChecked(i, codigosDirectos.Contains(p.Codigo));
            }

            for (int i = 0; i < clbCompuestos.Items.Count; i++)
            {
                var p = (PermisoBase)clbCompuestos.Items[i];
                clbCompuestos.SetItemChecked(i, codigosDirectos.Contains(p.Codigo));
            }

            MostrarArbolDelUsuario(idUsuario);
        }

        private void MostrarArbolDelUsuario(int idUsuario)
        {
            treePermisos.Nodes.Clear();

            var usuarioDAL = new DAL.UsuarioDAL();
            var codigos = usuarioDAL.GetPermisosCodigos(idUsuario);
            var atomicos = PerfilBLL.Instancia.GetPermisosAtomicos();
            var compuestos = PerfilBLL.Instancia.GetConjuntos();

            foreach (string codigo in codigos)
            {
                PermisoBase p = atomicos.Find(a => a.Codigo == codigo);
                if (p == null) p = compuestos.Find(c => c.Codigo == codigo);
                if (p == null) continue;

                TreeNode nodo = new TreeNode($"[{p.Codigo}] {p.Nombre}");
                nodo.ForeColor = p is PermisoCompuesto
                    ? System.Drawing.Color.DarkBlue
                    : System.Drawing.Color.DarkGreen;
                AgregarNodosRecursivo(nodo, p);
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

        // Guardar: reemplaza TODOS los permisos del usuario en una sola
        // operación con snapshot para poder hacer rollback después.
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
                // Recoger los códigos marcados en las dos listas.
                var codigos = new List<string>();
                foreach (PermisoBase p in clbAtomicos.CheckedItems)
                    codigos.Add(p.Codigo);
                foreach (PermisoBase p in clbCompuestos.CheckedItems)
                    codigos.Add(p.Codigo);

                // Una sola llamada: toma snapshot, reemplaza, audita, recalcula DVH.
                PerfilBLL.Instancia.ReemplazarPermisosDeUsuario(
                    usuarioSeleccionado.Id,
                    usuarioSeleccionado.Nombre,
                    codigos);

                MessageBox.Show(
                    "Permisos actualizados correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                MarcarPermisosDelUsuario(usuarioSeleccionado.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbAtomicos.Items.Count; i++)
                clbAtomicos.SetItemChecked(i, false);
            for (int i = 0; i < clbCompuestos.Items.Count; i++)
                clbCompuestos.SetItemChecked(i, false);
            treePermisos.Nodes.Clear();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            gestor.Desuscribir(this);
            PerfilBLL.Instancia.DesuscribirConjuntos(this);
            Close();
        }
    }
}
