using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using UnityEditor.Audio;
using System;
using System.Reflection;

public class BeatSpawnWindow : EditorWindow
{
	Vector2 BeatGraphScrollPos, windowScrollPos;
	bool tog;

	Spawner.BeatPositions BeatPos;

	AudioClip song;
	float BPM;


	int NumberOfBeats;
	float crotchet;
	string highestBeat;

	[MenuItem("Tools/uhhhh shit")] // topbar spot
	public static void ShowWindow()
	{
		GetWindow(typeof(BeatSpawnWindow));
	}

	private void OnGUI()
	{
		//windowScrollPos = EditorGUILayout.BeginScrollView(windowScrollPos);

		// main two parameters for it to work
		song = EditorGUILayout.ObjectField("Song:", song, typeof(AudioClip), false) as AudioClip;
		BPM = EditorGUILayout.FloatField("BPM:", BPM);

		// calculate how many beats exist
		NumberOfBeats = 0;
		crotchet = 60 / BPM;
		if (song)
			NumberOfBeats = Mathf.FloorToInt((song.length / 60) * BPM);

		// create array when there isnt one or when parameters change
		if (BeatPos.TR == null || BeatPos.TR.Length != NumberOfBeats)
		{
			BeatPos.TL = new bool[NumberOfBeats];
			BeatPos.ML = new bool[NumberOfBeats];
			BeatPos.BL = new bool[NumberOfBeats];
			BeatPos.TR = new bool[NumberOfBeats];
			BeatPos.MR = new bool[NumberOfBeats];
			BeatPos.BR = new bool[NumberOfBeats];
		}

		#region drawing beats
		// left header
		GUILayout.BeginHorizontal(GUILayout.Width(100));
		GUILayout.BeginVertical();
		EditorGUILayout.SelectableLabel("", GUILayout.Width(100));
		GUILayout.Label("Top Left", GUILayout.Width(100));
		GUILayout.Label("Mid Left", GUILayout.Width(100));
		GUILayout.Label("Bot Left", GUILayout.Width(100));
		GUILayout.Label("Top Right", GUILayout.Width(100));
		GUILayout.Label("Mid Right", GUILayout.Width(100));
		GUILayout.Label("Bot Right", GUILayout.Width(100));
		GUILayout.EndVertical();

		// start scroll
		BeatGraphScrollPos =
			EditorGUILayout.BeginScrollView(BeatGraphScrollPos, GUILayout.Width(1300), GUILayout.Height(200));
		GUILayout.BeginHorizontal();

		// iterate each column, one for each beat
		for (int i = 0; i < NumberOfBeats; i++)
		{
			GUILayout.BeginVertical();
			GUI.SetNextControlName("Beat" + i);
			EditorGUILayout.SelectableLabel("Beat " + (i + 1));

			BeatPos.TL[i] = EditorGUILayout.Toggle(BeatPos.TL[i]);
			BeatPos.ML[i] = EditorGUILayout.Toggle(BeatPos.ML[i]);
			BeatPos.BL[i] = EditorGUILayout.Toggle(BeatPos.BL[i]);
			BeatPos.TR[i] = EditorGUILayout.Toggle(BeatPos.TR[i]);
			BeatPos.MR[i] = EditorGUILayout.Toggle(BeatPos.MR[i]);
			BeatPos.BR[i] = EditorGUILayout.Toggle(BeatPos.BR[i]);
			GUILayout.EndVertical();
		}
		if (highestBeat != "") // focus if sound playing
			GUI.FocusControl(highestBeat);

		// close up formatting
		GUILayout.EndHorizontal();
		EditorGUILayout.EndScrollView();
		GUILayout.EndHorizontal();
		#endregion

		#region buttons
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Save")) // save button
		{
			// find the spawner in the current scene
			GameObject beatmanager = GameObject.Find("Beat Dot Spawner");
			if (beatmanager) // if it exists
			{
				Debug.Log("Save Successful");

				// set values
				beatmanager.GetComponent<Spawner>().BeatTimes = BeatPos;
				//beatmanager.GetComponent<Spawner>().BeatTimes.TR BeatPosTL, BeatPosML, BeatPosBL,  BeatPosTR, BeatPosMR, BeatPosBR };
			}													  
			else												  
				Debug.Log("Save Unsuccessful");					  
		}														  
		if (GUILayout.Button("Load")) // load
		{
			//PublicAudioUtil.PlayClip(song);
			// find the spawner in the current scene
			GameObject beatmanager = GameObject.Find("Beat Dot Spawner");
			if (beatmanager) // if it exists
			{
				Debug.Log("Load Successful");
				BeatPos = beatmanager.GetComponent<Spawner>().BeatTimes;
				//BeatPosTL = loadedPos[0];
				//BeatPosML = loadedPos[1];
				//BeatPosBL = loadedPos[2];
				//BeatPosTR = loadedPos[3];
				//BeatPosMR = loadedPos[4];
				//BeatPosBR = loadedPos[5];
			}
			else
				Debug.Log("Load Unsuccessful");
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Play")) // play sound
		{
			highestBeat = ""; // reset highlighted beat

			EditorSFX.StopAllClips();
			EditorSFX.PlayClip(song, 30);
		}
		if (GUILayout.Button("Stop")) // stop sound
		{
			GUI.FocusControl(null);
			EditorSFX.StopAllClips();
		}
		if (GUILayout.Button("Pause")) // stop sound
		{
			EditorSFX.PauseClip();
		}
		if (GUILayout.Button("Resume")) // stop sound
		{
			EditorSFX.ResumeClip();
		}
		GUILayout.EndHorizontal();
		#endregion

		//EditorGUILayout.EndScrollView();
	}
	private void Update()
	{
		float clipPos = EditorSFX.GetClipPosition();
		//highestBeat = "";

		for (int i = 0; i < NumberOfBeats; i++)
		{
			if (clipPos > i * crotchet)
				highestBeat = "Beat" + i;
		}
		if (highestBeat != "")
		{
			Repaint();
		}
	}
}
