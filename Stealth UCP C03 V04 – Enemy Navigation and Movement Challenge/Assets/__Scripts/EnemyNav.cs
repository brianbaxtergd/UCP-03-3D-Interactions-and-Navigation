using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNav : MonoBehaviour
{
    public enum eMode { idle, wait, preMoveRot, move, postMoveRot};

    [Header("Inscribed")]
    public bool drawGizmos;
    public List<Waypoint> waypoints;
    public float speed = 4f;
    public float angularSpeed = 90f;

    [Header("Dynamic")]
    [SerializeField]
    private eMode _mode = eMode.wait;
    public int wpNum = 0;
    public float pathTime;
    public float waitUntil;

    protected NavMeshAgent nav;

    public eMode mode
    {
        get
        {
            return _mode;
        }
        set
        {
            _mode = value;
        }
    }

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.stoppingDistance = 0.01f;

        pathTime = 0f;
        wpNum = 0;
        MoveToWaypoint(0);
    }

    void MoveToNextWaypoint()
    {
        int wpNum1 = wpNum + 1;
        if (wpNum1 >= waypoints.Count)
            wpNum1 = 0;

        MoveToWaypoint(wpNum1);
    }

    void MoveToWaypoint(int num)
    {
        wpNum = num;
        nav.SetDestination(waypoints[wpNum].pos);
        nav.isStopped = true;
        nav.updatePosition = false;

        mode = eMode.preMoveRot;
    }

    bool RotateTowards(Vector3 goalPos, float deg, bool rotYOnly = true)
    {
        Vector3 delta = goalPos - transform.position;
        if (rotYOnly)
            delta.y = 0;
        Quaternion r0 = transform.rotation;
        transform.LookAt(transform.position + delta);
        Quaternion r1 = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(r0, r1, deg);

        return (Quaternion.Angle(transform.rotation, r1) < 1);
    }

    private void FixedUpdate()
    {
        if (!Mathf.Approximately(nav.speed, speed) 
            || !Mathf.Approximately(nav.angularSpeed, angularSpeed))
        {
            nav.speed = speed;
            nav.angularSpeed = angularSpeed;
        }

        pathTime += Time.fixedDeltaTime;
        Vector3 preTurnRight = transform.right;

        switch (mode)
        {
            case eMode.idle:

                break;
            case eMode.wait:
                // Are we still waiting?
                if (pathTime < waitUntil)
                    break;
                // We've waited long enough, move on to the next Waypoint.
                MoveToNextWaypoint();
                break;
            case eMode.preMoveRot:
                Vector3 goalPos = waypoints[wpNum].pos;
                if (RotateTowards(goalPos, angularSpeed * Time.fixedDeltaTime))
                {
                    nav.isStopped = false;
                    nav.updatePosition = true;
                    mode = eMode.move;
                }
                break;
            case eMode.move:
                // Navigate towards the waypoint.
                if (!nav.pathPending && nav.remainingDistance <= nav.stoppingDistance)
                {
                    mode = eMode.postMoveRot;
                }
                break;
            case eMode.postMoveRot:
                if (RotateTowards(transform.position + waypoints[wpNum].fwd, angularSpeed * Time.fixedDeltaTime))
                {
                    waitUntil = pathTime + waypoints[wpNum].waitTime;
                    mode = eMode.wait;
                }
                break;
            default:
                break;
        }
    }

    const string gizmoIconPrefix = "GizmoIcon_128_";
    private void OnDrawGizmos()
    {
        if (!drawGizmos || !Application.isEditor || Application.isPlaying || waypoints.Count < 2)
        {
            return;
        }

        Vector3 p0, p1;
        Vector3 iconDrawLoc;
        List<Vector3> usedIconDrawLocs = new List<Vector3>();
        Gizmos.color = Color.red;
        Vector3 gizmoIconOverlapOffset = Camera.current.transform.right * 0.75f - Camera.current.transform.up * 0.25f;

        for (int i = 0; i < waypoints.Count; i++)
        {
            p0 = waypoints[i].pos + Vector3.up;
            p1 = Vector3.up + ((i < waypoints.Count - 1) ? waypoints[i + 1].pos : waypoints[0].pos);

            Gizmos.DrawLine(p0, p1);

            // Draw the number icons (up to 9).
            if (i < 10)
            {
                iconDrawLoc = p0 + Vector3.up;
                while (usedIconDrawLocs.Contains(iconDrawLoc))
                {
                    iconDrawLoc += gizmoIconOverlapOffset;
                }
                usedIconDrawLocs.Add(iconDrawLoc);
                Gizmos.DrawIcon(iconDrawLoc, gizmoIconPrefix + i, true);
            }
        }
    }
}
