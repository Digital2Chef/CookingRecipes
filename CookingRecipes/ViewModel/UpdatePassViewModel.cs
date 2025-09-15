using CookingRecipes.Model;
using CookingRecipes.Security;
using CookingRecipes.View;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookingRecipes.ViewModel
{
    public  class UpdatePassViewModel:INotifyPropertyChanged
    {
        //FIX ERROR WITH PASSWORD UPDATE!

        //declaring variables!
        private String oldPass;
        private String password;
        private String confirmPass;


        //properties

        public string OldPass
        {
            get => oldPass;
            set
            {
                if (oldPass != value)
                {
                    oldPass = value;
                    OnPropertyChanged(nameof(OldPass));
                }

            }
        }

        public string Password
        {
            get => password;
            set
            {
                if(password != value)
                {
                    password = value; 
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public String ConfirmPass
        {
            get => confirmPass;
            set
            {
                if(confirmPass != value)
                {
                    confirmPass = value;
                    OnPropertyChanged(nameof(ConfirmPass));
                }
            }
        }




        //onproperty changed event to alert UI!
        public event PropertyChangedEventHandler PropertyChanged;


        //method to implenet on property changed!
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        
        
        private MainViewModel mainVM;//variable in order to access mainviewmodel class!      

        public User CurrentUser => mainVM.CurrentUser;//accessing Current logged in user in order to display his name on screen!




        //ICommands!
        public ICommand updatePass { get; }
        public ICommand back { get; }

        //constructor
        public UpdatePassViewModel(MainViewModel mainVm) 
        {
            this.mainVM = mainVm;

            //command to navigate back to profile information window!
            back = new RelayCommand(o => navigateToPreviewsPage());

            updatePass = new RelayCommand(o => updatePassword());//method to update pass!
        
        }


        //method to validate that user will fill out all inputs and verify that new pass matches with confirm pass
        private bool areAllInputsFilledCorrectly()
        {
            if (string.IsNullOrWhiteSpace(OldPass))
            {
                MessageBox.Show("Old password's input can't be empty");
                return false;
            }
            else if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Password's input can't be empty");
                return false;
            }

            else if (string.IsNullOrWhiteSpace(ConfirmPass))
            {
                MessageBox.Show("Confirm password's  input can't be empty");
                return false;
            }
            else if (Password != ConfirmPass)
            {
                MessageBox.Show("Passwords don't match");
                return false;
            }

                return true;
        }


        //method to validate if old pass matches with existing password!
        private bool isOldPassValide()
        {
            //accessing txt file!
            string file = $"users.txt";

            if (!File.Exists(file)) 
            {
                MessageBox.Show("Couldn't find any registered users");

                return false;
            }

            //try-catch method to handle unexpected errors!
            try
            {


                //reading all lines!
                var lines = File.ReadAllLines(file).ToList();

               string targetUser = CurrentUser.Username;

                //for loop to read all file in order to find current user's details!
                for (int i =0; i < lines.Count; i++)
                {
                    var parts = lines[i].Split("|,_");//exempting the symbol "|" inside the txt!

                    if (parts.Length != 3) continue;
                    
                        //decoding username!
                        string decodedUsername = Encoding.UTF8.GetString(Convert.FromBase64String(parts[0]));

                        if (decodedUsername == targetUser)//if user found get the hashed pass and salt!
                        {
                            string storedHash = parts[1];
                            string storedSalt = parts[2];

                            //validating old password!
                            if (!PasswordHasher.verifyPass(OldPass, storedHash, storedSalt))
                            {
                                MessageBox.Show("Wrong password");
                                return false;

                            }
                        }
                        else
                        {
                            MessageBox.Show("User not found!");
                            return false;
                        }                  
                 

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"An unexpected error occured:{ex.Message}");
                return false;
            }
            return true;
        }


        //method to update password
        private void updatePassword()
        {
            if (areAllInputsFilledCorrectly() && isOldPassValide())
            {
                changePassInTxt();//method to re-write data in the txt!

                redirectToProfileInfoPage();// method to redirect in profile info page! 
            }
        }


        //method to confirm password change!
        private bool confirmPassChange()
        {
            var confirm = MessageBox.Show($"Are you sure you want to change your password?", "Attention", MessageBoxButton.YesNo);

            if (confirm == MessageBoxResult.Yes)
            {

                return true;
            }
            else
            {
                return false;
            }
        }


                //method to update pass inside the txt file which is stored
                private void changePassInTxt()
                {
                    //accessing txt file!
                    //accessing txt file!
                    string file = $"users.txt";

                    //returning error message whether txt doesn't exist!
                    if (!File.Exists(file))
                    {
                        MessageBox.Show("Couldn't find any registered users");
                        return;
                    }

            if (confirmPassChange()) //if user confirms password change re-write the file! 
            { 

                    //try-catch method to handle unexpected errors!
                    try
                    {
                        //reading all lines!
                        var lines = File.ReadAllLines(file).ToList();

                        string targetUser = CurrentUser.Username;

                        //for loop to read all lines and find current user!
                        for (int i = 0; i < lines.Count; i++)
                        {
                            var parts = lines[i].Split("|");
                            if (parts.Length != 3) continue;


                            //decoding username!
                            string decodedUsername = Encoding.UTF8.GetString(Convert.FromBase64String(parts[0]));
                            if (decodedUsername == targetUser)//if user founds we continue!
                            {
                                //creating a new hash and salt in order to hash new pass!
                                var (newHash, newSalt) = PasswordHasher.PasswordHash(Password);
                                string encodedUser = Convert.ToBase64String(Encoding.UTF8.GetBytes(targetUser));//econding again username!


                                //replacing line with hash and salt!
                                lines[i] = $"{encodedUser}|{newHash}|{newSalt}";

                                File.WriteAllLines(file, lines);//replacing line!



                            }


                        }


                    
                    }
                    catch (Exception ex)

                    {
                        MessageBox.Show($"An unexpected error occured:{ex.Message}");
                        return;
                    }
                MessageBox.Show("Password updated successfully!");
            }
        }


        //method to redirect in profile info page!
        private void redirectToProfileInfoPage()
        {
            mainVM.CurrentView = new ProfileInfo(mainVM);//redirect to profile's info page!
        }




        //method to navigate back to profile information window!
        private void navigateToPreviewsPage()
        {
            mainVM.CurrentView = new ProfileInfo(mainVM);
        }
    }
}
