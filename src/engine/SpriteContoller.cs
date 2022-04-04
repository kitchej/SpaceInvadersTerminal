namespace SimpleGameEngine{
    abstract class SpriteController{

        protected Display _display;
        protected Pawn? _sprite;
        public Pawn? Sprite {set {_sprite = value;}}
        protected int _speed;

        public SpriteController(Pawn? sprite, Display display, int speed){
            _display = display;
            _sprite = sprite;
            _speed = speed;
        }

        protected abstract void Behavior();

        protected virtual bool CheckDespawnConditions(){return false;}

        public void Initialize(){
            bool despawn;
            if (_sprite == null){
                throw new ArgumentNullException("\"sprite\" attrribute cannot be null");
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
