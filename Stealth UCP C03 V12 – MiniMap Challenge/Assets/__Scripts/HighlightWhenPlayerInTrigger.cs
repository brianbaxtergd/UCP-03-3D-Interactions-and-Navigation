using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NOTE: This only works with the AESampleShaders/CustomLightingToon shader!
/// </summary>
[RequireComponent( typeof(PlayerInteractable) )]
public class HighlightWhenPlayerInTrigger : MonoBehaviour {
    public GameObject gameObjectToHighlight;
    public float highlightSecondsPerPulse = 1;

    PlayerInteractable playerInteractable;
    Material mat;
    Color highlightColor;
    float outlineWidth;
    float startTime = -1;

	bool inited = false;

	// Use this for initialization
	void Start () {
        playerInteractable = GetComponent<PlayerInteractable>();
        // We don't need to check whether playerInteractable is null because of the RequireComponent above.

        if (gameObjectToHighlight == null)
        {
            Debug.LogWarning("HighlightWhenPlayerInTrigger:Start() - No gameObjectToHighlight set.");
			return;
        }
        else
        {
            mat = gameObjectToHighlight.GetComponent<Renderer>().material;
            highlightColor = mat.GetColor("_BorderColor");
            outlineWidth = mat.GetFloat("_Border");

            mat.SetFloat("_OutlineFront", 0f);
			mat.SetFloat("_Border", 0f);
        }
		inited = true;
    }

	// Update is called once per frame
	void Update () {
        if (!inited)
        {
            return;
        }

        if (playerInteractable.playerWithinTrigger)
		{
            mat.SetFloat("_OutlineFront", 1f);
			mat.SetColor("_BorderColor", highlightColor);

            if (startTime == -1f)
            {
                startTime = Time.time;
            }
			mat.SetFloat("_Border", outlineWidth * (0.5f - Mathf.Cos(Mathf.PI * 
                     (Time.time-startTime)/ highlightSecondsPerPulse) * 0.5f));
        }
        else
        {
			mat.SetFloat("_OutlineFront", 0f);
            mat.SetFloat("_Border", 0f);
            startTime = -1;
        }
	}
}
