using System.Linq;
using CoduckStudio;
using UnityEngine;

public class CardSlot : MonoBehaviour
{   
    [Header("References")]
    public SpriteRenderer emptySpriteRenderer;

    private bool _isEmpty = true;
    public bool isEmpty {
        get { return _isEmpty; }
        set {
            _isEmpty = value;

            card?.gameObject?.SetActive(!_isEmpty);
            emptySpriteRenderer.gameObject.SetActive(_isEmpty);
        }
    }

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

                isEmpty = false;
            }
            else {
                isEmpty = true;
            }
        }
    }

    [Header("Runtime")]
    public int index = 0;
    public bool isDragging = false;
    public bool isHovering = false;
    public SlotContainer currentContainer;

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

    private void OnDestroy()
    {
        if (card?.cardVisual?.gameObject) {
            Destroy(card.cardVisual.gameObject);
        }
    }

#region DragAndHover
    public bool isLocked = false;

    public void OnMouseDown() {
        if (isEmpty || isLocked) {
            return;
        }

        GetComponentInParent<SlotContainer>().BeginDrag(this);
        isDragging = true;
    }

    public void OnMouseDrag() {
        if (isEmpty || isLocked) {
            return;
        }

        transform.position = GetMouseWorldPosition();

        SlotContainer hoveredSlotContainer = FindObjectsByType<SlotContainer>(FindObjectsSortMode.None).ToList().Find((slotContainer) => slotContainer.isHovering);
        if (hoveredSlotContainer != null && hoveredSlotContainer.name != currentContainer.name) {
            currentContainer.SwapSlotContainer(hoveredSlotContainer);
        }

        GenericTooltip.Instance.Hide();
    }

    public void OnMouseUp() {
        if (isEmpty || isLocked) {
            return;
        }

        GetComponentInParent<SlotContainer>().EndDrag(this);
        isDragging = false;
    }

    public void OnMouseOver()
    {
        if (isEmpty || isHovering || SequenceManager.i.isPlaying) {
            return;
        }

        isHovering = true;

        GenericTooltip.Instance.Show(Tooltip.GetCardConfig(card.cardDefinition, card.actionContainer), card.gameObject);

        if (currentContainer.forceCardBigHeight && card.cardVisual.height == CardVisual.Height.Big)
            return;

        card.cardVisual.SetHeight(CardVisual.Height.Big);
    }

    public void OnMouseExit()
    {
        if (isEmpty) {
            return;
        }

        isHovering = false;

        GenericTooltip.Instance.Hide();

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
