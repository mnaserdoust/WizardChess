using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawns : Chessman
{
    public AudioSource audioSource;
    private Animator anim;
    public AudioSource dying;
    public AudioSource running;
    void Start()
    {
       audioSource = GetComponent<AudioSource>();
    }

    public void dyingSound()
    {
        dying.Play();
    }

    public void runningSound()
    {
        running.Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        anim = GetComponent<Animator>();
       
        if (other.tag == "TargetEnemy")
        {
           
            anim.SetTrigger("attack");
            audioSource.Play();
        }
        anim = GetComponent<Animator>();
        
        if (GetComponent<Animator>().tag== "TargetEnemy")
        {
            anim.SetTrigger("dying");
           
        }
    }

    public override bool[,] possibleMove()
    {

        bool[,] r = new bool[8, 8];
        Chessman c, c2;

        //white team move
        if(isWhite)
        {
            //diagnoal left
            if(CurrentX!=0 && CurrentZ !=7)
            {
                c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentZ + 1];
                if (c != null && !c.isWhite)
                    r[CurrentX - 1, CurrentZ + 1] = true;
            }
            //diagnoal right
            if (CurrentX != 7 && CurrentZ != 7)
            {
                c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentZ + 1];
                if (c != null && !c.isWhite)
                    r[CurrentX + 1, CurrentZ + 1] = true;
            }

            //middle
            if(CurrentZ!=7)
            {
                c = BoardManager.Instance.Chessmans[CurrentX, CurrentZ + 1];
                if (c == null)
                    r[CurrentX, CurrentZ + 1] = true;
            }

            //Second row on first move
            if(CurrentZ==1)
            {
                c = BoardManager.Instance.Chessmans[CurrentX, CurrentZ + 1];
                c2 = BoardManager.Instance.Chessmans[CurrentX, CurrentZ + 2];
                if (c == null && c2 == null)
                    r[CurrentX, CurrentZ + 2] = true;

            }
        }

        else
        {
            //diagnoal left
            if (CurrentX != 0 && CurrentZ != 0)
            {
                c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentZ - 1];
                if (c != null && c.isWhite)
                    r[CurrentX - 1, CurrentZ - 1] = true;
            }
            //diagnoal right
            if (CurrentX != 7 && CurrentZ != 0)
            {
                c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentZ - 1];
                if (c != null && c.isWhite)
                    r[CurrentX + 1, CurrentZ - 1] = true;
            }

            //middle
            if (CurrentZ != 0)
            {
                c = BoardManager.Instance.Chessmans[CurrentX, CurrentZ - 1];
                if (c == null)
                    r[CurrentX, CurrentZ - 1] = true;
            }

            //Second row on first move
            if (CurrentZ == 6)
            {
                c = BoardManager.Instance.Chessmans[CurrentX, CurrentZ - 1];
                c2 = BoardManager.Instance.Chessmans[CurrentX, CurrentZ - 2];
                if (c == null && c2 == null)
                    r[CurrentX, CurrentZ - 2] = true;

            }
        }

        return r;
    }
}
