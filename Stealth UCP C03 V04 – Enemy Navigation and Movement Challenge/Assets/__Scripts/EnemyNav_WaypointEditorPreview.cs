using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

[RequireComponent(typeof(NavMeshAgent))]
[ExecuteInEditMode]
public class EnemyNav_WaypointEditorPreview : MonoBehaviour
{
    [Tooltip("Enable this script to see the waypoint path previewed in the Editor.")]
    public bool previewWaypointPath = false;
    GameObject previewGO;

    EnemyNav enemyNav;

    private void OnEnable()
    {
        if (!Application.isEditor || Application.isPlaying)
        {
            return;
        }

        EditorApplication.update += PreviewPath;

        CreatePreviewGO();
    }

    private void OnDisable()
    {
        if (!Application.isEditor || Application.isPlaying)
        {
            return;
        }

        EditorApplication.update -= PreviewPath;

        DestroyImmediate(previewGO);
    }

    bool doNotDraw
    {
        get
        {
            if (!Application.isEditor || Application.isPlaying)
                return true;

            if (enemyNav == null)
                enemyNav = GetComponent<EnemyNav>();

            if (previewGO == null)
                CreatePreviewGO();

            if (!previewWaypointPath)
                return true;

            return false;
        }
    }

    void CreatePreviewGO()
    {
        Transform pGOTrans = transform.Find("___Waypoint_Walker___");
        if (pGOTrans == null)
        {
            previewGO = new GameObject("___Waypoint_Walker___");
            previewGO.AddComponent<SphereCollider>();
        }
        else
        {
            previewGO = pGOTrans.gameObject;
        }

        previewGO.GetComponent<SphereCollider>().radius = 0.2f;
        previewGO.transform.SetParent(transform);
        previewGO.SetActive(false);
    }

    void PreviewPath()
    {
        if (doNotDraw)
            return;

        previewGO.SetActive(true);

        Vector3 p0, p1;
        float t = Time.realtimeSinceStartup % enemyNav.waypoints.Count;

        // Show movement along path.
        for (int i = 0; i < enemyNav.waypoints.Count; i++)
        {
            p0 = enemyNav.waypoints[i].pos + Vector3.up;
            p1 = Vector3.up + ((i < enemyNav.waypoints.Count - 1) ? enemyNav.waypoints[i + 1].pos : enemyNav.waypoints[0].pos);
            if (0 < (t - i) && (t - i) < 1)
            {
                t -= i;
                Vector3 p01 = (1 - t) * p0 + t * p1;
                previewGO.transform.position = p01;
                if (i < enemyNav.waypoints.Count - 1)
                {
                    previewGO.transform.LookAt(enemyNav.waypoints[i + 1].pos + Vector3.up);
                }
                else
                {
                    previewGO.transform.LookAt(enemyNav.waypoints[0].pos + Vector3.up);
                }
            }
        }
    }
}
