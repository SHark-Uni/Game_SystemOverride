using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SitState : PlayerOnGroundState
{
    private BoxCollider2D _boxCol;
    public SitState(Player_Temp owner, StateMachine<Player_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
        : base(owner, stateMachine, name, rb, am)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void EntityUpdate()
    {
        base.EntityUpdate();

        OnSitDown();
    }

    void OnSitDown()
    {
        if (_playerMove.SitDown.WasPressedThisFrame())
        {
            _am.SetBool("isSitDown", true);
            _boxCol.size = new Vector2(0.3f, 0.25f);
            _boxCol.offset = new Vector2(0f, -0.2f);

        }
        else if (_playerMove.SitDown.WasReleasedThisFrame())
        {
            _am.SetBool("isSitDown", false);
            _boxCol.size = new Vector2(0.3f, 0.5f);
            _boxCol.offset = new Vector2(0f, -0.1f);
        }
    }
}
