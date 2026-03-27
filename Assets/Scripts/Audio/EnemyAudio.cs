using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAudio", menuName = "Game/Enemy Audio")]
public class EnemyAudio : ScriptableObject
{
    public AudioClip hit;
    public AudioClip death;
    public AudioClip attack;
    public AudioClip footstep;
    public AudioClip dodge;
}