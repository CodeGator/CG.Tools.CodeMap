using Microsoft.VisualStudio.TestTools.UnitTesting;
using Syncfusion.UI.Xaml.Diagram;
using Syncfusion.UI.Xaml.Diagram.Layout;
using System;
using System.CodeDom.Compiler;

namespace CG.Tools.CodeMap.ViewModels
{
    /// <summary>
    /// This class is a test fixture for the <see cref="MainWindowViewModelFixture"/>
    /// class.
    /// </summary>
    [TestClass]
    [TestCategory("Unit")]
    public class MainWindowViewModelFixtureFixture
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method ensures the <see cref="MainWindowViewModel.MainWindowViewModel"/>
        /// constructor properly initializes object instances.
        /// </summary>
        [TestMethod]
        public void ViewModelBase_OnPropertyChanged()
        {
            // Arrange ...
            
            // Act ..
            var result = new MainWindowViewModel();

            // Assert ..
            Assert.IsTrue(
                result.Filters != null,
                "The Filters property wasn't initialized."
                );
            Assert.IsTrue(
                result.Caption != null,
                "The Caption property wasn't initialized."
                );
            Assert.IsTrue(
                result.Status != null,
                "The Status property wasn't initialized."
                );
            Assert.IsTrue(
                result.DefaultConnectorType == ConnectorType.Line,
                "The DefaultConnectorType property wasn't initialized."
                );
            Assert.IsTrue(
                result.Nodes != null,
                "The Nodes property wasn't initialized."
                );
            Assert.IsTrue(
                result.Connectors != null,
                "The Connectors property wasn't initialized."
                );
            Assert.IsTrue(
                result.LayoutManager != null,
                "The LayoutManager property wasn't initialized."
                );
            Assert.IsTrue(
                result.LayoutManager.Layout != null,
                "The LayoutManager.Layout property wasn't initialized."
                );
            Assert.IsTrue(
                result.ScrollSettings != null,
                "The ScrollSettings property wasn't initialized."
                );
            Assert.IsTrue(
                result.SelectedItems != null,
                "The SelectedItems property wasn't initialized."
                );
            Assert.IsTrue(
                result.PageSettings != null,
                "The PageSettings property wasn't initialized."
                );
            Assert.IsTrue(
                (result.Constraints & GraphConstraints.ContextMenu) == GraphConstraints.None,
                "The Constraints property wasn't initialized."
                );

            Assert.IsTrue(
                result.FileExitCommand != null,
                "The FileExitCommand command wasn't initialized."
                );
            Assert.IsTrue(
                result.FileOpenCommand != null,
                "The FileOpenCommand command wasn't initialized."
                );
            Assert.IsTrue(
                result.FileCloseCommand != null,
                "The FileCloseCommand command wasn't initialized."
                );
            Assert.IsTrue(
                result.FilePrintCommand != null,
                "The FilePrintCommand command wasn't initialized."
                );
            Assert.IsTrue(
                result.HelpAboutCommand != null,
                "The HelpAboutCommand command wasn't initialized."
                );
            Assert.IsTrue(
                result.DiagramLayoutCommand != null,
                "The DiagramLayoutCommand command wasn't initialized."
                );
            Assert.IsTrue(
                result.DiagramZoomRestoreCommand != null,
                "The DiagramZoomRestoreCommand command wasn't initialized."
                );
            Assert.IsTrue(
                result.DiagramZoomInCommand != null,
                "The DiagramZoomInCommand command wasn't initialized."
                );
            Assert.IsTrue(
                result.DiagramZoomOutCommand != null,
                "The DiagramZoomOutCommand command wasn't initialized."
                );
            Assert.IsTrue(
                result.DiagramZoomToPageCommand != null,
                "The DiagramZoomToPageCommand command wasn't initialized."
                );
        }

        #endregion
    }
}
