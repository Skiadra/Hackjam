using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSelector : MonoBehaviour
{
    public RuntimeAnimatorController newAnimator;
    public RuntimeAnimatorController newAnimator2;

    void Start()
    {
        if (Movement.move.isMelee)
        {
            GetComponent<Animator>().runtimeAnimatorController = newAnimator;
        }
        else
        {
            GetComponent<Animator>().runtimeAnimatorController = newAnimator2;
        }
    }
}
