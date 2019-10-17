using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LightCone))]
public class PlayerInteractable_LightCone : PlayerInteractable
{
    private bool    canSeePlayer = false;

    LightCone       cone;

    // Use this for initialization
    void Start()
    {
        cone = GetComponentInChildren<LightCone>();
    }


    void FixedUpdate()
    {
        RaycastHit[] hits = cone.GetRaycastHits();
        canSeePlayer = false;
        if (hits != null)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider == null)
                {
                    continue;
                }
                // The other possibility for the following line would be to 
                //  GetComponentInParent<InteractingPlayer> on the collider, but 
                //  GetComponent is an inefficient call, so this should be faster.
                if (hits[i].collider.transform.root.gameObject ==
                    InteractingPlayer.S.gameObject)
                {
                    // This cone has seen the Player!
                    canSeePlayer = true;
                    break;
                }
            }
        }

        if (canSeePlayer != playerWithinTrigger)
        {
            playerWithinTrigger = canSeePlayer;
        }
    }
}
