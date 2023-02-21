using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

// @Author: Mohammad Zidane
public class Footsteps : NetworkBehaviour 
{
    public AudioSource footStepAudioSource;
    public AudioClip footstepsSound;
    private NetworkBool currentlyPlaying = false;
    
    void Update()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (currentlyPlaying == false)
             {
                 StartCoroutine(playFootStep());
             }
             return;
        }
        footStepAudioSource.Stop();
        currentlyPlaying = true;
    }
    IEnumerator playFootStep()
     {
         currentlyPlaying = true;
         // Pick a random footstep sound to play
         footStepAudioSource.clip = footstepsSound;
 
         // Pick a random pitch to play it at
         //int randomPitch = Random.Range(1, 3);
         //footStepAudioSource.pitch = (int)randomPitch;
 
         // Play the sound
         footStepAudioSource.Play();
         yield return new WaitForSeconds(footStepAudioSource.clip.length);
         currentlyPlaying = false;
     }
}
