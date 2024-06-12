using UnityEngine;
using TMPro;

public class CreateFloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh; 
    private float duration = 2.0f;

    // isDamage = TRUE = DAMAGE, FALSE = HEAL
    // isPreview = TRUE = PREVIEW, FALSE = REAL DAMAGE/HEAL

    public void Initialize(int text, Vector3 worldPosition, bool isDamage, bool isPreview)
    {
        if (text == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            GameManager.Instance.textDots.Add(gameObject);
            transform.position = worldPosition;
            
            textMesh.color = isPreview ? Color.gray : (isDamage ? Color.red : Color.green);
            textMesh.text = (isDamage ? "-" : "+") + text.ToString();
            
            // Destroy(gameObject, duration);
        }
    }
}
