using Unity.VisualScripting;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, target.position.x, Time.deltaTime * 10),
            Mathf.Lerp(transform.position.y, target.position.y, Time.deltaTime * 10),
            0
        );
    }
}
