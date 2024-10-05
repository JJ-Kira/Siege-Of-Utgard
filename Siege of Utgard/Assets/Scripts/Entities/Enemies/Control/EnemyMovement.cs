using UnityEngine;
using UnityEngine.AI;

namespace Enemies.Control {
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class EnemyMovement : MonoBehaviour {
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        private NavMeshAgent agent;
        private Animator animator;

        private Vector2 velocity;
        private Vector2 smoothDeltaPosition;

        private void Awake() {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            animator.applyRootMotion = true;
            agent.updatePosition = false;
            agent.updateRotation = true;
        }

        private void OnAnimatorMove() {
            Vector3 rootPosition = animator.rootPosition;
            rootPosition.y = agent.nextPosition.y;
            transform.position = rootPosition;
            agent.nextPosition = rootPosition;
        }

        private void Update() {
            SynchronizeAnimatorAndAgent();
        }

        private void SynchronizeAnimatorAndAgent() {
            Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
            worldDeltaPosition.y = 0;
            // Map 'worldDeltaPosition' to local space
            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);

            // Low-pass filter the deltaMove
            float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
            smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

            velocity = smoothDeltaPosition / Time.deltaTime;
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                velocity = Vector2.Lerp(Vector2.zero, velocity, agent.remainingDistance);
            }

            bool shouldMove = velocity.magnitude > 0.3f && agent.remainingDistance > agent.stoppingDistance;

            animator.SetBool(Move, shouldMove);
            animator.SetFloat(Velocity, velocity.magnitude);

            float deltaMagnitude = worldDeltaPosition.magnitude;
            if (deltaMagnitude > agent.radius / 2) {
                transform.position = Vector3.Lerp(animator.rootPosition, agent.nextPosition, smooth);
            }
        }

        public void StopMoving() {
            agent.isStopped = true;
            StopAllCoroutines();
        }
    }
}