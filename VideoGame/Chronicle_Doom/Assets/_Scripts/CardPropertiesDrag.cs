using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CardPropertiesDrag : MonoBehaviour
{
    public CardData card;
    public List<CardAbility> cardAbilities;
    [SerializeField] private DamageText damageText;
    [SerializeField] private Canvas mainCanvas;
    public Transform originalParent;
    public Transform actualParent;
    public Image artworkImage;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;
    public bool isDrag;
    public bool isOnBoard;
    public bool isParadox;

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
        }
    }

    public void ShowDamageText(int text, bool isDamage, bool isPreview)
    {
        if (damageText != null)
        {
            damageText.Initialize(text, isDamage, isPreview);
        }
    }

    private void Start()
    {
        originalParent = GameObject.FindGameObjectWithTag("Deck").transform;
        actualParent = originalParent;
        isDrag = false;
        isParadox = card.category == "paradox";
        cardAbilities = new List<CardAbility>();

        switch (card.cardID)
        {
            case 16:
                cardAbilities.Add(CardAbilities.JollyRogerDamage);
                cardAbilities.Add(CardAbilities.JollyRogerHeal);
                break;
            case 17:
                cardAbilities.Add(CardAbilities.BloodChalisDamage);
                cardAbilities.Add(CardAbilities.BloodChalisBuff);
                break;
            case 18:
                cardAbilities.Add(CardAbilities.BlackPlague);
                break;
            case 19:
                cardAbilities.Add(CardAbilities.Abduction);
                break;
            case 20:
                cardAbilities.Add(CardAbilities.MillenialKnowledgeHeal);
                cardAbilities.Add(CardAbilities.MillenialKnowledgeBuff);
                break;
            case 21:
                cardAbilities.Add(CardAbilities.DinosaurMeteor);
                break;
            case 22:
                cardAbilities.Add(CardAbilities.TzarBomba);
                break;
            default:
                cardAbilities.Add(new CardAbility(0, 0, 0, new List<string>()));
                break;
        }

        if (card != null)
        {
            AssignInfo();
        }
    }
}
