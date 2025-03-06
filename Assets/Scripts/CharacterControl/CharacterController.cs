using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Rigidbody2D _characterRigidbody;
    private CharacterState currentState = CharacterState.Idle;
    public Animator animator;
    private AudioManager audioManager => AudioManager.instance;

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

    public void SetState(CharacterState state)
    {
        CharacterState previousState = currentState;
        currentState = state;
        animator.SetBool("isSliding", currentState == CharacterState.Sliding);
        animator.SetBool("isAirborn", currentState == CharacterState.Airborn);

        if (currentState == previousState)
            return;

        if (currentState == CharacterState.Sliding)
        {
            audioManager.Play("Slide");
        }
        else
        {
            audioManager.Stop("Slide");

        }

        if (currentState == CharacterState.Grounded)
        {
            audioManager.Play("Click");
            audioManager.Play("Roll");
        }
        else
        {
            audioManager.Stop("Roll");
        }
    }
}