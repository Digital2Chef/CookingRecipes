using CookingRecipes.Model;
using CookingRecipes.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CookingRecipes.ViewModel
{
    //I have to fix the bug with ingredients number!
    public  class AddRecipeViewModel : INotifyPropertyChanged
    {
        //declaring variables
        private String food;
        private ObservableCollection<IngredientItem> ingredients = new ObservableCollection<IngredientItem>();

        private int ingredientsNumber;
        private String instructions;
        private String category;
        private String description;
        private String difficulty;
        private TimeSpan cookingTime;


        //creating an object from Recipe class
        Recipe newRecipe = new Recipe();

        private MainViewModel mainVM;//creating a mainVM object referrence!

      

        //public event to alert UI for changes
        public event PropertyChangedEventHandler PropertyChanged;

        //method to alterate UI
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //properties
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

        public  ObservableCollection<IngredientItem> Ingredients
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
                if (instructions != value)
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
                if (ingredientsNumber != value)
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
                if (category != value)
                {
                    category = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }

        public string Difficulty
        {
            get => difficulty;
            set
            {
                if(difficulty != value)
                {
                    difficulty = value;
                    OnPropertyChanged(nameof(Difficulty));

                }
            }
        }

        public TimeSpan CookingTime
        {
            get => cookingTime;
            set
            {
                if(cookingTime != value)
                {
                    cookingTime = value;
                    OnPropertyChanged(nameof(CookingTime));

                }
            }
        }

        //Relay command to create an event when submit and cancel btn clicks
        public ICommand submitRecipe { get; }
           
        public ICommand cancel { get; }

        //command to spaw a new text box to add multiple ingredients!
        public ICommand addMoreIngredientTextBoxes { get; }




        //creating a constructor
        public AddRecipeViewModel(MainViewModel mainVM) 
        {
            this.mainVM = mainVM;

            //    Ingredients = new ObservableCollection<string>() { string.Empty };//initializing Ingredient's list!
            Ingredients = new ObservableCollection<IngredientItem>();

            //cancel command
            cancel = new RelayCommand(o=> mainVM.CurrentView = new DashBoardView(mainVM));

            //spawing text boxes command
            addMoreIngredientTextBoxes = new RelayCommand(o => spawnIngredientTextBoxes());
            
            //submit recipe command
            submitRecipe = new RelayCommand(o =>
            {
                if (submit())
                {
                    mainVM.CurrentView = new DashBoardView(mainVM);
                }
            });

        }



        //Method to add mpore ingredient's text boxes
        private void spawnIngredientTextBoxes()
        {
            Ingredients.Add(new IngredientItem { Name = string.Empty});

        }

        //method to submit recipe
        private bool submit()
        {
            if (areInputsFilled() && confirmRecipe()) //if user complete all the inputs and confirms recipe!
            {
             assignValues();//method to assign values in the object's instance!
                return true;
            }
            else
            {
                return false;
            }
        }

        //method to assign values in the object!
        private void assignValues()
        {
          

            var recipe = new Recipe
            {
                Food = Food,
                Description = Description,
                Instructions = Instructions,
                Category = Category,
                Ingredients = new ObservableCollection<IngredientItem>(Ingredients),
                IngredientsNumber = IngredientsNumber,
                CookingTime = CookingTime,
                Difficulty = Difficulty
            };
            RecipeRepository.Recipes.Add(recipe);
        }

    



        //confirm recipe method
        private bool confirmRecipe()
        {

            string ingredientsText = string.Join(",", Ingredients.Select(i=> i.Name));      

            //recipe confirmation
            var confirm = MessageBox.Show($"Do you want to save the following recipe?\nFood:{Food}\nDescription:{Description}\nCategory:{Category}\nIngredients number:{IngredientsNumber}\nIngredients:{ingredientsText}" +
                $"\nInstructions:{Instructions}\nCooking time:{CookingTime} minutes\nDifficulty:{Difficulty}","Attention",MessageBoxButton.YesNo);
            if(confirm == MessageBoxResult.Yes)
            {
                MessageBox.Show("Recipe saved!");
                return true;
            }
            else
            {
                MessageBox.Show("You chose to discharge this recipe");
                return false;
            }
        }


        //prompt user to fill out all inputs
        private bool areInputsFilled()
        {
           

            if (string.IsNullOrWhiteSpace(Food))
            {
                MessageBox.Show("Recipe name can't be empty");
                return false;
            }

            else if (string.IsNullOrEmpty(Description))

            {
                MessageBox.Show("Description can't be empty");
                return false;
            }

            else if (string.IsNullOrEmpty(Category))
            {
                MessageBox.Show("Category can't be empty");
                return false;
            }

            else if (IngredientsNumber <= 0)
            {
                MessageBox.Show("Ingredients number must be a positive number");
                return false;
            }

            else if (Ingredients == null || !Ingredients.Any(i => !string.IsNullOrWhiteSpace(i.Name)))
            {
                MessageBox.Show("Ingredients can't be empty");
                return false;

            }

            else if (string.IsNullOrEmpty(Instructions))

            {
                MessageBox.Show("Instructions can't be empty");
                return false;
            }

            else if (CookingTime.TotalMinutes <= 0)
            {
                MessageBox.Show("Cooking time must be greater than zero");
                return false;
            }

            else if (string.IsNullOrEmpty(Difficulty))
            {
                MessageBox.Show("Difficulty checkbox can't be empty");
                return false;
            }

            else
            {
                return true;
            }
               
        }
    }
}
