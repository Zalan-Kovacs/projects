package walking.game.player;

public class MadlyRotatingBuccaneer extends Player {
    //fields
    private int turnCount = 0;

    //constructor
    public MadlyRotatingBuccaneer()
    {
        super();
    }

    //method
    public void turn()
    {
        switch(turnCount % 4){
            case 0:
                break;
            case 1:
                super.turn();
                break;
            case 2:
                super.turn();
                super.turn();
                break;
            case 3:
                super.turn();
                super.turn();
                super.turn();
                break;
        }
        turnCount++;
    }
}