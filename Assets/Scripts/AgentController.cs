using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    private Animator _anim;
    public bool Caged { get; set; }
    private NavMeshAgent _ai;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _ai = GetComponent<NavMeshAgent>();
    }

    public void SetCaged(bool status)
    {
        _anim.SetBool("isCaged", status);
        _ai.enabled = !status;
        Caged = status;
    }

    // Update is called once per frame
    void Update()
    {
        _anim.SetFloat("Velocity", _ai.velocity.magnitude);
    }
}
