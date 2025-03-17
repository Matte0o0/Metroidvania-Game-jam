using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIState
{
    void Enter();
    void Execute();
    void FixedExecute();
    void Exit();
}
