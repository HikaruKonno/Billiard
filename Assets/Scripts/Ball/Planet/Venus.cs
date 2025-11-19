using UnityEngine;

public class Venus : Planet
{
    protected override void Ability(Collision collision)
    {
        if(collision.gameObject.GetType() == typeof(Planet))
        {

        }
    }
}
