using System;

namespace UnofficalFretlightSDK
{
    // code inspired from Griffin PowerMate project by Thomas Hammer (www.thammer.net)
 
    /// <summary>
    /// Provides data for Fretlight events.
    /// </summary>
    public class FretlightEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the FretlightEventArgs
        /// </summary>
        /// <param name="state"></param>
        public FretlightEventArgs(FretlightState state)
        {
            State = state;
        }

        /// <summary>
        /// Gets the current Fretlight state.
        /// </summary>
        public FretlightState State { get; private set; }
    }
}
