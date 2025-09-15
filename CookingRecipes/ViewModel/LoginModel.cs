using CookingRecipes.Model;
using CookingRecipes.Security;
using CookingRecipes.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookingRecipes.ViewModel
{

    public  class LoginModel:INotifyPropertyChanged
    {
        private MainViewModel mainVm;

        //declaring variables
        private String username;
        private String password;
        private int  counter = 3;//each time user enters invalid credentials counter will reduce. Once it reach 0 account will get locked!

        //properties
        public string Username
        {
            get => username;
            set
            {
                if(username != value)
                {
                    username = value;
                    OnPropertyChanged(nameof(Username));
                }
              
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
        //event handler for OnPropertyChanged  !    
        public event PropertyChangedEventHandler PropertyChanged;

       //method to invoke alterations
       protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




       
        //Icommands for register page redirecting and login method!
        public ICommand register { get; }
        public ICommand login { get; }

        public ICommand exit { get; }


        //creating a constructor to handle login and go to register page method!
        public LoginModel(MainViewModel mainVm)
        {
            this.mainVm = mainVm;

            register = new RelayCommand(o => mainVm.CurrentView = new RegisterView(mainVm));//navigating into register page!

            login = new RelayCommand(o => navigateToDashBoardPage());//navigate to dashboard page after logging in successfuly!

            exit = new RelayCommand(o => exitApp());//exiting app method!
           
        }

        //exit method
        private void exitApp()
        {
            MessageBox.Show("Thanks for using my app!");
            Environment.Exit(0);//terminatin app!

        }
       
        //login logic
        private bool isLoggedIn()
        {
            if (areInputsField())
            {
                if (!validateCredentials())
                {
                    handleFailedLoginAttempts();
                    return false;
                }
                else
                {
                    MessageBox.Show("Logged in!");
                    return true;
                }
                
            }
            return false;
        }

        //navigating on main dashboard if logic was successful!
        private void navigateToDashBoardPage()
        {
            if (isLoggedIn())//if login == success redirect user in DashBoard page!
            {
                mainVm.CurrentView = new DashBoardView(mainVm);
            }
        }



        //method to handle failed login attempts
        private void handleFailedLoginAttempts()
        {
           if (counter > 0)
            { 
                counter--;
                MessageBox.Show($"{counter} more attempts remain. If you fail your account we will lock your account for security reasons");
                
            
         if(counter == 0)
            {
                lockAcc();//method to lock account!
            }
            }
        }
        //method to lock account
        private void lockAcc()
        {
            MessageBox.Show("Account locked");
            Environment.Exit(0);
        }
        


        //method to see if credentials exist
        private bool validateCredentials()
        {
            //declaring a variable called file to search in the txt for the credentials
            string file = $"users.txt";
            try
            {
                //validating if file exists
                if (File.Exists(file))
                {


                    //stream reader method to read the txt file

                    using (StreamReader read = new StreamReader(file))
                    {
                        string line;
                        while ((line = read.ReadLine()) != null)
                        {
                            string[] parts = line.Split("|");
                            if (parts.Length != 3) continue;
                            
                             string decodedUsername = Encoding.UTF8.GetString(Convert.FromBase64String(parts[0]));
                            string storedHash = parts[1];
                            string storedSalt = parts[2];

                            if (decodedUsername.Equals(Username))
                            {
                                if (PasswordHasher.verifyPass(Password, storedHash, storedSalt))
                                {

                                    mainVm.CurrentUser = new User { Username = decodedUsername };

                                    return true;
                                    
                                }
                                else
                                {
                                    MessageBox.Show("Invalid credentials");
                                    return false;
                                }
                            }
                        }

                        MessageBox.Show("Invalid credentials");
                        return false;

                    }

                }

            }
            catch (Exception ex) 
                {
                    MessageBox.Show($"An unexpected error occured:{ex.Message}");
                    return false;
                }
            return false;
            
        }

       

        //method to validate that user won't leave empty inputs!
        private bool areInputsField()
        {
            if (string.IsNullOrEmpty(Username))
            {
                MessageBox.Show("Username can't be emtpy");
                return false;
            }
            else if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Password can't be emtpy");
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}
