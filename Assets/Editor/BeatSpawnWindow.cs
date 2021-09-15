using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using UnityEditor.Audio;


public class BeatSpawnWindow : EditorWindow
{
    Vector2 scrollPos;
    bool tog;
    bool[] BeatPos;
    AudioClip song;
    float BPM;

	[MenuItem("Tools/uhhhh shit")] // topbar spot
	public static void ShowWindow()
	{
		GetWindow(typeof(BeatSpawnWindow));
	}

    private void OnGUI()
    {
        // main two parameters for it to work
        song = EditorGUILayout.ObjectField("Song:", song, typeof(AudioClip), false) as AudioClip;
        BPM = EditorGUILayout.FloatField("BPM:", BPM);

        // calculate how many beats exist
        int NumberOfBeats = 0;
        if (song)
            NumberOfBeats = Mathf.FloorToInt((song.length / 60) * BPM);

        // create array when there isnt one or when parameters change
        if (BeatPos == null || BeatPos.Length != NumberOfBeats)
            BeatPos = new bool[NumberOfBeats];

        // left header
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Label("", GUILayout.Width(100));
        GUILayout.Label("Top Left", GUILayout.Width(100));
        GUILayout.Label("Bot Left", GUILayout.Width(100));
        GUILayout.Label("Top Right", GUILayout.Width(100));
        GUILayout.Label("Bot Right", GUILayout.Width(100));
        GUILayout.EndVertical();

        // start scroll
        scrollPos =
            EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(800), GUILayout.Height(200));
        GUILayout.BeginHorizontal();

        // iterate each column, one for each beat
        for (int i = 0; i < NumberOfBeats; i++)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Beat "+(i+1));
            BeatPos[i] = EditorGUILayout.Toggle(BeatPos[i]);
            tog = EditorGUILayout.Toggle(tog);
            tog = EditorGUILayout.Toggle(tog);
            tog = EditorGUILayout.Toggle(tog);
            GUILayout.EndVertical();
        }

        // close up formatting
        GUILayout.EndHorizontal(); 
        EditorGUILayout.EndScrollView();
        GUILayout.EndHorizontal();
        
        // save button
        if (GUILayout.Button("Save"))
        {
            // find the spawner in the current scene
            GameObject beatmanager = GameObject.Find("Beat Dot Spawner");
            if (beatmanager) // if it exists
            {
                Debug.Log("Save Successful");
                beatmanager.GetComponent<Spawner>().BeatTimes = BeatPos; // set values
            }
            else
                Debug.Log("Save Unsuccessful");
        }
        if (GUILayout.Button("Load"))
        {
            //PublicAudioUtil.PlayClip(song);
            // find the spawner in the current scene
            GameObject beatmanager = GameObject.Find("Beat Dot Spawner");
            if (beatmanager) // if it exists
            {
                Debug.Log("Load Successful");
                BeatPos = beatmanager.GetComponent<Spawner>().BeatTimes; // set values
            }
            else
                Debug.Log("Load Unsuccessful");
        }
    }
}
