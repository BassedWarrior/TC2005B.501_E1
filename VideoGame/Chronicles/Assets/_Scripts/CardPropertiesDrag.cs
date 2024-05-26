using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPropertiesDrag : MonoBehaviour
{
    public Transform originalParent;
    public Transform actualParent;
    public CardCreator card;
    public Image artworkImage;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public bool isDrag;

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

    private void Start()
    {
        originalParent = GameObject.FindGameObjectWithTag("DragParent").transform;
        isDrag = false;
        if(card != null)
        {
            AssignInfo();
        }
    }
}
