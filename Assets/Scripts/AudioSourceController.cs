﻿using System.Collections;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    IEnumerator Start()
    {
        // Wait for the audio clip to finish playing, then delete the game object
        if(GetComponent<AudioSource>().clip!=null)
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        Destroy(this.gameObject);
    }
}
