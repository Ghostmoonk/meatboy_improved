using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    //Ceci ne marche pas car le Controller Character PUDUKU
    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.tag == "Player")
    //     {
    //         Debug.Log("Collision");
    //         if (other.gameObject.GetComponent<MeatBoy>().GetLastWallJumped() != this)
    //         {
    //             other.gameObject.GetComponent<MeatBoy>().AddJumpWallCount(1);
    //             other.gameObject.GetComponent<MeatBoy>().SetLastWallJumped(this);
    //         }
    //     }
    // }

}
