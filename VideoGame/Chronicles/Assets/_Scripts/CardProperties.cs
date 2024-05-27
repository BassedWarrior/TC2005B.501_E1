using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardProperties : MonoBehaviour
{
   public CardData card;
    public Image artworkImage;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public bool withMask = false;
    public bool inclusiveType;

    public void AssignInfo()
    {
        if (card == null)
        {
            Debug.LogWarning("CardData is not assigned.");
            return;
        }

        Sprite loadedSprite = Resources.Load<Sprite>("Sprite/Artwork" + card.cardID.ToString());
        if (loadedSprite != null)
        {
            artworkImage.sprite = loadedSprite;
        }

        energyText.text = card.cost.ToString();
        healthText.text = card.health.ToString();
        attackText.text = card.attack.ToString();
    }

    public void SetCardData(CardData newCard)
    {
        card = newCard;
        AssignInfo();
    }
}
