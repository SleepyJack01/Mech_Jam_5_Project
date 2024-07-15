using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReposistionState : EnemyState
{
    public ReposistionState(RifleBot rifleBot) : base(rifleBot) {}

    private Vector3 randomDirection;
    private Vector3 randomPosition;

    public override void OnStateEnter()
    {
        
    }

    public override void OnStateExecute()
    {
        PerformReposistionBehaviour();

        CheckForTransition();
    }

    private void PerformReposistionBehaviour()
    {
        // rotate model towards the player
        Vector3 direction = rifleBot.target.position - rifleBot.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        rifleBot.modelTransform.rotation = Quaternion.Slerp(rifleBot.modelTransform.rotation, lookRotation, Time.deltaTime * 5f);
        

        // move to a postion within 5 metres of the current position that is still with in the attack range of the player
        randomDirection = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        randomPosition = rifleBot.transform.position + randomDirection;

        rifleBot.agent.SetDestination(randomPosition);
    }

    private void CheckForTransition()
    {
        //when arriving at the random position, change state to attack state
        if (!rifleBot.PlayerIsActive())
        {
            rifleBot.ChangeState(new IdleState(rifleBot));
        }
        else if (rifleBot.agent.remainingDistance <= 0.5f)
        {
            rifleBot.hasReposistioned = true;
            rifleBot.ChangeState(new AttackState(rifleBot));
        }
        else if (!rifleBot.IsInRange())
        {
            rifleBot.ChangeState(new ApproachState(rifleBot));
        }
    }

    public override void OnStateExit()
    {
        
    } 
}
