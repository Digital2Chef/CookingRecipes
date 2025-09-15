using CookingRecipes.Model;
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
    /// Interaction logic for EditRecipe.xaml
    /// </summary>
    public partial class EditRecipe : UserControl
    {
        public EditRecipe(MainViewModel mainVM, Recipe selectedRecipe)
        {
            InitializeComponent();
            this.DataContext =new EditRecipeViewModel(mainVM, selectedRecipe);
        }
    }
}
