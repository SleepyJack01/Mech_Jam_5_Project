using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    protected RifleBot rifleBot;

    public EnemyState(RifleBot rifleBot)
    {
        this.rifleBot = rifleBot;
    }

    public abstract void OnStateEnter();
    public abstract void OnStateExecute();
    public abstract void OnStateExit();
}
