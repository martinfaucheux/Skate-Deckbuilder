using UnityEngine;
using UnityEngine.UI;

public class RideButton : MonoBehaviour
{
    public Button rideButton;

    private void Update()
    {
        rideButton.interactable = !SequenceManager.i.isPlaying && !RunManager.Instance.isRunOver && !BoardManager.i.CanAddCard();
    }

    public void OnClick_Ride()
    {
        RunManager.Instance.PlayHand();
    }
}
