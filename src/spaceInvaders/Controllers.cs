using SimpleGameEngine;

namespace spaceInvaders{
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
}