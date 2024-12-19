using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource; 

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource.clip != null)
        {
            audioSource.time = 28f; 
            audioSource.Play(); 
        }
    }
}
