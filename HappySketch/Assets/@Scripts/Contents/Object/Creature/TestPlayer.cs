using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.Playables;

public class TestPlayer : Creature
{
    private void Start()
    {
        ConnectInputActions(true);
    }

    private void Update()
    {
        SetRigidVelocity(moveDirection);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        this.gameObject.tag = ETag.Player.ToString();
        this.gameObject.layer = (int)ELayer.Player;
        CreatureType = ECreatureType.Player;
        
        return true;
    }

    public override void SetInfo(int templateID = 0)
    {

    }

    #region Input
    private Vector2 moveDirection = Vector2.zero;

    private void ConnectInputActions(bool isConnect)
    {
        Managers.Input.OnWASDKeyEntered -= OnArrowKey;
        Managers.Input.OnSpaceKeyEntered -= OnJumpKey;

        if (isConnect)
        {
            Managers.Input.OnWASDKeyEntered += OnArrowKey;
            Managers.Input.OnSpaceKeyEntered += OnJumpKey;
        }
    }

    public void OnArrowKey(Vector2 value)
    {
        moveDirection = value;
    }

    public void OnJumpKey()
    {
        InitRigidVelocityY();
        SetRigidVelocityY(5f);
    }
    #endregion
}
