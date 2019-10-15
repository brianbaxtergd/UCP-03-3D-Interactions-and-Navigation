#define DEBUG_ThirdPersonWallEdges_Raycasts
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThirdPersonWallCover))]
public class ThirdPersonWallEdges : MonoBehaviour {
    public float    camZoomWallEdgeDist = 1.5f;

    // Public get accessor properties for the four different edgeBools
    public bool     wallL { get{ return edgeBools[0]; } }
    public bool     wallR { get{ return edgeBools[1]; } }
    public bool     zoomL { get{ return edgeBools[2]; } }
    public bool     zoomR { get{ return edgeBools[3]; } }

    //Transform       wallLTrans, wallRTrans, zoomLTrans, zoomRTrans;
    private ThirdPersonWallCover tpwc;

    Transform[]     sensorTransforms;
    float[]         distances;
    bool[]          edgeBools = new bool[4];

	// Use this for initialization
	void Start () {
        tpwc = GetComponent<ThirdPersonWallCover>();
        // We don't need to check whether tpwc is null because of the RequireComponent compiler attribute above.

        // I'm putting both of these into arrays so that I can use a for loop throughout rather than repeat very similar code 4x
        sensorTransforms = new Transform[4];// { wallLTrans, wallRTrans, zoomLTrans, zoomRTrans };
        distances = new float[] { -tpwc.coverTriggerDist, tpwc.coverTriggerDist, -camZoomWallEdgeDist, camZoomWallEdgeDist };

        for (int i=0; i<sensorTransforms.Length; i++) {
			sensorTransforms[i] = new GameObject("WallEdgeSensor_"+i).transform;
            sensorTransforms[i].SetParent(transform);
            sensorTransforms[i].localPosition = new Vector3( distances[i], 1, 0 );
            edgeBools[i] = false; // Just to be sure, though they should have been initialized to false
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Check collisions with walls
		LayerMask coverMask = LayerMask.GetMask("Cover");
        Vector3 raycastDir = transform.forward;
        float raycastDist = tpwc.coverTriggerDist * 1.25f;
        RaycastHit hitInfo;
		Vector3 toSensorTransform;

        for (int i=0; i<sensorTransforms.Length; i++) {
            toSensorTransform = sensorTransforms[i].position - transform.position;
            // Check whether the sensorTransforms[i].position is inside a wall
            if (Physics.Raycast(transform.position, toSensorTransform, out hitInfo, toSensorTransform.magnitude, coverMask)) {
                // sensorTransforms[i] is inside a cover wall, which makes edgeBools[i] true (to keep the camera from zooming in that case)
                edgeBools[i] = true;
            } else {
                edgeBools[i] = Physics.Raycast(sensorTransforms[i].position, raycastDir, out hitInfo, raycastDist, coverMask);
            }
			#if DEBUG_ThirdPersonWallEdges_Raycasts
            Debug.DrawLine(sensorTransforms[i].position, sensorTransforms[i].position+raycastDir*raycastDist, edgeBools[i] ? Color.green : Color.red, 0, false);
			#endif
                
        }
        
	}


}
