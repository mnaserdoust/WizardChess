using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Chessman
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

        if (GetComponent<Animator>().tag == "TargetEnemy")
        {
            anim.SetTrigger("dying");
        }
    }
    public override bool[,] possibleMove()
    {
        bool[,] r = new bool[8, 8];
        Chessman c;
        int i, j;

        //TopLeft
        i = CurrentX;
        j = CurrentZ;
        while (true)
        {
            i--;
            j++;
            if (i < 0 || j >= 8) break;
            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;

            else
            {
                if (c.isWhite != isWhite) r[i, j] = true;
                break;
            }

        }

        //TopRight
        i = CurrentX;
        j = CurrentZ;
        while (true)
        {
            i++;
            j++;
            if (i >= 8 || j >= 8) break;
            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;

            else
            {
                if (c.isWhite != isWhite) r[i, j] = true;
                break;
            }

        }

        //DownLeft
        i = CurrentX;
        j = CurrentZ;
        while (true)
        {
            i--;
            j--;
            if (i < 0 || j < 0) break;
            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;

            else
            {
                if (c.isWhite != isWhite) r[i, j] = true;
                break;
            }

        }

        //downRight

        i = CurrentX;
        j = CurrentZ;
        while (true)
        {
            i++;
            j--;
            if (j < 0 || i >= 8) break;
            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;

            else
            {
                if (c.isWhite != isWhite) r[i, j] = true;
                break;
            }

        }


        //ROOk
        //right
        i = CurrentX;
        while (true)
        {
            i++;
            if (i >= 8) break;
            c = BoardManager.Instance.Chessmans[i, CurrentZ];
            if (c == null)
                r[i, CurrentZ] = true;

            else
            {
                if (c.isWhite != isWhite) r[i, CurrentZ] = true;
                break;
            }

        }

        //left
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0) break;
            c = BoardManager.Instance.Chessmans[i, CurrentZ];
            if (c == null)
                r[i, CurrentZ] = true;

            else
            {
                if (c.isWhite != isWhite) r[i, CurrentZ] = true;
                break;
            }

        }

        //up
        i = CurrentZ;
        while (true)
        {
            i++;
            if (i >= 8) break;
            c = BoardManager.Instance.Chessmans[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;

            else
            {
                if (c.isWhite != isWhite) r[CurrentX, i] = true;
                break;
            }

        }

        //down
        i = CurrentZ;
        while (true)
        {
            i--;
            if (i < 0) break;
            c = BoardManager.Instance.Chessmans[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;

            else
            {
                if (c.isWhite != isWhite) r[CurrentX, i] = true;
                break;
            }

        }
        

        return r;
    }
}

