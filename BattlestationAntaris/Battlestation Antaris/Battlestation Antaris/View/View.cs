using System;
using System.Collections.Generic;
using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View
{

    abstract class View
    {

        public bool is3D;

        public Color backgroundColor;

        public Controller controller;

        public List<HUDElement> allHUDs;

        public View(Controller controller)
        {
            this.controller = controller;
            this.allHUDs = new List<HUDElement>();
            this.is3D = false;
            this.backgroundColor = Color.Black;
        }

        public abstract void Initialize();

        public virtual void Draw()
        {
            this.controller.game.GraphicsDevice.Clear(this.backgroundColor);

            if (this.is3D)
            {
                this.controller.game.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            }

            foreach (HUDElement element in allHUDs)
            {
                element.Draw();
            }
        }

    }

}
