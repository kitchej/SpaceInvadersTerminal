using System.Threading;

namespace SimpleGameEngine{
    static class Mainloop{
        static bool EXIT = false;
        public static void mainloop(Display displayObj, Input inputObj, SpriteController[]? spriteContollers = null){
            List<Thread> contollerThreads = new List<Thread>();
            Thread inputThread = new Thread(inputObj.Listen);
            Thread displayThread = new Thread(displayObj.Refresh);
            displayThread.IsBackground = true;

            Thread controllerThread;
            if (spriteContollers != null){
                foreach(var controller in spriteContollers){
                    controllerThread = new Thread(controller.Initialize);
                    controllerThread.IsBackground = true;
                    contollerThreads.Add(controllerThread);
                }
            }
            
            
            inputThread.Start();
            displayThread.Start();

            if (contollerThreads.Count != 0){
                foreach(var thread in contollerThreads){
                    thread.Start();
                }
            }
            
        }
    }
}
