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
        string _id;

        bool _paused;

        Logger logger;

        public Pause(CentralController controller, string id){
            _controller = controller;
            _id = id;
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
}