using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

// @Author: Mohammad Zidane
public class Footsteps : NetworkBehaviour 
{
    public AudioSource footStepAudioSource;
    public AudioClip[] footStepAudioClips;
    private bool currentlyPlaying;
    public NetworkBool playFootstepSound;
    private int randomFootStep;

    public override void Spawned() 
    {
        currentlyPlaying = false;
        playFootstepSound = false;        
    }
    void Update()
    {   
        if (playFootstepSound)
        {
            if (currentlyPlaying == false)
             {
                 StartCoroutine(playFootStep());
             }
        }
    }
    IEnumerator playFootStep()
     {
        // Pick a random footstep sound to play
        randomFootStep = (int)Mathf.Floor(Random.Range(0, footStepAudioClips.Length));
        footStepAudioSource.clip = footStepAudioClips[randomFootStep];
        footStepAudioSource.pitch = Random.Range(0.8f, 1.1f);
 
        // Play the sound
        footStepAudioSource.Play();
        currentlyPlaying = true;
        yield return new WaitForSeconds(0.4f);
        footStepAudioSource.Stop();
        currentlyPlaying = false;
     }
}
