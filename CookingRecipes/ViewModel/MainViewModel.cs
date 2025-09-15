using CookingRecipes.Model;
using CookingRecipes.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingRecipes.ViewModel
{
    public class MainViewModel:INotifyPropertyChanged
    {
        //creating an object from class User
        private User currentUser;

        //event handler 
        public event PropertyChangedEventHandler PropertyChanged;

        //declaring variables
        private object currentView;


        //properties
        public object CurrentView
        {
            get=> currentView;
            set{
                if(currentView != value)
                {
                    currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
               }
        }


        public User  CurrentUser
        {
            get => currentUser;
            set
            {
                if (currentUser != value)
                {

                    currentUser = value;
                    OnPropertyChanged(nameof(CurrentUser));

                }
            }
        }


        //creating a constructor 
        public MainViewModel()
        {
            CurrentView = new LoginView(this);
        }



        //creating OnPropertyChanged method
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
