using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Sound/Data", order = 1)]

public class SoundData : ScriptableObject
{
    public AudioClip playerLaserSound;
    public AudioClip playerProjectileSound;
    public AudioClip alienProjectileSound;
    public AudioClip droneLaserSound;
    public AudioClip pickableSound;
    public AudioClip enemyExplotion;
    public AudioClip entityHit;
}
