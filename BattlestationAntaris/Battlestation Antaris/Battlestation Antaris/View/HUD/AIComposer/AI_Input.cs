using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD.AIComposer
{

    public class AI_Input : AI_Item
    {

        public enum InputType
        {
            DISTANCE,
            ORTHOGONAL_VELOCITY,
            ROTATION,
            SHIELD_HEALTH,
            HULL_HEALTH
        }

        public AI_Input(Vector2 abstractPosition, HUDType positionType, Game1 game) 
            : base(abstractPosition, positionType, game)
        {
            this.subType = InputType.DISTANCE;
            this.itemTypeName = "Input";
            this.typeString.String = this.itemTypeName;

            AddPort(AI_ItemPort.PortType.OUTPUT);

            this.subTypeString.String = this.subType.ToString();
        }

    }

}
