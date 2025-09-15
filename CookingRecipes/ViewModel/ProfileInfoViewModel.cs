using CookingRecipes.Model;
using CookingRecipes.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace CookingRecipes.ViewModel
{
    public class ProfileInfoViewModel:INotifyPropertyChanged
    {
        //declaring variables
        private String surname;
        private String lastname;
        private String email;
        private String phone;

        private MainViewModel mainVM;//variable in order to access mainviewmodel class!      

        public User CurrentUser=> mainVM.CurrentUser;//accessing Current logged in user in order to display his name on screen!
       
        //properties
        public string Surname
        {
            get => surname;
            set
            {
                if (surname != value)
                {
                    surname = value;
                    OnPropertyChanged(nameof(Surname));
                }
            }
        }

        public string Lastname
        {
            get => lastname;
            set
            {
                if (lastname != value)
                {
                    lastname = value;
                    OnPropertyChanged(nameof(Lastname));
                }
            }
        }

        public String Email
        {
            get => email;
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string Phone
        {
            get => phone;
            set
            {
                if (phone != value)
                {
                    phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }



        //PropertyChanged event handler!
        public event PropertyChangedEventHandler PropertyChanged;

       

        //OnPropertyChanged method to alert UI
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        //ICommands

        public ICommand addInfo { get; }

        public ICommand showInfo { get; }

        public ICommand updatePass { get; } 
        public ICommand back { get; }

        //constructor
        public ProfileInfoViewModel(MainViewModel mainVM) 
        {
          this.mainVM = mainVM;

            //back to dashboard!
            back = new RelayCommand(o => returnToDashBoardPage());

            //displaying window which shows user's info!
            showInfo = new RelayCommand(o=> userInfo());

            //add button logic!
            addInfo = new RelayCommand(o => addUsersInfo());

            //navigation to change password's panel!
            updatePass = new RelayCommand(o=> navigateOnUpdatePassWindow() );
        }

        //method to return to dashboard page!
        private void returnToDashBoardPage()
        {
            mainVM.CurrentView = new DashBoardView(mainVM); 
           
        }

        //method to display update password panel!
        private void navigateOnUpdatePassWindow()
        {
            mainVM.CurrentView = new UpdatePassPanel(mainVM);
        }

        //display user info window!
        private void userInfo()
        {
            mainVM.CurrentView = new  UserInfoWindowView(mainVM);
        }

        //method to add user's information
        private void addUsersInfo()
        {
            if (areAllInputsField()) 
            {
                assignValues();//method to assign values and prompt user to confirm changes!

                clearInputs();//clearing all inputs!
            }
        }


        //method to prompt user to fill out all the inputs!
        private bool areAllInputsField()
        {
            if (string.IsNullOrEmpty(Surname))
            {
                MessageBox.Show("Surname can't be empty");
                return false;

            }
            else if (string.IsNullOrEmpty(Lastname))
            {
                MessageBox.Show("Last name can't be empty");
                return false;
            }
            else if (string.IsNullOrEmpty(Email))
            {
                MessageBox.Show("Email can't be empty");
                return false;

            }
            else if (string.IsNullOrEmpty(Phone) || Phone.Length != 10)
            {
                MessageBox.Show("Please enter a valid phone number");
                return false;
            }

         
                return true;
        }

        //method to assign values!
        private void assignValues()
        {
            var info = new User
            {
                Surname = Surname,
                Lastname = Lastname,
                Email = Email,
                Phone = Phone,
            };

            //Adding information in the list if user confirms saving!
            if (confirm())
            {
                assignValuesInClass(info);  //assigning values in class's variables!

                UserRepository.UserInfoList.Add(info);//adding information in Information's repository list!

                storeUserInfo();//saving user's info in a txt file!

                MessageBox.Show("Information added!");
            }
        }


  

        //this method asks user whether he wants to add the values in the list or not!
        private bool confirm()
        {
            var confirm = MessageBox.Show($"Confirm your information please:\nSurname:{Surname}\nLast name:{Lastname}\nEmail:{Email}\nPhone:{Phone}","Attention",MessageBoxButton.YesNo);

            //logic behind Yes or No
            if (confirm == MessageBoxResult.Yes)
            {
                
                
                return true;

            }
            else
            {
                MessageBox.Show("You chose to discard your personal information");
                return false;
            }
        }

        //method to assign values in class's variables
        private void assignValuesInClass(User info)
        {
            info.Surname = Surname;
            info.Lastname = Lastname;
            info.Email = Email;
            info.Phone = Phone;
        }


        //method to clear inputs!
        private void clearInputs()
        {
            Surname = string.Empty;
            Lastname = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
        }



        //method to store user's info in a txt
        private void storeUserInfo()
        {
            //creating a new directory to store information for each user seperataly!
            string dir = "User_Data";
            Directory.CreateDirectory(dir);

            //declaring variables!
            string file = Path.Combine(dir, $"{CurrentUser.Username}.txt");


            //try-catch method to handle unexpected errors!
            try
            {


                //stream writer method to write data in the file!
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.WriteLine($"{Surname}|{Lastname}|{Email}|{Phone}");
             
                }
            }
            catch (Exception ex) 
            {
             MessageBox.Show($"An unexpected error occured:{ex.Message}");  
            
            }
        
        }
       
    }
}
