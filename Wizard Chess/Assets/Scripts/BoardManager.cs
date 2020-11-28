using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }
    private bool [,] allowedMoves { set; get; }

    public Chessman[,]Chessmans { set; get; }
    public Chessman selectedChessman;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = .5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessmanPrefabs;
    public List<GameObject> ActiveChessman;

    private Quaternion orientation = Quaternion.Euler(0, 180, 0);

    public bool isWhiteTurn = true;
    
    private NavMeshAgent naveAgent;
    public Animator anim;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private bool isBlackAI = false;
    private bool isWhiteAI = false;

    private bool isStartofgame = true;
    int max = 1390;
    int min = -1390;

    private bool foundAMove;
    //For AI
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        spawnAllChessman();
        
    } 

    // Update is called once per frame
    void Update()
    {
        DrawChessBoard();
        UpdateSelection();
        
        if(Input.GetMouseButtonDown(0))
        {
            
            if (selectionX >= 0 && selectionY >= 0 && isMoving == false)
            {
                if(selectedChessman == null)
                {
                    
                    //selet chessman
                    selectChessman(selectionX, selectionY);
                }
                else
                {
                    //movechessman
                    moveChessman(selectionX, selectionY);
                    //selectedChessman = null;
                }
            }
        }
        setToIdle();
        if(isStartofgame)
        {
            AI();
            isStartofgame = false;
        }

    }

    //for selecting the clicked chessman
    private bool selectChessman(int x, int z)
    {
        if (Chessmans[x, z] == null) return false;
        // Correct turn
        if (Chessmans[x, z].isWhite != isWhiteTurn) return false;

        bool hasAtLeastOneMove = false;
        allowedMoves = Chessmans[x, z].possibleMove();
        for(int i=0;i<8;i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if(allowedMoves[i,j])
                {
                    hasAtLeastOneMove = true;
                }
            }
        }

        if (!hasAtLeastOneMove)
            return false;
        selectedChessman = Chessmans[x, z];
        
        BoardHighlights.Instance.highlightAllowedMoves(allowedMoves);
        return true;
    }

    // for moving the selected chessamn to the new position
    public void moveChessman(int x, int z)
    {
        if(allowedMoves[x,z])
        {
            Chessman c = Chessmans[x, z];
            if(c!=null&& c.isWhite !=isWhiteTurn)
            {
                
                c.gameObject.tag = "TargetEnemy";
                //capture
                if (c.GetType()== typeof(King))
                {
                    //endgame
                    endGame();
                    return;
                }
                //anim = selectedChessman.GetComponent<Animator>();
                //anim.SetBool("attack", true);

                //anim.SetBool("isDying", true);
                   // System.Threading.Thread.Sleep(2000);
                ActiveChessman.Remove(c.gameObject);
                Destroy(c.gameObject,5f);
            }

            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentZ] = null;
            //selectedChessman.transform.position = gettileCenter(x, z);
            //Chessmans[x, z] = selectedChessman;
            naveAgent = selectedChessman.GetComponent<NavMeshAgent>();
            targetPosition = gettileCenter(x, z);
            naveAgent.destination = targetPosition;
            selectedChessman.setposition(x, z);
            Chessmans[x, z] = selectedChessman;
            
            anim = selectedChessman.GetComponent<Animator>();
            if (isWhiteTurn == false)
            {

                Chessman[,] temp = new Chessman[8, 8];


                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        temp[i, j] = Chessmans[i, j];
                    }
               // MinMax Bm = new MinMax(Chessmans);

                //Debug.Log(Bm.MinMaxi(1));
            }
            anim.SetBool("isMoving", true);
            isWhiteTurn = !isWhiteTurn;
            

            //foundAMove = true;
            isMoving = true;
            
        }

        BoardHighlights.Instance.hideHighlights();
        
        if (isMoving==false)
            selectedChessman = null;
        
    }
    private void SpawnBlackChessman(int index, int x, int z)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], gettileCenter(x,z), orientation) as GameObject;
        go.transform.SetParent(transform);
        Chessmans[x, z] = go.GetComponent<Chessman>();
        Chessmans[x, z].setposition(x, z);
        ActiveChessman.Add(go);
    }

    private void SpawnWhiteChessman(int index, int x, int z)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], gettileCenter(x, z), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        Chessmans[x, z] = go.GetComponent<Chessman>();
        Chessmans[x, z].setposition(x, z);
        ActiveChessman.Add(go);
    }
    private void UpdateSelection()
    {
        if (!Camera.main) return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;

        }
        else
        {
            selectionY = -1;
            selectionX = -1;
        }
            
    }
    private void DrawChessBoard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;


        for(int i =0; i<=8;i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        //Draw the Selection
        if(selectionX >=0 && selectionY >= 0)
        {
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX, Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));
            Debug.DrawLine(Vector3.forward * (selectionY +1)+ Vector3.right * selectionX, Vector3.forward * selectionY  + Vector3.right * (selectionX + 1));
        }
    }

    //Helper function to set the exact position of each chessaman
    private Vector3 gettileCenter(int x, int z)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * z) + TILE_OFFSET;

        return origin;

    }

    //Draw all the chessamn at the bginnign of the game;
    private void spawnAllChessman()
    {
        ActiveChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];
        //White team
        //king
        SpawnWhiteChessman(0, 3, 0);
        //queen
        SpawnWhiteChessman(1, 4, 0);
        //rooks
        SpawnWhiteChessman(2, 0, 0);
        SpawnWhiteChessman(2, 7, 0);
        //bishop
        SpawnWhiteChessman(3, 2, 0);
        SpawnWhiteChessman(3, 5, 0);
        //horse
        SpawnWhiteChessman(4, 1, 0);
        SpawnWhiteChessman(4, 6, 0);
        //pawns
        for(int i=0;i<8;i++)
        {
            SpawnWhiteChessman(5, i, 1);
        }

        //Black team
        //king
        SpawnBlackChessman(6, 4, 7);
        //queen
        SpawnBlackChessman(7, 3, 7);
        //rooks
        SpawnBlackChessman(8, 0, 7);
        SpawnBlackChessman(8, 7, 7);
        //bishop
        SpawnBlackChessman(9, 2, 7);
        SpawnBlackChessman(9, 5, 7);
        //horse
        SpawnBlackChessman(10,1, 7);
        SpawnBlackChessman(10,6, 7);
        //pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnBlackChessman(11, i, 6);
        }
    }

    //make the GO to idle
    private void setToIdle()
    {
        // 
        if (selectedChessman != null && isMoving == true)
        {
            
            if (naveAgent != null)
            {
                
                if (naveAgent.remainingDistance <= TILE_OFFSET )
                {
                    
                    anim.SetBool("isMoving", false);
                    selectedChessman = null;
                    isMoving = false;
                    AI();
                    
                }
            }
        }
    }


    private void endGame()
    {
        if(isWhiteTurn)
        {
            //white team wins
        }
        else
        {
            //black team wins
        }

        foreach(GameObject go in ActiveChessman)
        {
            Destroy(go);
        }

        isWhiteTurn = true;
        BoardHighlights.Instance.hideHighlights();
        spawnAllChessman();
    }

    private void AI()
    {
       
        System.Random random = new System.Random();
        bool correctSelection = false ;
        if (!isWhiteTurn)
        {
            if (isBlackAI == true)
            {
                
                while (!correctSelection)
                {
                    int SX = random.Next(8);
                    int SY = random.Next(8);
            
                    selectChessman(SX, SY);
                    if (selectedChessman != null)
                    {
                        if (!selectedChessman.isWhite)
                        {
                            foundAMove = false;
                            while (!foundAMove)
                            {
                                int newSX = random.Next(8);
                                int newSY = random.Next(8);
                                //selectChessman(newSX, newSY);
                                if (allowedMoves[newSX, newSY])
                                {
                                    
                                    
                                    moveChessman(newSX, newSY);
                                    
                                    foundAMove = true;
                                }
                                
                            }
                            correctSelection = true;
                        }
                    }
                }
            }
        }

        else if(isWhiteTurn)
        {
           
            if (isWhiteAI == true)
            {
                
                while (!correctSelection)
                {
                    
                    int SX = random.Next(8);
                    int SY = random.Next(8);

                    selectChessman(SX, SY);
                    if (selectedChessman != null)
                    {
                        
                        if (selectedChessman.isWhite)
                        {
                            
                            foundAMove = false;
                            while (!foundAMove)
                            {
                                //Debug.Log("6");
                                int newSX = random.Next(8);
                                int newSY = random.Next(8);
                                //selectChessman(newSX, newSY);
                                if (allowedMoves[newSX, newSY])
                                {
                                    //Debug.Log("7");
                                    Debug.Log(newSX + "ww" + newSY);
                                    moveChessman(newSX, newSY);
                                    foundAMove = true;
                                }
                                //Debug.Log("wwwwwwwwww");
                            }
                            correctSelection = true;
                        }
                    }
                }
            }
        }
    }


    public void CC()
    {
        isBlackAI = true;
        isWhiteAI = true;
        AI();
    }

    public  void HCE()
    {
        isBlackAI = true;
        AI();
    }

    public void HCD()
    {
        isBlackAI = true;
        AI();
    }

    public void HH()
    {
        isBlackAI = false;
        isBlackAI = false;
    }
    public void extGame()
    {

    }


   

    
}

