using CookingRecipes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingRecipes
{
  public   class UserRepository
    {
        public static ObservableCollection<User> UserInfoList { get; } = new ObservableCollection<User> ();

        //constructor
        public UserRepository() { }
    }
}
