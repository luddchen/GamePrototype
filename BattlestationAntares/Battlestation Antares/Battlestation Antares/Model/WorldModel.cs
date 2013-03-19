using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.View.HUD;
using Battlestation_Antares.View;

namespace Battlestation_Antares.Model {

    /// <summary>
    /// represents a model of the game world
    /// </summary>
    public class WorldModel {

        /// <summary>
        /// the game
        /// </summary>
        public Antares game;


        /// <summary>
        /// a minimap of this world
        /// </summary>
        public MiniMap miniMap;


        /// <summary>
        /// the players space ship
        /// </summary>
        public SpaceShip spaceShip;


        /// <summary>
        /// the players space station
        /// </summary>
        public SpaceStation spaceStation;


        /// <summary>
        /// a list of all player radars
        /// </summary>
        private List<SpatialObject> allRadars;


        /// <summary>
        /// a list of all player turrets
        /// </summary>
        public List<SpatialObject> allTurrets;


        /// <summary>
        /// a list of all spatial objects
        /// </summary>
        public List<SpatialObject> allObjects;


        /// <summary>
        /// a list of all Laser beams
        /// </summary>
        private List<SpatialObject> allWeapons;

        /// <summary>
        /// a list of all space dust
        /// </summary>
        private List<SpatialObject> allDust;


        public List<List<SpatialObject>> allDrawable;

        public List<List<SpatialObject>> allUpdatable;


        private List<SpatialObject> removeList;


        public Tools.DynamicOctree<SpatialObject> treeTest;

        private Tools.RayCastThreadPool RayCastPool;


        /// <summary>
        /// creates the world
        /// </summary>
        /// <param name="game">the game</param>
        public WorldModel( Antares game ) {
            this.game = game;
            this.allObjects = new List<SpatialObject>();
            this.allRadars = new List<SpatialObject>();
            this.allTurrets = new List<SpatialObject>();
            this.allDust = new List<SpatialObject>();
            this.allWeapons = new List<SpatialObject>();

            this.allDrawable = new List<List<SpatialObject>>();
            this.allDrawable.Add( this.allObjects );
            this.allDrawable.Add( this.allRadars );
            this.allDrawable.Add( this.allTurrets );
            this.allDrawable.Add( this.allDust );
            this.allDrawable.Add( this.allWeapons );

            this.allUpdatable = new List<List<SpatialObject>>();
            this.allUpdatable.Add( this.allObjects );
            this.allUpdatable.Add( this.allRadars );
            this.allUpdatable.Add( this.allTurrets );
            this.allUpdatable.Add( this.allDust );
            this.allUpdatable.Add( this.allWeapons );

            this.removeList = new List<SpatialObject>();

            this.miniMap = new MiniMap( Vector2.Zero, View.HUD.HUDType.RELATIV);

            // octree test
            this.treeTest = new Tools.DynamicOctree<SpatialObject>( 3, 1, 10, new BoundingBox( new Vector3( -5000, -5000, -5000 ), new Vector3( 5000, 5000, 5000 ) ) );

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
                    SpatialObject obj =
                        new SpatialObject( new Vector3( random.Next( 2400 ) - 1200, 0, random.Next( 2400 ) - 1200 ), "Models//TargetShip//targetship_2", content, this );
                    obj.isEnemy = true;
                    obj.miniMapIcon.color = MiniMap.ENEMY_COLOR;
                } else {
                    SpatialObject obj =
                        new SpatialObject( new Vector3( random.Next( 2400 ) - 1200, 0, random.Next( 2400 ) - 1200 ), "Models//Cubus//Cubus_0", content, this );
                    obj.isEnemy = true;
                    obj.miniMapIcon.color = MiniMap.ENEMY_COLOR;
                }
            }

            for ( int i = 0; i < 1; i++ ) {
                Turret turret = new Turret( new Vector3( random.Next( 2400 ) - 1200, 0, random.Next( 2400 ) - 1200 ), content, this );
            }

            for ( int i = 0; i < 6; i++ ) {
                Radar radar = new Radar( new Vector3( random.Next( 2400 ) - 1200, 0, random.Next( 2400 ) - 1200 ), content, this );
            }

            // create the player space ship
            this.spaceShip = new SpaceShip( new Vector3( 0, 30, 500 ), "Models//SpaceShip//spaceship1_2", content, this );
            this.spaceShip.isVisible = false;

            // create the player space station
            this.spaceStation = new SpaceStation( Vector3.Zero, "Models//SpaceStation/spacestation", content, this );

            // add dust near the players ship
            for ( int i = 0; i < 200; i++ ) {
                Dust dust = new Dust( spaceShip, content, this );
            }

        }


        /// <summary>
        /// update the world
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update( Microsoft.Xna.Framework.GameTime gameTime ) {
            // update all spatial objects
            foreach ( List<SpatialObject> list in this.allUpdatable ) {
                foreach ( SpatialObject obj in list ) {
                    obj.Update( gameTime );
                }
            }


            // octree test
            treeTest.Clear();

            bool shipVisible = this.spaceShip.isVisible;
            this.spaceShip.isVisible = true;

            foreach ( SpatialObject obj in this.allObjects ) {
                if ( obj.isVisible ) {
                    BoundingSphere itemSphere = new BoundingSphere( obj.bounding.Center + obj.globalPosition, obj.bounding.Radius );
                    if ( !treeTest.Add( obj, itemSphere ) ) {
                        treeTest.AddItem( obj, itemSphere );
                    }
                }
            }

            this.spaceShip.isVisible = shipVisible;

            this.treeTest.BuildTree();

            foreach ( SpatialObject o in this.allWeapons ) {
                if ( o is Laser ) {
                    float dist = float.MaxValue;
                    SpatialObject hit = treeTest.CastRay( new Ray( o.globalPosition - Math.Abs( o.scale.Z ) * o.rotation.Forward, o.rotation.Forward ), 0, ref dist );
                    if ( hit != null && dist < Math.Abs( o.scale.Z * 2 ) ) {
                        hit.onHit( o );
                        o.onHit( hit );
                        Console.Out.WriteLine( o + " -> " + hit );
                    }
                }
            }

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


        /// <summary>
        /// add a spatial object to this world
        /// </summary>
        /// <param name="obj">a spatial object</param>
        public void addObject( SpatialObject obj ) {
            if ( ( obj is Laser ) || ( obj is Missile ) ) {
                this.allWeapons.Add( obj );
            } else if ( obj is Dust ) {
                this.allDust.Add( obj );
            } else if ( obj is Radar ) {
                this.allRadars.Add( obj );
            } else if ( obj is Turret ) {
                this.allTurrets.Add( obj );
            } else {
                this.allObjects.Add( obj );
            }
        }


        private void RemoveNow() {
            foreach ( SpatialObject obj in this.removeList ) {
                if ( ( obj is Laser ) || ( obj is Missile ) ) {
                    this.allWeapons.Remove( obj );
                } else if ( obj is Dust ) {
                    this.allDust.Remove( obj );
                } else if ( obj is Radar ) {
                    this.allRadars.Remove( obj );
                } else if ( obj is Turret ) {
                    this.allTurrets.Remove( obj );
                } else {
                    this.allObjects.Remove( obj );
                }

                if ( obj.miniMapIcon != null ) {
                    obj.miniMapIcon.RemoveFromWorld();
                    obj.miniMapIcon = null;
                }
            }
            this.removeList.Clear();
        }


        /// <summary>
        /// trigger object remove
        /// </summary>
        /// <param name="laser">laser to remove</param>
        public void Remove( SpatialObject obj ) {
            this.removeList.Add( obj );
        }

    }

}
