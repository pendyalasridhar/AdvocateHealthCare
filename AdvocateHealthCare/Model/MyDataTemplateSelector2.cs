
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AdvocateHealthCare.Model
{
    public class MyDataTemplateSelector2 : DataTemplateSelector
    {
        public DataTemplate FirstItemStyle { get; set; }
        public DataTemplate ItemStyle { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {

            var grdviewitem = item as QuestionsPage.QuestionsHelper;

            if (grdviewitem._id == "1")
            {
                return FirstItemStyle;
            }
            else
            {
                return ItemStyle;
            }
            // return base.SelectTemplateCore(item, container);
        }
    }
}

