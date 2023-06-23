// This file contains the SoundManager class, which handles playing sounds in the game.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Static member variable to store the instance of the SoundManager class.
    public static SoundManager Instance;

    // Public AudioClip variables to store the shoot, sheep hit, and sheep dropped sounds.
    public AudioClip shootClip;
    public AudioClip sheepHitClip;
    public AudioClip sheepDroppedClip;

    private Vector3 cameraPosition;

    void Awake()
    {
        Instance = this;

        cameraPosition = Camera.main.transform.position;
    }

    // PlaySound() method plays an audio clip at the camera position.
    private void PlaySound(AudioClip clip)
    {
        // Play the audio clip at the camera position.
        AudioSource.PlayClipAtPoint(clip, cameraPosition);
    }

    // PlayShootClip() method plays the shoot sound.
    public void PlayShootClip()
    {
        // Play the shoot sound.
        PlaySound(shootClip);
    }

    // PlaySheepHitClip() method plays the sheep hit sound.
    public void PlaySheepHitClip()
    {
        // Play the sheep hit sound.
        PlaySound(sheepHitClip);
    }

    // PlaySheepDroppedClip() method plays the sheep dropped sound.
    public void PlaySheepDroppedClip()
    {
        // Play the sheep dropped sound.
        PlaySound(sheepDroppedClip);
    }
}