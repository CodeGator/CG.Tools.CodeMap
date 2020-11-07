using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CG.Tools.CodeMap.ViewModels
{
    /// <summary>
    /// This class is a test fixture for the <see cref="ViewModelBase"/>
    /// class.
    /// </summary>
    [TestClass]
    [TestCategory("Unit")]
    public class ViewModelBaseFixture
    {
        // *******************************************************************
        // Types.
        // *******************************************************************

        #region Types

        /// <summary>
        /// This class is for internal testing purposes.
        /// </summary>
        class TestVM : ViewModelBase 
        {
            protected string _a;
            public string A
            {
                get { return _a; }
                set
                {
                    _a = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method ensures the <see cref="ViewModelBase.PropertyChanged"/>
        /// event is fired when the <see cref="ViewModelBase.OnPropertyChanged(string)"/>
        /// method is called.
        /// </summary>
        [TestMethod]
        public void ViewModelBase_OnPropertyChanged()
        {
            // Arrange ...
            var propertyName = "";

            // Act ..
            var result = new TestVM();

            result.PropertyChanged += (s, e) =>
            {
                propertyName = e.PropertyName;
            };

            result.A = "blah";

            // Assert ..
            Assert.IsTrue(
                propertyName == "A", 
                "The property name was invalid.");
        }

        #endregion
    }
}
