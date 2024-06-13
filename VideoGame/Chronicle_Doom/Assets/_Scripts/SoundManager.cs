using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Scrollbar musicSlider;
    public Scrollbar sfxSlider;

    private void Start()
    {
        // Set the sliders to the current volume
        musicSlider.value = GameManager.Instance.GetVolume(GameManager.Instance.musicVolumeParameter);
        sfxSlider.value = GameManager.Instance.GetVolume(GameManager.Instance.sfxVolumeParameter);

        // Add listeners to the sliders
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // Set the volume by calling the GameManager's SetVolume method
    public void SetMusicVolume(float volume)
    {
        GameManager.Instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        GameManager.Instance.SetSFXVolume(volume);
    }
}