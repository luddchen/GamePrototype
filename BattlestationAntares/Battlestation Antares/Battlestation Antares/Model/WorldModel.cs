using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Battlestation_Antares.View.HUD;
using Battlestation_Antaris.Model;
using Battlestation_Antaris.View.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Battlestation_Antares.Tools;

namespace Battlestation_Antares.Model {

    /// <summary>
    /// represents a model of the game world
    /// </summary>
    class WorldModel {

        /// <summary>
        /// the game
        /// </summary>
        public Antares game;


        /// <summary>
        /// a minimap of this world
        /// </summary>
        private MiniMap miniMap;

        public MiniMapRenderer miniMapRenderer;

        public Grid grid;

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

        private List<SpatialObject> addList;

        private List<SpatialObject> removeList;


        public Tools.DynamicOctree<TactileSpatialObject> octree;

        private Tools.RayCastThreadPool RayCastPool;


        /// <summary>
        /// creates the world
        /// </summary>
        /// <param name="game">the game</param>
        public WorldModel( Antares game ) {
            this.game = game;
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
        }


        /// <summary>
        /// initialize the world content
        /// </summary>
        /// <param name="content">the game content manager</param>
        public void Initialize( ContentManager content ) {

            Add( new Skybox() );

            // background
            for ( int i = 0; i < 10; i++ ) {
                addBackgroundObject( "BGTest//test2" );
            }

            for ( int i = 0; i < 10; i++ ) {
                addBackgroundObject( "BGTest//test" );
            }

            Add( new Grid() );

            Random random = new Random();

            for ( int i = 0; i < 10; i++ ) {
                if ( random.Next( 2 ) == 0 ) {
                    TactileSpatialObject obj =
                        new TactileSpatialObject( "TargetShip//targetship_2", new Vector3( random.Next( 2400 ) - 1200, 0, random.Next( 2400 ) - 1200 ) );
                    obj.miniMapIcon.color = MiniMap.ENEMY_COLOR;
                } else {
                    TactileSpatialObject obj =
                        new TactileSpatialObject( "Cubus//Cubus_0", new Vector3( random.Next( 2400 ) - 1200, 0, random.Next( 2400 ) - 1200 ) );
                    obj.miniMapIcon.color = MiniMap.ENEMY_COLOR;
                }
            }

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

        private void addBackgroundObject( String model ) {
            float yaw = (float)( RandomGen.random.NextDouble() * Math.PI * 2 );
            float pitch = (float)( RandomGen.random.NextDouble() * Math.PI );
            float roll = (float)( RandomGen.random.NextDouble() * Math.PI * 2 );
            Matrix bgRot = Tools.Tools.YawPitchRoll( Matrix.Identity, yaw, pitch, roll );
            float scale = 1.25f + (float)RandomGen.random.NextDouble() * 1.5f;

            Add( new BackgroundObject( model, bgRot, scale) );
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

            // render minimap
            this.miniMapRenderer.Update( gameTime );

            // octree test
            octree.Clear();

            bool shipVisible = this.spaceShip.isVisible;
            this.spaceShip.isVisible = true;

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

    }

}
