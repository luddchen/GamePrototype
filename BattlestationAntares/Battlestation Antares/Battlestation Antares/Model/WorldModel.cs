using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD;
using HUD.HUD;
using Battlestation_Antaris.View.HUD;
using Battlestation_Antaris.Model;
using System.Collections.ObjectModel;

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

        public ReadOnlyCollection<SpatialObject> AllObjects {
            get {
                return this.allObjects.AsReadOnly();
            }
        }

        public List<TactileSpatialObject> allTactileObjects;


                /// <summary>
        /// a list of all player radars
        /// </summary>
        private List<Radar> allRadars;


        /// <summary>
        /// a list of all player turrets
        /// </summary>
        public List<Turret> allTurrets;

        /// <summary>
        /// a list of all weapons
        /// </summary>
        private List<TactileSpatialObject> allWeapons;

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

            // create 500 random objects to fill the space for testing
            Random random = new Random();

            for ( int i = 0; i < 10; i++ ) {
                if ( random.Next( 2 ) == 0 ) {
                    SpatialObjectOld obj =
                        new SpatialObjectOld( "TargetShip//targetship_2", new Vector3( random.Next( 2400 ) - 1200, 0, random.Next( 2400 ) - 1200 ) );
                    obj.miniMapIcon.color = MiniMap.ENEMY_COLOR;
                } else {
                    SpatialObjectOld obj =
                        new SpatialObjectOld( "Cubus//Cubus_0", new Vector3( random.Next( 2400 ) - 1200, 0, random.Next( 2400 ) - 1200 ) );
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
                Dust dust = new Dust( spaceShip );
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
                    SpatialObjectOld parent = ( (Laser)o ).parent;

                    float size = ( Math.Abs( o.scale.Z ) + o.attributes.Engine.CurrentVelocity / 2.0f );
                    Vector3 minPos = o.globalPosition - size * o.rotation.Forward;
                    float dist = size * 2.0f;

                    SpatialObject hit = octree.CastRay( new Ray( minPos, o.rotation.Forward ), 0, ref dist );

                    if ( hit is SpatialObjectOld && hit != null && hit != parent ) {
                        (hit as SpatialObjectOld).OnHit( parent.attributes.Laser.Damage );
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
