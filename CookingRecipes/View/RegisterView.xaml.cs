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
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {


        public RegisterView(MainViewModel mainVM)
        {
            InitializeComponent();
            this.DataContext = new RegisterModel(mainVM);

        }
        


        //method to send input data into property's value through the event!
        private void PasswordRegister_PasswordChanged(object sender, RoutedEventArgs e)
        {
            

            if(DataContext is RegisterModel RM)
            {
                RM.Password = ((PasswordBox)sender).Password;
            }

        
        }



        //method to send input data into property's value through the event!
        private void PasswordConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
           

            if (DataContext is RegisterModel RM)
            {
                RM.ConfirmPass = ((PasswordBox)sender).Password;
            }

        
        }



      
    }
}
