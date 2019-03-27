using BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Wander using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}WanderIcon.png")]
    public class WanderOnce : NavMeshMovement
    {
        [Tooltip("Minimum distance ahead of the current position to look ahead for a destination")]
        public SharedFloat minWanderDistance = 20;
        [Tooltip("Maximum distance ahead of the current position to look ahead for a destination")]
        public SharedFloat maxWanderDistance = 20;
        [Tooltip("The amount that the agent rotates direction")]
        public SharedFloat wanderRate = 2;
        [Tooltip("The minimum length of time that the agent should pause at each destination")]
        public SharedInt targetRetries = 1;
        
        private bool hasSetDestinationOnce;

        public override void OnStart()
        {
            hasSetDestinationOnce = false;
        }

        public override TaskStatus OnUpdate()
        {
            if (HasArrived()) {
                if (hasSetDestinationOnce)
                {
                    return TaskStatus.Success;
                }
                TrySetTarget();
            }
            return TaskStatus.Running;
        }

        private bool TrySetTarget()
        {
            var direction = transform.forward;
            var validDestination = false;
            var attempts = targetRetries.Value;
            var destination = transform.position;
            while (!validDestination && attempts > 0) {
                direction = direction + Random.insideUnitSphere * wanderRate.Value;
                destination = transform.position + direction.normalized * Random.Range(minWanderDistance.Value, maxWanderDistance.Value);
                validDestination = SamplePosition(destination);
                attempts--;
            }
            if (validDestination) {
                SetDestination(destination);
                hasSetDestinationOnce = true;
            }
            return validDestination;
        }

        // Reset the public variables
        public override void OnReset()
        {
            minWanderDistance = 20;
            maxWanderDistance = 20;
            wanderRate = 2;
            targetRetries = 1;
            hasSetDestinationOnce = false;
        }
    }
}
