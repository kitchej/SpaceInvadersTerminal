using SimpleGameEngine;

namespace spaceInvaders{
    class EnemyController: SpriteController{
        string _direction;
        CollisionInfo _collisionInfo;
        Spawner _projectileSpawner;
        Random rnd = new Random();
        int _count;
        Logger _logger;
        public EnemyController(Pawn sprite, Display display, int speed, Spawner projectileSpawner): base(sprite, display, speed){
            _projectileSpawner = projectileSpawner;
            _count = 0;
            _direction = "left";
            _logger = new Logger("EnemyController.log");
        }

        protected override bool CheckDespawnConditions()
        {
            if (_collisionInfo.CollisionOccurred){
                if (_collisionInfo.Entity.SpriteId == "projectile"){
                    _logger.Log($"{_sprite.SpriteId} detected collision with {_collisionInfo.Entity.SpriteId}");
                    return true;
                }
            }

            if(_sprite.LastCollided.CollisionOccurred){
                if (_sprite.LastCollided.Entity.SpriteId == "projectile"){
                    _logger.Log($"{_sprite.SpriteId} was informed of collision with {_sprite.LastCollided.Entity.SpriteId}");
                    return true;
                }
            }
            return false;
        }

        protected override void Behavior(){
            if (rnd.Next(0, 10) == rnd.Next(0, 10)){
                _projectileSpawner.SpawnSprite(_sprite.Coords[6].X, _sprite.Coords[6].Y + 1);
            }
            if (_direction == "left"){
                _collisionInfo = _sprite.MoveWest(1);
                _count ++;
            }
            else if (_direction == "right"){
                _collisionInfo = _sprite.MoveEast(1);
                _count ++;
            }
            if (_count == 1){
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

    class ProjectileController: SpriteController{
        CollisionInfo collisionInfo;
        public ProjectileController(Pawn? sprite, Display display, int speed): base(sprite, display, speed){}

        protected override void Behavior(){
           collisionInfo = _sprite.MoveNorth(1);
        }

        protected override bool CheckDespawnConditions()
        {
            if (collisionInfo.CollisionOccurred){
                collisionInfo.Entity.CollisionInfo = new CollisionInfo(true, _sprite);
                return true;
            }
            return false;
        }

    }

    class EnemyProjectileController: SpriteController{
        CollisionInfo collisionInfo;
        public EnemyProjectileController(Pawn? sprite, Display display, int speed): base(sprite, display, speed){}

        protected override void Behavior(){
           collisionInfo = _sprite.MoveSouth(1);
        }

        protected override bool CheckDespawnConditions()
        {
            if (collisionInfo.CollisionOccurred){
                return true;
            }
            return false;
        }

    }
}