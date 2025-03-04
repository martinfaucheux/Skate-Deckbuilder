using System.Linq;
using UnityEngine;

public class CardSlot : MonoBehaviour
{
    [HideInInspector] public int index = 0;
    [HideInInspector] public bool isDragging = false;
    [HideInInspector] public bool isHovering = false;
    public SlotContainer currentContainer;

    public Card _card;
    public Card card {
        get { return _card; }
        set {
            _card = value;

            if (_card != null) {
                _card.transform.SetParent(transform);
                _card.transform.localPosition = Vector3.zero;

                card.actionContainer.startTransform.localPosition = new Vector3(-1.5f, card.cardDefinition.groundStartY, 0);
                card.actionContainer.endTransform.localPosition = new Vector3(1.5f, card.cardDefinition.groundEndY, 0);

                card.currentSlot = this;
            }
        }
    }

    private void Awake() {
        currentContainer = GetComponentInParent<SlotContainer>();
    }

    public void SetSlotContext(int index, int cardCount)
    {
        this.index = index;
        transform.localPosition = new Vector3((index * 3) - ((cardCount - 1) * 3 / 2), transform.localPosition.y, transform.localPosition.z);
    }

    private void Update()
    {
        if (!currentContainer.allowCardSlotYOffset && !isDragging) {
            transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        }
    }

    #region DragAndHover
    public void OnMouseDown() {
        GetComponentInParent<SlotContainer>().BeginDrag(this);
        isDragging = true;
    }

    public void OnMouseDrag() {
        transform.position = GetMouseWorldPosition();

        SlotContainer hoveredSlotContainer = FindObjectsByType<SlotContainer>(FindObjectsSortMode.None).ToList().Find((slotContainer) => slotContainer.isHovering);
        if (hoveredSlotContainer != null && hoveredSlotContainer.name != currentContainer.name) {
            currentContainer.SwapSlotContainer(hoveredSlotContainer);
        }
    }

    public void OnMouseUp() {
        GetComponentInParent<SlotContainer>().EndDrag(this);
        isDragging = false;
    }

    public void OnMouseOver()
    {
        isHovering = true;

        if (currentContainer.forceCardBigHeight)
            return;

        card.cardVisual.SetHeight(CardVisual.Height.Big);
    }

    public void OnMouseExit()
    {
        isHovering = false;

        if (currentContainer.forceCardBigHeight)
            return;

        card.cardVisual.SetHeight(CardVisual.Height.Small);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(mousePos).z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
#endregion
}
