using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Cage a creature or thing!")]
    [TaskIcon("{SkinColor}WaitIcon.png")]
    public class CageTargetAction : Action
    {
        [Tooltip("The amount of time to wait")]
        public SharedFloat waitTime = 1;
        [Tooltip("Should the wait be randomized?")]
        public SharedBool randomWait = false;
        [Tooltip("The minimum wait time if random wait is enabled")]
        public SharedFloat randomWaitMin = 1;
        [Tooltip("The maximum wait time if random wait is enabled")]
        public SharedFloat randomWaitMax = 1;

        // The time to wait
        private float waitDuration;
        // The time that the task started to wait.
        private float startTime;
        // Remember the time that the task is paused so the time paused doesn't contribute to the wait time.
        private float pauseTime;
        public GameObject cageObject;
        public SharedGameObject targetSharedObject;

        public override void OnStart()
        {
            // Remember the start time.
            startTime = Time.time;
            if (randomWait.Value) {
                waitDuration = Random.Range(randomWaitMin.Value, randomWaitMax.Value);
            } else {
                waitDuration = waitTime.Value;
            }

        }

        public override TaskStatus OnUpdate()
        {
            // The task is done waiting if the time waitDuration has elapsed since the task was started.
            if (startTime + waitDuration < Time.time) {
                Bounds targetBounds = targetSharedObject.Value.GetComponent<CapsuleCollider>().bounds;

                // create the cage
                GameObject cageObjectInScene = GameObject.Instantiate(cageObject);

                // scale the cage based on size of target
                cageObjectInScene.transform.localScale = new Vector3(targetBounds.size.y, targetBounds.size.y, targetBounds.size.y);

                float cageHeight = cageObjectInScene.GetComponent<BoxCollider>().bounds.size.y*targetBounds.size.y;
                Vector3 cagePosition = new Vector3(targetSharedObject.Value.transform.position.x, targetBounds.min.y+ cageHeight/2, targetSharedObject.Value.transform.position.z);

                cageObjectInScene.transform.position = cagePosition;
                targetSharedObject.Value.transform.parent = cageObjectInScene.transform;

                // set target to caged status

                targetSharedObject.Value.layer =  LayerMask.NameToLayer("Caged");
                targetSharedObject.Value.GetComponent<AgentController>().SetCaged(true);

                return TaskStatus.Success;
            }
            // Otherwise we are still waiting.
            return TaskStatus.Running;
        }

        public override void OnPause(bool paused)
        {
            if (paused) {
                // Remember the time that the behavior was paused.
                pauseTime = Time.time;
            } else {
                // Add the difference between Time.time and pauseTime to figure out a new start time.
                startTime += (Time.time - pauseTime);
            }
        }

        public override void OnReset()
        {
            // Reset the public properties back to their original values
            waitTime = 1;
            randomWait = false;
            randomWaitMin = 1;
            randomWaitMax = 1;
        }
    }
}