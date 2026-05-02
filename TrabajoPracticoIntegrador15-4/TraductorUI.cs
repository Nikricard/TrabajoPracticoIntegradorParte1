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
        /// <summary>
        /// Traduce todos los controles de un formulario que tengan Tag asignado.
        /// También traduce ítems de MenuStrip si el formulario tiene uno.
        /// </summary>
        public static void Traducir(Control.ControlCollection controles, Idioma idioma)
        {
            foreach (Control c in controles)
            {
                if (c.Tag != null)
                    c.Text = idioma.Traducir(c.Tag.ToString());

                // Si tiene controles hijos (panels, groupbox, etc), los recorre también
                if (c.Controls.Count > 0)
                    Traducir(c.Controls, idioma);

                // Si es un MenuStrip, recorre sus ítems
                if (c is MenuStrip menu)
                    TraducirMenu(menu.Items, idioma);
            }
        }

        public static void TraducirMenu(ToolStripItemCollection items, Idioma idioma)
        {
            foreach (ToolStripItem item in items)
            {
                if (item.Tag != null)
                    item.Text = idioma.Traducir(item.Tag.ToString());

                if (item is ToolStripMenuItem subMenu)
                    TraducirMenu(subMenu.DropDownItems, idioma);
            }
        }
    }
}
