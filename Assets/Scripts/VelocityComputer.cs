using UnityEngine;


public class VelocityComputer : MonoBehaviour
{
    public Vector3 velocity { get; private set; }
    private Vector3 _lastPosition;
    public bool controlRotation = false;
    public float velocityThreshold = 0.1f;

    void Start()
    {
        _lastPosition = GetPosition();
    }

    void Update()
    {
        Vector3 currentPosition = GetPosition();
        velocity = (currentPosition - _lastPosition) / Time.deltaTime;
        _lastPosition = currentPosition;

        if (controlRotation)
        {
            Rotate();
        }
    }

    private Vector3 GetPosition()
    {
        Vector3 position = transform.position;
        position.z = 0;
        return position;
    }



    private void Rotate()
    {
        if (velocity.magnitude > velocityThreshold)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }
}