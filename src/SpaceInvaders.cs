using SimpleGameEngine;
using System.Reflection;

namespace spaceInvaders{
    
    class MoveWest: SpriteAction{
        public MoveWest(Pawn sprite): base(sprite){}

        public override void ExecuteAction(){
            _sprite.MoveWest(1);
        }
    }

    class MoveEast: SpriteAction{
        public MoveEast(Pawn sprite): base(sprite){}

        public override void ExecuteAction(){
            _sprite.MoveEast(1);
        }
    }

    class MoveNorth: SpriteAction{
        public MoveNorth(Pawn sprite): base(sprite){}

        public override void ExecuteAction(){
            _sprite.MoveNorth(1);
        }
    }

    class MoveSouth: SpriteAction{
        public MoveSouth(Pawn sprite): base(sprite){}

        public override void ExecuteAction(){
            _sprite.MoveSouth(1);
        }
    }

    class ShootProjectile: SpriteAction{
        Spawner _spawner;
        public ShootProjectile(Pawn sprite, Spawner spawner): base(sprite){
            _spawner = spawner;
        }
        public override void ExecuteAction(){
            _spawner.SpwawnSprite(_sprite.Coords[0].X, _sprite.Coords[0].Y - 1);
        }
    }

    class EnemyContoller: SpriteController{
        public EnemyContoller(Pawn sprite, Display display, int speed): base(sprite, display, speed){}

        protected override void Behavior(){
            _sprite.MoveNorth(1);
            Thread.Sleep(_speed);
            _sprite.MoveEast(1);
            Thread.Sleep(_speed);
            _sprite.MoveSouth(1);
            Thread.Sleep(_speed);
            _sprite.MoveWest(1);
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
    static class Program{

        static void Main(){
         Display screen = new Display(25, 60);
            Sprite northBounds = new Sprite(@"src\sprites\horizBorder.txt", 1, 0, "northBounds");
            Sprite southBounds = new Sprite(@"src\sprites\horizBorder.txt", 1, 24, "southBounds");
            Sprite westBounds = new Sprite(@"src\sprites\vertiBorder.txt", 0, 1, "westBounds");
            Sprite eastBounds = new Sprite(@"src\sprites\vertiBorder.txt", 59, 1, "eastBounds");
            Pawn ship = new Pawn(@"src\sprites\ship.txt", startx: 30, starty: 20, spriteId: "ship", display: screen);
            Pawn enemyShip = new Pawn(@"src\sprites\alien1.txt", startx: 30, starty: 12, spriteId: "enemyShip", display: screen);
            EnemyContoller enemyAi = new EnemyContoller(enemyShip, screen, 500);
            Spawner spawner = new Spawner(typeof(Pawn), new object[] { @"src\sprites\projectile.txt", 30, 21, "projectile", screen, 0, true}, 
                                          typeof(ProjectileContoller), new object?[] {null, screen, 50}, 
                                          screen);
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
            input.BindAction(ConsoleKey.Spacebar, new ShootProjectile(ship, spawner));
            
            Mainloop.mainloop(screen, input, new SpriteController[] {enemyAi});
        }
    }
}
