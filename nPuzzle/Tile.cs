namespace Puzzle.Core
{
    /// <summary>
    /// Tile of game field
    /// </summary>
    public abstract class Tile
    {
        /// <summary>
        /// Tile's row index
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Tile's column index
        /// </summary>
        public int Col { get; set; }
    }

    /// <summary>
    /// Tile with value on it.
    /// </summary>
    public class ValueTile : Tile
    {
        /// <summary>
        /// Value of this Tile.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Creates the value tile
        /// </summary>
        /// <param name="value">value of the tile</param>
        public ValueTile(int value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Empty tile
    /// </summary>
    public class GreyTile : Tile
    {

    }
}
