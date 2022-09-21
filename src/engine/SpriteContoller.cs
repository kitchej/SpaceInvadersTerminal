namespace SimpleGameEngine{

    abstract class Controller{
        protected Display _display;
        protected int _speed;

        public Controller(Display display, int speed){
            _display = display;
            _speed = speed;
        }

        protected abstract void Behavior();

        protected virtual bool CheckDespawnConditions(){return false;}

        public abstract void Initialize();

    }

    abstract class SpriteController: Controller{

        protected Pawn _sprite;

        public SpriteController(Display display, int speed, Pawn sprite): base(display, speed){
            _sprite = sprite;
        }

        public override void Initialize(){
            bool despawn;
            if (_sprite == null){
                throw new ArgumentNullException("\"sprite\" attribute cannot be null");
            }
            while (true){
                Thread.Sleep(_speed);
                Behavior();
                despawn = CheckDespawnConditions();
                if (despawn){
                    _display.DeleteSprite(_sprite);
                    return;
                }
            }
        }
    }
}
