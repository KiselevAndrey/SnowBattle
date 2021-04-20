using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [Header("Музыка")]
    [SerializeField] ClipListSO clipList;
    [SerializeField, Range(0f, 1f)] float normalVolume;

    [Header("UI elements")]
    //[SerializeField] Toggle masterToggle;
    //[SerializeField] Toggle musicToggle;
    //[SerializeField] Toggle effectsToggle;
    [SerializeField] Slider masterVolSlider;
    [SerializeField] Slider musicVolSlider;
    [SerializeField] Slider effectsVolSlider;

    [Header("Snapshots")]
    [SerializeField] AudioMixerSnapshot normal;
    [SerializeField] AudioMixerSnapshot pause;
    [SerializeField] AudioMixerSnapshot menu;

    [Header("Данные доп")]
    [SerializeField] AudioMixerGroup mixer;
    [SerializeField] MusicStatSO musicStats;
    [SerializeField] AudioSource audioSource;

    #region Awake Start
    private void Awake()
    {
        ToSnapshot(normal, 1f);
        Singleton();
    }

    void Singleton()
    {
        MusicManager[] musics = FindObjectsOfType<MusicManager>();
        for (int i = 0; i < musics.Length; i++)
        {
            if (musics[i].gameObject != gameObject)
            {
                Destroy(gameObject);
                break;
            }
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if(musicStats)
            LoadMusicOptions();

        clipList.Shuffle();
        NextMusic();
    }
    #endregion

    #region NextClip
    public void NextMusic() => NextMusic(clipList.NextClip(), normalVolume);

    public void NextMusic(AudioClip clip, float volume = 1f)
    {
        audioSource.clip = clip;
        audioSource.volume = Mathf.Lerp(0f, 1f, volume);
        audioSource.Play();
        StartCoroutine(NextAudioClip(clip));
    }

    IEnumerator NextAudioClip(AudioClip clip)
    {
        float time = clip.length;
        yield return new WaitForSeconds(time);
        if (audioSource.clip.name == clip.name)
        {
            NextMusic();
        }
    }
    #endregion

    #region Изменение настроек звука
    void LoadMusicOptions()
    {
        // установка тоглов музыки и эффектов
        //LoadToUI(masterToggle, musicStats.master, MixerGroup.MasterVolume);
        //LoadToUI(musicToggle, musicStats.music, MixerGroup.MusicVolume);
        //LoadToUI(effectsToggle, musicStats.effects, MixerGroup.EffectsVolume);

        // установка параметров slider
        LoadToUI(masterVolSlider, musicStats.master.volume, MixerGroup.MasterVolume);
        LoadToUI(musicVolSlider, musicStats.music.volume, MixerGroup.MusicVolume);
        LoadToUI(effectsVolSlider, musicStats.effects.volume, MixerGroup.EffectsVolume);
    }

    #region Toggle
    /// <summary>
    /// установка значения на сам UI element и выстановление параметров в mixerGroup
    /// </summary>
    void LoadToUI(Toggle toggle, MusicOptionSO musicOption, string mixerGroup)
    {
        toggle.isOn = musicOption.play;    // кнопка самого toggle
        SetFloatMixer(mixerGroup, musicOption, musicOption.play);
    }

    /// <summary>
    /// Сохраниение нового значения и установка его в mixerGroup
    /// </summary>
    void NewValue(string mixerGroup, MusicOptionSO musicOption, bool newValue)
    {
        musicOption.play = newValue;
        SetFloatMixer(mixerGroup, musicOption, newValue);
    }

    /// <summary>
    /// Вкл/выкл звука нужного mixerGroup
    /// </summary>
    void SetFloatMixer(string mixerGroup, MusicOptionSO musicOption, bool value)
    {
        mixer.audioMixer.SetFloat(mixerGroup, value ? SaveFloatToVolume(musicOption.volume) : -80);
    }
    #endregion

    #region Sliders
    /// <summary>
    /// установка значения на сам UI element и выстановление параметров в mixerGroup
    /// </summary>
    void LoadToUI(Slider slider, float value, string mixerGroup)
    {
        slider.value = value;    // кнопка самого toggle
        SetFloatMixer(mixerGroup, value);
    }

    /// <summary>
    /// Сохраниение нового значения и установка его в mixerGroup
    /// </summary>
    void NewValue(string mixerGroup, ref float oldValue, float newValue)
    {
        SetFloatMixer(mixerGroup, newValue);
        oldValue = newValue;
    }

    /// <summary>
    /// Вкл/выкл звука нужного mixerGroup
    /// </summary>
    void SetFloatMixer(string mixerGroup, float value) => mixer.audioMixer.SetFloat(mixerGroup, SaveFloatToVolume(value));

    float SaveFloatToVolume(float value) => Mathf.Lerp(-80, 0, value);
    #endregion

    #region UIActions
    public void SwitchMaster(bool value) => NewValue(MixerGroup.MasterVolume, musicStats.master, value);
    public void SwitchMusic(bool value) => NewValue(MixerGroup.MusicVolume, musicStats.music, value);
    public void SwitchEffects(bool value) => NewValue(MixerGroup.EffectsVolume, musicStats.effects, value);

    public void VolumeMaster(float value) => NewValue(MixerGroup.MasterVolume, ref musicStats.master.volume, value);
    public void VolumeMusic(float value) => NewValue(MixerGroup.MusicVolume, ref musicStats.music.volume, value);
    public void VolumeEffects(float value) => NewValue(MixerGroup.EffectsVolume, ref musicStats.effects.volume, value);
    #endregion

    #region Выбор Snapshots
    public void ToMenuSnapshot() => ToSnapshot(menu);
    public void ToNormalSnapshot() => ToSnapshot(normal);
    public void ToPauseSnapshot() => ToSnapshot(pause);
    public void ToSnapshot(AudioMixerSnapshot snapshot, float timeTransition = 1f)
    {
        if (!snapshot) return;

        snapshot.TransitionTo(timeTransition);
    }
    #endregion

    #endregion
}

public static class MixerGroup
{
    public static string MusicVolume = "Volume Music";
    public static string MasterVolume = "Volume Master";
    public static string EffectsVolume = "Volume Effects";
}
