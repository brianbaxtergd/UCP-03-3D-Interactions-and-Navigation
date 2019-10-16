using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ACS17AnimationManager : MonoBehaviour
{
    // Read public mode and turnDir information from EnemyNav on parent GameObject
    // Try using anim.CrossFade("ACS_Walk", 0);

    [Header("Inscribed")]
    public float animTransTime = 0f;

    private bool inited = false;
    private Animator anim;
    private EnemyNav eNav;
    private string currAnimState = "";

    private void Start()
    {
        anim = GetComponent<Animator>();

        eNav = transform.parent.GetComponent<EnemyNav>();
        if (eNav == null)
        {
            Debug.LogError("ACS17AnimationManager:Start() - Can't find EnemyNav component on self or parent.");
            return;
        }

        inited = true;
    }

    private void Update()
    {
        // If something went wrong during initialization, don't try to animate.
        if (!inited) return;

        // Animate based on the EnemyNav eMode.
        switch (eNav.mode)
        {
            case EnemyNav.eMode.idle:
            case EnemyNav.eMode.wait:
                CrossFade("ACS_Idle");
                break;

            case EnemyNav.eMode.preMoveRot:
            case EnemyNav.eMode.postMoveRot:
                if (eNav.turnDir == -1)
                    CrossFade("ACS_TurnLeft");
                else
                    CrossFade("ACS_TurnRight");
                break;

            case EnemyNav.eMode.move:
                CrossFade("ACS_Walk");
                break;
            default:
                break;
        }
    }

    void CrossFade(string newState)
    {
        if (newState != currAnimState)
        {
            anim.CrossFade(newState, animTransTime);
            currAnimState = newState;
        }
    }
}
