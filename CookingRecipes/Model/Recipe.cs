using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingRecipes.Model
{
 public  class Recipe:INotifyPropertyChanged

    {
        public event PropertyChangedEventHandler PropertyChanged;//event handler for on property change method
       
        
          protected void OnPropertyChanged(string propertyName)//method to change properties on live mode in UI
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        //declaring variables
        private String food;
        private ObservableCollection<IngredientItem> ingredients = new ObservableCollection<IngredientItem>();
        private int ingredientsNumber;
        private String instructions;
        private String category;
        private String description;
        private TimeSpan cookingTime;
        private String difficulty;



        // Helper property to display it as a string!
        public string IngredientsText => string.Join(", ", Ingredients.Select(i => i.Name));

        //inNotifyOnPropertyChanged properties

        public string Food
        {
            get => food;
            set
            {
                if (food != value)
                { 
                 food = value;
                    OnPropertyChanged(nameof(Food));
                }
            }
        }

        public ObservableCollection<IngredientItem> Ingredients
        {
            get => ingredients;
            set
            {
                if (ingredients != value)
                {
                    ingredients = value;
                    OnPropertyChanged(nameof(Ingredients));
                }
            }
        }

        public String Instructions
        {
            get => instructions;
            set
            {
                if(instructions != value)
                {
                    instructions = value;
                    OnPropertyChanged(nameof(Instructions));
                }
            }
        }

        public int IngredientsNumber
        {
            get => ingredientsNumber;
            set
            {
                if(ingredientsNumber != value)
                {
                    ingredientsNumber = value;
                    OnPropertyChanged(nameof(IngredientsNumber));
                }
            }
        }

        public string Category
        {
            get => category;
            set
            {
             if(category != value)
                {
                    category = value;
                    OnPropertyChanged(nameof(Category));
                }   
            }
        }


        public string Description
        {
            get => description;
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public TimeSpan CookingTime
        {
            get => cookingTime;
            set
            {
                if (cookingTime != value)
                {
                    cookingTime = value;
                    OnPropertyChanged(nameof(CookingTime));

                }
            }
        }

        public string Difficulty
        {
            get => difficulty;
            set
            {
                if (difficulty != value)
                {
                    difficulty = value;
                    OnPropertyChanged(nameof(Difficulty));
                }

            }
        }

        //public constructor
        public Recipe() 
        {
            //subscribing into collection changed event of observable collection
            Ingredients.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(IngredientsText));
                IngredientsNumber = Ingredients.Count;//automatically updating Ingredient's number count!
            };
        }


        //toString method
        public override string ToString()
        {
            return $"Food:{Food}\n" +
                   $"IngredientsNumber:{IngredientsNumber}\n" +
                   $"Ingredients{Ingredients}\n" +
                   $"Instructions:{Instructions}";
                
        }

    
    }
}
