using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class CardVisual : MonoBehaviour
{
    public SpriteRenderer cardSpriteRenderer;
    public SpriteRenderer maskSpriteRenderer;
    public SpriteRenderer backgroundSpriteRenderer;
    public Transform infoTop;
    public Transform infoBottom;
    public SortingGroup sortingGroup;
    public TextMeshPro energyCostText;
    public TextMeshPro scoreCostText;

    private void Awake()
    {
        SetHeight(Height.Small, true);
    }

    public enum Height
    {
        Small,
        Big
    }

    public Height height = Height.Big;
    public void SetHeight(Height height, bool instant = false)
    {
        if (this.height == height)
        {
            return;
        }

        this.height = height;

        if (instant)
        {
            infoTop.transform.localPosition = new Vector3(infoTop.transform.localPosition.x, ((height == Height.Small ? 4 : 9) / 2) - 0.85f);
            infoBottom.transform.localPosition = new Vector3(infoBottom.transform.localPosition.x, -(((height == Height.Small ? 4 : 9) / 2) - 0.85f));
            cardSpriteRenderer.size = new Vector2(cardSpriteRenderer.size.x, height == Height.Small ? 4 : 9);
            maskSpriteRenderer.size = new Vector2(maskSpriteRenderer.size.x, height == Height.Small ? 4 : 9);
            return;
        }

        float val = cardSpriteRenderer.size.y;
        DOTween.Kill(gameObject);
        DOTween.To(() => val, x => val = x, height == Height.Small ? 4 : 9, 0.2f).SetEase(Ease.InOutSine).OnUpdate(() =>
        {
            infoTop.transform.localPosition = new Vector3(infoTop.transform.localPosition.x, (val / 2) - 0.85f);
            infoBottom.transform.localPosition = new Vector3(infoBottom.transform.localPosition.x, -((val / 2) - 0.85f));
            cardSpriteRenderer.size = new Vector2(cardSpriteRenderer.size.x, val);
            maskSpriteRenderer.size = new Vector2(maskSpriteRenderer.size.x, val);
        });
    }

    public void Set(Card card)
    {
        target = card;
        if (backgroundSpriteRenderer)
        {
            transform.SetParent(GameObject.Find("CardVisuals").transform, false);
            backgroundSpriteRenderer.sprite = card.cardDefinition.sprite;
        }

        SetCostText();
        SetScoreText();
    }

    private void SetCostText()
    {
        int cost = target.cardDefinition.energyCost;
        if (cost > 0) {
            energyCostText.text = $"-{cost} <sprite=0>";
        }
        else {
            energyCostText.text = $"0 <sprite=0>";
        }
    }

    private void SetScoreText()
    {
        int gain = target.cardDefinition.energyGain;

        scoreCostText.text = "";
        if (target.cardDefinition.score > 0) {
            scoreCostText.text = $"+{target.cardDefinition.score} <sprite=1>";
        }
        if (gain > 0) {
            string space = target.cardDefinition.score > 0 ? " " : "";
            scoreCostText.text += $"{space}+{gain} <sprite=0>";
        }
    }

    private Card target;

    private Vector3 movementDelta;
    private Vector3 rotationDelta;
    // private float scaleDelta = 0;

    // private int savedIndex;
    // private float autoTiltAmount = 20;
    // private float manualTiltAmount = 10;
    // private float tiltSpeed = 15;

    private void Update()
    {
        if (!target || !target.currentSlot)
        {
            return;
        }

        sortingGroup.sortingOrder = target.currentSlot.isDragging ? 100 : target.currentSlot.index;

        // Position
        transform.position = Vector3.Lerp(transform.position, target.transform.position, 15 * Time.deltaTime);

        // Rotation
        Vector3 movement = transform.position - target.transform.position;
        movementDelta = Vector3.Lerp(movementDelta, movement, 10 * Time.deltaTime);
        Vector3 movementRotation = (target.currentSlot.isDragging ? movementDelta : movement) * 20;
        rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, 10 * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(rotationDelta.x, -30, 30));

        // Scale
        // bool shouldScaleUp = target.currentSlot.isDragging || (target.currentSlot.isHovering && target.currentSlot.currentContainer.selectedSlot == null);
        // float scaleTarget = shouldScaleUp ? 1.2f : 1;
        // scaleDelta = Mathf.Lerp(scaleDelta, scaleTarget, 20 * Time.deltaTime);
        // float scaleDeltaClamped = Mathf.Clamp(scaleDelta, 0, 1.2f);
        // transform.localScale = new Vector3(transform.localScale.x < 0 ? -scaleDeltaClamped : scaleDeltaClamped, scaleDeltaClamped, 1);

        // if (shouldScaleUp && transform.GetSiblingIndex() != transform.parent.childCount - 1) {
        //     transform.SetAsLastSibling();
        // }

        // Tilt
        // savedIndex = target.currentSlot.isDragging ? savedIndex : target.currentSlot.index;
        // float sine = Mathf.Sin(Time.time + savedIndex) * (target.currentSlot.isHovering ? .2f : 1);
        // float cosine = Mathf.Cos(Time.time + savedIndex) * (target.currentSlot.isHovering ? .2f : 1);

        // Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // float tiltX = target.currentSlot.isHovering ? ((offset.y) * manualTiltAmount) : 0;
        // float tiltY = target.currentSlot.isHovering ? ((offset.x * -1) * manualTiltAmount) : 0;
        // float tiltZ = target.currentSlot.isDragging ? transform.eulerAngles.z : 0;

        // float lerpX = Mathf.LerpAngle(transform.eulerAngles.x, tiltX + (sine * autoTiltAmount), tiltSpeed * Time.deltaTime);
        // float lerpY = Mathf.LerpAngle(transform.eulerAngles.y, tiltY + (cosine * autoTiltAmount), tiltSpeed * Time.deltaTime);
        // float lerpZ = Mathf.LerpAngle(transform.eulerAngles.z, tiltZ, tiltSpeed / 2 * Time.deltaTime);

        // transform.eulerAngles = new Vector3(lerpX, lerpY, lerpZ);
    }

    public void AddInfoTop(Transform transform, bool keepExisting = false)
    {
        if (!keepExisting) {
            foreach (Transform child in infoTop) {
                Destroy(child.gameObject);
            }
        }

        transform.SetParent(infoTop, false);
        transform.localPosition = Vector2.zero;
    }

    public void AddInfoBottom(Transform transform, bool keepExisting = false)
    {
        if (!keepExisting) {
            foreach (Transform child in infoBottom) {
                Destroy(child.gameObject);
            }
        }

        transform.SetParent(infoBottom, false);
        transform.localPosition = Vector2.zero;
    }
}
