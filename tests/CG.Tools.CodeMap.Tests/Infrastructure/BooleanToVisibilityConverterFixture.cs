using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Windows;

namespace CG.Tools.CodeMap.Infrastructure
{
    [TestClass]
    [TestCategory("Unit")]
    public class BooleanToVisibilityConverterFixture
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method ensures the <see cref="BooleanToVisibilityConverter.Convert(object, Type, object, System.Globalization.CultureInfo)"/>
        /// method performs a valid conversion.
        /// </summary>
        [TestMethod]
        public void BooleanToVisibilityConverter_Convert()
        {
            // Arrange ...
            var conv = new BooleanToVisibilityConverter();

            // Act ...
            var trueResult = conv.Convert(true, typeof(Visibility), null, CultureInfo.CurrentCulture);
            var falseResult = conv.Convert(false, typeof(Visibility), null, CultureInfo.CurrentCulture);

            // Assert ...
            Assert.IsTrue(
                (Visibility)trueResult == Visibility.Visible,
                "The return value was invalid."
                );
            Assert.IsTrue(
                (Visibility)falseResult == Visibility.Collapsed,
                "The return value was invalid."
                );
        }

        #endregion
    }
}
