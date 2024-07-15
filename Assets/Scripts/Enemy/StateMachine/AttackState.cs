using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyState
{
    public AttackState(RifleBot rifleBot) : base(rifleBot) {}

    public override void OnStateEnter()
    {
        
    }

    public override void OnStateExecute()
    {
        PerformAttackBehaviour();
        CheckForTransition();
    }

    private void PerformAttackBehaviour()
    {
        // rotate model towards the player
        Vector3 direction = rifleBot.target.position - rifleBot.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        rifleBot.modelTransform.rotation = Quaternion.Slerp(rifleBot.modelTransform.rotation, lookRotation, Time.deltaTime * 5f);


        // if the player is in the field of view and the attack timer is less than or equal to 0, attack
        if (rifleBot.IsPlayerInFov() && rifleBot.IsPlayerVisible())
        {
            rifleBot.Shoot();
        }
        
    }

    private void CheckForTransition()
    {
        if (!rifleBot.PlayerIsActive())
        {
            rifleBot.ChangeState(new IdleState(rifleBot));
        }
        else if (!rifleBot.IsInRange())
        {
            rifleBot.ChangeState(new ApproachState(rifleBot));
        }
        else if (!rifleBot.hasReposistioned)
        {
            rifleBot.ChangeState(new ReposistionState(rifleBot));
        }
    }

    public override void OnStateExit()
    {
        
    }
}
