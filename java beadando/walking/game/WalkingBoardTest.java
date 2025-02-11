package walking.game;
import walking.game.WalkingBoard;
import walking.game.util.Direction;

import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;
import static check.CheckThat.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;
import check.*;

import static check.CheckThat.Condition.*;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.DisabledIf;
import check.CheckThat;

public class WalkingBoardTest{
        
        
    @ParameterizedTest
    @CsvSource({"3", "4", "5", "6"})
    public void testSimpleInit(int size)
    {
        int[][] array = new int[size][size];
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                array[i][j] = WalkingBoard.BASE_TILE_SCORE;
            }
        }
        WalkingBoard walkingBoard = new WalkingBoard(size);
        assertNotEquals(array, walkingBoard.getTiles());
        assertArrayEquals(array, walkingBoard.getTiles());
        assertEquals(WalkingBoard.BASE_TILE_SCORE, walkingBoard.getTile(0,0));
        assertEquals(WalkingBoard.BASE_TILE_SCORE, walkingBoard.getTile(0,size-1));
        assertEquals(WalkingBoard.BASE_TILE_SCORE, walkingBoard.getTile(size-1,0));
        assertEquals(WalkingBoard.BASE_TILE_SCORE, walkingBoard.getTile(size-1,size-1));
    }
    
    @ParameterizedTest
    @CsvSource({"0, 2, 3","1, 2, 3"})
    public void testCustomInit(int x, int y, int expected)
    {
        int[][] array = new int[4][3];
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                array[i][j] = 2;
            }
        }
        WalkingBoard walkingboard = new WalkingBoard(array);
        assertEquals(expected, walkingboard.getTile(x,y));
        array[x][y] = 6;
        assertEquals(expected, walkingboard.getTile(x,y));
        int[][] testArray = walkingboard.getTiles();
        testArray[x][y] = 1;
        assertEquals(expected, walkingboard.getTile(x,y));
    }
    
    @Test
    public void testMoves()
    {
        WalkingBoard walkingboard = new WalkingBoard(3);

        walkingboard.moveAndSet(Direction.UP, 1);
        walkingboard.moveAndSet(Direction.RIGHT, 1);
        walkingboard.moveAndSet(Direction.RIGHT, 2);
        walkingboard.moveAndSet(Direction.RIGHT, 1);
        
        int[][] array = {
            {WalkingBoard.BASE_TILE_SCORE, 1, 2},
            {WalkingBoard.BASE_TILE_SCORE, WalkingBoard.BASE_TILE_SCORE, WalkingBoard.BASE_TILE_SCORE},
            {WalkingBoard.BASE_TILE_SCORE, WalkingBoard.BASE_TILE_SCORE, WalkingBoard.BASE_TILE_SCORE}
        };
        assertArrayEquals(array, walkingboard.getTiles());
    }
}