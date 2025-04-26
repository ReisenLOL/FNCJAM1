using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private TextMeshProUGUI volumePercentage;
    public void AdjustVolume()
    {
        mixer.SetFloat("Volume", volumeSlider.value);
        int volumePercentText = (int)(volumeSlider.value + 80);
        volumePercentage.text = volumePercentText.ToString() + "%";
    }
}
