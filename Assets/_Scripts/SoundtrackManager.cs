using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour {

    public AudioSource DefaultSoundtrack;

    List<AudioSource> overlappingAudioSources = new List<AudioSource>();

    AudioSource currentlyPlayingSoundtrack = null;

    void Start() {
        StopPlayingTrack();
    }
    private void OnTriggerEnter(Collider other) {
        if(currentlyPlayingSoundtrack != null) return;

        if(!other.gameObject.CompareTag("SoundtrackZone")) return;
        AudioSource sound = other.GetComponent<AudioSource>();
        if(!sound) return;

        StartPlayingTrack(sound);
    }
    private void OnTriggerExit(Collider other) {
        if(!other.gameObject.CompareTag("SoundtrackZone")) return;
        AudioSource sound = other.GetComponent<AudioSource>();
        if(!sound) return;

        if(currentlyPlayingSoundtrack != sound) return;

        StopPlayingTrack();
    }

    public void StartPlayingTrack(AudioSource soundtrack) {
        StopPlayingTrack();
        print("Playing soundtrack " + soundtrack.gameObject.name);
        currentlyPlayingSoundtrack = soundtrack;
        soundtrack.Play();

        DefaultSoundtrack.Stop();
    }
    public void StopPlayingTrack() {
        print("Stopping playing current track (if any)");
        if(currentlyPlayingSoundtrack)
            currentlyPlayingSoundtrack.Stop();
        currentlyPlayingSoundtrack = null;

        DefaultSoundtrack.Play();
    }
}
