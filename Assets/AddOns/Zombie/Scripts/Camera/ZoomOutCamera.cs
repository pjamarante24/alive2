using UnityEngine;

public class ZoomOutCamera : MonoBehaviour
{
    public float zoomSpeed = 1f;
    public float maxDistance = 10f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float distance = (transform.position - startPosition).magnitude;
        if (distance > maxDistance) zoomSpeed = 0;
        transform.Translate(Vector3.back * Time.deltaTime * zoomSpeed);
    }
}
