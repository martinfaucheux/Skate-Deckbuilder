using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Rigidbody2D _characterRigidbody;
    private CharacterState currentState = CharacterState.Grounded;
    public Animator animator;

    public void Move(PathData pathData)
    {
        _characterRigidbody.MovePosition(pathData.position);

        Vector3 tangent = pathData.tangent;
        _characterRigidbody.MoveRotation(Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg);
        SetState(pathData.characterState);
    }

    public void Move(Vector3 position)
    {
        _characterRigidbody.MovePosition(position);
    }

    private void SetState(CharacterState state)
    {
        currentState = state;
        animator.SetBool("isSliding", currentState == CharacterState.Sliding);
        animator.SetBool("isAirborn", currentState == CharacterState.Airborn);
    }
}