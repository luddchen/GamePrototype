using Battlestation_Antares.View.HUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Battlestation_Antares.Tools {

    public class DebugElement {
        public delegate Object DebugCalc( Object resultObj );

        public Boolean active;

        private Object obj;
        private String name;
        private DebugCalc debugCalc;

        public DebugElement( Object obj, String name, DebugCalc debugCalc ) {
            this.obj = obj;
            this.name = name;
            this.debugCalc = debugCalc;
            this.active = true;
        }

        public String getDebugString() {
            String debugString = "";
            if ( active ) {
                debugString = this.name + ": " + debugCalc( obj ).ToString();
            }
            return debugString;
        }

    }
}
