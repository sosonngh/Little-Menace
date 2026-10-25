using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic Instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.Play();
    }
}
