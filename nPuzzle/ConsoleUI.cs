using System;
using System.Text;
using Puzzle.Core;
using System.Text.RegularExpressions;
using System.Threading;

namespace Puzzle.CUI
{
    /// <summary>
    /// Console
    /// </summary>
    class ConsoleUI : Puzzle.IUserInterface
    {
        //Playing field
        private Field field;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="field">Field object</param>
        public ConsoleUI(Field field)
        {
            this.field = field;
            field.DisplayPlaying += UpdateUI;
            field.DisplayGeneration += delegate()
            {
                Thread.Sleep(50);
                UpdateUI();
            };
        }

        /// <summary>
        /// Start new game
        /// </summary>
        public void StartNewGame()
        {

            field.Generate();

            do
            {
                //UpdateUI();
                ProcessInput();
                
                if (field.State == GameState.SOLVED)
                {
                    Console.WriteLine("Vyhral si!");
                    Console.ReadLine();
                    StartNewGame();
                }
                //throw new NotImplementedException("This method is not implemented yet.");
            } while (true);
        }

        /// <summary>
        /// Show current game state (delegated method)
        /// </summary>
        public void UpdateUI()
        {
            
            //throw new NotImplementedException("This method is not implemented yet.");
            StringBuilder hraciePole = new StringBuilder();
            char znak = 'A';

            Console.Clear();

            for (int i = 0; i < field.ColumnCount; i++)
            {
                hraciePole.Append(" " + " " + i);
            }

            hraciePole.AppendLine();

            for (int row = 0; row < field.RowCount; row++)
            {

                hraciePole.Append(znak);

                for (int column = 0; column < field.ColumnCount; column++)
                {

                    Tile pole = field.Tiles[row, column];

                    if (pole is ValueTile)
                    {
                        if (((ValueTile)pole).Value < 10)
                        {
                            hraciePole.Append(" " + "0" + ((ValueTile)pole).Value);
                        }
                        else
                        {
                            hraciePole.Append(" " + ((ValueTile)pole).Value);
                        }
                    }
                    else
                    {
                        hraciePole.Append(" " + " " + " ");
                    }

                }
                hraciePole.AppendLine();
                znak++;
            }

            Console.WriteLine(hraciePole);
        }

        /// <summary>
        /// Process user input
        /// </summary>
        private void ProcessInput()
        {
            string vstup;
           
            Console.WriteLine("(X) EXIT");
            Console.WriteLine("(N) NEW");
            Console.WriteLine("(MB4) MOVE");
            Console.WriteLine("(Score) Score");

            vstup = Console.ReadLine();

            try
            {
                HandleInput(vstup);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            
        }

        private void HandleInput(string input)
        {

            Regex move = new Regex(@"[mM][a-dA-D][0-3]");
            if (move.IsMatch(input))
            {
                int riadok = input[1] - 'a';
                int stlpec = int.Parse(input[2].ToString());
                field.MoveTile(riadok, stlpec);
            }

            else if (input == "x" || input == "X")
            {
                Environment.Exit(0);
            }
            else if (input == "n" || input == "N")
            {
                field.Generate();
            }
            else if (input == "score")
            {
                PuzzleGame.puzzleGame.BestScore.ToString();
                Console.ReadLine();
            }
            else
            {
                throw new WrongFormatException("Zly vstup!");
            }
        }
    }
}
