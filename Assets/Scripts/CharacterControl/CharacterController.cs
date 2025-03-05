using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Rigidbody2D _characterRigidbody;

    public void Move(PathData pathData)
    {
        _characterRigidbody.MovePosition(pathData.position);

        Vector3 tangent = pathData.tangent;
        _characterRigidbody.MoveRotation(Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg);
    }

    public void Move(Vector3 position)
    {
        _characterRigidbody.MovePosition(position);
    }
}