using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Header("Scripts")]
    CharacterController characterController;

    [Header("Components")]
    Animator anim;
    [HideInInspector]
    public Animator Anim => anim;

    private void Awake()
    {
        //components
        anim = GetComponentInChildren<Animator>();

        //scripts
        characterController = GetComponent<CharacterController>();
    }
}
