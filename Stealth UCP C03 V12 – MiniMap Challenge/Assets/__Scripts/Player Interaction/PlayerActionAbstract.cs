using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class exists solely as the superclass of PlayerAction so that 
/// PlayerAction.Act() can be sealed (i.e., not able to be overridden
/// by any subclasses of PlayerAction.
/// </summary>
public abstract class PlayerActionAbstract : MonoBehaviour
{
    // This is a method of ensuring that PlayerAction.Act() will be called by any of its
    //  subclasses. Act() is declared abstract here and override final in PlayerAction
    //  so that any subclass of PlayerAction is not allowed to override Act further.
    // In PlayerAction, Act() calls Action(), which MUST be overridden by subclasses of
    //  PlayerAction.
    public abstract void Act();
    // public abstract void Action(); // This is declared in PlayerAction.
}
