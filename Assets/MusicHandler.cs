using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] sources = GetComponentsInChildren<AudioSource>();
        sources[Random.Range(0, sources.Length)].enabled = (true);
    }
}
