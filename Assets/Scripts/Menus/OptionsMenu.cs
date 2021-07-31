using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider audioSlider;
    public float masterVolume = 0;

    private void Start()
    {
        audioMixer.GetFloat("MasterVolume", out masterVolume);
        audioSlider.value = masterVolume;
    }

    public void UpdateMasterVolume()
    {
        audioMixer.SetFloat("MasterVolume", audioSlider.value);
    }

}
