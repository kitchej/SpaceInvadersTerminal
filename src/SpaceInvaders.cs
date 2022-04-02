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
    static class Program{
        static void Main(){
            Display screen = new Display(25, 60);
            Sprite northBounds = new Sprite(@"src\sprites\horizBorder.txt", 1, 0, 0, "northBounds");
            Sprite southBounds = new Sprite(@"src\sprites\horizBorder.txt", 1, 24, 0, "southBounds");
            Sprite westBounds = new Sprite(@"src\sprites\vertiBorder.txt", 0, 1, 0, "westBounds");
            Sprite eastBounds = new Sprite(@"src\sprites\vertiBorder.txt", 59, 1, 0, "eastBounds");
            Pawn ship = new Pawn(@"src\sprites\ship.txt", 10, 10, 0, "ship");
            screen.AddSprite(ship);
            screen.AddSprite(northBounds);
            screen.AddSprite(southBounds);
            screen.AddSprite(westBounds);
            screen.AddSprite(eastBounds);
            Input input = new Input(exitKey: ConsoleKey.Q);
            input.BindAction(ConsoleKey.A, new MoveWest(ship));
            input.BindAction(ConsoleKey.D, new MoveEast(ship));
            input.BindAction(ConsoleKey.W, new MoveNorth(ship));
            input.BindAction(ConsoleKey.S, new MoveSouth(ship));
            Mainloop.mainloop(screen, input);
        }
    }
}
