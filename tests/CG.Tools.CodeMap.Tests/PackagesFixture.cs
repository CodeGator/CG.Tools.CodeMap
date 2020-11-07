using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace CG.Tools.CodeMap
{
    /// <summary>
    /// This class is a test fixture for the <see cref="Package"/>
    /// class.
    /// </summary>
    [TestClass]
    [TestCategory("Unit")]
    public class PackagesFixture
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method ensures the <see cref="Package.Parse(string, string[], System.Action{string}, System.Action{string}, System.Action{string, string})"/>
        /// method properly parses a .net assembly.
        /// </summary>
        [TestMethod]
        public void Packages_Parse()
        {
            // Arrange ...
            var fileName = Path.Combine(
                Environment.CurrentDirectory,
                $"{typeof(PackagesFixture).Assembly.GetName().Name}.dll"
                );

            var statusCalled = 0;
            var nodeCalled = 0;
            var connectorCalled = 0;

            // Act ...
            Package.Parse(
                fileName,
                new string[] 
                { 
                    "System.*", 
                    "Microsoft.*", 
                    "netstandard"
                },
                status => statusCalled++,
                node => nodeCalled++,
                (parent, child) => connectorCalled++
                );

            // Assert ...
            Assert.IsTrue(statusCalled != 0, "Status delegate wasn't called.");
            Assert.IsTrue(nodeCalled != 0, "Node delegate wasn't called.");
            Assert.IsTrue(connectorCalled != 0, "Connector delegate wasn't called.");
        }

        #endregion
    }
}
