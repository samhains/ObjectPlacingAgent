using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ANIMATION
{
    PRAYING,
    WALKING
}
public class AnimationSwitcher : MonoBehaviour
{
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    public void SetAnimation(ANIMATION _a)
    {
        switch (_a)
        {
            case ANIMATION.PRAYING:
                _animator.SetBool("Praying", true);
                _animator.SetFloat("PrayType", Random.Range(1.0f, 1.0f));
                break;

            case ANIMATION.WALKING:
                _animator.SetBool("Praying", false);
                _animator.SetBool("InConversation", false);
                break;
        }

    }

    void LateUpdate()
    {
    }

}

