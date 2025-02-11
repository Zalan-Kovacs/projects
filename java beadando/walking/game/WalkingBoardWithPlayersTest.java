package walking.game;
import walking.game.WalkingBoardWithPlayers;
import walking.game.player.Player;
import walking.game.player.MadlyRotatingBuccaneer;

import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;
import static check.CheckThat.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;
import check.*;

public class WalkingBoardWithPlayersTest{
    @Test
    public void walk1(){
        WalkingBoardWithPlayers game1 = new WalkingBoardWithPlayers(3,2);
        int[] stepCounts1 = {1,1,1,1,1,1};
        int[] scoresGame1 = game1.walk(stepCounts1);
        int[][] finalBoard1 = game1.getTiles();
        int[] expectedPoints1 = {6, 9};
        int[][] expectedBoard1 = {
            {3, 2, 3},
            {6, 5, 4},
            {3, 3, 3}
        };
        assertArrayEquals(expectedPoints1, scoresGame1);
        assertArrayEquals(expectedBoard1, finalBoard1);
    }
    @Test
    public void walk2(){
        WalkingBoardWithPlayers game2 = new WalkingBoardWithPlayers(3,3);
        int[] stepCounts2 = {1,1,1,1,1,1,1,1,1};
        int[] scoresGame2 = game2.walk(stepCounts2);
        int[][] finalBoard2 = game2.getTiles();
        int[] expectedPoints2 = {3, 9, 6};
        int[][] expectedBoard2 = {
            {3, 2, 3},
            {3, 3, 5},
            {8, 7, 6}
        };
        assertArrayEquals(expectedPoints2, scoresGame2);
        assertArrayEquals(expectedBoard2, finalBoard2);
    }
}