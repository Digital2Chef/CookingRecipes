using CookingRecipes.Model;
using CookingRecipes.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CookingRecipes.ViewModel
{
   public class UserInfoWindowViewModel:INotifyPropertyChanged
    {
        //declaring variables
        private String surname;
        private String lastname;
        private String email;
        private String phone;
        private User selectedUser;

       

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

        public String Lastname
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

        public String Phone
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

        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                if (selectedUser != value)
                {
                    selectedUser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
        }

        //on property change event handler!
        public event PropertyChangedEventHandler PropertyChanged;

        //method to alert UI with changes!
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private MainViewModel mainVM;//variable in order to access mainviewmodel class!      
        

        public User CurrentUser => mainVM.CurrentUser;//accessing Current logged in user in order to display his name on screen   !

        public ObservableCollection<User> UserInfoList => UserRepository.UserInfoList;//sighting user's info list to viewmodel!




        //ICommands
         
        public ICommand delete { get; }
        public ICommand back { get; }


        //constructor
        public UserInfoWindowViewModel(MainViewModel mainVM)
        {


            this.mainVM = mainVM;

         
            //method to load all users
            loadUserInformation();

            //method to delete profile info!
            delete = new RelayCommand(o=> deleteInfo());
         
            //method to go back to profile info window!
            back = new RelayCommand(o=>returnToProfileInfoPage() );
            

        }

        //method to delete user's info
        private void deleteInfo()
        {
            if (!(SelectedUser is User selectedUser) || selectedUser == null)
            {
                MessageBox.Show("Please select user's information to delete");
                return;

            }
            else
            {
                if (validateDiscard())
                {
                    UserInfoList.Remove(SelectedUser);//removing selected user's info!
                    returnToProfileInfoWindow();//returing to previews window since there isn't any info available here!
                }
                else
                {
                    MessageBox.Show("You didn't erase your info!");
                    return;
                }
            }
        }


        //method to return to profile info window if user delete's his info
        private void returnToProfileInfoWindow()
        {
            MessageBox.Show("There aren't any information available here");
            mainVM.CurrentView = new ProfileInfo(mainVM);
        }

        //method to prompt user to validate whether he is sure to delete the info or not!
        private bool validateDiscard()
        {
            var confirm = MessageBox.Show("Do you really wish to delete your information?", "warning", MessageBoxButton.YesNo);

            if (confirm == MessageBoxResult.Yes)
            {
                deleteAndEraseFile();//deleting file with user's information!
                return true;

            }
            return false;
        }


        //method to delete information and erase txt file!
        private void deleteAndEraseFile()
        {
            string file = Path.Combine("User_Data",$"{CurrentUser.Username}.txt");//accessing user's info txt
            if (File.Exists(file))
            {
                File.Delete(file);//deleting file!
            }
            else
            {
                MessageBox.Show("Couldn't find any archive file to delete");
                return;
            }
        }


        //method to return to prewviews page!
        private void returnToProfileInfoPage()
        {
            mainVM.CurrentView = new ProfileInfo(mainVM);
        }


        //method to load user info!
        private void loadUserInformation()
        {
           if(CurrentUser == null) return;

            string dir = "User_Data";

            string file = Path.Combine(dir, $"{CurrentUser.Username}.txt");

            if (Directory.Exists(dir))
            {
                if(!File.Exists(file))
                {
                    MessageBox.Show("Couldn't find the user you were looking for");
                    return;
                }
                else
                {

                //try-catch method to handle unexpecte errors!
                    try
                    {
                        var data = File.ReadAllText(file).Split("|");
                        if (data.Length == 4)
                        {
                            //adding user info!
                            var userInfo = new User
                            {
                                Username = CurrentUser.Username,
                                Surname = data[0],
                                Lastname = data[1],
                                Email = data[2],
                                Phone = data[3]

                            };

                            UserInfoList.Clear();//clearing list in order not to display double info!

                            UserInfoList.Add(userInfo);//adding user's info in the list!


                            //Updating current user!
                            CurrentUser.Surname = data[0];
                            CurrentUser.Lastname = data[1];
                            CurrentUser.Email = data[2];
                            CurrentUser.Phone = data[3];


                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An unexpected error occured:{ex.Message}");
                    }
            }
            }
        }

    }
}
