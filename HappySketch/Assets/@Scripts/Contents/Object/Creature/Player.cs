using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using System;
using static Define;
using TMPro;


public enum EPlayerState
{
    None,
    Idle,
    Move,
    Jump,

    Hit,

    Collect, //수집
    Dead
}

public class Player : Creature
{
    #region input 변수
    [SerializeField]
    private float inputCooldown = 0.5f; // W와 S 키 쿨타임
    private float inputTime = 0f; // 입력시간 
    #endregion

    #region booster 변수
    [SerializeField]
    private int boosterCount = 0; // 부스터 게이지
    private const float boosterTime = 5f;  // 부스터 시간
    private float boosterTimer = 0f; // 부스터 현재시간
    #endregion

    #region hit 변수
    [SerializeField]
    private float hitInputIgnoreTime = 1.0f; // Hit 상태에서 입력 무시 시간
    private float hitTime = 0; // hit시간
    #endregion

    [SerializeField]
    private float moveSpeed = 5f; // 이동거리

    [SerializeField]
    private bool isUsingArrow; // 방향키 or wad
    private float jumpForce = 5f; // 점프 힘
    private int trackNum = 2; // 현재 트랙 위치
    [SerializeField, ReadOnly]
    private bool isJump = false; // 점프유무 3스테이지 용도

    #region playerState
    [SerializeField, ReadOnly]
    protected bool isPlayerStateLock = false;
    [SerializeField, ReadOnly]
    protected EPlayerState _playerState = EPlayerState.None;
    public virtual EPlayerState PlayerState
    {
        get { return _playerState; }
        protected set
        {

            if (value != EPlayerState.Idle && isPlayerStateLock)
                return;

            if (_playerState == EPlayerState.Dead)
                return;

            if (_playerState == value)
                return;

            bool isChangeState = true;
            switch (value)
            {
                case EPlayerState.Idle: isChangeState = IdleStateCondition(); break;
                case EPlayerState.Move: isChangeState = MoveStateCondition(); break;
                case EPlayerState.Jump: isChangeState = JumpStateCondition(); break;
                case EPlayerState.Hit: isChangeState = HitStateCondition(); break;
                case EPlayerState.Collect: break;
            }
            if (isChangeState == false)
                return;

            switch (_playerState)
            {
                case EPlayerState.Idle: IdleStateExit(); break;
                case EPlayerState.Move: MoveStateExit(); break;
                case EPlayerState.Jump: JumpStateExit(); break;
                case EPlayerState.Hit: HitStateExit(); break;
                case EPlayerState.Collect: break;

            }

            _playerState = value;
            // 추후 애니메이션 재생 들어가야함

            switch (value)
            {
                case EPlayerState.Idle: IdleStateEnter(); break;
                case EPlayerState.Move: MoveStateEnter(); break;
                case EPlayerState.Jump: JumpStateEnter(); break;
                case EPlayerState.Hit: HitStateEnter(); break;
                case EPlayerState.Collect: break;
            }
        }
    }
    #endregion

    #region inputControll
    [SerializeField, ReadOnly]
    private bool _isPlayerInputControll = false;

    public bool IsPlayerInputControll
    {
        get { return _isPlayerInputControll; }
        protected set
        {
            if (_isPlayerInputControll == value)
                return;

            _isPlayerInputControll = value;
            ConnectInputActions(value);

            if (_isPlayerInputControll)
            {

                if (coPlayerStateController == null)
                    coPlayerStateController = StartCoroutine(CoPlayerStateController());
            }
        }
    }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        this.gameObject.tag = ETag.Player.ToString();
        this.gameObject.layer = (int)ELayer.Player;
        CreatureType = ECreatureType.Player;
        PlayerState = EPlayerState.Idle;


        IsPlayerInputControll = true;
        trackNum = 2;
        targetPosition = beforePosition = transform.position;
        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        //base.SetInfo(templateID);

