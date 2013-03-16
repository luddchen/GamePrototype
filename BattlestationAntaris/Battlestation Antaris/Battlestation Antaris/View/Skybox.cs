using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlestation_Antaris.View
{
    class Skybox
    {
        private Game1 game;
        private Microsoft.Xna.Framework.Graphics.Model skybox;
        private Matrix[] boneTransforms;


        public Skybox(String name, Game1 game)
        {
            this.game = game;
            
            this.skybox = this.game.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(name);
            boneTransforms = new Matrix[this.skybox.Bones.Count];
        }

        /// <summary>
        /// draw this element
        /// </summary>
        public void Draw(Camera camera)
        {
            Tools.Draw3D.Draw(skybox, boneTransforms, camera.view, camera.projection,
                                this.game.world.spaceShip.globalPosition, Matrix.Identity, 
                                new Vector3(camera.farClipping * 0.99f));
        }
    }
}
