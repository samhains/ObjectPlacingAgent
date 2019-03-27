using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Returns a TaskStatus of running. Will only stop when interrupted or a conditional abort is triggered.")]
    [TaskIcon("{SkinColor}Action.png")]
    public class RayCastPointOnMesh : Action
    {
        public SharedVector3 target;
        public SharedQuaternion normal;
        public override void OnStart()
        {
        }

        public override TaskStatus OnUpdate()
        {
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            Vector3 pos = transform.position;
            RaycastHit hit;

            if (Physics.Raycast(pos, fwd, out hit, 1000))
            {
                target.Value = hit.point;
                normal.Value = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                return TaskStatus.Success;
            }
            
            return TaskStatus.Failure;
        }
    }
}
