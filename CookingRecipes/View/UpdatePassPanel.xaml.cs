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
    /// Interaction logic for UpdatePassPanel.xaml
    /// </summary>
    public partial class UpdatePassPanel : UserControl
    {
        public UpdatePassPanel(MainViewModel mainVM)
        {
            InitializeComponent();
            this.DataContext = new UpdatePassViewModel(mainVM);//accessing view model for this window!
        }



        //method to pass from password box the values into properties on viewmodel!
        private void oldPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UpdatePassViewModel UP)
            {
                UP.OldPass = ((PasswordBox)sender).Password;
            }
        }

        //method to pass from password box the values into properties on viewmodel!
        private void newPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UpdatePassViewModel UP)
            {
                UP.Password = ((PasswordBox)sender).Password;
            }
        }



        //method to pass from password box the values into properties on viewmodel!
        private void confirm_pass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UpdatePassViewModel UP)
            {
                UP.ConfirmPass = ((PasswordBox)sender).Password;
            }
        }
    }
}
