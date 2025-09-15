using CookingRecipes.Model;
using CookingRecipes.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookingRecipes.ViewModel
{
    public class ViewRecipesModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged; //creating an event to alert UI for changes

        private MainViewModel mainVM;

        public User CurrentUser => mainVM.CurrentUser; //accessing Current logged in user in order to display his name on screen!


        //method to allow OnPropertyChanged
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //creating a new Recipe's object's instance
        public ObservableCollection<Recipe> Recipes { get; }

        //creating a new object from Recipe's class
        private Recipe selectedItem;


        //ICommands
        public ICommand backButton { get; }

        public ICommand deleteRecipe { get; }

        public ICommand exportRecipe { get; }

        public ICommand goToEditRecipeWindow { get; }

        public ICommand storeRecipeInTxt { get; }

        //properties
        public Recipe SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }


        //constructor
        public ViewRecipesModel(MainViewModel mainVM)
        {
            this.mainVM = mainVM;
            Recipes = RecipeRepository.Recipes;//binding repository

            backButton = new RelayCommand(o => backToDashBoard());//method to return back to DashBoard page!

            deleteRecipe = new RelayCommand(o => deleteRecipes());//DELETE RECIPE METHOD!
           
              exportRecipe = new RelayCommand(o => exportRecipes());//method to export recipes!

            goToEditRecipeWindow = new RelayCommand(o=> editRecipe());

            storeRecipeInTxt = new RelayCommand(o => saveRecipeTxt());
          
            }


        //method to navigate in edit recipe window!
        private void editRecipe()
        {
            if (!(selectedItem is Recipe selectedRecipe) || selectedItem == null)
            {
                MessageBox.Show("Please select a recipe to edit");
                return;
            }
            else
            {
                mainVM.CurrentView = new EditRecipe(mainVM, selectedItem);
            }
            
        }

        //method to navigate back on DashBoard page!
        private void backToDashBoard()
        {
            mainVM.CurrentView = new DashBoardView(mainVM);

        }

        //method to delete recipe
        private void deleteRecipes()
        {
            if (selectedItem is Recipe selectedRecipe)
            {
                if (validateDelete()) 
                {
                    MessageBox.Show("Recipe deleted!");
                    Recipes.Remove(selectedRecipe);//removing recipe
                }
                else
                {
                    MessageBox.Show("Recipe is stil stored!");
                }
            }
            else
            {
                MessageBox.Show("Please select a recipe to delete!");
                return;
            }
        }

        //method to prompt user to confirm whether he wants to delete the recipe or not
        private bool validateDelete()
        {
            if(selectedItem is Recipe selectedRecipe) { 
            var confirm = MessageBox.Show($"Are you sure you want to delete the following recipe:\nFood:{selectedItem.Food}\nCategory:{selectedItem.Category}\n" +
                $"Description:{selectedItem.Description}\nIngredients number:{selectedItem.IngredientsNumber}\nIngredients:{selectedItem.IngredientsText}\n" +
                $"Instructions:{selectedItem.Instructions}\nCooking time:{selectedItem.CookingTime}\nDifficulty:{selectedItem.Difficulty}?", "Attention",MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {

                    return true;
                }
                }
                return false;
        }


        //method to export recipe in csv form!
        private void exportRecipes()
        {
            
            if (!(selectedItem is Recipe selectedRecipe) || selectedItem == null)
            {
                MessageBox.Show("Please select a recipe to export");
                return;
            }

            //validating that recipe repository won't be null and user will select one recipe!
            if (RecipeRepository.Recipes == null || RecipeRepository.Recipes.Count == 0)
            {
                MessageBox.Show("There is no data to export!");
                return;
            }

            else { 
                //creating a new object to save data!
                SaveFileDialog save = new SaveFileDialog()
                {
                    Filter = "CSV Files (*.csv)|*.csv",
                    FileName = "recipe_export.csv"
                };

            if(save.ShowDialog() == true)
            {

                    //try-catch method to handle unexpected errors!
            try
            {                     
                                             
                        //stream writer method to write data on the export file!
                        using (StreamWriter sw = new StreamWriter(save.FileName))
                        {
                            //headers
                            sw.WriteLine("Food,Category,Description,Ingredients Number,Ingredients,Instructions,Cooking Time, Difficulty");

                             //writting recipe info!
                             
                                    sw.WriteLine($"{EscapeCSV(selectedRecipe.Food)},{EscapeCSV(selectedRecipe.Category)}," +
                                        $"{EscapeCSV(selectedRecipe.Description)},{EscapeCSV(selectedRecipe.IngredientsNumber.ToString())},{EscapeCSV(selectedRecipe.IngredientsText)}," +
                                        $"{EscapeCSV(selectedRecipe.Instructions)},{EscapeCSV(selectedRecipe.CookingTime.ToString())}," +
                                        $"{EscapeCSV(selectedRecipe.Difficulty)}");
                                
                        }
                        
                                         

                }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occured:{ex.Message}");
            }
            }
            }
        }

        //method to wrap every field on double \ and replace any of them in order not to crack the export file!
        private string EscapeCSV(string field)
        {
            if (field == null) return "";

            //if there is a double \ I double it!
            field = field.Replace("\"", "\"\"");

            //wrapping field with double \
            return $"\"{field}\"";

        }


        //method to store recipe in a txt!
        private void saveRecipeTxt()
        {
            //return an error message if user does not select a recipe!
            if (!(selectedItem is Recipe selectedRecipe) || selectedItem == null)
            {
                MessageBox.Show("Please select a recipe to save");
                return;
            }


            //validating that recipe repository won't be null and user will select one recipe!
            if (RecipeRepository.Recipes == null || RecipeRepository.Recipes.Count == 0)
            {
                MessageBox.Show("There is no data to save!");
                return;
            }

            if (validateSaving()) { 

            //declaring a variable in order to create the path to save the file!
            string dir = "User_Recipes";

            Directory.CreateDirectory(dir);//creating new directory!

            //declaring a variable in order to create the file inside the dir!
            string file = Path.Combine(dir, $"{CurrentUser.Username}.txt");

            //try-catch method in order to handle unexpected errors!
            try
            {
                //Using stream writer method to write data in the file!
                using (StreamWriter sw = new StreamWriter(file))
                {
                    
                    sw.WriteLine($"Food name:{selectedRecipe.Food}|Category:{selectedRecipe.Category}|Description:{selectedRecipe.Description}|Ingredients number:{selectedRecipe.Category}|" +
                        $"Ingredients:{selectedRecipe.IngredientsText}|Instructions:{selectedRecipe.Instructions}|Cooking time:{selectedRecipe.CookingTime}|Difficulty:{selectedRecipe.Difficulty}");

                }

                


            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occured:{ex.Message}");

            }
            }
            
        }

        //ask user to validate whether he wants to save the recipe or not!
        private bool validateSaving()
        {
            var confirm = MessageBox.Show("Are you sure you want to save the recipe?", "Attention", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                MessageBox.Show("Data saved!");
                return true;
            }
            else
            {
                MessageBox.Show("Recipe hadn't saved");
                return false;
            }

        }
    }
}
