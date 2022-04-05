using SimpleGameEngine;

namespace spaceInvaders{
    
    static class Program{

        static void Main(){
         Display screen = new Display(40, 60);
            Sprite northBounds = new Sprite(@"src\spaceInvaders\sprites\horizBorder.txt", 1, 0, "northBounds");
            Sprite southBounds = new Sprite(@"src\spaceInvaders\sprites\horizBorder.txt", 1, 39, "southBounds");
            Sprite westBounds = new Sprite(@"src\spaceInvaders\sprites\vertiBorder.txt", 0, 1, "westBounds");
            Sprite eastBounds = new Sprite(@"src\spaceInvaders\sprites\vertiBorder.txt", 59, 1, "eastBounds");
            Pawn ship = new Pawn(@"src\spaceInvaders\sprites\ship.txt", startx: 30, starty: 20, spriteId: "ship", display: screen, stackOrder: 0);
            Pawn enemyShip = new Pawn(@"src\spaceInvaders\sprites\alien1.txt", startx: 30, starty: 12, spriteId: "enemyShip", display: screen);
            Spawner shipProjectileSpawner = new Spawner(typeof(Pawn), new object[] { @"src\spaceInvaders\sprites\projectile.txt", 30, 21, "projectile", screen, 0, true}, 
                                          typeof(ProjectileContoller), new object?[] {null, screen, 50}, 
                                          screen);
            Spawner enemyProjectileSpawner = new Spawner(typeof(Pawn), new object[] { @"src\spaceInvaders\sprites\projectile.txt", 30, 21, "enemyProjectile", screen, 0, true}, 
                                          typeof(EnemyProjectileContoller), new object?[] {null, screen, 50}, 
                                          screen);
            EnemyContoller enemyAi = new EnemyContoller(enemyShip, screen, 500, enemyProjectileSpawner);
            
            screen.AddSprite(ship);
            screen.AddSprite(northBounds);
            screen.AddSprite(southBounds);
            screen.AddSprite(westBounds);
            screen.AddSprite(eastBounds);
            screen.AddSprite(enemyShip);
            Input input = new Input(exitKey: ConsoleKey.Q);
            input.BindAction(ConsoleKey.A, new MoveWest(ship));
            input.BindAction(ConsoleKey.D, new MoveEast(ship));
            input.BindAction(ConsoleKey.W, new MoveNorth(ship));
            input.BindAction(ConsoleKey.S, new MoveSouth(ship));
            input.BindAction(ConsoleKey.Spacebar, new ShootProjectile(ship, shipProjectileSpawner));
            
            Mainloop.mainloop(screen, input, new SpriteController[] {enemyAi});
        }
    }
}
