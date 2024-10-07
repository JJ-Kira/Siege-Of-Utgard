using UnityEngine;
using UnityEngine.AI;

namespace Entities.Enemies.Control {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class EnemyMovement : MonoBehaviour {
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Velocity1 = Animator.StringToHash("Velocity");
    
        private NavMeshAgent Agent;
        private Animator Animator;

        private Vector2 Velocity;
        private Vector2 SmoothDeltaPosition;

        private void Awake() {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            Animator.applyRootMotion = true;
            Agent.updatePosition = false;
            Agent.updateRotation = true;
        }
    
        private void OnAnimatorMove() {
            Vector3 rootPosition = Animator.rootPosition;
            rootPosition.y = Agent.nextPosition.y;
            transform.position = rootPosition;
            Agent.nextPosition = rootPosition;
        }

        private void Update() {
            SynchronizeAnimatorAndAgent();
        }

        private void SynchronizeAnimatorAndAgent() {
            Vector3 worldDeltaPosition = Agent.nextPosition - transform.position;
            worldDeltaPosition.y = 0;
            // Map 'worldDeltaPosition' to local space
            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);

            // Low-pass filter the deltaMove
            float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
            SmoothDeltaPosition = Vector2.Lerp(SmoothDeltaPosition, deltaPosition, smooth);

            Velocity = SmoothDeltaPosition / Time.deltaTime;
            if (Agent.remainingDistance <= Agent.stoppingDistance) {
                Velocity = Vector2.Lerp(Vector2.zero, Velocity, Agent.remainingDistance);
            }

            bool shouldMove = Velocity.magnitude > 0.01f && Agent.remainingDistance > Agent.stoppingDistance && !Agent.isStopped;

            Animator.SetBool(Move, shouldMove);
            Animator.SetFloat(Velocity1, Velocity.magnitude);

            transform.LookAt(Agent.steeringTarget + transform.forward);

            //float deltaMagnitude = worldDeltaPosition.magnitude;
            //if (deltaMagnitude > Agent.radius / 2)
            //{
            //    transform.position = Vector3.Lerp(Animator.rootPosition, Agent.nextPosition, smooth);
            //}
        }
    }
}