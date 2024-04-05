using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public AudioSource platformAudio;

    [Header("Audio Clips")]
    [SerializeField] AudioClip projectileClip; // retro shoooosh 07
    [SerializeField] AudioClip attackedClip; //
    [SerializeField] AudioClip healingClip; //
    [SerializeField] AudioClip collectGemClip; // retro pickup 18
    [SerializeField] AudioClip collectHeartClip; //
    [SerializeField] AudioClip slidingDoorClip; //sliding door clip
    [SerializeField] AudioClip rotatingDoorClip; // automatic door
    [SerializeField] AudioClip teleportAmbientClip; // retro amience 02
    [SerializeField] AudioClip warpedClip; // retro charge magic 11
    [SerializeField] AudioClip resetSwitchClip; // retro cinematic short 02
    [SerializeField] AudioClip otherSwitchesClip; // retro impact 20
    [SerializeField] AudioClip platformClip;  // retro Ambience 11


    private void Awake()
    {
        downstairsTeleport.clip = teleportAmbientClip;
        downstairsTeleport.playOnAwake = true;
        downstairsTeleport.loop = true;
        upstairsTeleport.clip = teleportAmbientClip;
        upstairsTeleport.playOnAwake = true;
        upstairsTeleport.loop = true;
    }

    private void Start()
    {
        platformAudio.playOnAwake = false;
        playerAudio.playOnAwake = false;
        enemyAudio.playOnAwake = false;
        buildingAudio.playOnAwake = false;
        slidingDoor.playOnAwake = false;
        rotatingDoor.playOnAwake = false;
    }

    private void Update()
    {
        if(enemyAudio.IsDestroyed() || enemyAudio == null)
        {
            GameObject enemy = GameObject.Find("Enemy(Clone)");
            enemyAudio = enemy.GetComponent<AudioSource>();
        }
    }

    public void PlayWarpAudio()
    {
        playerAudio.clip = warpedClip;
        playerAudio.volume = 0.2f;
        playerAudio.Play();
    }

    public void PlayerAttackedAudio()
    {
        playerAudio.clip = attackedClip;
        playerAudio.Play();
    }

    public void EnemyAttackedAudio()
    {
        enemyAudio.clip = attackedClip;
        enemyAudio.Play();
    }

    public void PlayerHealAudio()
    {
        playerAudio.clip = healingClip;
        playerAudio.Play();
    }

    public void EnemyHealAudio()
    {
        enemyAudio.clip = healingClip;
        enemyAudio.Play();
    }

    public void PlayBuildingReset()
    {
        buildingAudio.clip = resetSwitchClip;
        buildingAudio.Play();
    }

    public void PlayBuildingOther()
    {
        buildingAudio.clip = otherSwitchesClip;
        buildingAudio.Play();
    }

    public void PlayRotatingDoor()
    {
        rotatingDoor.clip = rotatingDoorClip;
        rotatingDoor.Play();
    }

    public void PlaySlidingDoor()
    {
        slidingDoor.clip = slidingDoorClip;
        slidingDoor.Play();
    }

    public void PlayCollectGem()
    {
        playerAudio.volume = 0.2f;
        playerAudio.clip = collectGemClip;
        playerAudio.Play();
    }

    public void PlayPlatformAudio()
    {
        platformAudio.pitch = 0.19f;
        platformAudio.loop = true;
        platformAudio.clip = platformClip;
        platformAudio.Play();
    }

    public void PlayCollectHeart()
    {
        playerAudio.volume = 0.2f;
        playerAudio.clip = healingClip;
        playerAudio.Play();
    }

    public void StopPlaying(AudioSource audioSource)
    {
        audioSource.Stop();
    }
}
