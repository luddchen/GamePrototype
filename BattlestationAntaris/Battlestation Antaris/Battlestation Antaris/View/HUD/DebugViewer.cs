using Battlestation_Antaris.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlestation_Antaris.View.HUD
{
    public class DebugViewer : HUD2DContainer
    {
        public Boolean activeDebugging;

        private List<DebugElement> debugElements;

        private List<HUD2DString> outputStrings;

        public DebugViewer(Game1 game) : base(new Vector2(10f, 0.05f), HUDType.ABSOLUT_RELATIV, game)
        {
            this.activeDebugging = false;
            this.debugElements = new List<DebugElement>();
            this.outputStrings = new List<HUD2DString>();
        }

        public void Add(DebugElement debugElement)
        {
            HUD2DString newHUDStr = new HUD2DString("", null, null, null, null, 0.35f, 0.0f, this.game);
            this.debugElements.Add(debugElement);
            this.outputStrings.Add(newHUDStr);

            this.Add(newHUDStr);

        }

        public void Remove(DebugElement debugElement)
        {
            this.debugElements.Remove(debugElement);
            this.outputStrings.RemoveAt(0);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spritBatch)
        {
            int nrActive = 0;
            DebugElement debugElement;
            HUD2DString outputStr;
            for (int i = 0; i < debugElements.Count; i++)
            {
                debugElement = debugElements.ElementAt(i);
                outputStr = outputStrings.ElementAt(i);
                if (debugElement.active)
                {
                    nrActive++;
                }
                outputStr.String = debugElement.getDebugString();
                outputStr.abstractPosition = new Vector2(outputStr.size.X / 2, 10f * nrActive);
                outputStr.ClientSizeChanged();
            }
            base.Draw(spritBatch);
        }
    }
}
