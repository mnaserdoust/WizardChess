using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{

    
    private Animator anim;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TargetEnemy")
        {
            
           // Debug.Log("Anim");
           // anim = other.GetComponent<Animator>();
            //anim.SetTrigger("dying");
        }
    }
}
