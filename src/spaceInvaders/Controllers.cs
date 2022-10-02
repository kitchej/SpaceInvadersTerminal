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
        Thread? _projectileThread;
        int _enemyCount;
        Input _input;

        public MasterEnemyController(CentralController centralController, int speed, string controllerId, Spawner projectileSpawner, Input input): base(centralController, speed, controllerId){
            _projectileSpawner = projectileSpawner;
            _direction = "left";
            _count = 0;
            _passes = 0;
            _logger = new Logger("EnemyGroupLog.log");
            _projectileThread = null;
            Pawn s;
            int x = 13;
            int startx = x;
            int starty = 3;
            string[] shipTypes = {"alien1.txt", "alien2.txt", "alien3.txt"};
            _shooters = new Pawn[8];
            _sprites = new Pawn[3,8];
            _input = input;
            for (int i=0;i<3;i++){
                for(int j=0;j<8;j++){
                    s = new Pawn(Path.Join(new[] {"src", "spaceInvaders", "sprites", shipTypes[i]}), startx: startx, starty: starty, spriteId: "EnemyShip_" + shipTypes[i], centralController: _controller);
                    _enemyCount ++;
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
                   if (_collisionInfo.CollisionOccurred){
                        if(_collisionInfo.Entity.SpriteId == "ship" || _collisionInfo.Entity.SpriteId == "southBounds"){
                            EndGame(false);
                        }
                   }
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
                if (_speed > 125){
                    _speed -= 25;
                }
            }

            if (_projectileThread == null){
                foreach(Pawn? s in _shooters){
                    if (s != null){
                        shooterPool.Add(s);
                    }
                }

                shooter = shooterPool.ElementAt(rnd.Next(0, shooterPool.Count - 1));
                
                _projectileThread = _projectileSpawner.SpawnSprite(shooter.Coords[6].X, shooter.Coords[6].Y + 1); 
            }
            else if(!_projectileThread.IsAlive){
                foreach(Pawn? s in _shooters){
                    if (s != null){
                        shooterPool.Add(s);
                    }
                }
                shooter = shooterPool.ElementAt(rnd.Next(0, shooterPool.Count - 1));   
                _projectileThread = _projectileSpawner.SpawnSprite(shooter.Coords[6].X, shooter.Coords[6].Y + 1);
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

        void EndGame(bool win){
            _input.UnbindAction(ConsoleKey.A);
            _input.UnbindAction(ConsoleKey.D);
            _input.UnbindAction(ConsoleKey.Spacebar);
            _controller.PauseAll();
            _controller.DeleteAllSprites();
            if (win){
                _controller.AddSprite(new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "win.txt"}), 5, 5, "Win"));
            }
            else{
                _controller.AddSprite(new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "gameOver.txt"}), 5, 5, "GameOver"));
            }
            
        }

        public override void Initialize(){
            Pawn? despawn;
            while (true){
                // THIS TRY BLOCK NEEDS TO BE FIRST!
                // I DON'T KNOW WHY, BUT IF I MOVE IT THE GAME WILL NOT PAUSE PROPERLY
                try{
                    Thread.Sleep(_speed);
                }
                catch(ThreadInterruptedException){
                    /* 
                    This exception needs to be caught here or the game will 
                    crash if CentralController.ResumeAll() or ResumeController() is spammed. 
                    */
                }
                
                if (Stop){
                    try{
                        Thread.Sleep(Timeout.Infinite);
                    }
                    catch(ThreadInterruptedException){
                        continue;
                    }
                }
                
                Behavior();
                despawn = CheckDespawn();
                if (despawn != null){
                    _controller.DeleteSprite(despawn);
                    _enemyCount --;
                    if (_enemyCount == 0){
                        EndGame(true);
                    }
                    despawn.IsDespawned = true;
                    ChooseNewShooter(despawn);
                }
            }
        }
        
    }

    class ProjectileController: SpriteController{
        CollisionInfo collisionInfo;
        Logger logger;
        public ProjectileController(Pawn? sprite, CentralController centralController, string controllerId, int speed): base(centralController, speed, controllerId, sprite){
            logger = new Logger("Projectile.log");
        }

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
        CentralController _centralController;
        Logger _logger;
        Reset resetAction;
        public EnemyProjectileController(Pawn? sprite, CentralController centralController, string controllerId, int speed, Reset action): base(centralController, speed, controllerId, sprite){
            _centralController = centralController;
            _logger = new Logger("EnemyProjectile.log");
            resetAction = action;
        }

        protected override void Behavior(){
           collisionInfo = _sprite.MoveSouth(1);
        }

        protected override bool CheckDespawnConditions()
        {
            if (collisionInfo.CollisionOccurred){
                if (collisionInfo.Entity.SpriteId == "ship"){
                    resetAction.ExecuteAction();
                    return true;
                }
                return true;
            }
            return false;
        }
    }
}