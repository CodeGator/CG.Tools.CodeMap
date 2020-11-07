using Syncfusion.Windows.Shared;
using System;
using System.Windows;

namespace CG.Tools.CodeMap.ViewModels
{
    /// <summary>
    /// This class represents a view-model for diagram filtering.
    /// </summary>
    public class FilterViewModel : ViewModelBase
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        private MainWindowViewModel _owner;
        private string _filter;
        private bool _enabled;

        private DelegateCommand _filtersChangedCommand;

        #endregion

        // *******************************************************************
        // Commands.
        // *******************************************************************

        #region Commands

        /// <summary>
        /// This property contains the filters changed command.
        /// </summary>
        public DelegateCommand FiltersChangedCommand =>
            _filtersChangedCommand ?? (_filtersChangedCommand = new DelegateCommand(
                ExecuteFiltersChangedCommand,
                CanExecuteFiltersChangedCommand
                ));

        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a filter string.
        /// </summary>
        public string Filter 
        { 
            get { return _filter; }
            set 
            {
                _filter = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// This property indicates whether the filter is enabled, or not.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                OnPropertyChanged();
            }
        }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="FilterViewModel"/>
        /// class.
        /// </summary>
        /// <param name="owner">The parent view-model.</param>
        public FilterViewModel(
            MainWindowViewModel owner
            )
        {
            // Save the reference.
            _owner = owner;
        }

        #endregion

        // *******************************************************************
        // Command handlers.
        // *******************************************************************

        #region Command handlers

        /// <summary>
        /// This method is called when the filters are changed.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        void ExecuteFiltersChangedCommand(object args)
        {
            // The arg is actually the view.
            var owner = args as Window;

            // Reload the entire diagram.
            _owner.DiagramDotNetFile(owner);
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether it should be possible to call the <see cref="ExecuteFiltersChangedCommand(object)"/>
        /// method.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        /// <returns>True if the method should be called; false otherwise.</returns>
        bool CanExecuteFiltersChangedCommand(object args)
        {
            return true;
        }

        #endregion
    }
}
