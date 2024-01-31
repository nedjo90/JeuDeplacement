using System;
using System.Collections.Generic;
using System.Threading;

namespace JeuDeplacement
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            char[,]grille = creerGrille();
            LinkedList<int[]> snake = creerSnake(grille);
            int points = 0;
            Console.Clear();
            Console.CursorVisible = false;
            Afficher(grille);
            GenerateFood(grille);
            char sens = 'G';
            while (!Mordu(snake))
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    while (key.Key != ConsoleKey.UpArrow 
                           && key.Key != ConsoleKey.DownArrow 
                           && key.Key != ConsoleKey.LeftArrow 
                           && key.Key != ConsoleKey.RightArrow)
                    {
                        Thread.Sleep(100);
                        key = Console.ReadKey(true);
                    }
                    sens = pressedKey(key, sens);
                }

                if (sens == 'G')
                    deplacerGauche(grille, snake, ref points);
                else if(sens == 'D')
                    deplacerDroite(grille, snake, ref points);
                else if(sens == 'H')
                    deplacerHaut(grille, snake,ref points);
                else if(sens == 'B')
                    deplacerBas(grille, snake,ref points);

                Thread.Sleep(100);
            }

        }

        public static bool Mordu(LinkedList<int[]> snake)
        {
            int[] tete = snake.First.Value;
            int node = 0;
            foreach (int[]tab in snake)
            {
                if (node != 0 && tab[0] == tete[0] && tab[1] == tete[1])
                {
                    return true;
                }

                node++;
            }
            return false;
        }

        public static char pressedKey(ConsoleKeyInfo key, char sens)
        {
            if (key.Key == ConsoleKey.UpArrow && sens != 'B')
                sens = 'H';
            else if (key.Key == ConsoleKey.DownArrow && sens != 'H')
                sens = 'B';
            else if (key.Key == ConsoleKey.LeftArrow && sens != 'D')
                sens = 'G';
            else if (key.Key == ConsoleKey.RightArrow && sens != 'G')
                sens = 'D';
            return sens;
        }
        public static void deplacerGauche(char[,] grille, LinkedList<int[]> snake,ref int points)
        {
            int nextX = snake.First.Value[1] - 1;
            int nextY = snake.First.Value[0];
            int lastX = snake.Last.Value[1];
            int lastY = snake.Last.Value[0];
            if (nextX == 0)
            {
                nextX = 68;
            }
            snake.AddFirst(new[] {nextY, nextX});
            if (IsFood(grille, nextX, nextY))
            {
                AddPoints(ref points);   
                GenerateFood(grille);
            }
            else
            {
                snake.RemoveLast();
                Console.SetCursorPosition(lastX, lastY);
                Console.Write(' ');
                grille[lastY, lastX] = ' ';
            }
            Console.SetCursorPosition(nextX, nextY);
            Console.Write('O');
            grille[nextY, nextX] = 'O';
        }
        public static void deplacerDroite(char[,] grille, LinkedList<int[]> snake,ref int points)
        {
            int nextX = snake.First.Value[1] + 1;
            int nextY = snake.First.Value[0];
            int lastX = snake.Last.Value[1];
            int lastY = snake.Last.Value[0];
            if (nextX == 69)
            {
                nextX = 1;
            }
            snake.AddFirst(new[] {nextY, nextX});
            if (IsFood(grille, nextX, nextY))
            {
                AddPoints(ref points);   
                GenerateFood(grille);
            }
            else
            {
                snake.RemoveLast();
                Console.SetCursorPosition(lastX, lastY);
                Console.Write(' ');
                grille[lastY, lastX] = ' ';
            }
            Console.SetCursorPosition(nextX, nextY);
            Console.Write('O');
            grille[nextY, nextX] = 'O';
        }
        
        public static void deplacerHaut(char[,] grille, LinkedList<int[]> snake,ref int points)
        {
            int nextX = snake.First.Value[1];
            int nextY = snake.First.Value[0] - 1;
            int lastX = snake.Last.Value[1];
            int lastY = snake.Last.Value[0];
            if (nextY == 0)
            {
                nextY = 28;
            }
            snake.AddFirst(new[] {nextY, nextX});
            if (IsFood(grille, nextX, nextY))
            {
                AddPoints(ref points);   
                GenerateFood(grille);
            }
            else
            {
                snake.RemoveLast();
                Console.SetCursorPosition(lastX, lastY);
                Console.Write(' ');
                grille[lastY, lastX] = ' ';
            }
            Console.SetCursorPosition(nextX, nextY);
            Console.Write('O');
            grille[nextY, nextX] = 'O';
        }

        public static void deplacerBas(char[,] grille, LinkedList<int[]> snake,ref int points)
        {
            int nextX = snake.First.Value[1];
            int nextY = snake.First.Value[0] + 1;
            int lastX = snake.Last.Value[1];
            int lastY = snake.Last.Value[0];
            if (nextY == 29)
            {
                nextY = 1;
            }
            snake.AddFirst(new[] {nextY, nextX});
            if (IsFood(grille, nextX, nextY))
            {
                AddPoints(ref points);   
                GenerateFood(grille);
            }
            else
            {
                snake.RemoveLast();
                Console.SetCursorPosition(lastX, lastY);
                Console.Write(' ');
                grille[lastY, lastX] = ' ';
            }
            Console.SetCursorPosition(nextX, nextY);
            Console.Write('O');
            grille[nextY, nextX] = 'O';
        }
        public static bool IsFood(char[,] grille, int x, int y)
        {
            if (grille[y,x] == '@')
            {
                return true;
            }

            return false;
        }

        public static void AddPoints(ref int points)
        {
            points++;
            Console.SetCursorPosition(8, 30);
            Console.Write(points);
        }
        public static void GenerateFood(char[,]grille)
        {
            Random random = new Random();
            int x, y;
            do
            {
                x = random.Next(2, 68);
                //y = random.Next(2, 28);
                y = random.Next(2, 28);
            } while (grille[y, x] == 'O');
            Console.SetCursorPosition(x,y);
            Console.Write('@');
            grille[y, x] = '@';
        }
        public static LinkedList<int[]> creerSnake(char[,]grille)
        {
            int positionX = 70 / 2;
            int positionY = 30 / 2;
            LinkedList<int[]> snake = new LinkedList<int[]>();
            for (int i = 0; i < 10; i++)
            {
                int[] s = new int[]{ positionY, positionX + i };
                snake.AddLast(s);
                grille[positionY, positionX + i] = 'O';
            }
            
            return snake;
        }
        public static char[,] creerGrille()
        {
            int width = 70;
            int height = 30;
            char[,]grille = new char[height, width];
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    if (i == 0 || j == 0 || i == grille.GetLength(0) - 1 || j == grille.GetLength(1) - 1)
                    {
                        grille[i, j] = '#';
                    }
                    else
                    {
                        grille[i, j] = ' ';
                    }
                }
            }
            return grille;
        }
        public static void Afficher(char[,]grille)
        {
            Console.SetCursorPosition(0,0);
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    Console.Write(grille[i, j]);
                }
                Console.WriteLine("");
            }
            Console.WriteLine(
                "Points:                          \n"
                + "→ - Droite\t"
                + "← - Gauche\n"
                + "↑ - Haut\t"
                + "↓ - Bas"
            );
        }
    }
}