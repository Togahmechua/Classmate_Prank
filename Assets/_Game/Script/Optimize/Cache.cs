using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache 
{
    private static Dictionary<Collider2D, Hand> hand = new Dictionary<Collider2D, Hand>();

    public static Hand GetHand(Collider2D collider)
    {
        if (!hand.ContainsKey(collider))
        {
            hand.Add(collider, collider.GetComponent<Hand>());
        }

        return hand[collider];
    }
}
