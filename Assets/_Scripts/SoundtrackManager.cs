using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour {

    public AudioSource DefaultSoundtrack;

    List<AudioSource> overlappingAudioSources = new List<AudioSource>();

    void Start() {

    }
    private void OnTriggerStay(Collider other) {
        if(!other.gameObject.CompareTag("SoundtrackZone")) return;
        AudioSource sound = other.GetComponent<AudioSource>();
        if(!sound) return;

        // collect
        overlappingAudioSources.Add(sound);
    }
    private void FixedUpdate() {

        // reset the list for next frame
        overlappingAudioSources = new List<AudioSource>();
    }
}
