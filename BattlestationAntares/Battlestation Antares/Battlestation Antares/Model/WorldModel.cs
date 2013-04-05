using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Battlestation_Antares.View;
using Battlestation_Antares.View.HUD;
using Battlestation_Antaris.Model;
using Battlestation_Antaris.View.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Battlestation_Antaris.Control.AI;

namespace Battlestation_Antares.Model {

    /// <summary>
    /// represents a model of the game world
    /// </summary>
    class WorldModel {

        /// <summary>
        /// a minimap of this world
        /// </summary>
        private MiniMap miniMap;

        public MiniMapRenderer miniMapRenderer;

        /// <summary>
        /// the players space ship
        /// </summary>
        public SpaceShip spaceShip;


        /// <summary>
        /// the players space station
        /// </summary>
        public SpaceStation spaceStation;


        /// <summary>
        /// a list of all spatial objects
        /// </summary>
        private List<SpatialObject> allObjects;

        /// <summary>
        /// a list of all objects
        /// </summary>
        public ReadOnlyCollection<SpatialObject> AllObjects {
            get {
                return this.allObjects.AsReadOnly();
            }
        }
        
        private List<TactileSpatialObject> allTactileObjects;

        /// <summary>
        /// a list of all tactile objects
        /// </summary>
        public ReadOnlyCollection<TactileSpatialObject> AllTactileObjects {
            get {
                return this.allTactileObjects.AsReadOnly();
            }
        }

        private List<Radar> allRadars;

        /// <summary>
        /// a list of all radars
        /// </summary>
        public ReadOnlyCollection<Radar> AllRadars {
            get {
                return this.allRadars.AsReadOnly();
            }
        }

        private List<Turret> allTurrets;

        /// <summary>
        /// a list of all turrets
        /// </summary>
        public ReadOnlyCollection<Turret> AllTurrets {
            get {
                return this.allTurrets.AsReadOnly();
            }
        }

        private List<TactileSpatialObject> allWeapons;

        /// <summary>
        /// a list of all weapons
        /// </summary>
        public ReadOnlyCollection<TactileSpatialObject> AllWeapons {
            get {
                return this.allWeapons.AsReadOnly();
            }
        }

        private Grid grid;

        private SkySphere skybox;

        private List<SpatialObject> addList;

        private List<SpatialObject> removeList;


        public Tools.DynamicOctree<TactileSpatialObject> octree;

        private Tools.RayCastThreadPool RayCastPool;

        private DrawTree drawTree;

        private EnemyAI enemyAI;

        /// <summary>
        /// creates the world
        /// </summary>
        /// <param name="game">the game</param>
        public WorldModel() {
            this.allObjects = new List<SpatialObject>();
            this.allTactileObjects = new List<TactileSpatialObject>();

            this.allRadars = new List<Radar>();
            this.allTurrets = new List<Turret>();
            this.allWeapons = new List<TactileSpatialObject>();

            this.addList = new List<SpatialObject>();
            this.removeList = new List<SpatialObject>();

            this.miniMap = new MiniMap();
            this.miniMap.IsVisible = true;
            this.miniMapRenderer = new MiniMapRenderer( this.miniMap );
            this.miniMap.renderer = this.miniMapRenderer;

            // octree test
            this.octree = 
                new Tools.DynamicOctree<TactileSpatialObject>
                    ( 3, 1, 10, new BoundingBox( new Vector3( -5000, -5000, -5000 ), new Vector3( 5000, 5000, 5000 ) ) );

            this.RayCastPool = new Tools.RayCastThreadPool( this );

            this.drawTree = new DrawTree();
        }


        /// <summary>
        /// initialize the world content
        /// </summary>
        public void Initialize() {

            this.skybox = new SkySphere();
            this.grid = new Grid();

            Add( new BackgroundObject( "Planet", Tools.Tools.YawPitchRoll( Matrix.Identity, 0.3f + (float)Math.PI, 0.2f, 0.01f ), 2.2f, Matrix.CreateRotationX( 0.0002f ) ) );
            BackgroundObject rift = new BackgroundObject( "SpatialRift", Tools.Tools.YawPitchRoll( Matrix.Identity, -0.3f + (float)Math.PI, 0.02f, 0.4f ), 1.2f, null );
            Add( rift );
            this.enemyAI = new EnemyAI( rift );

            Random random = new Random();

            for ( int i = 0; i < 1; i++ ) {
                Turret turret = new Turret( new Vector3( random.Next( 2400 ) - 1200, 0, random.Next( 2400 ) - 1200 ) );
            }

            for ( int i = 0; i < 6; i++ ) {
                Radar radar = new Radar( new Vector3( random.Next( 2400 ) - 1200, 0, random.Next( 2400 ) - 1200 ) );
            }

            // create the player space ship
            this.spaceShip = new SpaceShip( new Vector3( 0, 30, 500 ) );
            this.spaceShip.isVisible = false;

            // create the player space station
            this.spaceStation = new SpaceStation( new Vector3() );

            // add dust near the players ship
            for ( int i = 0; i < 200; i++ ) {
                Add( new Dust( spaceShip ) );
            }

        }

