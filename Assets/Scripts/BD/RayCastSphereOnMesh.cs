using BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Returns a TaskStatus of running. Will only stop when interrupted or a conditional abort is triggered.")]
    [TaskIcon("{SkinColor}Action.png")]
    public class RayCastSphereOnMesh : Action
    {
        public SharedVector3 target;
        public SharedFloat radius;
        public override void OnStart()
        {
        }

        public override TaskStatus OnUpdate()
        {
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            Vector3 pos = transform.position + new Vector3(0, 1.0f, 0);
            RaycastHit hit;
            var hitColliders = Physics.OverlapSphere(pos, radius.Value);
            
 
            for (var i = 0; i < hitColliders.Length; i++) {
                if (hitColliders[i].gameObject.tag == "Map")
                {
                    target.Value = hitColliders[i].gameObject.transform.position;
                    return TaskStatus.Success;
                }
            }
            
            return TaskStatus.Failure;
        }
    }
}

