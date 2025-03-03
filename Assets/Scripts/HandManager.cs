using UnityEngine;

public class HandManager : Singleton<HandManager>
{
    public Transform cardsHolder;
    public float cardSpacing;

    public int startHandCount = 5;
    public GameObject cardPrefab;

    [Range(0, 1)]
    public float cardSize = 0.5f;

    void Start()
    {
        float offset = -(startHandCount - 1) * cardSpacing / 2;
        for (int cardIdx = 0; cardIdx < startHandCount; cardIdx++)
        {
            Vector3 cardPosition = transform.position + new Vector3(offset + cardIdx * cardSpacing, 0, 0);
            GameObject card = Instantiate(cardPrefab, cardPosition, Quaternion.identity, cardsHolder);
            card.transform.localScale = cardSize * Vector3.one;
        }
    }

}
