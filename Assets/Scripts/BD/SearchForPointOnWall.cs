using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Search for a place on a wall by combining the wander, and the within range tasks using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SearchIcon.png")]
    public class SearchForPointOnWall : NavMeshMovement
    {
        [Tooltip("Minimum distance ahead of the current position to look ahead for a destination")]
        public SharedFloat minWanderDistance = 20;
        [Tooltip("Maximum distance ahead of the current position to look ahead for a destination")]
        public SharedFloat maxWanderDistance = 20;
        [Tooltip("The amount that the agent rotates direction")]
        public SharedFloat wanderRate = 1;
        [Tooltip("The minimum length of time that the agent should pause at each destination")]
        public SharedFloat minPauseDuration = 0;
        [Tooltip("The maximum length of time that the agent should pause at each destination (zero to disable)")]
        public SharedFloat maxPauseDuration = 0;
        [Tooltip("The maximum number of retries per tick (set higher if using a slow tick time)")]
        public SharedInt targetRetries = 1;
        [Tooltip("The field of view angle of the agent (in degrees)")]
        public SharedFloat fieldOfViewAngle = 90;
        [Tooltip("The distance that the agent can see")]
        public SharedFloat viewDistance = 30;
        [Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
        public LayerMask ignoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        [Tooltip("The raycast offset relative to the pivot position")]
        public SharedVector3 offset;
        [Tooltip("The target raycast offset relative to the pivot position")]
        public SharedVector3 targetOffset;
        [Tooltip("The LayerMask of the objects that we are searching for")]
        public LayerMask objectLayerMask;
        [Tooltip("Specifies the maximum number of colliders that the physics cast can collide with")]
        public int maxCollisionCount = 200;
        
        public SharedGameObject returnedObject;

        private float pauseTime;
        private float destinationReachTime;

        private Collider[] overlapColliders;

        // Keep searching until an object is seen or heard (if senseAudio is enabled)
        public override TaskStatus OnUpdate()
        {
            if (HasArrived()) {
                // The agent should pause at the destination only if the max pause duration is greater than 0
                if (maxPauseDuration.Value > 0) {
                    if (destinationReachTime == -1) {
                        destinationReachTime = Time.time;
                        pauseTime = Random.Range(minPauseDuration.Value, maxPauseDuration.Value);
                    }
                    if (destinationReachTime + pauseTime <= Time.time) {
                        // Only reset the time if a destination has been set.
                        if (TrySetWallTarget()) {
                            destinationReachTime = -1;
                        }
                    }
                } else {
                    TrySetWallTarget();
                }
            }

            // Detect if any objects are within sight
            if (overlapColliders == null) {
                overlapColliders = new Collider[maxCollisionCount];
            }
//            returnedObject.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, overlapColliders, objectLayerMask, targetOffset.Value, ignoreLayerMask, useTargetBone.Value, targetBone);
            // If an object was seen then return success
            if (returnedObject.Value != null) {
                return TaskStatus.Success;
            }

            // No object has been seen or heard so keep searching
            return TaskStatus.Running;
        }

        private bool TrySetWallTarget()
        {
            var direction = transform.forward;
            var validDestination = false;
            var attempts = targetRetries.Value;
            var destination = transform.position;
            Debug.Log(attempts);
            while (!validDestination && attempts > 0) {
                direction = direction + Random.insideUnitSphere * wanderRate.Value;
                destination = transform.position + direction.normalized * Random.Range(minWanderDistance.Value, maxWanderDistance.Value);
                validDestination = SamplePosition(destination);
                attempts--;
            }
            if (validDestination) {
                SetDestination(destination);
            }
            return validDestination;
        }

        // Reset the public variables
        public override void OnReset()
        {
            base.OnReset();

            minWanderDistance = 20;
            maxWanderDistance = 20;
            wanderRate = 2;
            minPauseDuration = 0;
            maxPauseDuration = 0;
            targetRetries = 1;
            fieldOfViewAngle = 90;
            viewDistance = 30;
        }
    }
}
