using UnityEngine;

namespace StatePattern.Enemy
{
    public class PatrollingState : IState
    {
        public EnemyController Owner { get; set; }
        private IStateMachine stateMachine;
        private int currentPatrollingIndex = -1;

        // Store the destination for the enemy's patrolling behavior.
        private Vector3 destination;


        public PatrollingState(IStateMachine stateMachine) => this.stateMachine = stateMachine;

        // Initialize the current patrolling index to -1 to start from the first waypoint.
        

        public void OnStateEnter()
        {
            SetNextWaypointIndex();
            destination = GetDestination();
            MoveTowardsDestination();
        }

        private void MoveTowardsDestination()
        {
            // Set the 'isStopped' property of the character's navigation agent to false.
            // This ensures that the agent is actively navigating towards the destination.
            Owner.Agent.isStopped = false;

            // Set the destination for the character's navigation agent.
            Owner.Agent.SetDestination(destination);
        }

        private void SetNextWaypointIndex()
        {
            // Check if the currentPatrollingIndex has reached the end of the waypoint list.
            if (currentPatrollingIndex == Owner.Data.PatrollingPoints.Count - 1)
                // If it has, reset it to 0 to start patrolling from the first waypoint.
                currentPatrollingIndex = 0;
            else
                // Otherwise, increment the index to move to the next waypoint.
                currentPatrollingIndex++;
        }

        // Helper method to get the destination position for the current waypoint.
        private Vector3 GetDestination() => Owner.Data.PatrollingPoints[currentPatrollingIndex];

        public void Update()
        {
            if (ReachedDestination())
                stateMachine.ChangeState(States.IDLE);
        }

        private bool ReachedDestination() => Owner.Agent.remainingDistance <= Owner.Agent.stoppingDistance;

        public void OnStateExit() { }
    }
}