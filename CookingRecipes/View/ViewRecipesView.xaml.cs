using CookingRecipes.ViewModel;
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

namespace CookingRecipes.View
{
    /// <summary>
    /// Interaction logic for ViewRecipesView.xaml
    /// </summary>
    public partial class ViewRecipesView : UserControl
    {
        public ViewRecipesView(MainViewModel mainVM)
        {
            InitializeComponent();
            this.DataContext = new ViewRecipesModel(mainVM);
            
        }

    }
}
