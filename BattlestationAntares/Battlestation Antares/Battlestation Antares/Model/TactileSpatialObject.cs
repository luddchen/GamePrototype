using System;
using System.Collections.Generic;
using Battlestation_Antares;
using Battlestation_Antares.Control;
using Battlestation_Antares.Model;
using Battlestation_Antares.Tools;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
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

            protected Dictionary<Command, Action> controlDictionary;

            /// <summary>
            /// the number of rotations until the rotation matrix should be repaired
            /// </summary>
            public static int MAX_ROTATION_UNTIL_REPAIR = 3600;

            /// <summary>
            /// a counter to determine the need of rotation matrix repair
            /// </summary>
            protected int rotationRepairCountdown;

        #endregion


        public TactileSpatialObject( String modelName, Vector3 position = new Vector3(), Matrix? rotation = null, Vector3? scale = null, bool isVisible = true )
            : base( modelName, position: position, rotation: rotation, scale: scale, isVisible: isVisible ) 
        {
            this.controlDictionary = new Dictionary<Command, Action>();
            this.attributes = new SpatialObjectAttributes( SpatialObjectFactory.GetAttributes( modelName ) );
            this.objectType = ObjectType.FRIEND;
            this.rotationRepairCountdown = TactileSpatialObject.MAX_ROTATION_UNTIL_REPAIR;

            this.bounding = SpatialObjectFactory.GetBounding( modelName );

            Antares.world.Add( this );
            _initControlDictionary();

            this.miniMapIcon = new MiniMapIcon( SpatialObjectFactory.GetMapIcon( modelName ), this );
            this.miniMapIcon.color = MiniMap.FRIEND_COLOR;
            _initMiniMapIcon();
        }

        protected virtual void _initControlDictionary() {
            this.controlDictionary[Command.INCREASE_THROTTLE] = _increaseThrottle;
            this.controlDictionary[Command.DECREASE_THROTTLE] = _decreaseThrottle;
            this.controlDictionary[Command.ZERO_THROTTLE] = _zeroThrottle;
            this.controlDictionary[Command.YAW_LEFT] = _yawLeft;
            this.controlDictionary[Command.YAW_RIGHT] = _yawRight;
            this.controlDictionary[Command.PITCH_DOWN] = _pitchDown;
            this.controlDictionary[Command.PITCH_UP] = _pitchUp;
            this.controlDictionary[Command.ROLL_CLOCKWISE] = _rollClockwise;
            this.controlDictionary[Command.ROLL_ANTICLOCKWISE] = _rollAnticlockwise;
        }


        protected virtual void _initMiniMapIcon() {}

        #region methods

        /// <summary>
        /// update the spatial object
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update( GameTime gameTime ) {
            if ( Math.Abs(this.attributes.Engine.CurrentVelocity) > this.attributes.Engine.ResetForce ) {
                this.globalPosition += Vector3.Multiply( rotation.Forward, this.attributes.Engine.CurrentVelocity );
            }
            if ( Math.Abs( this.attributes.EngineYaw.CurrentVelocity ) > this.attributes.EngineYaw.ResetForce ) {
                this.rotation = Tools.Yaw( this.rotation, this.attributes.EngineYaw.CurrentVelocity );
                this.rotationRepairCountdown--;
            }
            if ( Math.Abs( this.attributes.EnginePitch.CurrentVelocity ) > this.attributes.EnginePitch.ResetForce ) {
                this.rotation = Tools.Pitch( this.rotation, this.attributes.EnginePitch.CurrentVelocity );
                this.rotationRepairCountdown--;
            }
            if ( Math.Abs( this.attributes.EngineRoll.CurrentVelocity ) > this.attributes.EngineRoll.ResetForce ) {
                this.rotation = Tools.Roll( this.rotation, this.attributes.EngineRoll.CurrentVelocity );
                this.rotationRepairCountdown--;
            }

            // repair rotation matrix if necessary
            if ( this.rotationRepairCountdown < 0 ) {
                this.rotation = Tools.Repair( this.rotation );
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

        public virtual void InjectControl( Command command ) {
            Action action;
            if ( controlDictionary.TryGetValue( command, out action ) ) {
                if ( action != null ) {
                    action();
                }
            }
        }

        public virtual void InjectControl( List<Command> commandSequence ) {
            foreach ( Command command in commandSequence ) {
                InjectControl( command );
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