        /// <summary>
        /// trigger object add
        /// </summary>
        /// <param name="obj">object to add</param>
        public void Add( SpatialObject obj ) {
            this.addList.Add( obj );
        }

        /// <summary>
        /// trigger object remove
        /// </summary>
        /// <param name="laser">object to remove</param>
        public void Remove( SpatialObject obj ) {
            this.removeList.Add( obj );
        }


        private void _addNow() {
            foreach ( SpatialObject obj in this.addList ) {
                this.allObjects.Add( obj );

                if ( obj is TactileSpatialObject ) {
                    this.allTactileObjects.Add( obj as TactileSpatialObject );

                    if ( ( obj is Laser ) || ( obj is Missile ) ) {
                        this.allWeapons.Add( obj as TactileSpatialObject );
                    } else if ( obj is Radar ) {
                        this.allRadars.Add( obj as Radar );
                    } else if ( obj is Turret ) {
                        this.allTurrets.Add( obj as Turret );
                    }
                }

                if ( obj is Grid ) {
                    this.grid = obj as Grid;
                }
            }

            this.addList.Clear();
        }

        private void _removeNow() {
            foreach ( SpatialObject obj in this.removeList ) {
                this.allObjects.Remove( obj );

                if (obj is TactileSpatialObject) {
                    this.allTactileObjects.Remove( obj as TactileSpatialObject );

                    if ( ( obj is Laser ) || ( obj is Missile ) ) {
                        this.allWeapons.Remove( obj as TactileSpatialObject );
                    } else if ( obj is Radar ) {
                        this.allRadars.Remove( obj as Radar );
                    } else if ( obj is Turret ) {
                        this.allTurrets.Remove( obj as Turret );
                    }

                    if ( (obj as TactileSpatialObject).miniMapIcon != null ) {
                        (obj as TactileSpatialObject).miniMapIcon.RemoveFromWorld();
                        (obj as TactileSpatialObject).miniMapIcon = null;
                    }
                }
            }
            this.removeList.Clear();
        }


        /// <summary>
        /// update the world
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update( Microsoft.Xna.Framework.GameTime gameTime ) {

            // remove all deleted objects
            _removeNow();

            // add all recievied objects
            _addNow();

            // update all spatial objects
            foreach ( SpatialObject obj in this.allObjects ) {
                obj.Update( gameTime );
            }
            this.skybox.Update( gameTime );

            this.enemyAI.Update( gameTime );

            // render minimap
            this.miniMapRenderer.Update( gameTime );

            // octree test
            octree.Clear();

            bool shipVisible = this.spaceShip.isVisible;
            this.spaceShip.isVisible = true;    // ship only visible for hit and collision check

            foreach ( TactileSpatialObject obj in this.allTactileObjects ) {
                if ( obj.isVisible ) {
                    BoundingSphere itemSphere = new BoundingSphere( obj.bounding.Center + obj.globalPosition, obj.bounding.Radius );
                    if ( !octree.Add( obj, itemSphere ) ) {
                        octree.AddItem( obj, itemSphere );
                    }
                }
            }

            this.spaceShip.isVisible = shipVisible;

            this.octree.BuildTree();

            foreach ( TactileSpatialObject o in this.allWeapons ) {
                if ( o is Laser ) {
                    TactileSpatialObject parent = ( (Laser)o ).parent;

                    float size = ( Math.Abs( o.scale.Z ) + o.attributes.Engine.CurrentVelocity / 2.0f );
                    Vector3 minPos = o.globalPosition - size * o.rotation.Forward;
                    float dist = size * 2.0f;

                    SpatialObject hit = octree.CastRay( new Ray( minPos, o.rotation.Forward ), 0, ref dist );

                    if ( hit is TactileSpatialObject && hit != null && hit != parent ) {
                        (hit as TactileSpatialObject).OnHit( parent.attributes.Laser.Damage );
                        Remove( o );
                    }
                } else {
                    // Missile
                }
            }
            //Console.WriteLine();

            //BoundingSphere shipSphere = this.spaceShip.bounding;
            //shipSphere.Center += this.spaceShip.globalPosition;

            //List<Tuple<SpatialObject, SpatialObject>> collList =
            //    this.treeTest.CheckCollisions(
            //        new Tuple<SpatialObject, BoundingSphere>( this.spaceShip, shipSphere ) );

            //Console.Out.WriteLine("Collisions : " + collList.Count);

            //if ( collList.Count > 0 ) {
            //    this.game.activeSituation.view.backgroundColor = Color.Red;
            //} else {
            //    this.game.activeSituation.view.backgroundColor = Color.White;
            //}

            // test for threaded raycasting of all turrets
            //this.RayCastPool.StartRayCasting();

        }

        public void Draw( Camera camera ) {
            this.drawTree.init( this.allObjects, camera );
            this.skybox.Draw( camera );
            this.drawTree.Draw( camera );
            this.grid.Draw( camera );
        }

        public void SetGridVisible( bool visibility ) {
            this.grid.isVisible = visibility;
        }

    }

}
