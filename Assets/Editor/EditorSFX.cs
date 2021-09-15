using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

// Thanks to Thom_Denick https://forum.unity.com/threads/way-to-play-audio-in-editor-using-an-editor-script.132042/#post-4767824
public static class EditorSFX
{

	public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
	{
		Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;

		Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
		MethodInfo method = audioUtilClass.GetMethod(
			"PlayPreviewClip",
			BindingFlags.Static | BindingFlags.Public,
			null,
			new Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
			null
		);

		Debug.Log(method);
		method.Invoke(
			null,
			new object[] { clip, startSample, loop }
		);
	}

	public static void StopAllClips()
	{
		Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;

		Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
		MethodInfo method = audioUtilClass.GetMethod(
			"StopAllPreviewClips",
			BindingFlags.Static | BindingFlags.Public,
			null,
			new Type[] { },
			null
		);

		Debug.Log(method);
		method.Invoke(
			null,
			new object[] { }
		);
	}
	// https://github.com/Unity-Technologies/UnityCsReference/blob/61f92bd79ae862c4465d35270f9d1d57befd1761/Editor/Mono/Audio/Bindings/AudioUtil.bindings.cs#L30
	public static float GetClipPosition()
	{
		Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
		Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
		MethodInfo method = audioUtilClass.GetMethod(
			"GetPreviewClipPosition",
			BindingFlags.Static | BindingFlags.Public
			);

		float position = (float)method.Invoke(
			null,
			new object[] { }
		);

		return position;
	}
	public static void ResumeClip()
	{
		Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
		Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
		MethodInfo method = audioUtilClass.GetMethod(
			"ResumePreviewClip",
			BindingFlags.Static | BindingFlags.Public
			);

		method.Invoke(
			null,
			new object[] { }
		);
	}
	public static void PauseClip()
	{
		Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
		Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
		MethodInfo method = audioUtilClass.GetMethod(
			"PausePreviewClip",
			BindingFlags.Static | BindingFlags.Public
			);

		method.Invoke(
			null,
			new object[] { }
		);
	}
}