using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{

    public AudioClip[] BGM = new AudioClip[4];
    AudioSource audioSource;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine("Playlist");
    }

    IEnumerator Playlist()
    {
        audioSource.clip = BGM[index];
        audioSource.Play();
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            if(!audioSource.isPlaying)
            {
                index++;
                if (index >= 4)
                    index = 0;
                audioSource.clip = BGM[index];
                audioSource.Play();
            }
        }
    }
}
