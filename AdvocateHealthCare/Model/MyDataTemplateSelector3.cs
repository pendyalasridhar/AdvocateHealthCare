using MyToolkit.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AdvocateHealthCare.Model
{
    public class MyDataTemplateSelector3 : Windows.UI.Xaml.Controls.DataTemplateSelector
    {
        public DataTemplate FirstItemStyle { get; set; }
        public DataTemplate ItemStyle { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {

            var grdviewitem = item as JournalPage.Journalinfo;

            if (grdviewitem._id == "1")
            {
                return FirstItemStyle;
            }
            else
            {
                return ItemStyle;
            }
        }
    }
}