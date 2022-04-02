using SimpleGameEngine;

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
            
            Mainloop.mainloop(screen, input, new SpriteController[] {enemyAi});
        }
    }
}
