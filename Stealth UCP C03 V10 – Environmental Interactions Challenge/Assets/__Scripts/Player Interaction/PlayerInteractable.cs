using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script allows the player to interact with objects in the environment
///  when she is within their trigger collider.
/// <para>We do not require a Collider component here because I wanted it to
/// be easily extensible to things like the LightCone that don't have an
/// attached Collider.</para>
/// </summary>

public class PlayerInteractable : MonoBehaviour {
    // This script recognizes the player from the InteractingPlayer Component.
    [Tooltip("If set to None, this will trigger when the player enters the trigger.")]
    public KeyCode      triggeringKey = KeyCode.None;

    List<PlayerAction>  playerActions = new List<PlayerAction>();

    [SerializeField]
    protected bool _playerWithinTrigger;
    public bool playerWithinTrigger
    {
        get
        {
            // Only return true if there are actions to perform.
            return (playerActions.Count > 0) ? _playerWithinTrigger : false;
        }
        protected set
        {
            _playerWithinTrigger = value;
        }
    }


    protected virtual void Awake()
    {
        Collider coll = GetComponent<Collider>();
        if (coll != null && !coll.isTrigger) {
            Debug.LogWarning("PlayerInteractable:Awake – If there is a Collider on " +
                             "this GameObject, it must be set to isTrigger! " +
                             "Setting to isTrigger now.");
            coll.isTrigger = true;
        }
    }


    protected void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InteractingPlayer>() != null)
        {
            playerWithinTrigger = true;
        }
    }


    protected void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractingPlayer>() != null)
        {
            playerWithinTrigger = false;
        }
    }


    protected virtual void Update()
    {
        if (playerWithinTrigger)
        {
            if (triggeringKey == KeyCode.None)
            {
                ExecuteActions();
            }
            else if (Input.GetKeyDown(triggeringKey))
            {
                // Execute actions!
                ExecuteActions();
            }
        }
    }


    protected void ExecuteActions()
    {
        // Because IPAs might be removed from playerActions as part of this for loop,
        //  we will work through them backwards from the end of the List.
        PlayerAction ipa;
        for (int i=playerActions.Count-1; i>=0; i--)
        {
            ipa = playerActions[i];
            ipa.Act();
            if (ipa.timesLeftToActivate == 0)
            {
                playerActions.Remove(ipa);
            }
        }
    }


    public void Register(PlayerAction ipa) // Don't read too much into it. I don't even like beer. ;)
    {
        if (!playerActions.Contains(ipa))
        {
            playerActions.Add(ipa);
        }
    }

}
