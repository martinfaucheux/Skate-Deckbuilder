using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject[] actionContainerPrefabs;
    public int cardCount = 5;

    [Range(0, 1)]
    public float cardSize = 0.5f;
    void Start()
    {

        List<Card> cards = new List<Card>();
        for (int cardIdx = 0; cardIdx < cardCount; cardIdx++)
        {
            GameObject cardGameObject = Instantiate(cardPrefab);
            Card card = cardGameObject.GetComponent<Card>();
            cards.Add(card);

            ActionContainer actionContainer = InstantiateRandomActionContainer();
            card.AssignActionContainer(actionContainer);
            cardGameObject.transform.localScale = cardSize * Vector3.one;
        }
        // HandManager.i.AddManyCards(cards);
    }

    public ActionContainer InstantiateRandomActionContainer()
    {
        GameObject prefab = actionContainerPrefabs[Random.Range(0, actionContainerPrefabs.Length)];
        return Instantiate(prefab).GetComponent<ActionContainer>();
    }
}
