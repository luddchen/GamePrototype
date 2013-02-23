using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpatialObjectAttributesLibrary;

namespace Battlestation_Antaris.Model
{

    /// <summary>
    /// the basis class for spatial objects
    /// </summary>
    public class SpatialObject
    {

        /// <summary>
        /// the number of rotations until the rotation matrix should be repaired
        /// </summary>
        public static int MAX_ROTATION_UNTIL_REPAIR = 14400;

        /// <summary>
        /// the 3D model
        /// </summary>
        public Microsoft.Xna.Framework.Graphics.Model model3d;


        /// <summary>
        /// the transform matrices for all 3D model parts
        /// </summary>
        public Matrix[] boneTransforms;


        /// <summary>
        /// the physical attributes of the spatial object
        /// </summary>
        public SpatialObjectAttributes attributes;


        /// <summary>
        /// the objects rotation matrix
        /// rotation.Forward is the moving direction
        /// </summary>
        public Matrix rotation;


        /// <summary>
        /// the position within the world model
        /// </summary>
        public Vector3 globalPosition;


        /// <summary>
        /// the visibility of this spatial object
        /// </summary>
        public bool isVisible;


        /// <summary>
        /// a bounding sphere that contains the full (non translated) 3D model
        /// </summary>
        public BoundingSphere bounding;


        /// <summary>
        /// the containing world model
        /// </summary>
        protected WorldModel world;


        /// <summary>
        /// a counter to determine the need of rotation matrix repair
        /// </summary>
        private int rotationRepairCountdown;


        /// <summary>
        /// apply reset force if no active pitching
        /// </summary>
        private bool resetPitch = true;


        /// <summary>
        /// apply reset force if no active yawing
        /// </summary>
        private bool resetYaw = true;


        /// <summary>
        /// apply reset force if no active rolling
        /// </summary>
        private bool resetRoll = true;


        /// <summary>
        /// apply reset force if no active rolling
        /// </summary>
        private bool resetEngine = true;


        /// <summary>
        /// create a new spatial object within the world model
        /// </summary>
        /// <param name="position">world position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        /// <param name="world">the world model</param>
        public SpatialObject(Vector3 position, String modelName, ContentManager content, WorldModel world)
        {
            this.world = world;
            this.isVisible = true;
            this.globalPosition = position;
            this.rotation = Matrix.Identity;
            this.model3d = content.Load<Microsoft.Xna.Framework.Graphics.Model>(modelName);
            this.boneTransforms = new Matrix[model3d.Bones.Count];
            this.attributes = new SpatialObjectAttributes();
            this.rotationRepairCountdown = SpatialObject.MAX_ROTATION_UNTIL_REPAIR;

            // compute the bounding sphere of the whole 3D model
            this.bounding = new BoundingSphere();
            this.model3d.CopyAbsoluteBoneTransformsTo(this.boneTransforms);
            foreach (ModelMesh mesh in model3d.Meshes)
            {
                this.bounding = BoundingSphere.CreateMerged(mesh.BoundingSphere.Transform(this.boneTransforms[mesh.ParentBone.Index]), this.bounding);
            }

            this.world.addObject(this);
        }


        /// <summary>
        /// update the spatial object
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public virtual void Update(GameTime gameTime)
        {
            ApplyMovement(gameTime);
            ApplyRotation(gameTime);
        }


        /// <summary>
        /// apply object movement
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected void ApplyMovement(GameTime gameTime)
        {
            this.globalPosition += Vector3.Multiply(rotation.Forward, this.attributes.Engine.CurrentVelocity);

            if (resetEngine)
            {
                this.attributes.Engine.ApplyResetForce();
            }

            resetEngine = true;
        }


        /// <summary>
        /// apply engines rotation
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected void ApplyRotation(GameTime gameTime) 
        {

            // rotate
            if (this.attributes.EngineYaw.CurrentVelocity != 0)
            {
                this.rotation = Tools.Tools.Yaw(this.rotation, this.attributes.EngineYaw.CurrentVelocity);
                this.rotationRepairCountdown--;
            }

            if (this.attributes.EnginePitch.CurrentVelocity != 0)
            {
                this.rotation = Tools.Tools.Pitch(this.rotation, this.attributes.EnginePitch.CurrentVelocity);
                this.rotationRepairCountdown--;
            }

            if (this.attributes.EngineRoll.CurrentVelocity != 0)
            {
                this.rotation = Tools.Tools.Roll(this.rotation, this.attributes.EngineRoll.CurrentVelocity);
                this.rotationRepairCountdown--;
            }


            // repair rotation matrix if necessary
            if (this.rotationRepairCountdown < 0)
            {
                Vector3 fwd = this.rotation.Forward;
                Vector3 up = this.rotation.Up;
                Vector3 right = this.rotation.Right;
                fwd.Normalize();
                up.Normalize();
                right.Normalize();

                this.rotation = Matrix.Identity;
                this.rotation.Forward = fwd;
                this.rotation.Up = up;
                this.rotation.Right = right;
                this.rotationRepairCountdown = SpatialObject.MAX_ROTATION_UNTIL_REPAIR;
                Console.Out.WriteLine("repair rotation matrix");
            }


            // apply reset forces if no active control
            if (resetPitch)
            {
                this.attributes.EnginePitch.ApplyResetForce();
            }

            if (resetYaw)
            {
                this.attributes.EngineYaw.ApplyResetForce();
            }

            if (resetRoll)
            {
                this.attributes.EngineRoll.ApplyResetForce();
            }

            resetPitch = true;
            resetYaw = true;
            resetRoll = true;
        }


