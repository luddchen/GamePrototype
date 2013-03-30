using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD;
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

        #endregion

        #region constructors

        public TactileSpatialObject( String modelName, Vector3 position = new Vector3(), Matrix? rotation = null, Vector3 scale = new Vector3(), bool isVisible = true )
            : base( modelName, position: position, rotation: rotation, scale: scale, isVisible: isVisible ) {
            this.controlDictionary = new Dictionary<Battlestation_Antares.Control.Control, Action>();
        }

        #endregion

        #region methods

        public virtual void OnHit( float damage ) {
        }

        public virtual void OnCollision( TactileSpatialObject otherObject ) {
        }

        public void InjectControl( Battlestation_Antares.Control.Control control ) {
            Action action;
            if ( controlDictionary.TryGetValue( control, out action ) ) {
                if ( action != null ) {
                    action();
                }
            }
        }

        public void InjectControl( List<Battlestation_Antares.Control.Control> controlSequence ) {
            foreach ( Battlestation_Antares.Control.Control control in controlSequence ) {
                InjectControl( control );
            }
        }

        #endregion

    }

}
