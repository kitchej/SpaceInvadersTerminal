using SimpleGameEngine;

namespace spaceInvaders{
    class MasterEnemyController: SpriteController{
        Pawn[] pawns;
        string _direction;
        CollisionInfo _collisionInfo;
        Spawner _projectileSpawner;
        Random rnd = new Random();
        int _count;
        int _passes;
        Logger _logger;

        public EnemyController(Pawn? sprite, Display display, int speed, int row, Spawner projectileSpawner): base(sprite, display, speed){
            _display = display;
            _speed = speed;
            _sprite = null;
            _projectileSpawner = projectileSpawner;
            _direction = "left";

        }
    }

}