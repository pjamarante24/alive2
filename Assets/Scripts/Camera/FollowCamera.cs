using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    Vector3 offset;

    private void Start()
    {
        offset = target.transform.position - transform.position;
    }

    private void LateUpdate()
    {
        float desiredAngle = target.transform.eulerAngles.y;

        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        transform.position = target.transform.position - (rotation * offset);

        transform.LookAt(target.transform);
    }

}
