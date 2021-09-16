using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    Conductor conductor;
    [SerializeField, Header("Beat Spawning")]
    GameObject beatDot;
    float lastbeat = 0;
	[SerializeField]
	Sprite[] BeatImages;

	[SerializeField]
	int Anticipation = 3;

	[Header("SkeletonTracking"),SerializeField]
	Transform Skeleton;
	[SerializeField]
	float speed;

	[System.Serializable]
	public struct BeatPositions
	{
		public bool[] TL;
		public bool[] ML;
		public bool[] BL;
		public bool[] TR;
		public bool[] MR;
		public bool[] BR;
	}
	public BeatPositions BeatTimes;


    // Start is called before the first frame update
    void Start()
    {
		//   GetComponent<Animation>().
		//Debug.Log(BeatTimes[0]);
    }

    public void SpawnBeat(int index)
    {
		//Vector3 spawnPos = new Vector3();
		beatDot.GetComponent<BeatVisualizer>().beatsToFull = Anticipation;//Random.Range(1, 4);
		beatDot.transform.eulerAngles = new Vector3(0, 0, 60 * index);
        beatDot.GetComponent<SpriteRenderer>().sprite = BeatImages[index];
        Instantiate(beatDot, transform);

    }
    // Update is called once per frame
    void Update()
    {
		transform.position = Vector3.Lerp(transform.position, Skeleton.position, Time.deltaTime * speed);


        if (conductor.SongPosition > lastbeat + conductor.crotchet && conductor.CurrentBeatIndex - Anticipation >= 0)
        {
            lastbeat += conductor.crotchet;
			if (BeatTimes.TL[conductor.CurrentBeatIndex + Anticipation])
				SpawnBeat(2);
			if (BeatTimes.ML[conductor.CurrentBeatIndex + Anticipation])
				SpawnBeat(3);
			if (BeatTimes.BL[conductor.CurrentBeatIndex + Anticipation])
				SpawnBeat(4);
			if (BeatTimes.TR[conductor.CurrentBeatIndex + Anticipation])
				SpawnBeat(1);
			if (BeatTimes.MR[conductor.CurrentBeatIndex + Anticipation])
				SpawnBeat(0);
			if (BeatTimes.BR[conductor.CurrentBeatIndex + Anticipation])
				SpawnBeat(5);

		}

	}
}
