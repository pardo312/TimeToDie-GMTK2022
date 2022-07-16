using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void LookAt2D(Transform target, Vector3 direction)
    {
        //Calculate Look At 2D
        Vector3 dir = direction - target.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        target.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
