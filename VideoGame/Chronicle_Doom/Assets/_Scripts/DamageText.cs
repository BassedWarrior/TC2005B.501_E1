using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh; 

    // isDamage = TRUE = DAMAGE, FALSE = HEAL
    // isPreview = TRUE = PREVIEW, FALSE = REAL DAMAGE/HEAL

    public void Initialize(int text, bool isDamage, bool isPreview)
    {
        Debug.Log($"Initializing damageText with value {text}");
        if (text == 0)
        {
            Debug.Log($"Deactivated gameObject because value {text} == 0");
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            
            textMesh.color = isPreview ? Color.gray : (isDamage ? Color.red : Color.green);
            textMesh.text = (isDamage ? "-" : "+") + text.ToString();
            Debug.Log($"Activated gameObject because value {text} != 0\n"
                      + $"New text: {textMesh.text}");
        }
    }
}
