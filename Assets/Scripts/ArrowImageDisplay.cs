using UnityEngine;
using UnityEngine.UI;

public class ArrowImageDisplay : MonoBehaviour
{
    public Image upImage;
    public Image downImage;
    public Image leftImage;
    public Image rightImage;

    public void Start()
    {
        CardTypeConfiguration cfg = CardTypeConfiguration.i;
        upImage.color = cfg.blueColor;
        rightImage.color = cfg.redColor;
        downImage.color = cfg.yellowColor;
        leftImage.color = cfg.greenColor;
    }
}