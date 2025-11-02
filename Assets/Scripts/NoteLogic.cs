using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteLogic : MonoBehaviour
{
    public int maxPlays = 0;
    private int currentPlays = 0;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public bool CanPlay()
    {
        return currentPlays < maxPlays;
    }

    public void PlayNote()
    {
        if (CanPlay())
        {
            currentPlays++;
            if (audioSource != null)
                audioSource.Play();
        }
    }
}