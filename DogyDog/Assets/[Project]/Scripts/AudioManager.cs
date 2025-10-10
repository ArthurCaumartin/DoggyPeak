using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _backProba = 0.2f;

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
        if (Random.value < _backProba)
            PlaySound(BarkClip);
    }
}
