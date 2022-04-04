using SimpleGameEngine;
using System.Reflection;

namespace spaceInvaders{
    
    class MoveWest: SpriteAction{
        public MoveWest(Pawn sprite): base(sprite){}

        public override void ExecuteAction(){
            Sprite.MoveWest(1);
        }
    }

    class MoveEast: SpriteAction{
        public MoveEast(Pawn sprite): base(sprite){}

        public override void ExecuteAction(){
            Sprite.MoveEast(1);
        }
    }

    class MoveNorth: SpriteAction{
        public MoveNorth(Pawn sprite): base(sprite){}

        public override void ExecuteAction(){
            Sprite.MoveNorth(1);
        }
    }

    class MoveSouth: SpriteAction{
        public MoveSouth(Pawn sprite): base(sprite){}

        public override void ExecuteAction(){
            Sprite.MoveSouth(1);
        }
    }

    class ShootProjectile: SpriteAction{
        Spawner _spawner;
        public ShootProjectile(Pawn sprite, Spawner spawner): base(sprite){
            _spawner = spawner;
        }
        public override void ExecuteAction(){
            _spawner.SpwawnSprite(Sprite.Coords[0].X, Sprite.Coords[0].Y - 1);
        }
    }

    class EnemyContoller: SpriteController{
        string _direction = "left";
        CollisionInfo collisionInfo;
        Spawner _projectileSpawner;
        Random rnd = new Random();
        int _count;
        public EnemyContoller(Pawn sprite, Display display, int speed, Spawner projectileSpawner): base(sprite, display, speed){
            _projectileSpawner = projectileSpawner;
            _count = 0;
        }

        protected override void Behavior(){
            if (rnd.Next(0, 10) == rnd.Next(0, 10)){
                _projectileSpawner.SpwawnSprite(_sprite.Coords[6].X, _sprite.Coords[6].Y + 1);
            }
            if (_direction == "left"){
                collisionInfo = _sprite.MoveWest(1);
                _count ++;
            }
            else if (_direction == "right"){
                collisionInfo = _sprite.MoveEast(1);
                _count ++;
            }
            if (_count == 8){
                    if(_direction == "right"){
                        _direction = "left";
                        _count = 0;
                    }
                    else if(_direction == "left"){
                        _direction = "right";
                        _count = 0;
                    }
                }
        }
    }

    class ProjectileContoller: SpriteController{
        CollisionInfo collisionInfo;
        public ProjectileContoller(Pawn? sprite, Display display, int speed): base(sprite, display, speed){}

        protected override void Behavior(){
           collisionInfo = _sprite.MoveNorth(1);
        }

        protected override bool CheckDespawnConditions()
        {
            if (collisionInfo.CollisionOccured){
                return true;
            }
            return false;
        }

    }

    class EnemyProjectileContoller: SpriteController{
        CollisionInfo collisionInfo;
        public EnemyProjectileContoller(Pawn? sprite, Display display, int speed): base(sprite, display, speed){}

        protected override void Behavior(){
           collisionInfo = _sprite.MoveSouth(1);
        }

        protected override bool CheckDespawnConditions()
        {
            if (collisionInfo.CollisionOccured){
                return true;
            }
            return false;
        }

    }
    static class Program{

        static void Main(){
         Display screen = new Display(25, 60);
            Sprite northBounds = new Sprite(@"src\sprites\horizBorder.txt", 1, 0, "northBounds");
            Sprite southBounds = new Sprite(@"src\sprites\horizBorder.txt", 1, 24, "southBounds");
            Sprite westBounds = new Sprite(@"src\sprites\vertiBorder.txt", 0, 1, "westBounds");
            Sprite eastBounds = new Sprite(@"src\sprites\vertiBorder.txt", 59, 1, "eastBounds");
            Pawn ship = new Pawn(@"src\sprites\ship.txt", startx: 30, starty: 20, spriteId: "ship", display: screen, stackOrder: 0);
            Pawn enemyShip = new Pawn(@"src\sprites\alien1.txt", startx: 30, starty: 12, spriteId: "enemyShip", display: screen);
            Spawner shipProjectileSpawner = new Spawner(typeof(Pawn), new object[] { @"src\sprites\projectile.txt", 30, 21, "projectile", screen, 0, true}, 
                                          typeof(ProjectileContoller), new object?[] {null, screen, 50}, 
                                          screen);
            Spawner enemyProjectileSpawner = new Spawner(typeof(Pawn), new object[] { @"src\sprites\projectile.txt", 30, 21, "enemyProjectile", screen, 0, true}, 
                                          typeof(EnemyProjectileContoller), new object?[] {null, screen, 50}, 
                                          screen);
            EnemyContoller enemyAi = new EnemyContoller(enemyShip, screen, 500, enemyProjectileSpawner);
            
            screen.AddSprite(ship);
            screen.AddSprite(northBounds);
            screen.AddSprite(southBounds);
            screen.AddSprite(westBounds);
            screen.AddSprite(eastBounds);
            screen.AddSprite(enemyShip);
            Input input = new Input(exitKey: ConsoleKey.Q);
            input.BindAction(ConsoleKey.A, new MoveWest(ship));
            input.BindAction(ConsoleKey.D, new MoveEast(ship));
            input.BindAction(ConsoleKey.W, new MoveNorth(ship));
            input.BindAction(ConsoleKey.S, new MoveSouth(ship));
            input.BindAction(ConsoleKey.Spacebar, new ShootProjectile(ship, shipProjectileSpawner));
            
            Mainloop.mainloop(screen, input, new SpriteController[] {enemyAi});
        }
    }
}
