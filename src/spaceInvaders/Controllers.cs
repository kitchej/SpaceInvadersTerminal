using SimpleGameEngine;

namespace spaceInvaders{
    class MasterEnemyController: Controller{
        string _direction;
        CollisionInfo _collisionInfo;
        Spawner _projectileSpawner;
        Random rnd = new Random();
        int _count;
        int _passes;
        Logger _logger;
        Pawn[,] _sprites;
        Pawn?[] _shooters;

        public MasterEnemyController(CentralController centralController, int speed, string controllerId, Spawner projectileSpawner): base(centralController, speed, controllerId){
            _projectileSpawner = projectileSpawner;
            _direction = "left";
            _count = 0;
            _passes = 0;
            _logger = new Logger("EnemyGroupLog.log");
            Pawn s;
            int x = 13;
            int startx = x;
            int starty = 2;
            string[] shipTypes = {"alien1.txt", "alien2.txt", "alien3.txt"};
            _shooters = new Pawn[8];
            _sprites = new Pawn[3,8];
            for (int i=0;i<3;i++){
                for(int j=0;j<8;j++){
                    s = new Pawn(Path.Join(new[] {"src", "spaceInvaders", "sprites", shipTypes[i]}), startx: startx, starty: starty, spriteId: "EnemyShip_" + shipTypes[i], centralController: _controller);
                    _controller.AddSprite(s);
                    _sprites[i,j] = s;
                    startx += 5;
                    if (i==2){
                        _shooters[j] = s;
                    }
                }
                startx = x;
                starty += 5;
            }
        }

        void MoveAllWest(int distance){
            for(int i=0;i<3;i++){
                for(int j=0;j<8;j++){
                   _collisionInfo = _sprites[i,j].MoveWest(distance);
                }
            }
        }

        void MoveAllEast(int distance){
            for(int i=0;i<3;i++){
                for(int j=7;j>=0;j--){
                   _collisionInfo = _sprites[i,j].MoveEast(distance);
                }
            }
        }

        void MoveAllSouth(int distance){
            for(int i=2;i>=0;i--){
                for(int j=0;j<8;j++){
                   _collisionInfo = _sprites[i,j].MoveSouth(distance);
                }
            }
        }

        Pawn? CheckDespawn(){
            foreach(Pawn sprite in _sprites){
                if (sprite.IsDespawned){
                    continue;
                }
                if (sprite.LastCollided.CollisionOccurred){
                    if(sprite.LastCollided.Entity.SpriteId == "projectile"){
                        return sprite;
                    }
                }
            }
            return null;
        }

        protected override void Behavior(){
            Pawn shooter;
            List<Pawn> shooterPool = new List<Pawn>();
            Thread thread;
            
            if (_direction == "left"){
                MoveAllWest(1);
                _count ++;
            }
            else if (_direction == "right"){
                MoveAllEast(1);
                _count ++;
            }
            if (_count == 6){
                if(_direction == "right"){
                    _direction = "left";
                    _count = 0;
                    _passes ++;
                }
                else if(_direction == "left"){
                    _direction = "right";
                    _count = 0;
                    _passes ++;
                }
            }
            if (_passes == 3){
                MoveAllSouth(1);
                _passes = 0;
            }

            if (3 == 3){
                foreach(Pawn? s in _shooters){
                    if (s != null){
                        shooterPool.Add(s);
                    }
                }
                shooter = shooterPool.ElementAt(rnd.Next(0, 7));
                thread = _projectileSpawner.SpawnSprite(shooter.Coords[6].X, shooter.Coords[6].Y + 1);
            }

        }

        void ChooseNewShooter(Pawn despawn){
            for(int i=0;i<8;i++){
                if(_shooters[i] == despawn){
                    if(_sprites[1,i].IsDespawned != true){
                        _shooters[i] = _sprites[1,i];
                    }
                    else if(_sprites[0,i].IsDespawned != true){
                        _shooters[i] = _sprites[0,i];
                    }
                    else{
                        _shooters[i] = null;
                    }
                }
            }
        }

        public override void Initialize(){
            Pawn? despawn;
            while (true){
                Thread.Sleep(_speed);
                if (Stop){
                    try{
                        Thread.Sleep(Timeout.Infinite); // Thread will sleep until another thread wakes it up

                    }
                    catch(ThreadInterruptedException){
                        continue;
                    }
                }
                Behavior();
                despawn = CheckDespawn();
                if (despawn != null){
                    _controller.DeleteSprite(despawn);
                    despawn.IsDespawned = true;
                    ChooseNewShooter(despawn);
                }
            }
        }
        
    }

    class ProjectileController: SpriteController{
        CollisionInfo collisionInfo;
        public ProjectileController(Pawn? sprite, CentralController centralController, string controllerId, int speed): base(centralController, speed, controllerId, sprite){}

        protected override void Behavior(){
           collisionInfo = _sprite.MoveNorth(1);
        }

        protected override bool CheckDespawnConditions()
        {
            if (collisionInfo.CollisionOccurred){
                collisionInfo.Entity.CollisionInfo = new CollisionInfo(true, _sprite);
                return true;
            }
            if(_sprite.LastCollided.CollisionOccurred){
                if(_sprite.LastCollided.Entity.SpriteId.Contains("EnemyShip")){
                    return true;
                }
            }
            return false;
        }

    }

    class EnemyProjectileController: SpriteController{
        CollisionInfo collisionInfo;
        Input _input;
        public EnemyProjectileController(Pawn? sprite, Input input, CentralController centralController, string controllerId, int speed): base(centralController, speed, controllerId, sprite){
            _input = input;
        }

        protected override void Behavior(){
           collisionInfo = _sprite.MoveSouth(1);
        }

        protected override bool CheckDespawnConditions()
        {
            if (collisionInfo.CollisionOccurred){
                // if (collisionInfo.Entity.SpriteId == "ship"){
                    
                //     return true;
                // }
                return true;
            }
            return false;
        }

    }

    class ShipHitDetector: Controller{

        Input _input;
        CollisionInfo? _collisionInfo;
        Pawn _sprite;

        public ShipHitDetector(Pawn sprite, Input input, CentralController centralController, string controllerId, int speed): base(centralController, speed, controllerId){
            _collisionInfo = null;
            _sprite = sprite;
            _input = input;
        }

        protected override void Behavior(){}
        protected override bool CheckDespawnConditions(){
            if (_sprite.LastCollided.CollisionOccurred){
                if (_sprite.LastCollided.Entity.SpriteId == "enemyProjectile"){
                    return true;
                }
            }
            return false;
        }

        public override void Initialize(){
            bool gameOver;
            while(true){
                //Thread.Sleep(_speed);
                gameOver = CheckDespawnConditions();
                if (gameOver){
                    break;
                }
            }
        }
    }
}