using System;

namespace CG.Tools.CodeMap.Infrastructure
{
    /// <summary>
    /// This interface represents an object that closes a window.
    /// </summary>
    public interface ICloseWindow
    {
        /// <summary>
        /// This method closes a window.
        /// </summary>
        /// <param name="result">Optional results for the close operation.</param>
        void Close(bool? result);
    }
}
