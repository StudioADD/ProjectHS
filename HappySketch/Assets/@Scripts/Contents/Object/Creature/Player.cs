using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using static Define;


public enum EPlayerState
{
    None,
    Idle,
    Move,
    Jump,

    Hit,

    Dead
}

public class Player : Creature
{
    [SerializeField]
    private float inputCooldown = 0.5f; // W와 S 키 쿨타임
    private float inputTime = 0f;
    private bool IsInput = true; // 입력 가능여부
    [SerializeField]
    private float hitInputIgnoreTime = 1.0f; // Hit 상태에서 입력 무시 시간
    private float hitTime = 0;
    private bool IsHit = false; // 히트상태 여부
    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private bool isUsingWASD;


    #region playerState
    [SerializeField, ReadOnly] protected bool isPlayerStateLock = false;
    [SerializeField, ReadOnly] protected EPlayerState _playerState = EPlayerState.None;
    public virtual EPlayerState PlayerState
    {
        get { return _playerState; }
        protected set
        {
            Debug.LogWarning(moveDirection);
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
            }
            if (isChangeState == false)
                return;

            switch (_playerState)
            {
                case EPlayerState.Idle: IdleStateExit(); break;
                case EPlayerState.Move: MoveStateExit(); break;
                case EPlayerState.Jump: JumpStateExit(); break;

                case EPlayerState.Hit: HitStateExit(); break;

            }

            _playerState = value;
            // 추후 애니메이션 재생 들어가야함

            switch (value)
            {
                case EPlayerState.Idle: IdleStateEnter(); break;
                case EPlayerState.Move: MoveStateEnter(); break;
                case EPlayerState.Jump: JumpStateEnter(); break;

                case EPlayerState.Hit: HitStateEnter(); break;
            }
        }
    }
    #endregion

    #region inputControll
    [SerializeField, ReadOnly] private bool _isPlayerInputControll = false;

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
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        this.gameObject.tag = ETag.Player.ToString();
        this.gameObject.layer = (int)ELayer.Player;
        CreatureType = ECreatureType.Player;
        PlayerState = EPlayerState.Idle;


        IsPlayerInputControll = true;
        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        //base.SetInfo(templateID);

        //세팅 필요한경우 
    }
    #endregion

    #region Input
    private Vector2 moveDirection = Vector2.zero;

    private void ConnectInputActions(bool isConnect)
    {
        // 추후 wasd 따로 나누고 arrow 도 나눠서 캐릭터 2개 나눠야함


        Managers.Input.OnWASDKeyEntered -= OnArrowKey;
        Managers.Input.OnSpaceKeyEntered -= OnJumpKey;
        Managers.Input.OnArrowKeyEntered -= OnArrowKey;

        if (isConnect)
            if (isUsingWASD)
            {
                Managers.Input.OnWASDKeyEntered += OnArrowKey;
                Managers.Input.OnSpaceKeyEntered += OnJumpKey;
            }
            else
            {
                Managers.Input.OnArrowKeyEntered += OnArrowKey;
                Managers.Input.OnSpaceKeyEntered += OnJumpKey;
            }


    }
    public void OnArrowKey(Vector2 value)
    {

        if (isPlayerStateLock == false && IsInput == true)
        {
            Debug.LogWarning(IsInput);
            if (value.y > 0) // W 키
            {
                // 위 진행시 따로 처리 예정
            }
            else
            {
                moveDirection = value;
            }
            PlayerState = EPlayerState.Move;
        }
        else
            moveDirection = Vector2.zero;

    }




    public void OnJumpKey()
    {
        PlayerState = EPlayerState.Jump;
    }
    #endregion

    #region Idle
    protected virtual bool IdleStateCondition()
    {
        if (moveDirection != Vector2.zero)
            return false;


        return true;
    }

    protected virtual void IdleStateEnter()
    {
        InitRigidVelocityX();
        isPlayerStateLock = false;
    }

    protected virtual void UpdateIdleState()
    {
        if (inputTime < inputCooldown)
            IsInput = false;
        else
            IsInput = true;
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
        if (inputTime < inputCooldown)
            return false;
        return true;
    }

    protected virtual void HitStateEnter()
    {
        isPlayerStateLock = true;
        InitRigidVelocityY();
        inputTime = 0;

        // 원래 위치 저장

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
            IsHit = false;
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
    protected virtual bool MoveStateCondition()
    {
        if (moveDirection.x == 0)
            return false;


        return true;
    }

    protected virtual void MoveStateEnter()
    {

    }

    protected virtual void UpdateMoveState()
    {
        Debug.LogWarning(IsInput);
        if (IsInput)
        {
            Movement();
            IsInput = false;
            inputTime = 0;
        }
        if (moveDirection.x == 0)
            PlayerState = EPlayerState.Idle;
    }

    protected virtual void MoveStateExit()
    {

    }

    private void Movement()
    {
        SetRigidVelocity(moveDirection * 10f); // 추후 타일간 이동으로 바꾸면 될듯
    }
    #endregion

    #region Jump
    protected virtual bool JumpStateCondition()
    {

        return true;
    }

    protected virtual void JumpStateEnter()
    {
        InitRigidVelocityY();
        SetRigidVelocityY(jumpForce);
    }

    protected virtual void UpdateJumpState()
    {
        //수정 요망 점프하자마자 바뀜 다른거 필요할듯
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.36f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                PlayerState = EPlayerState.Idle; // 바닥에 닿으면 Idle 상태로 변경
            }
        }
    }

    protected virtual void JumpStateExit()
    {

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
                if (!IsInput)
                    inputTime += Time.deltaTime; ;
                if (IsHit)
                    hitTime += Time.deltaTime; ;
            }

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
    }

}
