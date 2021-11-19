using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPositionChange : MonoBehaviour
{
    protected float previousValue = 0f;
    public void UpdateX(float on)
    {
        if (on == 1.0 && previousValue != on) {
            Vector3 oldPos = transform.localPosition;
            transform.localPosition = new Vector3(oldPos.x + 3, oldPos.y + 2, oldPos.z);
        }
        previousValue = on;
    }
}
