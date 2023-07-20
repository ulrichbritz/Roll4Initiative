using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class CharacterSoundEffectsManager : MonoBehaviour
    {
        private AudioSource audioSource;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySprintingSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.SprintingSoundFX);
        }
    }
}

