using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CG.Tools.CodeMap.ViewModels
{
    /// <summary>
    /// This class represents a base view-model implementation.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        // *******************************************************************
        // Events.
        // *******************************************************************

        #region Events

        /// <summary>
        /// This event is raised whenever a property value is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        // *******************************************************************
        // Protected methods.
        // *******************************************************************

        #region Protected methods

        /// <summary>
        /// This method raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(
            [CallerMemberName] string propertyName = null
            )
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName)
                );
        }

        #endregion
    }
}
