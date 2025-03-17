using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterState
{
    void Enter();
    void Execute();
    void FixedExecute();
    void Exit();
}

//{
//    void Enter(PlayerInput playerInput);
//    void Execute(PlayerInput playerInput);
//    void Exit(PlayerInput playerInput);
//}
