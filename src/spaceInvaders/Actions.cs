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
        public ShootProjectile(Pawn sprite, Spawner spawner): base(sprite){
            _spawner = spawner;
        }
        public override void ExecuteAction(){
            _spawner.SpawnSprite(Sprite.Coords[0].X, Sprite.Coords[0].Y - 1);
        }
    }

    class StartGame: GameAction{
        Display _display;
        Input _input;
        public StartGame(Display display, Input input){
            _display = display;
            _input = input;
        }
        public override void ExecuteAction()
        {
            Init.InitGame(_display, _input);
        }
    }
}