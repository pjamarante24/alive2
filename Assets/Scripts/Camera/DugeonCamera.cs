using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DugeonCamera : MonoBehaviour
{
    public GameObject target;
    public float damping = 1;
    Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.transform.position;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.transform.position + offset;
        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
        transform.position = desiredPosition;

        transform.LookAt(target.transform.position);
    }
}
