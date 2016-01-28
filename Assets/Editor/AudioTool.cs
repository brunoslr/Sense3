using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;


public static class PublicAudioUtil
{
    public static void Playclip(AudioClip clip)
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "PlayClip",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new System.Type[] {
                typeof(AudioClip)
            },
            null
        );
        method.Invoke(
            null,
            new object[] {
                clip
            }
        );
    } // PlayClip()

    public static void StopAllClips()
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass =
              unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "StopAllClips",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new System.Type[] { },
            null
        );
        method.Invoke(
            null,
            new object[] { }
        );
    }// StopAllClips

} // class PublicAudioUtil


public class AudioTool : EditorWindow {

    AudioClip clip;
    AudioClip[] Clips = new AudioClip[2];

    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/Audio Tool")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(AudioTool));

    }

    void OnGUI()
    {
        GUILayout.Label("Audio Track Mixer", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);
        clip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip 1", clip, typeof(AudioClip), true);
        Clips[0] = (AudioClip)EditorGUILayout.ObjectField("Audio Clip 2", Clips[0], typeof(AudioClip), true);
        Clips[1] = (AudioClip)EditorGUILayout.ObjectField("Audio Clip 3", Clips[1], typeof(AudioClip), true);

        if (GUILayout.Button("Play audio Clip"))
        {   
            PublicAudioUtil.Playclip(clip);
            PublicAudioUtil.Playclip(Clips[0]);
            PublicAudioUtil.Playclip(Clips[1]);
        }

        if (GUILayout.Button("Stop audio Clip"))
        {
            PublicAudioUtil.StopAllClips();
        }

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
            myBool = EditorGUILayout.Toggle("Toggle", myBool);
            myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}
