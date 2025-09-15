using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingRecipes.Model
{
    //I created this class in order to be able to accept user's inputs on Ingredient field and bind them!. Without this class I couldn't store Ingredient's observable collection list values, I couldn't validate whether the user
    //filled out the inputs etc.
    public class IngredientItem:INotifyPropertyChanged
    {
        //declaring variables
        private String name;


        //creating properties with OnPropertyChanged method!
        public string Name
        {
            get => name;
            set
            {
                if(name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));    
                }
            }
        }
        
        //creating a public event in order to alert UI for changed values
        public event PropertyChangedEventHandler PropertyChanged;



        //OnPropertyChanged method
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

    }
}
