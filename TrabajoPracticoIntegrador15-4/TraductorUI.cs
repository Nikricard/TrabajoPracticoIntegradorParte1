using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabajoPracticoIntegrador15_4
{
    public static class TraductorUI
    {
        // Esta clase siver para traducir todos los controles de un formulario que tengan Tag asignado.
        public static void Traducir(Control.ControlCollection controles, Idioma idioma)
        {
            foreach (Control c in controles)
            {
                if (c.Tag is string clave)
                    c.Text = idioma.Traducir(clave);

                // Si tiene controles hijos los recorre también
                if (c.Controls.Count > 0)
                    Traducir(c.Controls, idioma);

                // Si es un MenuStrip, recorre sus ítems
                if (c is MenuStrip menu)
                    TraducirMenu(menu.Items, idioma);
                
                //Si es un StatusStrip, recorre sus items
                if (c is StatusStrip status)
                    TraducirMenu(status.Items, idioma);
            }
        }

        public static void TraducirMenu(ToolStripItemCollection items, Idioma idioma)
        {
            foreach (ToolStripItem item in items)
            {
                // Solo traduce si el Tag es un string
                if (item.Tag is string clave)
                    item.Text = idioma.Traducir(clave);

                if (item is ToolStripMenuItem subMenu)
                    TraducirMenu(subMenu.DropDownItems, idioma);
            }
        }
    }
}
