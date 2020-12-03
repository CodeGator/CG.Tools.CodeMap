using CG.Reflection;
using CG.Tools.CodeMap.Infrastructure;
using Microsoft.Win32;
using Syncfusion.UI.Xaml.Diagram;
using Syncfusion.UI.Xaml.Diagram.Controls;
using Syncfusion.UI.Xaml.Diagram.Layout;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace CG.Tools.CodeMap.ViewModels
{
    /// <summary>
    /// This class represents a view-model for the main window.
    /// </summary>
    public class MainWindowViewModel : DiagramViewModel
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        private ObservableCollection<FilterViewModel> _filters;
        private string _filePath;
        private string _status;
        private string _caption;
        private bool _isBusy;

        private DelegateCommand _fileExitCommand;
        private DelegateCommand _fileOpenCommand;
        private DelegateCommand _fileCloseCommand;
        private DelegateCommand _filePrintCommand;

        private DelegateCommand _helpAboutCommand;
        private DelegateCommand _diagramLayoutCommand;
        
        private DelegateCommand _diagramZoomRestoreCommand;
        private DelegateCommand _diagramZoomInCommand;
        private DelegateCommand _diagramZoomOutCommand;
        private DelegateCommand _diagramZoomToPageCommand;

        private DelegateCommand _itemDeletingCommand;

        #endregion

        // *******************************************************************
        // Commands.
        // *******************************************************************

        #region Commands

        /// <summary>
        /// This property contains the file|exit command.
        /// </summary>
        public DelegateCommand FileExitCommand =>
            _fileExitCommand ?? (_fileExitCommand = new DelegateCommand(
                ExecuteFileExitCommand,
                CanExecuteFileExitCommand
                ));

        /// <summary>
        /// This property contains the file|open command.
        /// </summary>
        public DelegateCommand FileOpenCommand =>
            _fileOpenCommand ?? (_fileOpenCommand = new DelegateCommand(
                ExecuteFileOpenCommand,
                CanExecuteFileOpenCommand
                ));

        /// <summary>
        /// This property contains the file|close command.
        /// </summary>
        public DelegateCommand FileCloseCommand =>
            _fileCloseCommand ?? (_fileCloseCommand = new DelegateCommand(
                ExecuteFileCloseCommand,
                CanExecuteFileCloseCommand
                ));

        /// <summary>
        /// This property contains the file|print to page command.
        /// </summary>
        public DelegateCommand FilePrintCommand =>
            _filePrintCommand ?? (_filePrintCommand = new DelegateCommand(
                ExecuteFilePrintCommand,
                CanExecuteFilePrintCommand
                ));

        /// <summary>
        /// This property contains the help|about command.
        /// </summary>
        public DelegateCommand HelpAboutCommand =>
            _helpAboutCommand ?? (_helpAboutCommand = new DelegateCommand(
                ExecuteHelpAboutCommand,
                CanExecuteHelpAboutCommand
                ));

        /// <summary>
        /// This property contains the diagram|layout command.
        /// </summary>
        public DelegateCommand DiagramLayoutCommand =>
            _diagramLayoutCommand ?? (_diagramLayoutCommand = new DelegateCommand(
                ExecuteDiagramLayoutCommand,
                CanExecuteDiagramLayoutCommand
                ));

        /// <summary>
        /// This property contains the diagram|zoom restore command.
        /// </summary>
        public DelegateCommand DiagramZoomRestoreCommand =>
            _diagramZoomRestoreCommand ?? (_diagramZoomRestoreCommand = new DelegateCommand(
                ExecuteDiagramZoomRestoreCommand,
                CanExecuteDiagramZoomRestoreCommand
                ));

        /// <summary>
        /// This property contains the diagram|zoom in command.
        /// </summary>
        public DelegateCommand DiagramZoomInCommand =>
            _diagramZoomInCommand ?? (_diagramZoomInCommand = new DelegateCommand(
                ExecuteDiagramZoomInCommand,
                CanExecuteDiagramZoomInCommand
                ));

        /// <summary>
        /// This property contains the diagram|zoom out command.
        /// </summary>
        public DelegateCommand DiagramZoomOutCommand =>
            _diagramZoomOutCommand ?? (_diagramZoomOutCommand = new DelegateCommand(
                ExecuteDiagramZoomOutCommand,
                CanExecuteDiagramZoomOutCommand
                ));

        /// <summary>
        /// This property contains the diagram|zoom to page command.
        /// </summary>
        public DelegateCommand DiagramZoomToPageCommand =>
            _diagramZoomToPageCommand ?? (_diagramZoomToPageCommand = new DelegateCommand(
                ExecuteDiagramZoomToPageCommand,
                CanExecuteDiagramZoomToPageCommand
                ));

        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the application caption.
        /// </summary>
        public string Caption 
        { 
            get { return _caption; }
            set
            {
                // Set the value.
                _caption = value;

                // Tell the world what happened.
                OnPropertyChanged("Caption");
            }
        }

        /// <summary>
        /// This property contains the currently open file.
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                // Set the value.
                _filePath = value;

                // Update the caption and status.
                if (string.IsNullOrEmpty(_filePath))
                {
                    Caption = $"CodeMap - [{typeof(MainWindowViewModel).Assembly.ReadFileVersion()}]";
                    Status = $"Ready";
                }
                else
                {
                    Caption = $"CodeMap - [{typeof(MainWindowViewModel).Assembly.ReadFileVersion()}] " +
                        $"({Path.GetFileName(_filePath)})";
                    Status = $"Opened: {Path.GetFileName(_filePath)}";
                }

                // Tell the world what happened.
                OnPropertyChanged("FilePath");
            }
        }

        /// <summary>
        /// This property contains the application status.
        /// </summary>
        public string Status 
        {
            get { return _status; }
            set
            {
                // Set the value.
                _status = value;

                // Tell the world what happened.
                OnPropertyChanged("Status");
            }
        }

        /// <summary>
        /// This property contains the diagram filters.
        /// </summary>
        public ObservableCollection<FilterViewModel> Filters
        {
            get { return _filters; }
            set
            {
                // Set the value
                _filters = value;

                // Tell the world what happened.
                OnPropertyChanged("Filters");
            }
        }

        /// <summary>
        /// This property indicates whether the app is busy.
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                // Set the value.
                _isBusy = value;

                // Tell the world what happened.
                OnPropertyChanged("IsBusy");
            }
        }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="MainWindowViewModel"/>
        /// class.
        /// </summary>
        public MainWindowViewModel()
        {
            // Setup the filters.
            _filters = new ObservableCollection<FilterViewModel>()
            {
                new FilterViewModel(this) { Filter = "System.*", Enabled = true },
                new FilterViewModel(this) { Filter = "netstandard", Enabled = true },
                new FilterViewModel(this) { Filter = "mscorlib", Enabled = true },
                new FilterViewModel(this) { Filter = "Microsoft.*", Enabled = true }
            };

            // Initialize the VM.
            IntitializeViewModel();
        }

        #endregion

        // *******************************************************************
        // Command handlers.
        // *******************************************************************

        #region Command handlers

        /// <summary>
        /// This method is called to exit the application.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        protected virtual void ExecuteFileExitCommand(object args)
        {
            // Shutdown the app.
            App.Current.Shutdown();
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether it should be possible to call the <see cref="ExecuteFileExitCommand(object)"/>
        /// method.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        /// <returns>True if the method should be called; false otherwise.</returns>
        protected virtual bool CanExecuteFileExitCommand(object args)
        {
            return true;
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to open a file.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        protected virtual void ExecuteFileOpenCommand(object args)
        {
            // The arg is actually the view.
            var owner = args as Window;

            // Create the "file open" dialog.
            var openFileDialog = new OpenFileDialog();

            // Configure the dialog.
            openFileDialog.Filter = "Libraries|*.dll|Executables|*.exe|All Files|*.*";
            openFileDialog.DefaultExt = ".dll";
            openFileDialog.Title = "Select a .NET assembly";
            openFileDialog.CheckPathExists = true;
            
            // Prompt the user.
            if (openFileDialog.ShowDialog(owner) == true)
            {
                // Get the selected file path.
                FilePath = openFileDialog.FileName;

                // Load the diagram.
                DiagramDotNetFile(owner);
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether it should be possible to call the <see cref="ExecuteFileOpenCommand(object)"/>
        /// method.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        /// <returns>True if the method should be called; false otherwise.</returns>
        protected virtual bool CanExecuteFileOpenCommand(object args)
        {
            return true;
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to close a file.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        protected virtual void ExecuteFileCloseCommand(object args)
        {
            // The arg is actually the view.
            var owner = args as Window;

            // Reset the file name.
            FilePath = "";

            // Reset the diagram.
            DiagramDotNetFile(owner);
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether it should be possible to call the <see cref="ExecuteFileCloseCommand(object)"/>
        /// method.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        /// <returns>True if the method should be called; false otherwise.</returns>
        protected virtual bool CanExecuteFileCloseCommand(object args)
        {
            return true;
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to print the file.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        protected virtual void ExecuteFilePrintCommand(object args)
        {
            // TODO : figure this out.
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether it should be possible to call the <see cref="ExecuteFilePrintCommand(object)"/>
        /// method.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        /// <returns>True if the method should be called; false otherwise.</returns>
        protected virtual bool CanExecuteFilePrintCommand(object args)
        {
            return true;
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to show the about box.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        protected virtual void ExecuteHelpAboutCommand(object args)
        {
            // The arg is actually the view.
            var owner = args as Window;
            
            // Show the app details.
            MessageBox.Show(
                owner,
                $"CodeMap - [{typeof(MainWindowViewModel).Assembly.ReadFileVersion()}]" +
                $"{Environment.NewLine}Copyright © 2019 - {DateTime.Today.Year} " +
                $"by CodeGator. All rights reserved.{Environment.NewLine}{Environment.NewLine}" +
                $"Warning: This computer program is protected by copyright law and international treaties. Unauthorized reproduction or distribution of this program, or any portion of it, may result in severe civil and criminal penalties, and will be prosecuted to the full extent of the law.",
                "About CodeMap",
                MessageBoxButton.OK
                );
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether it should be possible to call the <see cref="ExecuteHelpAboutCommand(object)"/>
        /// method.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        /// <returns>True if the method should be called; false otherwise.</returns>
        protected virtual bool CanExecuteHelpAboutCommand(object args)
        {
            return true;
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to layout the diagram.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        protected virtual void ExecuteDiagramLayoutCommand(object args)
        {
            // The arg is actually the view.
            var owner = args as Window;
            try
            {
                Status = "Updating Layout";
                var uw = owner as IUpdateDiagram;
                uw?.Update();
            }
            finally
            {
                Status = "Ready";
            }            
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether it should be possible to call the <see cref="ExecuteDiagramLayoutCommand(object)"/>
        /// method.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        /// <returns>True if the method should be called; false otherwise.</returns>
        protected virtual bool CanExecuteDiagramLayoutCommand(object args)
        {
            return true; 
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to zoom the diagram restore.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        protected virtual void ExecuteDiagramZoomRestoreCommand(object args)
        {
            (Info as IGraphInfo).Commands.Reset.Execute(
                new ResetParameter() { Reset = Reset.ZoomPan }
                );
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether it should be possible to call the <see cref="ExecuteDiagramZoomRestoreCommand(object)"/>
        /// method.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        /// <returns>True if the method should be called; false otherwise.</returns>
        protected virtual bool CanExecuteDiagramZoomRestoreCommand(object args)
        {
            return true;
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to zoom the diagram in.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        protected virtual void ExecuteDiagramZoomInCommand(object args)
        {
            (Info as IGraphInfo).Commands.Zoom.Execute(
                new ZoomPositionParameter() { ZoomCommand = ZoomCommand.ZoomIn, ZoomFactor = 0.2 }
                );
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether it should be possible to call the <see cref="ExecuteDiagramZoomInCommand(object)"/>
        /// method.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        /// <returns>True if the method should be called; false otherwise.</returns>
        protected virtual bool CanExecuteDiagramZoomInCommand(object args)
        {
            return true;
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to zoom the diagram out.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        protected virtual void ExecuteDiagramZoomOutCommand(object args)
        {
            (Info as IGraphInfo).Commands.Zoom.Execute(new ZoomPositionParameter() { ZoomCommand = ZoomCommand.ZoomOut, ZoomFactor = 0.2 });
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether it should be possible to call the <see cref="ExecuteDiagramZoomOutCommand(object)"/>
        /// method.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        /// <returns>True if the method should be called; false otherwise.</returns>
        protected virtual bool CanExecuteDiagramZoomOutCommand(object args)
        {
            return true;
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to zoom the diagram to page.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        protected virtual void ExecuteDiagramZoomToPageCommand(object args)
        {
            (Info as IGraphInfo).Commands.FitToPage.Execute(null);
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether it should be possible to call the <see cref="ExecuteDiagramZoomToPageCommand(object)"/>
        /// method.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        /// <returns>True if the method should be called; false otherwise.</returns>
        protected virtual bool CanExecuteDiagramZoomToPageCommand(object args)
        {
            return true;
        }

        // *******************************************************************

        /// <summary>
        /// This method is called when an item is in the process of being removed
        /// from the diagram.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        protected virtual void ExecuteItemDeletingCommand(object args)
        {
            var e = args as ItemDeletingEventArgs;
            if (null != e)
            {
                var nodeViewModel = e.Item as NodeViewModel;

                // Prompt the user first.
                e.Cancel = MessageBox.Show(
                        $"This will delete the '{nodeViewModel.ID}' node and ALL it's children. " +
                        Environment.NewLine + Environment.NewLine +
                        $"This action is not undoable! " + 
                        Environment.NewLine + Environment.NewLine +
                        "Are you SURE you want to do this?",
                        Caption,
                        MessageBoxButton.YesNo
                    ) != MessageBoxResult.Yes;

                // Delete any children.
                e.DeleteSuccessors = true;
            }            
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether it should be possible to call the <see cref="ExecuteItemDeletingCommand(object)"/>
        /// method.
        /// </summary>
        /// <param name="args">The arguments for the operation.</param>
        /// <returns>True if the method should be called; false otherwise.</returns>
        protected virtual bool CanExecuteItemDeletingCommand(object args)
        {
            return true;
        }

        #endregion

        // *******************************************************************
        // Internal protected methods.
        // *******************************************************************

        #region Internal protected methods

        /// <summary>
        /// This method creates a dependency diagram for the currently opened .NET assembly.
        /// </summary>
        /// <param name="owner">The diagram window.</param>
        internal protected void DiagramDotNetFile(
            Window owner
            )
        {
            try
            {
                // Set the busy flag.
                IsBusy = true;

                // Initialize the diagram.
                IntitializeViewModel();

                // Does the file actually exist?
                if (File.Exists(_filePath))
                {
                    // Make an array of selected filters.
                    var filters = _filters
                        .Where(x => x.Enabled == true)
                        .Select(x => x.Filter)
                        .ToArray();

                    // Parse out the references for the file.
                    Package.Parse(
                       _filePath,
                       filters,
                       status => SetStatus(status),
                       asm => AddNode(asm),
                       (parent, child) => AddConnector(parent, child)
                    );
                }

                // Force the diagram to perform a layout.
                Status = "Updating Layout";
                var ud = owner as IUpdateDiagram;
                ud.Update();
            }
            finally
            {
                // Update the status.
                Status = "Ready";

                // Clear the busy flag.
                IsBusy = false;
            }
        }

        #endregion

        // *******************************************************************
        // Private methods.
        // *******************************************************************

        #region Private methods

        /// <summary>
        /// This method sets the text for the <see cref="Status"/> property.
        /// </summary>
        /// <param name="status"></param>
        private void SetStatus(string status)
        {
            // Is there anything to write?
            if (false == string.IsNullOrEmpty(status))
            {
                // Play nicely with the UI.
                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Background,
                    new ThreadStart(() => Status = status)
                    );
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method adds a connector to the diagram.
        /// </summary>
        /// <param name="parentName">The parent node to use for the operation.</param>
        /// <param name="childName">The child node to use for the operation..</param>
        private void AddConnector(
            string parentName,
            string childName
            )
        {
            // Play nicely with the UI.
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                new ThreadStart(() =>
                {
                    // Get the parent node.
                    var parentNode = (Nodes as NodeCollection).FirstOrDefault(
                        x => x.ID.Equals(parentName)
                        ) as NodeViewModel;

                    // Get the child node.
                    var childNode = (Nodes as NodeCollection).FirstOrDefault(
                        x => x.ID.Equals(childName)
                        ) as NodeViewModel;

                    // Did we find both?
                    if (null != parentNode && null != childNode)
                    {
                        // Add a connector to the diagram.
                        (Connectors as ConnectorCollection).Add(
                            new ConnectorViewModel() 
                            {
                                SourceNode = parentNode, 
                                TargetNode = childNode,
                                Constraints = ConnectorConstraints.Default & ~ConnectorConstraints.Selectable
                            });
                    }
                }));
        }

        // *******************************************************************

        /// <summary>
        /// This method adds a node to the diagram.
        /// </summary>
        /// <param name="asmName">The assembly name to use for the operation.</param>
        private void AddNode(
            string asmName
            )
        {
            // Play nicely with the UI.
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                new ThreadStart(() => 
                {
                    // Add a node to the diagram.
                    (Nodes as NodeCollection).Add(
                        new NodeViewModel()
                        {
                            ID = asmName,
                            Shape = new RectangleGeometry() { Rect = new Rect(0, 0, 1, 1) },
                            ShapeStyle = (Nodes as NodeCollection).Count == 0
                                ? App.Current.Resources["ShapeStyle1"] as Style
                                : App.Current.Resources["ShapeStyle2"] as Style,
                            OffsetX = -200,
                            OffsetY = -200,
                            UnitHeight = (Nodes as NodeCollection).Count == 0 ? 75 : 50,
                            UnitWidth = (Nodes as NodeCollection).Count == 0 ? asmName.Length * 10 : asmName.Length * 7,
                            Annotations = new ObservableCollection<IAnnotation>()
                            {
                                new AnnotationEditorViewModel()
                                {
                                    Content = $"{asmName}"
                                }
                            },
                            Constraints = NodeConstraints.Default & ~(NodeConstraints.Rotatable | NodeConstraints.InheritRotatable)
                        });  
                }));
        }

        // *******************************************************************

        /// <summary>
        /// This method iniializes the properties that are bound to the diagram
        /// on the UI.
        /// </summary>
        private void IntitializeViewModel()
        {
            Caption = $"CodeMap - [{typeof(MainWindowViewModel).Assembly.ReadFileVersion()}]";
            Status = "Ready";

            DefaultConnectorType = ConnectorType.Line;

            Nodes = new NodeCollection();
            Connectors = new ConnectorCollection();

            LayoutManager = new LayoutManager()
            {
                Layout = new RadialTreeLayout()
                {
                    HorizontalSpacing = 10,
                    VerticalSpacing = 55,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                },
                RefreshFrequency = RefreshFrequency.ArrangeParsing
            };

            ScrollSettings = new ScrollSettings()
            {
                ScrollLimit = ScrollLimit.Diagram,
            };

            SelectedItems = new SelectorViewModel()
            {
                SelectorConstraints = SelectorConstraints.Default & ~SelectorConstraints.QuickCommands,
            };

            PageSettings = new PageSettings()
            {
                PageBackground = new SolidColorBrush(Colors.White),
                PageBorderBrush = new SolidColorBrush(Colors.Transparent),
            };

            Constraints = GraphConstraints.Default & ~GraphConstraints.ContextMenu;

            ItemDeletingCommand = _itemDeletingCommand ?? (_itemDeletingCommand = new DelegateCommand(
                ExecuteItemDeletingCommand,
                CanExecuteItemDeletingCommand
                ));
        }

        #endregion
    }
}
