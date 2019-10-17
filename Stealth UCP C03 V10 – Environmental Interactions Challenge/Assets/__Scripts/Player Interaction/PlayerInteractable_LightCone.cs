using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LightCone))]
public class PlayerInteractable_LightCone : PlayerInteractable
{
    private bool canSeePlayer = false;

    LightCone cone;

    private void Start()
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
                    continue;

                if (hits[i].collider.transform.root.gameObject == InteractingPlayer.S.gameObject)
                {
                    canSeePlayer = true;
                    break;
                }
            }
        }

        if (canSeePlayer != playerWithinTrigger)
            playerWithinTrigger = canSeePlayer;
    }
}
