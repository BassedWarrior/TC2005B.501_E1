using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardProperties : MonoBehaviour
{
    public CardCreator card;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image artworkImage;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public int cardIndex;

    void Start()
    {
        AssignInfo();
    }
    public void AssignInfo()
    {
        if (card != null)
        {
            nameText.text = card.name;
            descriptionText.text = card.description;
            artworkImage.sprite = card.artwork;
            energyText.text = card.energyCost.ToString();
            healthText.text = card.health.ToString();
            attackText.text = card.attack.ToString();
        }
    }
}
