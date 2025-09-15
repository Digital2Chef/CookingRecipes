using CookingRecipes.Model;
using CookingRecipes.View;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookingRecipes.ViewModel
{
    public class EditRecipeViewModel:INotifyPropertyChanged
    {

        private readonly MainViewModel _mainVM;

        public event PropertyChangedEventHandler PropertyChanged;

        private String ingredientsNumberText;

        //method to alert UI for changes
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
      
        //creating a new selected recipe's object
        private  Recipe selectedItem { get; set; }


     
        //properties

        public string IngredientsNumberText
        {
            get => ingredientsNumberText;
            set
            {
                if (ingredientsNumberText != value)
                {
                    ingredientsNumberText = value;
                    OnPropertyChanged(nameof(IngredientsNumberText));
                }
                if (selectedItem != null)
                {
                    if (int.TryParse(IngredientsNumberText, out int number))
                    {
                        SelectedItem.IngredientsNumber = number;
                    }
                }
            }
        }
        public Recipe SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;

                    OnPropertyChanged(nameof(selectedItem));

                    IngredientsNumberText = selectedItem?.IngredientsNumber.ToString();//synchronizing text box with the current value of the variable IngredientsNumberText!
                }

               
            }
        }
       



        //ICommands
        public ICommand editRecipe { get; }
        public ICommand backToViewRecipe { get; }

        public ICommand addIngredientTextBox { get; }


        //public constructor
        public EditRecipeViewModel(MainViewModel mainVM, Recipe selectedItem)
        {
            _mainVM = mainVM;

            SelectedItem = selectedItem ?? throw new ArgumentNullException(nameof(selectedItem));

            editRecipe = new RelayCommand(o =>
            {
                save();//save changes!

            } );

            backToViewRecipe = new RelayCommand(o => returnBack());//return back to view recipe page!

            addIngredientTextBox = new RelayCommand(o=> addIngredientTextBoxes());  //adding more textboxes for ingredients!

        }

        //Method to add mpore ingredient's text boxes!
        private void addIngredientTextBoxes()
        {
            selectedItem.Ingredients.Add(new IngredientItem { Name = string.Empty });

        }


        //recipe method!
        private void save()
        {
            if (confirmChanges() )
            {
                MessageBox.Show("Recipe updated!");
                
                returnBack();//back to view recipe window!
            }
           
        }

        //method to confirm changes!
        private bool confirmChanges()
        {
            if ( areInputsFilled() && selectedItem != null)
            {
                var confirm = MessageBox.Show($"Are you sure you want to implement the following changes?:Food:{selectedItem.Food}\n" +
                    $"Category:{selectedItem.Category}\nDescription:{selectedItem.Description}\nIngredients Number{selectedItem.IngredientsNumber}\nIngredients:{selectedItem.IngredientsText}\n" +
                    $"Instructions:{selectedItem.Instructions}\nCooking Time:{selectedItem.CookingTime}\nDifficulty:{selectedItem.Difficulty}","Attention",MessageBoxButton.YesNo);

                //if confirm == Yes save return true!
                if (confirm == MessageBoxResult.Yes)
                {
                    return true;
                }
               
            }
            return false;
        }


        //method to return to view recipe window!
        private void returnBack()
        {
            _mainVM.CurrentView = new ViewRecipesView(_mainVM);
        }

        //method to validate that user won' leave any empty inputs!
        private bool areInputsFilled()
        {
            //declaring a parsed variable in order to parse IngredientsNumber 

            if (string.IsNullOrWhiteSpace(selectedItem.Food))
            {
                MessageBox.Show("Recipe name can't be empty");
                return false;
            }

            else if (string.IsNullOrEmpty(selectedItem.Description))

            {
                MessageBox.Show("Description can't be empty");
                return false;
            }

            else if (string.IsNullOrEmpty(selectedItem.Category))
            {
                MessageBox.Show("Category can't be empty");
                return false;
            }

            else if (selectedItem.IngredientsNumber <= 0)
            {
                MessageBox.Show("Ingredients number must be a positive number");
                return false;
            }

            else if (selectedItem.Ingredients == null || !selectedItem.Ingredients.Any(i => !string.IsNullOrWhiteSpace(i.Name)))
            {
                MessageBox.Show("Ingredients can't be empty");
                return false;

            }

            else if (string.IsNullOrEmpty(selectedItem.Instructions))

            {
                MessageBox.Show("Instructions can't be empty");
                return false;
            }

            else if (selectedItem.CookingTime.TotalMinutes <= 0)
            {
                MessageBox.Show("Cooking time must be greater than zero");
                return false;
            }

            else if (string.IsNullOrEmpty(selectedItem.Difficulty))
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
