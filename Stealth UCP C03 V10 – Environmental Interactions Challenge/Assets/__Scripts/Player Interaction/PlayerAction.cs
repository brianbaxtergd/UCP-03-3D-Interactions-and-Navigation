using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerInteractable))]
public abstract class PlayerAction : PlayerActionAbstract
{
    // Once timesLeftToActivate reaches 0, this IPlayerAction will be removed
    //  from the List on a PlayerInteractable.
    // If timesLeftToActivate is -1, this IPlayerAction will never be removed from the list.
    [SerializeField]
    [Tooltip("Number of times that this Interactable can be activated. If set to" +
             "-1, there is no limit to the number of activations.")]
    private int _timesLeftToActivate = -1;
    public int timesLeftToActivate
    {
        get { return _timesLeftToActivate; }
        private set { _timesLeftToActivate = value; }
    }

    void Awake()
    {
        PlayerInteractable playInt = GetComponent<PlayerInteractable>();
        playInt.Register(this);
    }

    /// <summary>
    /// This method is called by PlayerInteractable and must NOT be overridden 
    /// by subclasses of PlayerAction.
    /// </summary>
    public override sealed void Act()
    {
        if (timesLeftToActivate > 0)
        {
            timesLeftToActivate--;
        }

        Action();
    }

    /// <summary>
    /// This method must be overridden in subclasses of PlayerAction.
    /// </summary>
    public abstract void Action();
}

