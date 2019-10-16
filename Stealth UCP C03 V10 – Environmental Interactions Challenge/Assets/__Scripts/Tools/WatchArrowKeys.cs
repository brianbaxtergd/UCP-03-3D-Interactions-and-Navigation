using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchArrowKeys : MonoBehaviour
{
    public bool up, down, left, right;

    private void Update()
    {
        up    = Input.GetKey(KeyCode.UpArrow)    || Input.GetKey(KeyCode.W);
        down  = Input.GetKey(KeyCode.DownArrow)  || Input.GetKey(KeyCode.S);
        left  = Input.GetKey(KeyCode.LeftArrow)  || Input.GetKey(KeyCode.A);
        right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

        Vector3 pos = transform.position + Vector3.up;
        if (up)    { Debug.DrawRay(pos, Vector3.forward, Color.cyan); }
        if (down)  { Debug.DrawRay(pos, Vector3.back, Color.cyan); }
        if (left)  { Debug.DrawRay(pos, Vector3.left, Color.cyan); }
        if (right) { Debug.DrawRay(pos, Vector3.right, Color.cyan); }
    }
}
