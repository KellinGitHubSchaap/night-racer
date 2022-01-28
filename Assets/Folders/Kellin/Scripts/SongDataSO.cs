using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Song Data", menuName = "Create Song Data", order = 0)]
public class SongDataSO : ScriptableObject
{
    public AudioClip song;          // Whats the song that needs to be played
}
