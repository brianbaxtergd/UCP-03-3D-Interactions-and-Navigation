using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAction_TriggerAlarm : PlayerAction
{

    public override void Action()
    {
        AlertModeManager.SwitchToAlertMode();
    }

}
