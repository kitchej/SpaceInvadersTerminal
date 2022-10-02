namespace SimpleGameEngine{

    abstract class Controller{
        protected CentralController _controller;
        protected int _speed;
        public string Id {get; set;}

        public bool Stop {get; set;}

        public Controller(CentralController centralController, int speed, string controllerId){
            _controller = centralController;
            _speed = speed;
            Id = controllerId;
        }

        protected abstract void Behavior();

        protected virtual bool CheckDespawnConditions(){return false;}

        public abstract void Initialize();

    }

    abstract class SpriteController: Controller{

        protected Pawn _sprite;

        public SpriteController(CentralController centralController, int speed, string controllerId, Pawn sprite): base(centralController, speed, controllerId){
            _sprite = sprite;
        }

        public override void Initialize(){
            bool despawn;
            if (_sprite == null){
                throw new ArgumentNullException("\"sprite\" attribute cannot be null");
            }
            while (true){
                if (Stop){
                    try{
                        Thread.Sleep(Timeout.Infinite);
                    }
                    catch(ThreadInterruptedException){
                        continue;
                    }
                }
                try{
                    Thread.Sleep(_speed);
                }
                catch(ThreadInterruptedException){
                    /* 
                    This exception needs to be caught here or the game will 
                    crash if CentralController.ResumeAll() or ResumeController() is spammed. 
                    */
                }  
                
                Behavior();
                despawn = CheckDespawnConditions();
                if (despawn){
                    _controller.DeleteSprite(_sprite);
                    _controller.RemoveController(Id);
                    return;
                }
            }
        }
    }
}
