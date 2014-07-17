using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnofficalFretlightSDK
{
    /// <summary>
    /// State for the Fretlight Foot Switch - Off or On
    /// code inspired by Thomas Hammer (www.thammer.net) from Griffin PowerMate project 
    /// </summary>
    public enum FootSwitchState
    {
        Off = 0, On = 1
    }

    /// <summary>
    /// Holds the state for the Fretlight foot switch
    /// switch one and switch two settings.
    /// </summary>
    public struct FretlightState
    {
        private FootSwitchState switchOne; // byte 0
        private FootSwitchState switchTwo; // byte 1
        private bool valid;

        /// <summary>
        /// Initializes a new instance (valid) of FretlightStat.
        /// </summary>
        /// <param name="switchOne"></param>
        /// <param name="switchTwo"></param>
        public FretlightState(FootSwitchState switchOne,
                              FootSwitchState switchTwo)
        {
            this.switchOne = switchOne;
            this.switchTwo = switchTwo;
            this.valid = true;
        }

        /// <summary>
        /// Gets status of Fretlight Foot Switch One (On/Off)
        /// </summary>
        public FootSwitchState SwitchOne
        {
            get { return switchOne; }
        }

        /// <summary>
        /// Gets status of Fretlight Foot Switch Two (On/Off)
        /// </summary>
        public FootSwitchState SwitchTwo
        {
            get { return switchTwo; }
        }

        /// <summary>
        /// Gets a value indicating in the FretlightState instance is valid.
        /// </summary>
        public bool IsValid
        {
            get { return valid; }
        }
    }
}

