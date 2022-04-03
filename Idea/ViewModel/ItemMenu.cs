using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Controls;

namespace Idea.ViewModel
{
    public class ItemMenu
    {
        public ItemMenu(string header, List<SubItem> subItems)
        {
            Header = header;
            SubItems = subItems;
        }

        public ItemMenu(string header, UserControl screen)
        {
            Header = header;
            Screen = screen;
        }

        public string Header { get; private set; }
        public List<SubItem> SubItems { get; private set; }
        public UserControl Screen { get; private set; }
    }
}
