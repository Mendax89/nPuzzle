using System;
using System.Text;
using System.Threading;
using Puzzle.CUI;

namespace Puzzle.Core
{
    /// <summary>
    /// Game state
    /// </summary>
    public enum GameState
    {
        GENERATION,
        PLAYING,
        SOLVED
    }

    public delegate void ShiftPresentationHandler();

    /// <summary>
    /// Field logic
    /// </summary>
    public class Field
    {
        public event ShiftPresentationHandler DisplayPlaying;
        public event ShiftPresentationHandler DisplayGeneration;

        //Shift selected tile to grey tile
        private const int MOVE_LEFT_TILE = 0;
        private const int MOVE_RIGHT_TILE = 1;
        private const int MOVE_UP_TILE = 2;
        private const int MOVE_DOWN_TILE = 3;

        ///Used in field generation
        private const int SHUFFLE_MOVEMENTS = 100;

        //Field dimension
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }

        //Field of tiles
        public Tile[,] Tiles { get; set; }

        //Game state
        public GameState State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private GreyTile greyTile;

        /// <summary>
        /// Field constructor
        /// </summary>
        /// <param name="rowCount">Rows count</param>
        /// <param name="columnCount">Columns count</param>
        public Field(int rowCount, int columnCount)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            Tiles = new Tile[rowCount, columnCount];

            Init();
        }

        /// <summary>
        /// Field initialization
        /// </summary>
        private void Init()
        {
            greyTile = new GreyTile();
            int n = 1;
            for (int row = 0; row < RowCount; row++)
            {
                for (int column = 0; column < ColumnCount; column++)
                {
                    Tiles[row, column] = new ValueTile(n);
                    Tiles[row, column].Row = row;
                    Tiles[row, column].Col = column;
                    n++;
                }
            }
            Tiles[RowCount -1, ColumnCount - 1] = greyTile;
            Tiles[RowCount - 1, ColumnCount - 1].Row = RowCount - 1;
            Tiles[RowCount - 1, ColumnCount - 1].Col = ColumnCount - 1;
        }

        /// <summary>
        /// Field generator
        /// </summary>
        public void Generate()
        {
            //Set game state
            State = GameState.GENERATION;

            //Shuffle field's tiles
            Random rnd = new Random();
            int direction = MOVE_DOWN_TILE;
            for (int shifts = 0; shifts < SHUFFLE_MOVEMENTS; shifts++)
            {
                int newDirection;
                // It is possible to define next value as follows (it selects another direction from previous one): 
                //       while (direction == (newDirection = rnd.Next(0, 4)));
                // However, this generator is not so sophisticated. Use next one to get better result (field shuffling)
                newDirection = direction == 0 || direction == 1 ? rnd.Next(2, 4) : rnd.Next(0, 2);

                bool moved = false;
                switch (newDirection)
                {
                    case MOVE_LEFT_TILE: moved = MoveTile(greyTile.Row, greyTile.Col - 1); break;
                    case MOVE_RIGHT_TILE: moved = MoveTile(greyTile.Row, greyTile.Col + 1); break;
                    case MOVE_UP_TILE: moved = MoveTile(greyTile.Row - 1, greyTile.Col); break;
                    case MOVE_DOWN_TILE: moved = MoveTile(greyTile.Row + 1, greyTile.Col); break;
                }


                if (moved)
                {
                    direction = newDirection;
                }
                else
                {
                    shifts--;
                }
            }

            //Set game state
            State = GameState.PLAYING;
        }

        /// <summary>
        /// Moves specified tile identified by column and row index
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <returns>True if tile have been moved, false otherwise</returns>
        public bool MoveTile(int row, int col)
        {
            if (row >= 0 && row < RowCount && col >= 0 && col < ColumnCount)
            {
                return MoveTile(Tiles[row, col]);
            }

            return false;
        }

        /// <summary>
        /// Moves specified tile
        /// </summary>
        /// <param name="tile">Tile</param>
        /// <returns>True if tile have been moved, false otherwise</returns>
        public bool MoveTile(Tile tile)
        {
            int moznostPosunu = Math.Abs(greyTile.Row - tile.Row) + Math.Abs(greyTile.Col - tile.Col);
            
            if (moznostPosunu == 1)
            {
                int pomocnyCol;
                int pomocnyRow;
                pomocnyCol = greyTile.Col;
                pomocnyRow = greyTile.Row;

                greyTile.Col = tile.Col;
                greyTile.Row = tile.Row;

                tile.Col = pomocnyCol;
                tile.Row = pomocnyRow;

                Tiles[tile.Row, tile.Col] = tile;
                Tiles[greyTile.Row, greyTile.Col] = greyTile;

                if (State==GameState.GENERATION)
                {
                    if (DisplayGeneration!=null)
                    {
                        DisplayGeneration();                        
                    }
                }
                else
                {
                    if (DisplayPlaying!=null)
                    {
                        DisplayPlaying();                        
                    }
                }


                if (IsSolved()) { State = GameState.SOLVED; }
                
                return true;
            }


            return false;
        }

        /// <summary>
        /// Tests if the game is solved
        /// </summary>
        /// <returns>True if the game is in final state, false otherwise</returns>
        private bool IsSolved()
        {
            //throw new NotImplementedException("This method is not implemented yet.");
            int hodnota = 0;
            int zhoda = 0;
            if (State == GameState.PLAYING)
            {
                for (int row = 0; row < RowCount; row++)
                {
                    for (int column = 0; column < ColumnCount; column++)
                    {
                        Tile t = Tiles[row, column];
                        hodnota++;
                        if (t is ValueTile)
                        {
                            if (((ValueTile)t).Value == hodnota)
                            {
                                zhoda++;
                            }
                        }
                    }
                }
                if (hodnota - 1 == zhoda)
                {
                    return true;
                }
            }

            return false;
        }

        static void Main(string[] args)
        {
            Field field = new Field(4, 4);

            field.State = GameState.PLAYING;
            bool isSolved = field.IsSolved();
            field.Generate();
            IUserInterface userInterface = new ConsoleUI(field);
            userInterface.UpdateUI();
            userInterface.StartNewGame();

            //field.MoveTile(3, 3);
            //userInterface.UpdateUI();

            //Field field = new Field(4, 4);
            //field.State = GameState.PLAYING;
            //bool isSolved = field.IsSolved();
            //field.Generate();
            //IUserInterface userInterface = new ConsoleUI(field);
            //userInterface.UpdateUI();
            //Console.WriteLine("Is solved before generation: " + isSolved);
            //Console.WriteLine("Is solved after generation: " + field.IsSolved());
            
            
            //Console.WriteLine("Is solved before generation: " + isSolved);
            //Console.WriteLine("Is solved after generation: " + field.IsSolved());

            Console.ReadLine();
        }       
    }
}
