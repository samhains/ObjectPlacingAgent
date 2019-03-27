using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Samples NavMesh For Nearest Valid Point.")]
    [TaskIcon("{SkinColor}Action.png")]
    public class NavMeshSamplePoint : Action
    {
        public SharedVector3 ValidNavMeshPoint;
        public SharedVector3 TestPoint;
        public SharedFloat Radius;
        
        public override void OnStart()
        {
            
        }

        public override TaskStatus OnUpdate()
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(TestPoint.Value, out hit, Radius.Value, -1)) {
                ValidNavMeshPoint.Value = hit.position;
                return TaskStatus.Success;
            }            
            return TaskStatus.Failure;
        }
    }
}
