//INUTIL COMO JULIAN

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject objectToFollow;
    public Button principalButton;
    public Button collectionButton;
    public Button highScoreButton;
    public float transitionDuration;
    private float initialCameraX;
    private bool transitioning = false;
    private float transitionTimer = 0.0f;
    private float targetXPosition;

    void Start()
    {
        initialCameraX = mainCamera.transform.position.x;
        principalButton.interactable = false;
    }

    void Update()
    {
        if (transitioning)
        {
            transitionTimer += Time.deltaTime;

            float t = Mathf.Clamp01(transitionTimer / transitionDuration);
            float newXPosition = Mathf.Lerp(initialCameraX, targetXPosition, t);

            Vector3 newPosition = new Vector3(newXPosition, mainCamera.transform.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = newPosition;

            if (t >= 1.0f)
            {
                initialCameraX= targetXPosition;
                transitioning = false;
            }
        }

        if (objectToFollow != null)
        {
            Vector3 newObjectPosition = new Vector3(mainCamera.transform.position.x, objectToFollow.transform.position.y, objectToFollow.transform.position.z);
            objectToFollow.transform.position = newObjectPosition;
        }
    }

    public void MoveToMenu(int menuPosition)
    {
        if (!transitioning)
        {
            switch(menuPosition)
            {
                case 0:
                    principalButton.interactable = false;
                    collectionButton.interactable = true;
                    highScoreButton.interactable = true;
                    break;
                case -20:
                    principalButton.interactable = true;
                    collectionButton.interactable = false;
                    highScoreButton.interactable = true;
                    break;
                case 20:
                    principalButton.interactable = true;
                    collectionButton.interactable = true;
                    highScoreButton.interactable= false;
                    break;
            }

            targetXPosition = menuPosition;
            transitionTimer = 0.0f;
            transitioning = true;
        }
    }
    public void PlayScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
