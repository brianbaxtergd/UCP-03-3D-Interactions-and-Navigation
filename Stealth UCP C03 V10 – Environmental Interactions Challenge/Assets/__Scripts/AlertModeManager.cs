#define DEBUG_AlertModeManager_ConsoleLogAlertModeChange

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertModeManager : MonoBehaviour
{
    static private AlertModeManager S;

    public delegate void AlertModeStatusChangeDelegate(bool alertMode);

    // This delegate sends out a notification any time the ALERT_MODE changes
    //  It will be used by everything in the environment that needs to react to
    //  Alert mode being enabled.
    static public AlertModeStatusChangeDelegate alertModeStatusChangeDelegate;
    static private bool _ALERT_MODE = false;
    static public bool ALERT_MODE
    {
        get
        {
            return _ALERT_MODE;
        }
        private set
        {
            bool announce = (value != _ALERT_MODE);
            _ALERT_MODE = value;
            if (announce && alertModeStatusChangeDelegate != null)
            {
                alertModeStatusChangeDelegate(_ALERT_MODE);
            }
        }
    }

    [Tooltip("Check this to turn the Alert on/off manually (next Update).")]
    public bool testAlertTrigger = false;

    // Use this for initialization
    void Awake()
    {
        S = this;

#if DEBUG_AlertModeManager_ConsoleLogAlertModeChange
        // This could have been part of the ALERT_MODE setter property, but I
        //  wanted to have an example of adding a method to a delegate.
        alertModeStatusChangeDelegate += LogToConsole;
#endif

        // Note that this assignment to false should NOT be expected to call the
        //  delegate because the initial value of _ALERT_MODE is also false.
        ALERT_MODE = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ALERT_MODE && testAlertTrigger)
        {
            ALERT_MODE = true;
        }
        if (ALERT_MODE && !testAlertTrigger)
        {
            ALERT_MODE = false;
        }
    }

#if DEBUG_AlertModeManager_ConsoleLogAlertModeChange
    private void LogToConsole(bool tf)
    {
        Debug.Log("ALERT_MODE changed to " + tf);
    }
#endif

    /// <summary>
    /// This is the method to call to turn on Alert mode, and it should be used
    /// by the ACS-17 Enemy, the SecurityCamera, and the SecurityGate
    /// </summary>
    /// <param name="newAlertMode">If set to <c>true</c> new alert mode.</param>
    public static void SwitchToAlertMode(bool newAlertMode = true)
    {
        if (S == null)
        {
            Debug.LogError("AlertModeManager:SwitchToAlertMode() - Method called " +
                           "with no AlertModeManager in Scene!");
            return;
        }

        if (ALERT_MODE != newAlertMode)
        {
            S.testAlertTrigger = newAlertMode;
            ALERT_MODE = newAlertMode;
        }
    }


}