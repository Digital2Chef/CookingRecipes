using CookingRecipes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingRecipes
{
    public class RecipeRepository
    {
        //creating an observable collection list
        public static  ObservableCollection<Recipe> Recipes { get; set; } = new ObservableCollection<Recipe>();
        
        //contructor
        public RecipeRepository() { }
    }
}
