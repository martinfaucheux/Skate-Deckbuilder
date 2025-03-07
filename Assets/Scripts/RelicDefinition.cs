using UnityEngine;

[CreateAssetMenu(fileName = "RelicDefinition", menuName = "RelicDefinition", order = 0)]
public class RelicDefinition : ScriptableObject
{
    public Sprite sprite;
    [TextArea] public string description;
}
