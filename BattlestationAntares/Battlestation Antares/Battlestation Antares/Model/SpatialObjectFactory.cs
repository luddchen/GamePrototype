using System;
using System.Collections.Generic;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpatialObjectAttributesLibrary;

namespace Battlestation_Antares.Model {

    class SpatialObjectFactory {

        private static Dictionary<String, Tuple<Microsoft.Xna.Framework.Graphics.Model,BoundingSphere>> modelsAndBoundings;

        private static Dictionary<String, SpatialObjectAttributes> attributes;

        private static Dictionary<String, Texture2D> mapIcons;

        public static Microsoft.Xna.Framework.Graphics.Model GetModel( String modelName ) {
            Tuple<Microsoft.Xna.Framework.Graphics.Model, BoundingSphere> modelAndBounding;
            if ( SpatialObjectFactory.modelsAndBoundings.TryGetValue( modelName, out modelAndBounding ) ) {
                return modelAndBounding.Item1;
            } else {
                SpatialObjectFactory._createModelAndBounding( modelName );
                return SpatialObjectFactory.modelsAndBoundings[modelName].Item1;
            }
        }

        public static BoundingSphere GetBounding( String modelName ) {
            Tuple<Microsoft.Xna.Framework.Graphics.Model, BoundingSphere> modelAndBounding;
            if ( SpatialObjectFactory.modelsAndBoundings.TryGetValue( modelName, out modelAndBounding ) ) {
                return modelAndBounding.Item2;
            } else {
                SpatialObjectFactory._createModelAndBounding( modelName );
                return SpatialObjectFactory.modelsAndBoundings[modelName].Item2;
            }
        }

        public static SpatialObjectAttributes GetAttributes( String modelName ) {
            SpatialObjectAttributes a;
            if ( SpatialObjectFactory.attributes.TryGetValue( modelName, out a ) ) {
                return a;
            } else {
                try {
                    a = Antares.content.Load<SpatialObjectAttributes>( "Objects//" + modelName + "//Attributes//" + modelName );
                } catch ( Exception e ) {
                    a = Antares.content.Load<SpatialObjectAttributes>( "Objects//Template//Attributes//Template");
                }
                SpatialObjectFactory.attributes[modelName] = a;
                return a;
            }
        }

        public static Texture2D GetMapIcon( String modelName ) {
            Texture2D icon;
            if ( SpatialObjectFactory.mapIcons.TryGetValue( modelName, out icon ) ) {
                return icon;
            } else {
                try {
                    icon = Antares.content.Load<Texture2D>( "Objects//" + modelName + "//MiniMap//" + modelName );
                } catch ( Exception e ) {
                    icon = Antares.content.Load<Texture2D>( "Objects//Template//MiniMap//Template" );
                }
                SpatialObjectFactory.mapIcons[modelName] = icon;
                return icon;
            }
        }

        private static void _createModelAndBounding( String modelName ) {
            Microsoft.Xna.Framework.Graphics.Model model;
            try {
                model = Antares.content.Load<Microsoft.Xna.Framework.Graphics.Model>( "Objects//" + modelName + "//Model//" + modelName );
            } catch ( Exception e ) {
                model = Antares.content.Load<Microsoft.Xna.Framework.Graphics.Model>( "Objects//Template//Model//Template" );
            }
            BoundingSphere bounding = new BoundingSphere();
            Matrix[] boneTransforms = new Matrix[model.Bones.Count];
            model.Root.Transform = Matrix.Identity;
            model.CopyAbsoluteBoneTransformsTo( boneTransforms );
            foreach ( ModelMesh mesh in model.Meshes ) {
                bounding = BoundingSphere.CreateMerged( mesh.BoundingSphere.Transform( boneTransforms[mesh.ParentBone.Index] ), bounding );
            }
            SpatialObjectFactory.modelsAndBoundings[modelName] = new Tuple<Microsoft.Xna.Framework.Graphics.Model, BoundingSphere>( model, bounding );
            Console.WriteLine( "created " + modelName + " ( " + bounding + " )" );
        }




        public static SpatialObject buildSpatialObject( Type spatialObjectType ) {
            SpatialObject newObj;

            if ( spatialObjectType.Equals( typeof( Battlestation_Antares.Model.Radar ) ) ) {
                newObj = new Radar( Vector3.Zero );
            } else if ( spatialObjectType.Equals( typeof( Battlestation_Antares.Model.Turret ) ) ) {
                newObj = new Turret( Vector3.Zero );
            } else {
                throw new ArgumentException( "Factory: Unknown Type" );
            }
            return newObj;
        }


        public static void Initialize() {
            SpatialObjectFactory.modelsAndBoundings = new Dictionary<String, Tuple<Microsoft.Xna.Framework.Graphics.Model, BoundingSphere>>();
            SpatialObjectFactory.attributes = new Dictionary<string, SpatialObjectAttributes>();
            SpatialObjectFactory.mapIcons = new Dictionary<string, Texture2D>();
        }
    }

}
