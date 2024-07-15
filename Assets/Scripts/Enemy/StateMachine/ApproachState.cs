using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachState : EnemyState
{
    public ApproachState(RifleBot rifleBot) : base(rifleBot) {}

    public override void OnStateEnter()
    {

    }

    public override void OnStateExecute()
    {
        PerformApproachBehaviour();
        CheckForTransition();
    }

    private void PerformApproachBehaviour()
    {
        // rotate model towards the player
        Vector3 direction = rifleBot.target.position - rifleBot.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        rifleBot.modelTransform.rotation = Quaternion.Slerp(rifleBot.modelTransform.rotation, lookRotation, Time.deltaTime * 5f);


        Vector3 randomDirection = new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));
        Vector3 randomPosition = rifleBot.target.position + randomDirection;

        rifleBot.agent.SetDestination(randomPosition);

    }

    private void CheckForTransition()
    {
        if (!rifleBot.PlayerIsActive())
        {
            rifleBot.ChangeState(new IdleState(rifleBot));
        }
        else if(rifleBot.IsInRange())
        {
            rifleBot.hasReposistioned = true;
            rifleBot.ChangeState(new ReposistionState(rifleBot));
        }
    }

    public override void OnStateExit()
    {
        
    }
}
