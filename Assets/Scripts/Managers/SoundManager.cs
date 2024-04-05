using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioSource enemyAudio;
    [SerializeField] AudioSource buildingAudio;
    [SerializeField] AudioSource slidingDoor;
    [SerializeField] AudioSource rotatingDoor;
    [SerializeField] AudioSource upstairsTeleport;
    [SerializeField] AudioSource downstairsTeleport;

    [Header("Audio Clips")]
    [SerializeField] AudioClip attackedClip;
    [SerializeField] AudioClip healingClip;
    [SerializeField] AudioClip collectGemClip;
    [SerializeField] AudioClip collectHeartClip;
    [SerializeField] AudioClip slidingDoorClip;
    [SerializeField] AudioClip rotatingDoorClip;
    [SerializeField] AudioClip teleportClip;
    [SerializeField] AudioClip warpedClip;
    [SerializeField] AudioClip resetSwitchClip;
    [SerializeField] AudioClip otherSwitchesClip;
}
