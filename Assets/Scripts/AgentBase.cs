using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;

public class AgentBase : MonoBehaviour
{
    private BehaviorTree _behaviorTree;
    private Animator _anim;
    private AnimationSwitcher _animSwitcher;
    public GameObject ConversationPartner { get; set; }
    public GameObject ActiveObject;
    public bool InConversation { get; set; }
    public bool Cooldown { get; set; }
    // Start is called before the first frame update
    private NavMeshAgent _ai;
    void Start()
    {
        _ai = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _animSwitcher = GetComponent<AnimationSwitcher>();
        _behaviorTree = GetComponent<BehaviorTree>();
        _anim.SetFloat("IdleType", Random.value);
        _anim.SetFloat("Offset", Random.value);
        
    }

    public void PlayAnimation(string animName)
    {
        _anim.SetBool(animName, true);
    }

    // Update is called once per frame
    void Update()
    {
        float velocity = _ai.velocity.magnitude;
        _anim.SetFloat("Velocity", velocity);

        //if (velocity > 0.3)
        //{
        //    _animSwitcher.SetAnimation(ANIMATION.WALKING);
        //}

        var result = Mathf.Lerp(0, 1, Mathf.InverseLerp(1.5f, 3, _ai.velocity.magnitude));
        _anim.SetFloat("WalkType", result);
        
    }
}
