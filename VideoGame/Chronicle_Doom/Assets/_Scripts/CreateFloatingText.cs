using UnityEngine;
using TMPro;

public class CreateFloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh; 
    private float duration = 2.0f;

    //TRUE= DAMAGE, FALSE= HEAL
    public void Initialize(int text, Vector3 worldPosition, bool isDamage)
    {
        transform.position = worldPosition;
        if (isDamage)
        {
            textMesh.color = Color.red;
            textMesh.text = "-"+text.ToString();
        }
        else
        {
            textMesh.color = Color.green;
            textMesh.text = "+"+text.ToString();
        }
        Destroy(gameObject, duration);
    }
}