        /// <summary>
        /// inject multiple control requests to this object
        /// </summary>
        /// <param name="controlSequence"></param>
        public virtual void InjectControl(List<Control.Control> controlSequence) 
        {
            foreach (Control.Control control in controlSequence)
            {
                InjectControl(control);
            }
        }


        /// <summary>
        /// inject a single control request to this object
        /// </summary>
        /// <param name="control"></param>
        public virtual void InjectControl(Control.Control control)
        {
            // experimental control stuff

            switch (control)
            {
                case Control.Control.PITCH_UP:
                    this.attributes.EnginePitch.CurrentVelocity += this.attributes.EnginePitch.Acceleration;
                    if (this.attributes.EnginePitch.CurrentVelocity > this.attributes.EnginePitch.MaxVelocity)
                    {
                        this.attributes.EnginePitch.CurrentVelocity = this.attributes.EnginePitch.MaxVelocity;
                    }
                    resetPitch = false;
                    break;

                case Control.Control.PITCH_DOWN:
                    this.attributes.EnginePitch.CurrentVelocity -= this.attributes.EnginePitch.Acceleration;
                    if (this.attributes.EnginePitch.CurrentVelocity < -this.attributes.EnginePitch.MaxVelocity)
                    {
                        this.attributes.EnginePitch.CurrentVelocity = -this.attributes.EnginePitch.MaxVelocity;
                    }
                    resetPitch = false;
                    break;

                case Control.Control.YAW_LEFT:
                    this.attributes.EngineYaw.CurrentVelocity += this.attributes.EngineYaw.Acceleration;
                    if (this.attributes.EngineYaw.CurrentVelocity > this.attributes.EngineYaw.MaxVelocity)
                    {
                        this.attributes.EngineYaw.CurrentVelocity = this.attributes.EngineYaw.MaxVelocity;
                    }
                    resetYaw = false;
                    break;

                case Control.Control.YAW_RIGHT:
                    this.attributes.EngineYaw.CurrentVelocity -= this.attributes.EngineYaw.Acceleration;
                    if (this.attributes.EngineYaw.CurrentVelocity < -this.attributes.EngineYaw.MaxVelocity)
                    {
                        this.attributes.EngineYaw.CurrentVelocity = -this.attributes.EngineYaw.MaxVelocity;
                    }
                    resetYaw = false;
                    break;

                case Control.Control.ROLL_CLOCKWISE:
                    this.attributes.EngineRoll.CurrentVelocity += this.attributes.EngineRoll.Acceleration;
                    if (this.attributes.EngineRoll.CurrentVelocity > this.attributes.EngineRoll.MaxVelocity)
                    {
                        this.attributes.EngineRoll.CurrentVelocity = this.attributes.EngineRoll.MaxVelocity;
                    }
                    resetRoll = false;
                    break;

                case Control.Control.ROLL_ANTICLOCKWISE:
                    this.attributes.EngineRoll.CurrentVelocity -= this.attributes.EngineRoll.Acceleration;
                    if (this.attributes.EngineRoll.CurrentVelocity < -this.attributes.EngineRoll.MaxVelocity)
                    {
                        this.attributes.EngineRoll.CurrentVelocity = -this.attributes.EngineRoll.MaxVelocity;
                    }
                    resetRoll = false;
                    break;

                case Control.Control.INCREASE_THROTTLE:
                    this.attributes.Engine.CurrentVelocity += this.attributes.Engine.Acceleration;
                    if (this.attributes.Engine.CurrentVelocity > this.attributes.Engine.MaxVelocity)
                    {
                        this.attributes.Engine.CurrentVelocity = this.attributes.Engine.MaxVelocity;
                    }
                    resetEngine = false;
                    break;

                case Control.Control.DECREASE_THROTTLE:
                    this.attributes.Engine.CurrentVelocity -= this.attributes.Engine.Acceleration;
                    if (this.attributes.Engine.CurrentVelocity < -this.attributes.Engine.MaxVelocity)
                    {
                        this.attributes.Engine.CurrentVelocity = -this.attributes.Engine.MaxVelocity;
                    }
                    resetEngine = false;
                    break;

                case Control.Control.ZERO_THROTTLE:
                    this.attributes.Engine.CurrentVelocity = 0f;
                    resetEngine = false;
                    break;
            }

        }

    }
}

