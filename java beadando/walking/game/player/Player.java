package walking.game.player;
import walking.game.util.Direction;

public class Player{
    //fields
    private int score = 0;
    public int getScore()
    {
        return score;
    }
    protected Direction direction = Direction.UP;
    public Direction getDirection()
    {
        return this.direction;
    }
    //constructor
    public Player()
    {
        this.score = 0;
        this.direction = Direction.UP;
    }

    //methods
    public void addToScore(int score)
    {
        this.score += score;
    }
    public void turn()
    {
        switch(this.direction){
            case UP:
                this.direction = Direction.RIGHT;
                break;
            case RIGHT:
                this.direction = Direction.DOWN;
                break;
            case DOWN:
                this.direction = Direction.LEFT;
                break;
            case LEFT:
                this.direction = Direction.UP;
                break;
        }
    }
}