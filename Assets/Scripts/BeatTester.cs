using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTester : MonoBehaviour
{
    [SerializeField]
    Conductor conductor;
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
            Debug.Log("beat");
            lastbeat += conductor.crotchet;
            if (GetComponent<SpriteRenderer>())
            {
                SpriteRenderer sp = GetComponent<SpriteRenderer>();
                sp.color = Random.ColorHSV();
            }
        }

    }
}
