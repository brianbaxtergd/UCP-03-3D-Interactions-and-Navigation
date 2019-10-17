using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction_DeactivateGameObject : PlayerAction {
    public GameObject gameObjectToDeactivate;
    
    public override void Action()
    {
        if (gameObjectToDeactivate != null)
        {
            gameObjectToDeactivate.SetActive(false);
        }
    }
}
