using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatVisualizer : MonoBehaviour
{
    [SerializeField]
    Conductor conductor;
    float firstbeat = 0; // reference to the time the object was spawned
    float lastbeat = 0; // last recorded position
    [SerializeField]
    public float beatsToFull = 3, LifetimeBeats = 0;

    bool onPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        if (conductor == null)
            conductor = GameObject.FindObjectOfType<Conductor>();

        lastbeat = conductor.currentBeat;
        firstbeat = lastbeat;
		transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (conductor.SongPosition > lastbeat + conductor.crotchet)
        {
            //Debug.Log("beat");
            lastbeat += conductor.crotchet;
            LifetimeBeats++;
            if (LifetimeBeats >= beatsToFull)
            {
                if (onPlayer) // if colliding with player's hand
                    GameManager.score++; // increment score

				//Debug.Log(transform.localScale);
                Destroy(gameObject); // destroy beat
            }
        }
		transform.localScale = Vector3.one * Mathf.Lerp(0, 1, (conductor.SongPosition - firstbeat) / ( (conductor.crotchet * beatsToFull)));//(conductor.SongPosition - firstbeat) / ((conductor.SongPosition) + conductor.crotchet * beatsToFull)*2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            onPlayer = true;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            onPlayer = false;
    }
}
