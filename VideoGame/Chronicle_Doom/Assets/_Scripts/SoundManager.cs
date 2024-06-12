using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Scrollbar masterSlider;
    public Scrollbar musicSlider;
    public Scrollbar sfxSlider;

    private void Start()
    {
        // Set the sliders to the current volume
        masterSlider.value = GameManager.Instance.GetVolume(GameManager.Instance.masterVolumeParameter);
        musicSlider.value = GameManager.Instance.GetVolume(GameManager.Instance.musicVolumeParameter);
        sfxSlider.value = GameManager.Instance.GetVolume(GameManager.Instance.sfxVolumeParameter);

        // Add listeners to the sliders
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // Set the volume by calling the GameManager's SetVolume method
    public void SetMasterVolume(float volume)
    {
        GameManager.Instance.SetMasterVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        GameManager.Instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        GameManager.Instance.SetSFXVolume(volume);
    }
}
