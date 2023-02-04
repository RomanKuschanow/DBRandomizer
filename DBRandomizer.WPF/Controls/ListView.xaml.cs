using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBRandomizer.Controls
{
    /// <summary>
    /// Логика взаимодействия для CollumnDataSet.xaml
    /// </summary>
    public partial class ListView : UserControl
    {
        double size = 0;

        public ListView()
        {
            InitializeComponent();
            grid.DataContext = this;
        }


        #region Propertys

        public VerticalAlignment Aligment
        {
            get => (VerticalAlignment)GetValue(AligmentProperty);
            set => SetValue(AligmentProperty, value);
        }

        public static readonly DependencyProperty AligmentProperty = DependencyProperty.Register("Aligment", typeof(VerticalAlignment), typeof(ListView));

        public object ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(ListView));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ListView));

        #endregion

        private void ItemsControl_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            if (((ItemsControl)sender).ActualHeight > size)
                //scroll.ScrollToEnd();
            size = ((ItemsControl)sender).ActualHeight;
        }
    }
}
