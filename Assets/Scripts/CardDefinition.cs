using UnityEngine;

[CreateAssetMenu(fileName = "CardDefinition", menuName = "CardDefinition", order = 0)]
public class CardDefinition : ScriptableObject
{
    public Sprite sprite;
    public float groundStartY;
    public float groundEndY;

    public ActionContainer actionContainerPrefab;
}