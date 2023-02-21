using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

// @Author: Mohammad Zidane
public class Footsteps : NetworkBehaviour 
{
    public AudioSource footStepAudioSource;
    public AudioClip footstepsSound;
    private bool currentlyPlaying;
    private NetworkBool playFootstepSound;

    void Spawned() 
    {
        currentlyPlaying = false;
        playFootstepSound = false;
        
    }
    void Update()
    {
        if (Object.HasInputAuthority)
        {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                playFootstepSound = true;
            }
            else
            {
                playFootstepSound = false;
            }
        }

        if (playFootstepSound)
        {
            if (currentlyPlaying == false)
             {
                 StartCoroutine(playFootStep());
             }
        }
        else 
        {
            footStepAudioSource.Stop();
            currentlyPlaying = false;
        }
    }
    IEnumerator playFootStep()
     {
        // Pick a footstep sound to play
        footStepAudioSource.clip = footstepsSound;
        //int randomPitch = Random.Range(1, 3);
        //footStepAudioSource.pitch = (int)randomPitch;
 
        // Play the sound
        footStepAudioSource.Play();
        currentlyPlaying = true;
        yield return new WaitForSeconds(footStepAudioSource.clip.length);
        currentlyPlaying = false;
     }
}
