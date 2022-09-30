using System.Threading;
using spaceInvaders;

namespace SimpleGameEngine{
    static class Mainloop{
        public static void mainloop(Display displayObj, Input inputObj, Controller[]? spriteControllers = null){
            List<Thread> controllerThreads = new List<Thread>();
            Thread inputThread = new Thread(inputObj.Listen);
            Thread displayThread = new Thread(displayObj.Refresh);
            displayThread.IsBackground = true;

            Thread controllerThread;
            if (spriteControllers != null){
                foreach(var controller in spriteControllers){
                    controllerThread = new Thread(controller.Initialize);
                    controllerThread.IsBackground = true;
                    controllerThreads.Add(controllerThread);
                }
            }

            ThreadController test = new ThreadController(controllerThreads[0], spriteControllers[0]);

            inputObj.BindAction(ConsoleKey.P, new Pause(test));
            
            inputThread.Start();
            displayThread.Start();

            if (controllerThreads.Count != 0){
                foreach(var thread in controllerThreads){
                    thread.Start();
                }
            }
            
        }
    }
}
