using BE;
using BLL;
using ABS;
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
    public partial class frmPerfil : Form, IObservadorIdioma
    {
        private Usuario usuarioSeleccionado = null;

        private readonly GestorIdioma gestor = GestorIdioma.Instancia;
        public void ActualizarIdioma(Idioma idioma)
        => TraductorUI.Traducir(this.Controls, idioma);

        public frmPerfil()
        {
            InitializeComponent();
        }

        private void frmPerfil_Load(object sender, EventArgs e)
        {
            gestor.Suscribir(this);
            if (gestor.IdiomaActivo != null)
                ActualizarIdioma(gestor.IdiomaActivo);  // Aplica idioma actual

            CargarPerfiles();
            CargarGrillaUsuarios();

        }

        private void CargarPerfiles()
        {
            List<Perfil> perfiles = PerfilBLL.Instancia.GetAllPerfiles();
            cmbPerfil.DataSource = perfiles;
            cmbPerfil.DisplayMember = "Nombre";
            cmbPerfil.ValueMember = "IdPerfil";
        }

        private void CargarGrillaUsuarios()
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = UsuarioBLL.Instancia.GetAll();

            // Ocultar columna de contraseña
            if (dgvUsuarios.Columns["Contraseña"] != null)
                dgvUsuarios.Columns["Contraseña"].Visible = false;
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count != 1) return;

            usuarioSeleccionado = (Usuario)dgvUsuarios.SelectedRows[0].DataBoundItem;
            txtNombre.Text = usuarioSeleccionado.Nombre;

        }

        private void cmbPerfil_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPerfil.SelectedItem is Perfil perfil)
            {
                MostrarArbolPermisos(perfil);
            }
        }

        private void MostrarArbolPermisos(Perfil perfil)
        {
            treePermisos.Nodes.Clear();

            if (perfil?.Permiso == null) return;

            TreeNode raiz = new TreeNode($"[{perfil.Permiso.Codigo}] {perfil.Permiso.Nombre}");
            raiz.Tag = perfil.Permiso;

            // Recorrido recursivo del árbol Composite
            AgregarNodosRecursivo(raiz, perfil.Permiso);

            treePermisos.Nodes.Add(raiz);
            treePermisos.ExpandAll();
        }

        private void AgregarNodosRecursivo(TreeNode nodo, IPermiso permiso)
        {
            if (permiso is PermisoCompuesto compuesto)
            {
                foreach (IPermiso hijo in compuesto.Hijos)
                {
                    TreeNode nodoHijo = new TreeNode($"[{hijo.Codigo}] {hijo.Nombre}");
                    nodoHijo.Tag = hijo;

                    // Ícono diferente para compuesto vs atómico
                    nodoHijo.ForeColor = hijo is PermisoCompuesto
                        ? System.Drawing.Color.DarkBlue
                        : System.Drawing.Color.DarkGreen;

                    nodo.Nodes.Add(nodoHijo);

                    // Llamada recursiva para seguir bajando en el árbol
                    AgregarNodosRecursivo(nodoHijo, hijo);
                }
            }
        }

        private void btnAsignar_Click(object sender, EventArgs e)
        {
            if (usuarioSeleccionado == null)
            {
                MessageBox.Show("Seleccione un usuario.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbPerfil.SelectedItem is not Perfil perfilSeleccionado)
            {
                MessageBox.Show("Seleccione un perfil.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Pasa nombre de usuario y nombre de perfil para la bitácora
                PerfilBLL.Instancia.AsignarPerfil(
                    usuarioSeleccionado.Id,
                    usuarioSeleccionado.Nombre,
                    perfilSeleccionado.IdPerfil,
                    perfilSeleccionado.Nombre
                );

                MessageBox.Show(
                    $"Perfil '{perfilSeleccionado.Nombre}' asignado a " +
                    $"'{usuarioSeleccionado.Nombre}'.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarGrillaUsuarios();
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
