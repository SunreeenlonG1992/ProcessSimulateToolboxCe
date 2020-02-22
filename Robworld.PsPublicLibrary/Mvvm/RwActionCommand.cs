using System;
using System.Windows.Input;

namespace Robworld.PsPublicLibrary.Mvvm
{
    /// <summary>
    /// Defines a Command in a WPF application
    /// </summary>
    public class RwActionCommand : ICommand
    {
        #region Fields
        private readonly Action<object> executeHandler;
        private readonly Func<object, bool> canExecuteHandler;
        #endregion

        #region Events
        /// <summary>
        /// Register or unregister a action command
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of an action command
        /// </summary>
        /// <param name="execute">The method that executes the command</param>
        /// <param name="canExecute">The method that decides if the command can be executed</param>
        public RwActionCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            executeHandler = execute ?? throw new ArgumentNullException("Execute cannot be null");
            canExecuteHandler = canExecute;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check whether the command can be executed
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            if (canExecuteHandler == null)
            { return true; }
            return canExecuteHandler(parameter);
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            executeHandler(parameter);
        }
        #endregion
    }
}