        //세팅 필요한경우 
    }


    #region Input
    private Vector2 moveDirection = Vector2.zero;

    private void ConnectInputActions(bool isConnect)
    {
        // 추후 wasd 따로 나누고 arrow 도 나눠서 캐릭터 2개 나눠야함


        Managers.Input.OnWASDKeyEntered -= OnArrowKey;
        Managers.Input.OnSpaceKeyEntered -= OnJumpKey;
        Managers.Input.OnArrowKeyEntered -= OnArrowKey;
        Managers.Input.OnSpaceKeyEntered -= OnBoosterKey;
        if (isConnect)
            if (isUsingArrow)
            {
                Managers.Input.OnWASDKeyEntered += OnArrowKey;
                Managers.Input.OnSpaceKeyEntered += OnBoosterKey;
            }
            else
            {
                Managers.Input.OnArrowKeyEntered += OnArrowKey;
                Managers.Input.OnSpaceKeyEntered += OnBoosterKey;
            }


    }
    public void OnArrowKey(Vector2 value)
    {

        moveDirection = value;
        PlayerState = EPlayerState.Move;

        if (value.y > 0)
        {
            if (inputTime < inputCooldown)
            {
                inputTime = 0f;
            }
            else
            { //전진 쿨일때 전진 X
                moveDirection = Vector2.zero;
            }
        }


    }

    public void OnJumpKey()
    {
        PlayerState = EPlayerState.Jump;
    }
    public void OnBoosterKey()
    {
        if (boosterCount == 3)
        {
            boosterTimer = 0;
            boosterCount = 0;
            inputCooldown = 0.25f;
        }

    }
    #endregion

    #region Idle
    protected virtual bool IdleStateCondition()
    {


        return true;
    }

    protected virtual void IdleStateEnter()
    {
        InitRigidVelocityX();
        isPlayerStateLock = false;

    }

    protected virtual void UpdateIdleState()
    {
        //InputUpdate();
    }

    protected virtual void IdleStateExit()
    {

    }
    #endregion

    #region Hit
    [SerializeField]
    private float hitBackDistance = 3.0f; // 뒤로 밀려나는 거리
    private Renderer playerRenderer; // 캐릭터의 렌더러

    private Coroutine blinkCoroutine;
    protected virtual bool HitStateCondition()
    {
        if (boosterTimer >= 0)
            return false;
        if (PlayerState == EPlayerState.Move)
        {
            transform.position = beforePosition;
            trackNum -= (int)moveDirection.x;
        }
        return true;
    }

    protected virtual void HitStateEnter()
    {
        isPlayerStateLock = true;
        InitRigidVelocityY();
        inputTime = 0;
        //move중에 히트시 해당위치로 이동


        // 뒤로 밀려나기
        SetRigidVelocity(-transform.forward * hitBackDistance);

        // 렌더러 가져오기
        playerRenderer = GetComponent<Renderer>();
        blinkCoroutine = StartCoroutine(BlinkCoroutine()); // 깜박임 코루틴 시작
    }

    protected virtual void UpdateHitState()
    {

        hitTime += Time.deltaTime;
        if (hitTime >= hitInputIgnoreTime)
        {
            hitTime = 0;
            PlayerState = EPlayerState.Idle; // 히트 상태 종료
        }
    }

    protected virtual void HitStateExit()
    {
        // 앞으로
        SetRigidVelocity(transform.forward * hitBackDistance);

        StopCoroutine(blinkCoroutine);

        isPlayerStateLock = false;

        if (playerRenderer != null)
        {
            playerRenderer.enabled = true; // 렌더러 활성화
        }

    }

    private IEnumerator BlinkCoroutine()
    {
        while (hitTime < hitInputIgnoreTime)
        {
            if (playerRenderer != null)
            {
                playerRenderer.enabled = !playerRenderer.enabled; // 가시성을 토글
            }
            yield return new WaitForSeconds(0.1f); // 0.1초 간격으로 깜박임
        }
    }

    #endregion

    #region Move
    private Vector3 targetPosition = Vector3.zero; // 이동할 위치
    private Vector3 beforePosition = Vector3.zero;
    protected virtual bool MoveStateCondition()
    {

        if (moveDirection.x == 0 && moveDirection.y <= 0 || isJump)
            return false;
        if (inputTime < inputCooldown && moveDirection.y > 0)
            return false;


        if (trackNum + (int)moveDirection.x < 0 || trackNum + (int)moveDirection.x > 3)
        {
            return false;
        }

        return true;
    }

    protected virtual void MoveStateEnter()
    {
        if (moveDirection.y > 0)
        {
            moveDirection.y *= moveSpeed;
        }

        beforePosition = transform.position;
        targetPosition = transform.position + new Vector3(moveDirection.x, 0, moveDirection.y);

    }

    protected virtual void UpdateMoveState()
    {
        Movement();
        if (transform.position == targetPosition)
        {
            PlayerState = EPlayerState.Idle; ;
        }


    }

    protected virtual void MoveStateExit()
    {
        if (hitTime > 0)
            beforePosition = transform.position;
    }

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, 0.03f); // 이동속도 data로 뺄수 있게 해줄것
    }
    #endregion

    #region Jump
    protected virtual bool JumpStateCondition()
    {
        if (isJump)
            return false;

        return true;
    }

    protected virtual void JumpStateEnter()
    {
        InitRigidVelocityY();
        SetRigidVelocityY(jumpForce);
    }

    protected virtual void UpdateJumpState()
    {

    }

    protected virtual void JumpStateExit()
    {

    }
    #endregion

    #region Collect
    protected virtual bool CollectStateCondition()
    {


        return true;
    }

    protected virtual void CollectStateEnter()
    {
        isPlayerStateLock = true;
    }

    protected virtual void UpdateCollectState()
    {

        //수집 시 필요한거 


    }

    protected virtual void CollectStateExit()
    {
        isPlayerStateLock = false;
    }
    #endregion

    #region co
    Coroutine coPlayerStateController = null;
    protected IEnumerator CoPlayerStateController()
    {
        float timer = 0.0f;
        while (IsPlayerInputControll)
        {
            if (true) // 죽어있지않을떄
            {
                timer += Time.deltaTime;
                if (timer >= 1.0f)
                {
                    timer -= 1.0f;
                }
            }
            if (inputTime < inputCooldown)
                inputTime += Time.deltaTime;
            BoosterTimeUpdate();

            switch (PlayerState)
            {
                case EPlayerState.Idle: UpdateIdleState(); break;
                case EPlayerState.Move: UpdateMoveState(); break;
                case EPlayerState.Jump: UpdateJumpState(); break;

                case EPlayerState.Hit: UpdateHitState(); break;

            }

            yield return null;
        }

        coPlayerStateController = null;
    }
    #endregion

    // 임시 hit
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            PlayerState = EPlayerState.Hit;
        }
        else if (other.CompareTag("Booster"))
        {
            boosterCount++;
            if (boosterCount > 3)
                boosterCount = 3;
        }
    }

    private void BoosterTimeUpdate()
    {
        if (boosterTimer >= 0)
        {
            boosterTimer += Time.deltaTime;
            if (boosterTimer >= boosterTime)
            {
                boosterTimer = -1;
                inputCooldown = 0.5f;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isJump = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isJump = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isJump = false;
        }
    }

}
