using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CG.Tools.CodeMap
{
    /// <summary>
    /// This class utility contains logic to parse .NET packages.
    /// </summary>
    public static class Package
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// Thius method recursively parses references for a .NET assembly.
        /// </summary>
        /// <param name="filePath">The path to use for the operation.</param>
        /// <param name="filters">The filters (if any) to use for the operation.</param>
        /// <param name="statusAction">A status delegate.</param>
        /// <param name="nodeAction">A node delegate.</param>
        /// <param name="connectorAction">A connector delegate.</param>
        public static void Parse(
            string filePath,
            string[] filters,
            Action<string> statusAction,
            Action<string> nodeAction,
            Action<string, string> connectorAction
            )
        {
            var table = new Dictionary<string, IList<string>>();

            // Should we supply default filters?
            if (filters == null)
            {
                filters = new string[0];
            }

            // First open and parse the assembly.

            // Tell the user what's happening.
            statusAction($"Opening: '{Path.GetFileName(filePath)}'");

            try
            {
                // Load the assembly.
                var assembly = Assembly.LoadFrom(
                    filePath
                    );

                statusAction($"Parsing: '{Path.GetFileName(filePath)}'");
                GetReferencedAssemblies(
                    assembly,
                    filters,
                    ref table
                    );
            }
            catch (Exception ex)
            {
                // Tell the user what's happening.
                statusAction($"Error: {ex.Message} while parsing: '{Path.GetFileName(filePath)}'");

                // No results.
                table.Clear();

                // Nothing left to do.
                return;
            }            

            // Now add nodes to the diagram.

            var count = table.Keys.Count;
            var x = 0;
            var nodes = 0;
            foreach (var asmName in table.Keys)
            {
                // Tell the user what's happening.
                statusAction($"Adding {x++} of {table.Count} Nodes.");

                // Add the node to the diagram.
                nodeAction(asmName);

                // Keep track of the node count.
                nodes++;
            }

            // Now add connectors to the diagram.

            var connectors = 0;
            foreach (var asmName in table.Keys)
            {
                var children = table[asmName];
                count = children.Count;
                x = 0;
                foreach (var childName in children)
                {
                    // Tell the user what's happening.
                    statusAction($"Adding {x++} of {children.Count} Connectors to '{asmName}'.");

                    // Add the connector to the diagram.
                    connectorAction(asmName, childName);

                    // Keep track of the connector count.
                    connectors++;
                }
            }

            // Tell the user what's happening.
            statusAction($"Done! {nodes} nodes added with {connectors} connectors.");
        }

        #endregion

        // *******************************************************************
        // Private methods.
        // *******************************************************************

        #region Private methods

        /// <summary>
        /// This method recursively parses out the references for a .NET assembly.
        /// </summary>
        /// <param name="asm">The assembly to us for the operation.</param>
        /// <param name="filters">The filter to use for the operation.</param>
        /// <param name="table">A dictionary of references.</param>
        private static void GetReferencedAssemblies(
            this Assembly asm,
            string[] filters,
            ref Dictionary<string, IList<string>> table
            )
        {
            // Get the name of the assembly.
            var asmName = asm.GetName();

            // Should we ignore this assembly?
            if (table.Keys.Any(x => x == asmName.Name) ||
                filters.Any(x => x.IsMatch(asmName.Name)))
            {
                return; // Nothing more to do.
            }

            // Add the assembly to the table.
            table[asmName.Name] = new List<string>();

            // Loop through all the references.
            foreach (var reference in asm.GetReferencedAssemblies())
            {
                // Should we ignore this assembly?
                if (table.Keys.Any(x => x == reference.Name) ||
                    filters.Any(x => x.IsMatch(reference.Name)))
                {
                    continue; // Nothing more to do.
                }

                // Add the reference to the table.
                table[asmName.Name].Add(reference.Name);

                try
                {
                    // Load the assembly.
                    var asmTemp = Assembly.Load(
                        reference
                        );

                    // Pase the references for the assembly.
                    GetReferencedAssemblies(
                        asmTemp,
                        filters,
                        ref table
                        );
                }
                catch
                {
                    // Just ignore assemblies that won't load.
                }
            }
        }

        #endregion
    }
}
