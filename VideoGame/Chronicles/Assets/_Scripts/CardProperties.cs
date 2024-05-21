using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardProperties : MonoBehaviour
{
    public CardCreator card;
    public Image artworkImage;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public bool withMask = false;
    public bool inclusiveType;

    private void Start()
    {
        AssignInfo();
    }
    public void AssignInfo()
    {
        if (card != null)
        {
            artworkImage.sprite = card.artwork;
            energyText.text = card.energyCost.ToString();
            healthText.text = card.health.ToString();
            attackText.text = card.attack.ToString();
        }
    }
}
