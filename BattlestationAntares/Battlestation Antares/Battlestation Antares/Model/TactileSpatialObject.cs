using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD;
using SpatialObjectAttributesLibrary;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.Tools;
using Battlestation_Antares;

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

        #region constructors

        public TactileSpatialObject( String modelName, Vector3 position = new Vector3(), Matrix? rotation = null, Vector3? scale = null, bool isVisible = true )
            : base( modelName, position: position, rotation: rotation, scale: scale, isVisible: isVisible ) 
        {
            this.controlDictionary = new Dictionary<Battlestation_Antares.Control.Control, Action>();
            this.attributes = new SpatialObjectAttributes();
            this.objectType = ObjectType.FRIEND;
            this.rotationRepairCountdown = TactileSpatialObject.MAX_ROTATION_UNTIL_REPAIR;

            // compute the bounding sphere of the whole 3D model and initialize world lightning
            this.bounding = new BoundingSphere();
            this.model.CopyAbsoluteBoneTransformsTo( this.boneTransforms );
            foreach ( ModelMesh mesh in model.Meshes ) {
                this.bounding = BoundingSphere.CreateMerged( mesh.BoundingSphere.Transform( this.boneTransforms[mesh.ParentBone.Index] ), this.bounding );
                foreach ( BasicEffect effect in mesh.Effects ) {
                    Draw3D.Lighting1( effect );
                }
            }

            Antares.world.Add( this );
        }

        #endregion

        #region methods

        public virtual void OnHit( float damage ) {
            if ( this.attributes.Shield.ApplyDamage( damage ) ) {
                this.attributes.Hull.ApplyDamage( damage );
            }
        }

        public virtual void OnCollision( TactileSpatialObject otherObject ) {
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

    }

}
