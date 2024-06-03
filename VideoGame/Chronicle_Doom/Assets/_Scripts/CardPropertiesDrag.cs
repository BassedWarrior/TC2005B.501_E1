using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPropertiesDrag : MonoBehaviour
{
    public CardData card;
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Canvas mainCanvas;
    public Transform originalParent;
    public Transform actualParent;
    public Image artworkImage;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;
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
            if (card.IsAlive() && card != null)
            {
                if (card.damage > 0 && !card.isDamaged)
                {
                    ShowFloatingText(transform.position, card.damage, true, true);
                }
            }
        }
    }

    public void ShowFloatingText(Vector3 worldPosition, int text, bool isDamage, bool isPreview)
    {
        GameObject floatingTextInstance = Instantiate(floatingTextPrefab, mainCanvas.transform);
        CreateFloatingText floatingText = floatingTextInstance.GetComponent<CreateFloatingText>();

        if (floatingText != null)
        {
            floatingText.Initialize(text, worldPosition, isDamage, isPreview);
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
