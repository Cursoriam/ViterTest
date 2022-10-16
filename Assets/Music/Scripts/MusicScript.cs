using UnityEngine;
using UnityEngine.UI;

public class MusicScript : MonoBehaviour
{
    [SerializeField] private GameObject musicPanel;
    [SerializeField] private Text musicVolumeText;
    [SerializeField] private Slider musicVolumeSlider;
    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        var volume = _audioSource.volume = PlayerPrefs.GetFloat(Constants.MusicVolumeParamName,
            1.0f);
        musicVolumeText.text = (int)(volume * 100) + "%";
        musicPanel.SetActive(PlayerPrefs.GetInt(Constants.MusicPanelIsActiveParamName, 0) == 1);
        musicVolumeSlider.value = volume;
    }

    public void ChangeSound()
    {
        _audioSource.volume = musicVolumeSlider.value;
        musicVolumeText.text = (int)(_audioSource.volume * 100) + "%";
        SaveMusicParams();
    }

    public void ChangeMusicPanelActiveState()
    {
        if (!Utilities.IsPointerOverUIObject())
        {
            musicPanel.SetActive(!musicPanel.activeInHierarchy);
            SaveMusicParams();
        }
    }

    private void SaveMusicParams()
    {
        PlayerPrefs.SetFloat(Constants.MusicVolumeParamName, _audioSource.volume);
        PlayerPrefs.SetInt(Constants.MusicPanelIsActiveParamName, musicPanel.activeInHierarchy ? 1 : 0);
        PlayerPrefs.Save();
    }
}
