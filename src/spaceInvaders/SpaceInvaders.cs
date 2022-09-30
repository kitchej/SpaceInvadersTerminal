using SimpleGameEngine;

namespace spaceInvaders{
    
    static class Program{

        static void Main(){
            CentralController mainController = new CentralController();
            Display screen = new Display(mainController, 40, 60);
            Input input = new Input();

            // Intro Screen

            screen.DrawSprite(new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "introGraphic.txt"}), 5, 5, "Header"));
            screen.Show();
            Console.ReadKey();

            // Game Init

            Sprite northBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "horizBorder.txt"}), 1, 0, "northBounds");
            Sprite southBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "horizBorder.txt"}), 1, 39, "southBounds");
            Sprite westBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "vertiBorder.txt"}), 0, 1, "westBounds");
            Sprite eastBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "vertiBorder.txt"}), 59, 1, "eastBounds");

            MasterEnemyController masterEnemyController = new MasterEnemyController(mainController, 500, "MasterEnemyCon", new Spawner(typeof(Pawn), 
                                                                                                                        new object[] { Path.Join(new[] {"src", "spaceInvaders", "sprites", "projectile.txt"}), 30, 21, "enemyProjectile", mainController, 0, true}, 
                                                                                                                        typeof(EnemyProjectileController), 
                                                                                                                        new object?[] {null, input, mainController, "EnemyProjectileCon", 40}, 
                                                                                                                        mainController));
            
            Pawn ship = new Pawn(Path.Join(new[] {"src", "spaceInvaders", "sprites", "ship.txt"}), startx: 30, starty: 36, spriteId: "ship", centralController: mainController, stackOrder: 0);
            Spawner shipProjectileSpawner = new Spawner(typeof(Pawn), 
                                                        new object[] { Path.Join(new[] {"src", "spaceInvaders", "sprites", "projectile.txt"}), 30, 21, "projectile", mainController, 0, true}, 
                                                        typeof(ProjectileController), 
                                                        new object?[] {null, mainController, "ProjectileCon", 40}, 
                                                        mainController);

            mainController.AddSprite(ship);
            mainController.AddSprite(northBounds);
            mainController.AddSprite(southBounds);
            mainController.AddSprite(westBounds);
            mainController.AddSprite(eastBounds);
            
            input.BindAction(ConsoleKey.Q, new EndGame(mainController));
            input.BindAction(ConsoleKey.A, new MoveWest(ship));
            input.BindAction(ConsoleKey.D, new MoveEast(ship));
            input.BindAction(ConsoleKey.W, new MoveNorth(ship));
            input.BindAction(ConsoleKey.S, new MoveSouth(ship));
            input.BindAction(ConsoleKey.Spacebar, new ShootProjectile(ship, shipProjectileSpawner, 1));
            input.BindAction(ConsoleKey.P, new Pause(mainController, input));
            
            mainController.mainloop(screen, input, new Controller[] {masterEnemyController});

            
            
            
        }
    }
}
