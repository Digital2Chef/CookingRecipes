using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookingRecipes.ViewModel
{
    public class RelayCommand : ICommand

    {

        //this method gets executed when user press a btn
        private readonly Action<object> execute;

        //method to define if action can execute
        private readonly Func<object, bool> canExecute;

        //contructor that gets the execute method and the execute condition
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        //defining whether the command can execute
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        //execution of command
        public void Execute(object parameter)
        {
            execute(parameter);

        }

        //the event which alerts UI that condition of execute had changed
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
