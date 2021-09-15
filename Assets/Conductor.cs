using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{ // many aspects taken from https://web.archive.org/web/20190214221754/http://ludumdare.com/compo/2014/09/09/an-ld48-rhythm-game-part-2/
    public float BPM;
    public float SongPosition;
    public float offset;
    public float crotchet; // the time duration of a beat, calculated from the bpm
    public float currentBeat = 0;
    public int CurrentBeatIndex = 0;

    //public List<bool> BeatTimes;

    AudioSource sound;

    double dsptimesong; // dps at frame of song start

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        sound.Play();
        dsptimesong = AudioSettings.dspTime;

        crotchet = 60 / BPM;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        SongPosition = (float)(AudioSettings.dspTime - dsptimesong) * sound.pitch - offset;
        if (SongPosition > currentBeat + crotchet)
        {
            currentBeat += crotchet;
            CurrentBeatIndex++;
        }
    }
}
