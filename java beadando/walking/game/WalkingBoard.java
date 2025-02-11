package walking.game;
import walking.game.util.Direction;


public class WalkingBoard 
{
    //fields
    private int[][] tiles;
    public int[][] getTiles()
    {
        int[][] tileValues = new int[tiles.length][];
        for(int i = 0; i < tiles.length; i++)
        {
            tileValues[i] = new int[tiles[i].length];
            for(int j = 0; j < tiles[i].length; j++)
            {
                tileValues[i][j] = tiles[i][j];
            }
        }
        return tileValues;
    }
    private int x = 0;
    private int y = 0;
    public static final int BASE_TILE_SCORE = 3;
    //constructors
    public WalkingBoard(int size)
    {
        this.tiles = new int[size][size];
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                tiles[i][j] = BASE_TILE_SCORE;
            }
        }
    }
    public WalkingBoard(int[][] tiles)
    {
        this.tiles = new int[tiles.length][];
        for(int i = 0; i < tiles.length; i++)
        {
            this.tiles[i] = new int[tiles[i].length];
            for(int j = 0; j < tiles[i].length; j++)
            {
                if(tiles[i][j] <= BASE_TILE_SCORE)
                {
                    this.tiles[i][j] = BASE_TILE_SCORE;
                }
                else
                {
                    this.tiles[i][j] = tiles[i][j];
                }
            }
        }
    }
    //methods
    public int[] getPosition()
    {
        int[] xy = {this.x,this.y};
        return xy;
    }
    public boolean isValidPosition(int x, int y)
    {
        if(x >= 0 && x < tiles.length && y >= 0 && y < tiles[x].length)
        {
            return true;
        }
        else {return false;}
    }
    public int getTile(int x, int y)
    {
        if(isValidPosition(x,y))
        {
            return tiles[x][y];
        }
        else
        {
            throw new IllegalArgumentException();
        }
    }
    public static int getXStep(Direction direction)
    {
        switch(direction){
            case UP:
                return -1;
            case RIGHT:
                return 0;
            case DOWN:
                return 1;
            case LEFT:
                return 0;
            default:
                throw new IllegalArgumentException();
        }
    }
    public static int getYStep(Direction direction)
    {
        switch(direction){
            case UP:
                return 0;
            case RIGHT:
                return 1;
            case DOWN:
                return 0;
            case LEFT:
                return -1;
            default:
                throw new IllegalArgumentException();
        }
    }
    public int moveAndSet(Direction direction, int value)
    {
        if(isValidPosition(x+getXStep(direction),y+getYStep(direction)))
        {
            int tile = tiles[x+getXStep(direction)][y+getYStep(direction)];
            tiles[x+getXStep(direction)][y+getYStep(direction)] = value;
            this.x = x + getXStep(direction);
            this.y = y + getYStep(direction);
            return tile;
        }
        else
        {
            return 0;
        }
    }
}