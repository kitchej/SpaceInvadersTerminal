using SimpleGameEngine;
using System.Runtime.InteropServices;

namespace spaceInvaders{
    
    static class Program{

        
        

            

        static void Main(){

            Console.SetWindowSize(60, 41);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)){
                // Below is needed to prevent the console window from being resized when started from an executable
                const int MF_BYCOMMAND = 0x00000000;
                const int SC_CLOSE = 0xF060;
                const int SC_MINIMIZE = 0xF020;
                const int SC_MAXIMIZE = 0xF030;
                const int SC_SIZE = 0xF000;

                [DllImport("user32.dll")]
                static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

                [DllImport("user32.dll")]
                static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

                [DllImport("kernel32.dll", ExactSpelling = true)]
                static extern IntPtr GetConsoleWindow();

                IntPtr handle = GetConsoleWindow();
                IntPtr sysMenu = GetSystemMenu(handle, false);

                if (handle != IntPtr.Zero)
                {
                    DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
                    DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
                    DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                    DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
                }
            }

            CentralController mainController = new CentralController();
            Display screen = new Display(mainController, 40, 60);
            Input input = new Input();

            // Intro Screen

            screen.DrawSprite(new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "introGraphic.txt"}), 5, 5, "Header"));
            screen.Show();
            Console.ReadKey();

            // Game Init
            Pawn ship = new Pawn(Path.Join(new[] {"src", "spaceInvaders", "sprites", "ship.txt"}), startx: 30, starty: 36, spriteId: "ship", centralController: mainController, stackOrder: 0);
            Spawner shipProjectileSpawner = new Spawner(typeof(Pawn), 
                                                        new object[] { Path.Join(new[] {"src", "spaceInvaders", "sprites", "projectile.txt"}), 30, 21, "projectile", mainController, 0, true}, 
                                                        typeof(ProjectileController), 
                                                        new object?[] {null, mainController, "ProjectileCon", 40}, 
                                                        mainController);

            Sprite livesLabel = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "livesLabel.txt"}), startx: 1, starty: 1, spriteId: "livesLabel");
            Sprite life1 = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "life.txt"}), startx: 7, starty: 1, spriteId: "life1", collisions: false);
            Sprite life2 = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "life.txt"}), startx: 8, starty: 1, spriteId: "life2", collisions: false);
            Reset resetAction = new Reset(mainController, new Stack<Sprite>(new Sprite[]{life1, life2}), ship, input);

            Sprite northBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "horizBorder.txt"}), 1, 0, "northBounds");
            Sprite southBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "horizBorder.txt"}), 1, 39, "southBounds");
            Sprite westBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "vertiBorder.txt"}), 0, 1, "westBounds");
            Sprite eastBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "vertiBorder.txt"}), 59, 1, "eastBounds");

            MasterEnemyController masterEnemyController = new MasterEnemyController(mainController, 500, "MasterEnemyCon", new Spawner(typeof(Pawn), 
                                                                                                                        new object[] { Path.Join(new[] {"src", "spaceInvaders", "sprites", "projectile.txt"}), 30, 21, "enemyProjectile", mainController, 0, true}, 
                                                                                                                        typeof(EnemyProjectileController), 
                                                                                                                        new object?[] {null, mainController, "EnemyProjectileCon", 40, resetAction}, 
                                                                                                                        mainController),
                                                                                                                        input);
            
            mainController.AddSprite(ship);
            mainController.AddSprite(northBounds);
            mainController.AddSprite(southBounds);
            mainController.AddSprite(westBounds);
            mainController.AddSprite(eastBounds);
            mainController.AddSprite(livesLabel);
            mainController.AddSprite(life1);
            mainController.AddSprite(life2);
            
            input.BindAction(ConsoleKey.Q, new EndGame(mainController));
            input.BindAction(ConsoleKey.A, new MoveWest(ship));
            input.BindAction(ConsoleKey.D, new MoveEast(ship));
            input.BindAction(ConsoleKey.W, new MoveNorth(ship));
            input.BindAction(ConsoleKey.S, new MoveSouth(ship));
            input.BindAction(ConsoleKey.Spacebar, new ShootProjectile(ship, shipProjectileSpawner, 1));
            
            mainController.Mainloop(screen, input, new Controller[] {masterEnemyController});
        }
    }
}
