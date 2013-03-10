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

        MixerType mixerType;

        public AI_Mixer(Vector2 abstractPosition, HUDType positionType, Game1 game)
            : base(abstractPosition, positionType, "Mixer", game)
        {
            this.mixerType = MixerType.AVG;

            AddPort(AI_ItemPort.PortType.INPUT);
            AddPort(AI_ItemPort.PortType.INPUT);
            AddPort(AI_ItemPort.PortType.OUTPUT);
        }

    }

}
