using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpatialObjectAttributesLibrary;

namespace Battlestation_Antaris.Model
{

    public class SpatialObject
    {
        public Microsoft.Xna.Framework.Graphics.Model model3d;

        public Matrix[] boneTransforms;

        //public float speed;

        public SpatialObjectAttributes attributes;

        public Matrix rotation;

        public Vector3 globalPosition;

        public bool draw;

        public BoundingSphere bounding;

        bool resetPitch = true;
        bool resetYaw = true;
        bool resetRoll = true;

        public SpatialObject(Vector3 position, String modelName, ContentManager content)
        {
            this.draw = true;
            this.globalPosition = position;
            this.rotation = Matrix.Identity;
            this.model3d = content.Load<Microsoft.Xna.Framework.Graphics.Model>(modelName);
            this.boneTransforms = new Matrix[model3d.Bones.Count];
            //this.speed = 0.0f;
            this.attributes = new SpatialObjectAttributes();

            // experimental bounding stuff --------------------------
            this.bounding = new BoundingSphere();
            this.model3d.CopyAbsoluteBoneTransformsTo(this.boneTransforms);
            foreach (ModelMesh mesh in model3d.Meshes)
            {
                this.bounding = BoundingSphere.CreateMerged( mesh.BoundingSphere.Transform(this.boneTransforms[mesh.ParentBone.Index]), this.bounding);
            }
            // -------------------------------------------
        }

        public SpatialObject(Vector3 position, String modelName, ContentManager content, WorldModel world) : this(position, modelName, content)
        {
            world.allObjects.Add(this);
        }

        public virtual void Update(GameTime gameTime)
        {
            this.globalPosition += Vector3.Multiply(rotation.Forward, this.attributes.Engine.CurrentVelocity);
            Pitch(this.attributes.EnginePitch.CurrentVelocity);
            Yaw(this.attributes.EngineYaw.CurrentVelocity);
            Roll(this.attributes.EngineRoll.CurrentVelocity);

            if (resetPitch)
            {
                if (this.attributes.EnginePitch.CurrentVelocity >= this.attributes.EnginePitch.Acceleration)
                {
                    this.attributes.EnginePitch.CurrentVelocity -= this.attributes.EnginePitch.Acceleration;
                }
                else if (this.attributes.EnginePitch.CurrentVelocity <= -this.attributes.EnginePitch.Acceleration)
                {
                    this.attributes.EnginePitch.CurrentVelocity += this.attributes.EnginePitch.Acceleration;
                }
                else
                {
                    this.attributes.EnginePitch.CurrentVelocity = 0;
                }
            }

            if (resetYaw)
            {
                if (this.attributes.EngineYaw.CurrentVelocity >= this.attributes.EngineYaw.Acceleration)
                {
                    this.attributes.EngineYaw.CurrentVelocity -= this.attributes.EngineYaw.Acceleration;
                }
                else if (this.attributes.EngineYaw.CurrentVelocity <= -this.attributes.EngineYaw.Acceleration)
                {
                    this.attributes.EngineYaw.CurrentVelocity += this.attributes.EngineYaw.Acceleration;
                }
                else
                {
                    this.attributes.EngineYaw.CurrentVelocity = 0;
                }
            }

            if (resetRoll)
            {
                if (this.attributes.EngineRoll.CurrentVelocity >= this.attributes.EngineRoll.Acceleration)
                {
                    this.attributes.EngineRoll.CurrentVelocity -= this.attributes.EngineRoll.Acceleration;
                }
                else if (this.attributes.EngineRoll.CurrentVelocity <= -this.attributes.EngineRoll.Acceleration)
                {
                    this.attributes.EngineRoll.CurrentVelocity += this.attributes.EngineRoll.Acceleration;
                }
                else
                {
                    this.attributes.EngineRoll.CurrentVelocity = 0;
                }
            }

            resetPitch = true;
            resetYaw = true;
            resetRoll = true;
        }

        public virtual void InjectControl(List<Control.Control> controlSequence) 
        {
            foreach (Control.Control control in controlSequence)
            {
                InjectControl(control);
            }
        }

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
                    break;

                case Control.Control.DECREASE_THROTTLE:
                    this.attributes.Engine.CurrentVelocity -= this.attributes.Engine.Acceleration;
                    if (this.attributes.Engine.CurrentVelocity < -this.attributes.Engine.MaxVelocity)
                    {
                        this.attributes.Engine.CurrentVelocity = -this.attributes.Engine.MaxVelocity;
                    }
                    break;

                case Control.Control.ZERO_THROTTLE:
                    this.attributes.Engine.CurrentVelocity = 0f;
                    break;
            }

        }

        protected void Pitch(float angle)
        {
            Matrix axisRotation = Matrix.CreateFromAxisAngle(rotation.Right, angle);
            rotation = rotation * axisRotation;
        }

        protected void Roll(float angle)
        {
            Matrix axisRotation = Matrix.CreateFromAxisAngle(rotation.Forward, angle);
            rotation = rotation * axisRotation;
        }

        protected void Yaw(float angle)
        {
            Matrix axisRotation = Matrix.CreateFromAxisAngle(rotation.Up, angle);
            rotation = rotation * axisRotation;
        }
    }
}

