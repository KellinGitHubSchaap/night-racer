using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    [SerializeField] private List<Transform> checkPoints = new List<Transform>();

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            checkPoints.Add(transform.GetChild(i));
        }
    }





    //// Draw all child gameobjects (aka the checkpoints) 
    //void OnDrawGizmos()
    //{
    //    if (transform.childCount > 0)
    //    {
    //        Vector3 offset = Vector3.up * 1.0f;     // Offset in Height of the visualization of the checkpoints
    //        Vector3 last = Vector3.zero;            // Last is the previous curr, it is need to draw a line between the points

    //        for (int i = 0; i < transform.childCount; i++)
    //        {
    //            Vector3 curr = transform.GetChild(i).position;      // Curr is the current Checkpoint 
    //            Gizmos.color = Color.yellow;
    //            Gizmos.DrawLine(curr, curr + offset * 2);

    //            Gizmos.color = Color.green;
    //            Gizmos.DrawLine(new Vector3(curr.x, curr.y + offset.y, curr.z - 2), new Vector3(curr.x, curr.y + offset.y, curr.z + 2));

    //            // Draw a line be the checkpoints last and current
    //            if (i > 0)
    //            {
    //                Gizmos.color = Color.blue;
    //                Gizmos.DrawLine(last + offset, curr + offset);
    //                Gizmos.color = Color.black;
    //                Gizmos.DrawLine(last, curr);
    //            }

    //            last = curr;
    //        }
    //    }
    //}

}
