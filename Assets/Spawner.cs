using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    Conductor conductor;
    [SerializeField]
    GameObject beatDot;
    float lastbeat = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (conductor.SongPosition > lastbeat + conductor.crotchet)
        {
            lastbeat += conductor.crotchet;
            Vector3 spawnPos = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(0.3f, 2), -1.419f);
            beatDot.GetComponent<BeatVisualizer>().beatsToFull = Random.Range(1, 4);
            beatDot.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Random.ColorHSV();
            Instantiate(beatDot, spawnPos, Quaternion.identity, transform);
        }

    }
}
