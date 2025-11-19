using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : Planet
{
    // ÚG”»’è
    protected override void OnTriggerEnter(Collider other)
    {
        // Å‰‚É“–‚½‚Á‚½‚Ì‚ªCometPocket‚¾‚Á‚½‚çˆ—
        if (other.gameObject.tag == "CometPocket")
        {

        }
    }

    protected override void Ability(Collision collision)
    {
       
    }
}