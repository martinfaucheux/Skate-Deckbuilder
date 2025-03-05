using UnityEngine;

[CreateAssetMenu(fileName = "CardDefinition", menuName = "CardDefinition", order = 0)]
public class CardDefinition : ScriptableObject
{
    public Sprite sprite;
    public float groundStartY;
    public float groundEndY;

    public ActionContainer actionContainerPrefab;
    [Tooltip("If the player doesn't have enough energy when entering the sequence, they will lose the round.")]
    public int energyCost = 0;
    [Tooltip("If the player completes the sequence, they will gain this much energy.")]
    public int energyGain = 0;
    public int score = 0;
}