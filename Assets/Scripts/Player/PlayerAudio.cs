using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAudio", menuName = "Game/Player Audio")]
public class PlayerAudio : ScriptableObject
{
    [Header("공격")]
    public List<AudioClip> AttackAudioClips;
    [Header("죽음")]
    public List<AudioClip> DeathAudioClips;
    [Header("회피")]
    public List<AudioClip> DodgeAudioClips;
}