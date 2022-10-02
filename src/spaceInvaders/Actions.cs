using SimpleGameEngine;

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
        int _limit;

        Thread? _thread;
        public ShootProjectile(Pawn sprite, Spawner spawner, int limit): base(sprite){
            _spawner = spawner;
            _limit = limit;
            _thread = null;
        }
        public override void ExecuteAction(){
            if (_thread == null || !_thread.IsAlive){
                _thread = _spawner.SpawnSprite(Sprite.Coords[0].X, Sprite.Coords[0].Y - 1);
            }
            
        }
    }

    class Pause: GameAction{
        CentralController _controller;
        Input _input;

        bool _paused;

        Logger logger;

        public Pause(CentralController controller, Input input){
            _controller = controller;
            _input = input;
            _paused = false;
            logger = new Logger("Pause.log");
        }

        public override void ExecuteAction(){
            if (_paused){
                _controller.ResumeAll();
                _paused = false;
            }
            else{
                _controller.PauseAll();
                _paused = true;
            }
            
        }
    }

    class EndGame: GameAction{
        CentralController con;
        public EndGame(CentralController controller){
            con = controller;
        }
        public override void ExecuteAction(){
            con.EndGame();
        }
    }

    class Reset: GameAction{
        CentralController _con;
        int _lives;
        Stack<Sprite> _livesSprites;
        Pawn _ship;
        Input _input;

        public Reset(CentralController controller, Stack<Sprite> livesSprites, Pawn ship, Input input){
            _con = controller;
            _lives = livesSprites.Count;
            _livesSprites = livesSprites;
            _ship = ship;
            _input = input;
        }

        public override void ExecuteAction(){
            if (_lives == 0){
                _input.UnbindAction(ConsoleKey.A);
                _input.UnbindAction(ConsoleKey.D);
                _input.UnbindAction(ConsoleKey.Spacebar);
                _con.PauseAll();
                _con.DeleteAllSprites();
                _con.AddSprite(new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "gameOver.txt"}), 5, 5, "GameOver"));
            }
            else{
                
                // Pause game
                _con.PauseAll();
                KeyValuePair<ConsoleKey, GameAction> moveLeft;
                KeyValuePair<ConsoleKey, GameAction> moveRight;
                KeyValuePair<ConsoleKey, GameAction> shoot;
                // Unbind movement and shooting
                moveLeft = _input.UnbindAction(ConsoleKey.A);
                moveRight = _input.UnbindAction(ConsoleKey.D);
                shoot = _input.UnbindAction(ConsoleKey.Spacebar);
                _con.DeleteSprite(_ship);
                List<Sprite> toRemove = new List<Sprite>();
                // Remove shot projectiles
                foreach(var sprite in _con.Sprites){
                    if (sprite.SpriteId.Contains("projectile") || sprite.SpriteId.Contains("enemyProjectile")){
                        toRemove.Add(sprite);
                    }
                }
                foreach(var sprite in toRemove){
                    _con.DeleteSprite(sprite);
                }
                // Delete one life
                Thread.Sleep(1000);
                _con.DeleteSprite(_livesSprites.Pop());
                _lives --;

                // Resume game
                _con.AddSprite(_ship);
                _input.BindAction(moveLeft);
                _input.BindAction(moveRight);
                _input.BindAction(shoot);
                _con.ResumeAll();
            }
        } 
    }
}