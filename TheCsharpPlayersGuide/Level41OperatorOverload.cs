using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp1;

public record BlockCoordinate(int Row, int Column)
{
    public static BlockCoordinate operator +(BlockCoordinate coord ,BlockOffset offset)
        => new BlockCoordinate(coord.Row + offset.RowOffset, coord.Column + offset.ColumnOffset);
    public static BlockCoordinate operator + (BlockOffset offset, BlockCoordinate coord)
        => new BlockCoordinate(coord.Row + offset.RowOffset, coord.Column + offset.ColumnOffset);

    public static BlockCoordinate operator +(BlockCoordinate coord, Direction direction)
        => direction switch
        {
            Direction.East => new BlockCoordinate(coord.Row - 1, coord.Column),
            Direction.West => new BlockCoordinate(coord.Row + 1, coord.Column),
            Direction.North => new BlockCoordinate(coord.Row, coord.Column + 1),
            Direction.South => new BlockCoordinate(coord.Row, coord.Column - 1)
        };
    
    public int this[int index]
    {
        get => index == 0 ? Row : index == 1 ? Column : throw new ArgumentOutOfRangeException(nameof(index));
        // Could also use patterns here. index switch { 0 => Row, 1 => Column }
    }
}

public record BlockOffset(int RowOffset, int ColumnOffset)
{
    public static implicit operator BlockOffset(Direction direction)
        => direction switch
        {
            Direction.East => new BlockOffset(-1, 0),
            Direction.West => new BlockOffset(1, 0),
            Direction.North => new BlockOffset(0, 1),
            Direction.South => new BlockOffset(0, -1),
        };
}
public enum Direction { North, East, South, West }