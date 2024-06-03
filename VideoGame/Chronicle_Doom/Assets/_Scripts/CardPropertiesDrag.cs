using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPropertiesDrag : MonoBehaviour
{
    public Transform originalParent;
    public Transform actualParent;
    public CardData card;
    public Image artworkImage;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public bool isDrag;
    public bool isOnBoard;

    public void AssignInfo()
    {
        if (card != null)
        {
            Sprite loadedSprite = Resources.Load<Sprite>("Sprite/Artwork" + card.cardID.ToString());
            if (loadedSprite != null)
            {
                artworkImage.sprite = loadedSprite;
            }
            energyText.text = card.cost.ToString();
            attackText.text = card.attack.ToString();
            healthText.text = card.health.ToString();
            if (card.damage > 0)
            {
                damageText.text = "-" + card.damage.ToString();
                damageText.enabled = true;
            }
            else
            {
                damageText.enabled = false;
            }
        }
    }

    private void Start()
    {
        originalParent = GameObject.FindGameObjectWithTag("Deck").transform;
        actualParent = originalParent;
        isDrag = false;
        if(card != null)
        {
            AssignInfo();
        }
    }
}
