package walking.game;
import walking.game.player.Player;
import walking.game.player.MadlyRotatingBuccaneer;


public class WalkingBoardWithPlayers extends WalkingBoard 
{
    //fields
    private Player[] players;
    private int round = 0;
    public static final int SCORE_EACH_STEP = 13;

    //constructors
    public WalkingBoardWithPlayers(int[][] board, int playerCount)
    {
        super(board);
        initPlayers(playerCount);
    }
    public WalkingBoardWithPlayers(int size, int playerCount)
    {
        super(size);
        initPlayers(playerCount);
    }

    //methods
    private void initPlayers(int playerCount)
    {
        this.players = new Player[playerCount];
        if(playerCount < 2)
        {
            throw new IllegalArgumentException();
        }
        else
        {
            players[0] = new MadlyRotatingBuccaneer();
            for(int i = 1; i < players.length; i++)
            {
                players[i] = new Player();
            }
        }
    }
    public int[] walk(int... stepCounts)
    {
        int[] playerScores = new int[players.length];
        int steps = 1;
        
        for(int i = 0; i < stepCounts.length; i++)
        {   
            int scoreThisTurn = 0;
            Player current = players[i % players.length];
            current.turn();
            for(int j = 0; j < stepCounts[i]; j++)
            {
                int valueThisTurn = 0;
                if(steps > SCORE_EACH_STEP)
                {
                    valueThisTurn = SCORE_EACH_STEP;
                }
                else
                {
                    valueThisTurn = steps;
                }
                scoreThisTurn = moveAndSet(current.getDirection(), valueThisTurn);
                current.addToScore(scoreThisTurn);

                steps++;
            }
            playerScores[i % players.length] = current.getScore();
        }
        return playerScores;
    }
}
