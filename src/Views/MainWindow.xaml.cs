using CG.Tools.CodeMap.Infrastructure;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Shared;
using System;

namespace CG.Tools.CodeMap.Views
{
    /// <summary>
    /// This class represents the code-behind for the main window.
    /// </summary>
    public partial class MainWindow : ChromelessWindow, IUpdateDiagram
    {
        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="MainWindow"/>
        /// class.
        /// </summary>
        public MainWindow()
        {
            // Tell Syncfusion we'll be using the skin styles.
            SfSkinManager.ApplyStylesOnApplication = true;

            // Keep the designer happy.
            InitializeComponent();
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc/>
        public void Update()
        {
            // Force the diagram to layout again.
            diagram.LayoutManager.Layout.UpdateLayout();
        }

        #endregion

        // *******************************************************************
        // Protected methods.
        // *******************************************************************

        #region Protected methods

        /// <inheritdoc/>
        protected override void OnClosed(EventArgs e)
        {
            // Cleanup the skin manager.
            SfSkinManager.Dispose(this);

            // Give the base class a chance.
            base.OnClosed(e);
        }

        #endregion
    }
}
