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

        public static void Parse(
            string filePath,
            string[] filters,
            Action<string> statusAction,
            Action<string> nodeAction,
            Action<string, string> connectorAction
            )
        {
            if (filters == null)
            {
                filters = new string[0];
            }

            var table = new Dictionary<string, IList<string>>();

            statusAction($"Opening: {Path.GetFileName(filePath)}");
            var assembly = Assembly.LoadFrom(
                filePath
                );
            
            statusAction($"Parsing: {Path.GetFileName(filePath)}");
            GetReferencedAssemblies(
                assembly,
                filters,
                ref table
                );

            var count = table.Keys.Count;
            var x = 0;
            foreach (var asmName in table.Keys)
            {
                statusAction($"Adding {x++} of {table.Count} Nodes.");
                nodeAction(asmName);
            }

            foreach (var asmName in table.Keys)
            {
                var children = table[asmName];
                count = children.Count;
                x = 0;
                foreach (var childName in children)
                {
                    statusAction($"Adding {x++} of {children.Count} Nodes.");
                    connectorAction(asmName, childName);
                }
            }

            statusAction($"Done! {table.Count} nodes added.");
        }

        #endregion

        // *******************************************************************
        // Private methods.
        // *******************************************************************

        #region Private methods

        private static void GetReferencedAssemblies(
            this Assembly asm,
            string[] filters,
            ref Dictionary<string, IList<string>> table
            )
        {
            var asmName = asm.GetName();

            if (table.Keys.Any(x => x == asmName.Name) ||
                filters.Any(x => x.IsMatch(asmName.Name)))
            {
                return;
            }

            table[asmName.Name] = new List<string>();
            foreach (var reference in asm.GetReferencedAssemblies())
            {
                if (table.Keys.Any(x => x == reference.Name) ||
                    filters.Any(x => x.IsMatch(reference.Name)))
                {
                    continue;
                }

                table[asmName.Name].Add(reference.Name);

                var asmTemp = Assembly.Load(reference);
                GetReferencedAssemblies(
                    asmTemp,
                    filters,
                    ref table
                    );
            }
        }

        #endregion
    }
}
