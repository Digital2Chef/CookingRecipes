using CookingRecipes.Model;
using CookingRecipes.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookingRecipes.ViewModel
{
    public class DashBoardViewModel

    {


        private MainViewModel mainVM;//creating this in order to access MainViewModel!

        public User CurrentUser => mainVM.CurrentUser;//accessing current logged in user!


        //iCommands
        public ICommand goToAddRecipeWindow { get;}
        public ICommand goToEditRecipeWindow { get;}
        
        public ICommand goToViewRecipesWindow { get;}
        public ICommand viewRecipesWindow { get;}
        public ICommand logout { get;}

        public ICommand profile { get;} 
        
        
        
        //constructor
        public DashBoardViewModel(MainViewModel mainVM) 
        {
            this.mainVM = mainVM;

            //display edit recipe window command
            goToAddRecipeWindow = new RelayCommand(o =>mainVM.CurrentView = new AddRecipeInputs(mainVM));


            //redirect to view recipes page
            goToViewRecipesWindow= new RelayCommand(o =>
            {
                if (areThereAnyRecipes()) 
                {
                    mainVM.CurrentView = new ViewRecipesView(mainVM);
                    }
            });

            //logout and redirect user on login page
            logout = new RelayCommand(o=> 
            {

                logoutUser();//logout method



            });

            //navigating to ProfileInfo window
            profile = new RelayCommand(o=>mainVM.CurrentView = new ProfileInfo(mainVM));
           
        }

        //method to visit add personal info window
        private void addPersonalInfoWindow()
        {
            mainVM.CurrentView = new ProfileInfo(mainVM);
        }

        //method to navigate in view recipes panel
        private bool areThereAnyRecipes()
        {
            if(RecipeRepository.Recipes.Count > 0)
            {
                return true;
            }
            else
            {

                MessageBox.Show("There aren't any recipes stored!","Error");
                return false;
            }
        }

        //logout method
        private void logoutUser()
        {
            if (agreeLogout())
            {
                mainVM.CurrentView = new LoginView(mainVM);
            }
            else
            {
                return;
            }
        }

        //logout confirmation method
        private bool agreeLogout()
        {
            var confirmLogout = MessageBox.Show("Do you want to logout?", "Attention", MessageBoxButton.YesNo);

            if(confirmLogout == MessageBoxResult.Yes)
            {
                return true;
            }
            return false;
        }
       
    }
}
