using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMax 
{

    Chessman[,] bg;
    int max = 1390;
    int min = -1390;

    int bestMoveI;
    int bestMoveJ;
    int bestMovea;
    int bestMoveb;
    public Chessman selectedChessman;

    public MinMax(Chessman[,] temp)
    {
         Chessman[,] boardgame =  temp;
        bg = boardgame;
    }

    public int MinMaxi( int depth)
    {
        Debug.Log("Mimax Called");
        return maxLevel(bg, depth);

    }

    int maxLevel(Chessman[,] boardi, int depth)
    {
        Chessman[,] board = new Chessman[8, 8];
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                board[i, j] = boardi[i, j];
            }
        Debug.Log("Depth Max =" + depth);
        int value ;
        if (depth == 0 || isGameEnded(board))
            return weightCalculator(board);
        //board.getMoves();
        int best = weightCalculator(board);
        //int move;
        Chessman[,] nextBoard = new Chessman[8, 8];
        Debug.Log("Max Called");

        //board.selectChessman(0, 0);
        //Debug.Log(board.selectedChessman.possibleMove()[0,1].ToString());
        for (int i = 0; i < 8; i++)
        {
            
            for (int j = 0; j < 8; j++)
            {
                
                if (selectChessman(board, i, j))
                {

                    bool[,] temp = board[i,j].possibleMove();
                    for (int a = 0; a < 8; a++)
                    {
                        for (int b = 0; b < 8; b++)
                        {
                            board =bg;
                            if (temp[a, b])
                            {
                                for (int k = 0; k < 8; k++)
                                    for (int h = 0; h < 8; h++)
                                    {
                                        nextBoard[k, h] = null;
                                    }
                                //Debug.Log(weightCalculator(board));
                                nextBoard = moveChessman(board,i,j,a, b);
                                value = minLevel(nextBoard,depth-1);
                                
                                if (value > best)
                                {
                                    Debug.Log(temp[a, b].ToString() + i + " -" + j + " -" + a + " -" + b);
                                    best = value;
                                }
                                
                            }
                        }
                    }
                }

            }
        }
        return best;
    }

    int minLevel(Chessman[,] boardi, int depth)
    {
        Chessman[,] board = new Chessman[8, 8];
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                board[i, j] = boardi[i, j];
            }
        int value;
        Debug.Log("Depth Min ="+depth);
        if (depth == 0 || isGameEnded(board))
            return weightCalculator(board);
        //board.getMoves();
        int best = weightCalculator(board);
       

        Chessman[,] nextBoard = new Chessman[8,8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (selectChessman(board, i, j))
                {
                    bool[,] temp = board[i, j].possibleMove();

                    for (int a = 0; a < 8; a++)
                    {
                        for (int b = 0; b < 8; b++)
                        {
                            if (temp[a, b])
                            {
                                for (int k = 0; k < 8; k++)
                                    for (int h = 0; h < 8; h++)
                                    {
                                        nextBoard[k, h] = null;
                                    }
                                nextBoard = moveChessman(board, i, j, a, b);
                                value = maxLevel(nextBoard, depth - 1);
                                if (value < best)
                                {
                                    Debug.Log("min "+temp[a, b].ToString() + i + " -" + j + " -" + a + " -" + b);
                                    best = value;
                                }
                            }
                        }
                    }
                }


            }
        }

        return best;
    }

    private int weightCalculator(Chessman[,] tempChessmans)
    {


        int weight = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
               // Debug.Log(i + " a" + j);
                if (tempChessmans[i, j] != null)
                {
                    //Debug.Log(i + " a" + j);
                    if (tempChessmans[i, j].isWhite)
                    {
                        if (tempChessmans[i, j].GetType() == typeof(Pawns))
                        {
                            weight += 10;

                        }
                        else if (tempChessmans[i, j].GetType() == typeof(Rook))
                        {
                            weight += 50;

                        }
                        else if (tempChessmans[i, j].GetType() == typeof(Horse))
                        {
                            weight += 30;

                        }
                        else if (tempChessmans[i, j].GetType() == typeof(Bishop))
                        {
                            weight += 30;

                        }
                        else if (tempChessmans[i, j].GetType() == typeof(Queen))
                        {
                            weight += 90;

                        }
                        else if (tempChessmans[i, j].GetType() == typeof(King))
                        {
                            weight += 900;

                        }
                    }
                    else //if it is black
                    {
                        if (tempChessmans[i, j].GetType() == typeof(Pawns))
                        {
                            weight -= 10;

                        }
                        else if (tempChessmans[i, j].GetType() == typeof(Rook))
                        {
                            weight -= 50;

                        }
                        else if (tempChessmans[i, j].GetType() == typeof(Horse))
                        {
                            weight -= 30;

                        }
                        else if (tempChessmans[i, j].GetType() == typeof(Bishop))
                        {
                            weight -= 30;

                        }
                        else if (tempChessmans[i, j].GetType() == typeof(Queen))
                        {
                            weight -= 90;

                        }
                        else if (tempChessmans[i, j].GetType() == typeof(King))
                        {
                            weight -= 900;

                        }
                    }
                }
            }
        }
        return weight;
    }

    public bool isGameEnded(Chessman[,] board)
    {
        // If the isEnded value becomes true it means there is alt least one move
        bool isended =true;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (selectChessman(board, i, j))
                    isended = false;
            }
        }
        Debug.Log("Is ended" + (isended).ToString());
        return isended;
    }


    //for selecting the clicked chessman
    private bool selectChessman(Chessman[,] board, int x, int z)
    {
        bool[,] allowedMoves;
        if (board[x, z] == null) return false;
        // Correct turn
        if (!board[x, z].isWhite) return false;

        bool hasAtLeastOneMove = false;
        allowedMoves = board[x, z].possibleMove();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j])
                {
                    //Debug.Log("Has at least one move");
                    hasAtLeastOneMove = true;
                }
            }
        }

        if (!hasAtLeastOneMove)
            return false;
        
        return true;
    }

    // for moving the selected chessamn to the new position
    public Chessman[,] moveChessman(Chessman[,] board, int x, int z, int a, int b)
    {
        Chessman c = board[a, b];
        Chessman[,] temp = new Chessman[8,8];
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                temp[i, j] = board[i, j];
            }
        if (c != null)
        {
            //capture
            if (c.GetType() == typeof(King))
            {
                //endgame
                //endGame();
                return null;
            }
            temp[a, b] = null;
        }
        c = board[x, z];
        if (c != null)
        {
            //c.setposition(a, b);
            temp[a, b] = c;
            temp[x, z] = selectedChessman;
        }
        return temp;
    }
}
