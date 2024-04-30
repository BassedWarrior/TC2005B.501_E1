using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPropertiesDrag : MonoBehaviour
{
    public bool isDrag;
    public Transform originalParent;
    public Transform actualParent;
    public SpriteRenderer spriteRenderer;
    public CardCreator card;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image artworkImage;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public int cardIndex;

    public void AssignInfo()
    {
        if (card != null)
        {
            spriteRenderer.sprite= card.artwork;
            nameText.text = card.name;
            descriptionText.text = card.description;
            artworkImage.sprite = card.artwork;
            energyText.text = card.energyCost.ToString();
            healthText.text = card.health.ToString();
            attackText.text = card.attack.ToString();
        }
    }
    private void Start()
    {
        originalParent = GameObject.FindGameObjectWithTag("DragParent").transform;
        isDrag = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        AssignInfo();

    }
}
