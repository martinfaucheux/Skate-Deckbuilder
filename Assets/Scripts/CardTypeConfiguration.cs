using UnityEngine;


public class CardTypeConfiguration : Singleton<CardTypeConfiguration>
{
    public KeyCode blueKey = KeyCode.UpArrow;
    public KeyCode redKey = KeyCode.RightArrow;
    public KeyCode yellowKey = KeyCode.DownArrow;
    public KeyCode greenKey = KeyCode.LeftArrow;

    public Color redColor = Color.red;
    public Color blueColor = Color.blue;
    public Color yellowColor = Color.yellow;
    public Color greenColor = Color.green;

    public Color TypeToColor(CardType type)
    {
        switch (type)
        {
            case CardType.Blue:
                return blueColor;
            case CardType.Red:
                return redColor;
            case CardType.Yellow:
                return yellowColor;
            case CardType.Green:
                return greenColor;
            default:
                throw new System.Exception("Invalid color type");
        }
    }

    public KeyCode TypeToKey(CardType type)
    {
        switch (type)
        {
            case CardType.Blue:
                return blueKey;
            case CardType.Red:
                return redKey;
            case CardType.Yellow:
                return yellowKey;
            case CardType.Green:
                return greenKey;
            default:
                throw new System.Exception("Invalid card type");
        }
    }

    public static CardType GetRandomType() => (CardType)Random.Range(0, 4);

}