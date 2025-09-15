using CookingRecipes.Model;
using CookingRecipes.Security;
using CookingRecipes.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CookingRecipes.ViewModel
{
      public class RegisterModel:INotifyPropertyChanged
    {
        //public event to alterate UI
        public event PropertyChangedEventHandler PropertyChanged;

        //method to invoke alterations
         protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        MainViewModel mainVM;
     

        //declaring variables
        private String username;
         private String password;
        private String confirmPass;

        //properties
        public string Username
        {
            get => username; 
            set
            {
                if (username != value) 
                {
                 username = value;
                }
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                if (password != value)
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
                if (confirmPass != value)
                {
                    confirmPass = value;
                    OnPropertyChanged(nameof(ConfirmPass));
                }
            }
        }
        

        //creating a new object from User class
        private User newUser = new User();

       

        //Relay command for register btn
        public ICommand registerCommand { get; }

        //Relay command for button back
        public ICommand backToLogin { get; }

        //creating a constructor
        public RegisterModel(MainViewModel mainVM)
        {
            this.mainVM = mainVM;

            backToLogin = new RelayCommand(o => mainVM.CurrentView = new LoginView(mainVM));//back to register page command

            //command to proceed with register
            registerCommand = new RelayCommand(o =>
            {
               
                register();
            });

        }



        //method to register user
        public void register()
        {
           

            if (areInputsField())
            {
                storeCredentials();//method to store credentials!

                saveCredentials();//saving credentials in a txt file!

                backToLoginAfterSuccessfulRegistration();//redirecting to login page!
            }
           

        }


        //method to navigate back two login page after successful registration!
        private void backToLoginAfterSuccessfulRegistration()
        {
            mainVM.CurrentView = new LoginView(mainVM);
        }


        //method to validate that user won't leave any empty inputs!
        private bool areInputsField()
        {
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Username can't be emtpy");
                return false;
            }
            else if(string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password can't be emtpy");
                return false;
            }
            else if (string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Please confirm password");
                return false;
            }
            else if(Password != ConfirmPass)
    {
                MessageBox.Show("Passwords do not match");
                return false;
            }
            else
            {
                return true;
            }
        }

        //pass values in the object's variable!
        private void assignValues()
        {
            
            newUser.Username = username;
            newUser.Password = password;
        }

        //confirm registration!
        private bool confirmCredentials()
        {
            var confirm = MessageBox.Show($"Are following credentials correct?\nUsername:{username}\nPassword:{password}", "Attention", MessageBoxButton.YesNo);

            if (confirm == MessageBoxResult.Yes)
            {
                MessageBox.Show("User registered!");
                return true;
            }
            else
            {
                MessageBox.Show("Registration cancelled");
                return false;
            }
                
        }
        //method to store credentials in a txt file!
        private void storeCredentials()
        {
          
            if (usernameIsUnique(Username) && confirmCredentials() )
            {
                assignValues();//method to assign values!
            }

         
        }

        //method to store credentials on a txt !
        private void saveCredentials()
        {
            //creating a new txt file to save credentials!
            string file = $"users.txt";


            //being sure that there special characters won't be a problem!
            string encodedUsername = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Username));

            //hashing entered password!
            var (hash, salt) = PasswordHasher.PasswordHash(Password);

            //try-catch method to handle unexpected errors!
            try
            {


                //stream writer method to write credentials in the txt file!
                using (StreamWriter save = new StreamWriter(file, append: true))
                {
                    save.WriteLine($"{encodedUsername}|{hash}|{salt}");
                   
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"An unexpected error occured:{ex.Message}");

            }
        
        }

    
        //method to validate that user doesn't already exist!
        private bool usernameIsUnique(string usernameToCheck)
        {
            //declaring a string file variable to find txt!
            string file = $"users.txt";

            if (!File.Exists(file))
            {
                return false;
            }
            else
            {
                try
                {
                    //reading file to check if username already exists!
                    using (StreamReader read = new StreamReader(file))
                    {
                        string line;
                        while((line = read.ReadLine()) != null)
                        {
                            string[] part = line.Split("|");//removing '|'!
                            if(part.Length >= 1)
                            {
                                string username = Encoding.UTF8.GetString(Convert.FromBase64String(part[0]));//decoding username!


                                if (username.Equals(usernameToCheck, StringComparison.OrdinalIgnoreCase))
                                {
                                    MessageBox.Show("Username already exists, try a new one");
                                    return false;//username exists!
                                }
                               
                            } 
                        }

                          }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occured:{ex.Message}");
                    return false;
                }

                return true;//username is unique!
            }
        }
    }
}
