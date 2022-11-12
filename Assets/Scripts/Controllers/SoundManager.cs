using UnityEngine;
using DG.Tweening;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField, Range(0f, 1f)]

    private float menuVolume;

    [SerializeField]

    private AudioClip[] clips;

    private AudioSource audioSource;

    [SerializeField]

    private float fadeTime;

    private bool inGameOver;

    private void Awake()
    {
        Instance = this;
        
        if(clips.Length > 0)
        {
            GameEventManager.GameStart += OnGameStart;
            GameEventManager.GameOver += OnGameOver;

            audioSource = GetComponent<AudioSource>();
            audioSource.volume = menuVolume;
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.Play();
        }
    }

    public void OnGameStart()
    {
        StartCoroutine(GameStart());
    }

    public void OnGameOver()
    {
        StartCoroutine(GameOver());
    }

    IEnumerator GameStart()
    {
        if(inGameOver)
            yield return new WaitForSeconds(2f * fadeTime);
        audioSource.DOFade(1f, fadeTime);
    }

    IEnumerator GameOver()
    {
        inGameOver = true;
        audioSource.DOFade(0f, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        audioSource.Stop();
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
        audioSource.DOFade(menuVolume, fadeTime);
        inGameOver = false;
    }
}
