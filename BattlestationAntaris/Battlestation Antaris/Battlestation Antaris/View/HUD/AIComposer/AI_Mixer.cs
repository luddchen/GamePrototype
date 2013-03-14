using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD.AIComposer
{

    public class AI_Mixer : AI_Item
    {

        public enum MixerType
        {
            AVG,
            MULTIPLY,
            MIN,
            MAX
        }

        public AI_Mixer(Vector2 abstractPosition, HUDType positionType, Game1 game)
            : base(abstractPosition, positionType, game)
        {
            this.typeString.String = "Mixer";

            AddPort(AI_ItemPort.PortType.INPUT);
            AddPort(AI_ItemPort.PortType.INPUT);
            AddPort(AI_ItemPort.PortType.OUTPUT);

            SetSubType(MixerType.AVG);
        }

    }

}
