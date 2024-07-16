using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState(RifleBot rifleBot) : base(rifleBot) {}

    public override void OnStateEnter()
    {
        // set navmesh agent destination to null
        rifleBot.agent.SetDestination(rifleBot.transform.position);
    }

    public override void OnStateExecute()
    {
        if (rifleBot.PlayerIsActive())
        {
            rifleBot.ChangeState(new ApproachState(rifleBot));
        }
    }

    public override void OnStateExit()
    {
    }
}
