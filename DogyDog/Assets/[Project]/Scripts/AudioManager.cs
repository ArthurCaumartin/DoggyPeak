using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource _audioSource;
    private bool _isBarking;

    public AudioClip BlockSound;
    public AudioClip GrabSound;
    public AudioClip GoodBoySound;
    public AudioClip BarkClip;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void PlayBark()
    {
        _isBarking = !_isBarking;
        if (_isBarking)
            PlaySound(BarkClip);
    }
}
