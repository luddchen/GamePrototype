using System;
using System.Collections.Generic;
using Battlestation_Antares;
using Battlestation_Antares.Tools;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpatialObjectAttributesLibrary;

namespace Battlestation_Antaris.Model {

    class TactileSpatialObject : SpatialObject {

        public enum ObjectType {

            FRIEND,

            ENEMY,

            OBSTACLE,

            PROJECTILE
        }

        #region elements

            public BoundingSphere bounding;

            public ObjectType objectType;

            public MiniMapIcon miniMapIcon;

            public SpatialObjectAttributes attributes;

            protected Dictionary<Battlestation_Antares.Control.Control, Action> controlDictionary;

            /// <summary>
            /// the number of rotations until the rotation matrix should be repaired
            /// </summary>
            public static int MAX_ROTATION_UNTIL_REPAIR = 14400;

            /// <summary>
            /// a counter to determine the need of rotation matrix repair
            /// </summary>
            protected int rotationRepairCountdown;

        #endregion


        public TactileSpatialObject( String modelName, Vector3 position = new Vector3(), Matrix? rotation = null, Vector3? scale = null, bool isVisible = true )
            : base( modelName, position: position, rotation: rotation, scale: scale, isVisible: isVisible ) 
        {
            this.controlDictionary = new Dictionary<Battlestation_Antares.Control.Control, Action>();
            this.attributes = new SpatialObjectAttributes();
            this.objectType = ObjectType.FRIEND;
            this.rotationRepairCountdown = TactileSpatialObject.MAX_ROTATION_UNTIL_REPAIR;

            _initBounding();

            Antares.world.Add( this );
            _initActionDictionary();

            this.miniMapIcon = new MiniMapIcon( null, this );
            _initMiniMapIcon();
        }

        protected virtual void _initActionDictionary() {
            this.controlDictionary[Battlestation_Antares.Control.Control.INCREASE_THROTTLE] = _increaseThrottle;
            this.controlDictionary[Battlestation_Antares.Control.Control.DECREASE_THROTTLE] = _decreaseThrottle;
            this.controlDictionary[Battlestation_Antares.Control.Control.ZERO_THROTTLE] = _zeroThrottle;
            this.controlDictionary[Battlestation_Antares.Control.Control.YAW_LEFT] = _yawLeft;
            this.controlDictionary[Battlestation_Antares.Control.Control.YAW_RIGHT] = _yawRight;
            this.controlDictionary[Battlestation_Antares.Control.Control.PITCH_DOWN] = _pitchDown;
            this.controlDictionary[Battlestation_Antares.Control.Control.PITCH_UP] = _pitchUp;
            this.controlDictionary[Battlestation_Antares.Control.Control.ROLL_CLOCKWISE] = _rollClockwise;
            this.controlDictionary[Battlestation_Antares.Control.Control.ROLL_ANTICLOCKWISE] = _rollAnticlockwise;
        }

        protected virtual void _initMiniMapIcon() {
            this.miniMapIcon.Texture = Antares.content.Load<Texture2D>( "Sprites//Circle" );
            this.miniMapIcon.color = MiniMap.FRIEND_COLOR;
        }

        protected virtual void _initBounding() {
            this.bounding = new BoundingSphere();
            this.model.CopyAbsoluteBoneTransformsTo( this.boneTransforms );
            foreach ( ModelMesh mesh in model.Meshes ) {
                this.bounding = BoundingSphere.CreateMerged( mesh.BoundingSphere.Transform( this.boneTransforms[mesh.ParentBone.Index] ), this.bounding );
                foreach ( BasicEffect effect in mesh.Effects ) {
                    Draw3D.Lighting1( effect );
                }
            }
        }


        #region methods

        /// <summary>
        /// update the spatial object
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update( GameTime gameTime ) {
            if ( this.attributes.Engine.CurrentVelocity != 0 ) {
                this.globalPosition += Vector3.Multiply( rotation.Forward, this.attributes.Engine.CurrentVelocity );
            }
            if ( this.attributes.EngineYaw.CurrentVelocity != 0 ) {
                this.rotation = Tools.Yaw( this.rotation, this.attributes.EngineYaw.CurrentVelocity );
                this.rotationRepairCountdown--;
            }
            if ( this.attributes.EnginePitch.CurrentVelocity != 0 ) {
                this.rotation = Tools.Pitch( this.rotation, this.attributes.EnginePitch.CurrentVelocity );
                this.rotationRepairCountdown--;
            }
            if ( this.attributes.EngineRoll.CurrentVelocity != 0 ) {
                this.rotation = Tools.Roll( this.rotation, this.attributes.EngineRoll.CurrentVelocity );
                this.rotationRepairCountdown--;
            }

            // repair rotation matrix if necessary
            if ( this.rotationRepairCountdown < 0 ) {
                Tools.Repair( ref this.rotation );
                this.rotationRepairCountdown = MAX_ROTATION_UNTIL_REPAIR;
            }

            this.attributes.Update( gameTime );
        }

        public virtual void OnHit( float damage ) {
            float unabsorbedDamage = this.attributes.Shield.ApplyDamage( damage );
            if ( unabsorbedDamage > 0 ) {
                if ( this.attributes.Hull.ApplyDamage( unabsorbedDamage ) > 0 ) {
                    OnDeath();
                }
            }
        }

        public virtual void OnCollision( TactileSpatialObject otherObject ) {
        }

        public virtual void OnDeath() {
        }

        public virtual void InjectControl( Battlestation_Antares.Control.Control control ) {
            Action action;
            if ( controlDictionary.TryGetValue( control, out action ) ) {
                if ( action != null ) {
                    action();
                }
            }
        }

        public virtual void InjectControl( List<Battlestation_Antares.Control.Control> controlSequence ) {
            foreach ( Battlestation_Antares.Control.Control control in controlSequence ) {
                InjectControl( control );
            }
        }

        #endregion


        #region predefined Actions

        protected void _increaseThrottle() {
            this.attributes.Engine.Accelerate();
        }

        protected void _decreaseThrottle() {
            this.attributes.Engine.Decelerate();
        }

        protected void _zeroThrottle() {
            this.attributes.Engine.CurrentVelocity = 0f;
        }

        protected void _pitchUp() {
            this.attributes.EnginePitch.Accelerate();
        }

        protected void _pitchDown() {
            this.attributes.EnginePitch.Decelerate();
        }

        protected void _yawLeft() {
            this.attributes.EngineYaw.Accelerate();
        }

        protected void _yawRight() {
            this.attributes.EngineYaw.Decelerate();
        }

        protected void _rollClockwise() {
            this.attributes.EngineRoll.Accelerate();
        }

        protected void _rollAnticlockwise() {
            this.attributes.EngineRoll.Decelerate();
        }

        #endregion

    }

}
