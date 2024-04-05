using System.Reflection;

namespace SimpleGameEngine{

    class CannotFindConstructor: Exception{
        public CannotFindConstructor(): base(){}
        public CannotFindConstructor(string message): base(message){}
        public CannotFindConstructor(string message, Exception innerException): base(message, innerException){}
    }

    class Spawner{
        CentralController _controller;
        Type _spriteCls;
        object[] _spriteAttr;
        Type _spriteControllerCls;
        object[] _spriteControllerAttr;
        ConstructorInfo? _spriteConstructor;
        ConstructorInfo? _spriteControllerConstructor;

        Random _numGen;

        // For spriteAttr & controllerAttr: You need to include ALL parameters of the target constructor, even default parameters
        public Spawner(Type spriteCls, object[] spriteAttr, Type spriteControllerCls, object[] controllerAttr, CentralController centralController){
            _controller = centralController;
            _spriteCls = spriteCls;
            _spriteAttr = spriteAttr;
            _spriteControllerAttr = controllerAttr;
            _spriteControllerCls = spriteControllerCls;
            _numGen = new Random();

            Type[] spriteAttrTypes = new Type[_spriteAttr.Length];
            for (int i=0; i<_spriteAttr.Length; i++){
                spriteAttrTypes[i] = _spriteAttr[i].GetType();
            }
            _spriteConstructor = _spriteCls.GetConstructor(spriteAttrTypes);
            if (_spriteConstructor == null){
                throw new CannotFindConstructor($"No constructor found for \"{_spriteCls}\" with the specified types");
            }

            Type[] spriteControllerAttrTypes = new Type[_spriteControllerAttr.Length];
            for (int i=0; i<_spriteControllerAttr.Length; i++){
                if (_spriteControllerAttr[i] == null){
                    spriteControllerAttrTypes[i] = spriteCls;
                    continue;
                }
                spriteControllerAttrTypes[i] = _spriteControllerAttr[i].GetType();
            }
            _spriteControllerConstructor = _spriteControllerCls.GetConstructor(spriteControllerAttrTypes);
            if (_spriteControllerConstructor == null){
                throw new CannotFindConstructor($"No constructor found for \"{_spriteControllerCls}\" with the specified types");
            }
        }

        public Thread SpawnSprite(int startx, int starty){
            var newSprite = _spriteConstructor.Invoke(_spriteAttr) as Pawn;
            _spriteControllerAttr[0] = newSprite;
            newSprite.MoveTo(startx, starty);
            var newController = _spriteControllerConstructor.Invoke(_spriteControllerAttr) as SpriteController;
            

            _controller.AddSprite(newSprite);
            Thread controllerThread = new Thread(newController.Initialize);
            newController.Id = newController.Id + _numGen.Next(10000, 99999);
            _controller.AddController(newController, controllerThread);
            
            controllerThread.IsBackground = true;
            controllerThread.Start();
            return controllerThread;
        }
    }
}