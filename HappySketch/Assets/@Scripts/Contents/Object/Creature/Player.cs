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
using UnityEngine.Playables;
using System.Text;
using static UnityEngine.GraphicsBuffer;


public enum EPlayerState
{
    None,
    Idle,
    Move,
    Jump,
    Run,
    Hit,
    LeftCollect, //수집
    RightCollect, //수집

    Dead
}

public class Player : Creature
{
    [SerializeField]
    private EStageType stageType;

    #region input 변수
    [SerializeField]
    private float inputCooldown = 0.5f; // W와 S 키 쿨타임
    [SerializeField, ReadOnly]
    private float inputTime = 0f; // 입력시간 
    #endregion

    #region booster 변수
    [SerializeField]
    private int boosterCount = 0; // 부스터 게이지
    private const float boosterTime = 5f;  // 부스터 시간
    private float boosterTimer = -1f; // 부스터 현재시간
    #endregion

    #region hit 변수
    [SerializeField]
    private float hitInputIgnoreTime = 1.0f; // Hit 상태에서 입력 무시 시간
    [SerializeField, ReadOnly]
    private float hitTime = -1; // hit시간
    #endregion

    [SerializeField]
    private float moveSpeed = 5f; // 이동거리

    [SerializeField]
    private bool isUsingArrow; // 방향키 or wad
    private float jumpForce = 5f; // 점프 힘
    [SerializeField, ReadOnly]
    private int trackNum = 2; // 현재 트랙 위치
    [SerializeField, ReadOnly]
    private bool isJump = false; // 점프유무 3스테이지 용도

    #region playerState
    [SerializeField, ReadOnly]
    protected bool isInputRock = false;
    [SerializeField, ReadOnly]
    protected EPlayerState _playerState = EPlayerState.None;
    public virtual EPlayerState PlayerState
    {
        get { return _playerState; }
        protected set
        {

            if (_playerState == EPlayerState.Dead)
            {
                return;
            }

            if (_playerState == value)
            {
                return;
            }

            bool isChangeState = true;
            switch (value)
            {
                case EPlayerState.Idle: isChangeState = IdleStateCondition(); break;
                case EPlayerState.Move: isChangeState = MoveStateCondition(); break;
                case EPlayerState.Jump: isChangeState = JumpStateCondition(); break;
                case EPlayerState.Hit: isChangeState = HitStateCondition(); break;
                case EPlayerState.LeftCollect: isChangeState = LeftCollectStateCondition(); break;
                case EPlayerState.RightCollect: isChangeState = RightCollectStateCondition(); break;
                case EPlayerState.Run: isChangeState = RunStateCondition(); break;
            }
            if (isChangeState == false)
            {
                return;
            }

            switch (_playerState)
            {
                case EPlayerState.Idle: IdleStateExit(); break;
                case EPlayerState.Move: MoveStateExit(); break;
                case EPlayerState.Jump: JumpStateExit(); break;
                case EPlayerState.Hit: HitStateExit(); break;
                case EPlayerState.LeftCollect: LeftCollectStateExit(); break;
                case EPlayerState.RightCollect: RightCollectStateExit(); break;
                case EPlayerState.Run: RunStateExit(); break;

            }

            _playerState = value;
            PlayAnimation(value);

            switch (value)
            {
                case EPlayerState.Idle: IdleStateEnter(); break;
                case EPlayerState.Move: MoveStateEnter(); break;
                case EPlayerState.Jump: JumpStateEnter(); break;
                case EPlayerState.Hit: HitStateEnter(); break;
                case EPlayerState.LeftCollect: LeftCollectStateEnter(); break;
                case EPlayerState.RightCollect: LeftCollectStateEnter(); break;
                case EPlayerState.Run: RunStateEnter(); break;
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
            {
                return;
            }

            _isPlayerInputControll = value;
            ConnectInputActions(value);

            if (_isPlayerInputControll)
            {

                if (coPlayerStateController == null)
                {
                    coPlayerStateController = StartCoroutine(CoPlayerStateController());
                }
            }
        }
    }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        this.gameObject.tag = ETag.Player.ToString();
        this.gameObject.layer = (int)ELayer.Player;
        CreatureType = ECreatureType.Player;
        //PlayerState = EPlayerState.Idle;


        IsPlayerInputControll = true;
        trackNum = 2;
        targetPosition = beforePosition = transform.position;
        SetInfo((int)stageType);
        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        stageType = (EStageType)templateID;
        switch (stageType)
        {
            case EStageType.None: PlayerState = EPlayerState.Run; break; // 추후 스테이지2 나오면  바꿔야함
            case EStageType.SharkAvoidance: PlayerState = EPlayerState.Idle; break;
        }


    }


    #region Input
    private Vector2 moveDirection = Vector2.zero;

    private void ConnectInputActions(bool isConnect)
    {

        switch (stageType)
        {
            case EStageType.None: Stage2ConnectInputActions(isConnect); break;
            case EStageType.SharkAvoidance: SharkAvoidanceConnectInputActions(isConnect); break;
                // 스테이지 추가
        }




    }

    #region SharkAvoidance

    public void SharkAvoidanceConnectInputActions(bool isConnect)
    {
        Managers.Input.OnWASDKeyEntered -= OnArrowKeySharkAvoidance;
        Managers.Input.OnArrowKeyEntered -= OnArrowKeySharkAvoidance;
        Managers.Input.OnSpaceKeyEntered -= OnBoosterKeySharkAvoidance;
        Managers.Input.OnEndKeyEntered -= OnBoosterKeySharkAvoidance;
        if (isConnect)
        {
            if (isUsingArrow)
            {
                Managers.Input.OnArrowKeyEntered += OnArrowKeySharkAvoidance;
                Managers.Input.OnEndKeyEntered += OnBoosterKeySharkAvoidance;
            }
            else
            {
                Managers.Input.OnWASDKeyEntered += OnArrowKeySharkAvoidance;
                Managers.Input.OnSpaceKeyEntered += OnBoosterKeySharkAvoidance;
            }
        }
    }

    public void OnArrowKeySharkAvoidance(Vector2 value)
    {
        if (isInputRock)
        {
            return;
        }

        moveDirection = value;
        PlayerState = EPlayerState.Move;

        if (value.y > 0)
        {
            if (inputTime >= inputCooldown)
            {
                inputTime = 0f;
            }
            else
            { //전진 쿨일때 전진 X
                moveDirection = Vector2.zero;
            }
        }


    }

    public void OnBoosterKeySharkAvoidance()
    {
        if (isInputRock)
        {
            return;
        }
        if (boosterCount == 3)
        {
            boosterTimer = 0;
            boosterCount = 0;
            inputCooldown = 0.25f;
        }

    }
    #endregion

    #region stage2
    public void Stage2ConnectInputActions(bool isConnect)
    {
        Managers.Input.OnWASDKeyEntered -= OnArrowKeySharkAvoidance;
        Managers.Input.OnArrowKeyEntered -= OnArrowKeySharkAvoidance;
        Managers.Input.OnSpaceKeyEntered -= OnBoosterKeySharkAvoidance;
        Managers.Input.OnEndKeyEntered -= OnBoosterKeySharkAvoidance;
        Managers.Input.OnWASDKeyEntered -= OnArrowKeyStage2;
        Managers.Input.OnArrowKeyEntered -= OnArrowKeyStage2;
        Managers.Input.OnSpaceKeyEntered -= OnJumpKey;
        if (isConnect)
        {
            if (isUsingArrow)
            {
                Managers.Input.OnArrowKeyEntered += OnArrowKeyStage2;
                Managers.Input.OnSpaceKeyEntered += OnJumpKey;
            }
            else
            {
                Managers.Input.OnWASDKeyEntered += OnArrowKeyStage2;
                Managers.Input.OnSpaceKeyEntered += OnJumpKey;
            }
        }

    }

    public void OnArrowKeyStage2(Vector2 value)
    {

        if (value.x > 0)
        {

            PlayerState = EPlayerState.RightCollect;
        }
        else if (value.x < 0)
        {
            PlayerState = EPlayerState.LeftCollect;
        }


    }

    #endregion

    #region stage3

    public void OnJumpKey()
    {
        if (isInputRock)
        {
            return;
        }

        PlayerState = EPlayerState.Jump;
    }
    #endregion

    #endregion

    #region Idle
    protected virtual bool IdleStateCondition()
    {


        return true;
    }

    protected virtual void IdleStateEnter()
    {
        InitRigidVelocityX();

    }

    protected virtual void UpdateIdleState()
    {

    }

    protected virtual void IdleStateExit()
    {

    }
    #endregion

    #region Hit
    [SerializeField]
    private float hitBackDistance = 3.0f; // 뒤로 밀려나는 거리

    protected virtual bool HitStateCondition()
    {
        if (boosterTimer >= 0)
        {
            return false;
        }
        return true;
    }

    protected virtual void HitStateEnter()
    {
        InitRigidVelocityY();
        isInputRock = true;
        hitTime = 0;




        // 뒤로 밀려나기
        this.transform.position = new Vector3(transform.position.x, beforePosition.y, transform.position.z);
        beforePosition = transform.position;
        targetPosition = transform.position;
        targetPosition.z -= hitBackDistance;

         
    }

    protected virtual void UpdateHitState()
    {
        Movement();
        hitTime += Time.deltaTime;
        if (hitTime >= hitInputIgnoreTime)
        {
            hitTime = -1;
            if (stageType == EStageType.None) // 추후 변경
                PlayerState = EPlayerState.Run;
            else
                PlayerState = EPlayerState.Idle; // 기절 상태 종료
        }
    }

    protected virtual void HitStateExit()
    {

        isInputRock = false;
    }

    #endregion


    #region Move
    [SerializeField, ReadOnly]
    private Vector3 targetPosition = Vector3.zero; // 이동할 위치
    [SerializeField, ReadOnly]
    private Vector3 beforePosition = Vector3.zero;
    protected virtual bool MoveStateCondition()
    {
        if (isInputRock)
        {
            return false;
        }

        if (moveDirection.x == 0 && moveDirection.y <= 0 || isJump)
        {
            return false;
        }
        if (inputTime < inputCooldown && moveDirection.y > 0)
        {
            return false;
        }


        if (trackNum + (int)moveDirection.x < 0 || trackNum + (int)moveDirection.x > 3)
        {
            return false;
        }

        return true;
    }

    protected virtual void MoveStateEnter()
    {

        trackNum += (int)moveDirection.x;
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
            PlayerState = EPlayerState.Idle;
        }


    }

    protected virtual void MoveStateExit()
    {
        if (targetPosition != transform.position)
        {
            transform.position = beforePosition;
            trackNum -= (int)moveDirection.x;
        }
    }

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed / inputCooldown * Time.deltaTime); // 이동속도 data로 뺄수 있게 해줄것
    }
    #endregion

    #region Jump
    protected virtual bool JumpStateCondition()
    {
        if (isJump)
        {
            return false;
        }

        return true;
    }

    protected virtual void JumpStateEnter()
    {
        beforePosition = transform.position;
        targetPosition = transform.position + Vector3.forward*moveSpeed; // 추후 이동거리로 뺄것
        // 목표 위치로의 벡터 계산
        Vector3 direction = targetPosition - transform.position;

        // 목표까지의 수평 거리와 목표 높이를 계산
        float distance = direction.magnitude;


        // 최종적으로 물리적인 점프를 위한 속도 계산
        Vector3 velocity = Vector3.forward * distance * moveSpeed + Vector3.up * jumpForce;
        InitRigidVelocityY();
        SetRigidVelocity(velocity);

       
    }

    protected virtual void UpdateJumpState()
    {

        if (rigid.velocity.y == 0)
        {
            PlayerState = EPlayerState.Run;
        }

    }

    
    protected virtual void JumpStateExit()
    {

    }
    #endregion

    #region Run
    protected virtual bool RunStateCondition()
    {

        return true;
    }

    protected virtual void RunStateEnter()
    {

        beforePosition = transform.position;
        targetPosition =  new Vector3(0, transform.position.y, 1000); // 추후 트랙 끝 position 받아올것

    }

    protected virtual void UpdateRunState()
    {
        Running();

        if (transform.position == targetPosition)
        {
            PlayerState = EPlayerState.Idle;
        }


    }

    protected virtual void RunStateExit()
    {

    }
    protected void Running()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.01f * moveSpeed ); // 추후 2스테이지 data로 뺄것
    }


    #endregion


    #region Left Collect
    protected virtual bool LeftCollectStateCondition()
    {
        if (PlayerState != EPlayerState.Run)
            return false;
        return true;
    }

    protected virtual void LeftCollectStateEnter()
    {

    }

    protected virtual void UpdateLeftCollectState()
    {
        Running();
        //수집 시 필요한거 
        if (IsEndCurrentState(PlayerState))
        {
            PlayerState = EPlayerState.Run;
        }

    }

    protected virtual void LeftCollectStateExit()
    {
    }
    #endregion


    #region Right Collect
    protected virtual bool RightCollectStateCondition()
    {
        if (PlayerState != EPlayerState.Run)
            return false;
        return true;
    }

    protected virtual void RightCollectStateEnter()
    {

    }

    protected virtual void UpdateRightCollectState()
    {
        Running();
        //수집 시 필요한거 
        if (IsEndCurrentState(PlayerState))
        {
            PlayerState = EPlayerState.Run;
        }

    }

    protected virtual void RightCollectStateExit()
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
            }

            if (inputTime < inputCooldown)
            {
                inputTime += Time.deltaTime;
            }
            BoosterTimeUpdate();

            switch (PlayerState)
            {
                case EPlayerState.Idle: UpdateIdleState(); break;
                case EPlayerState.Move: UpdateMoveState(); break;
                case EPlayerState.Jump: UpdateJumpState(); break;
                case EPlayerState.Hit: UpdateHitState(); break;
                case EPlayerState.Run: UpdateRunState(); break;
                    case EPlayerState.LeftCollect : UpdateLeftCollectState(); break;
                    case EPlayerState.RightCollect : UpdateRightCollectState(); break;  
            }


            //yield return new WaitForSeconds(0.01f);
            yield return null;
        }

        coPlayerStateController = null;
    }
    #endregion

    #region Animation
    protected void PlayAnimation(EPlayerState state)
    {
        if (animator == null)
        {
            return;
        }

        animator.Play(state.ToString());
    }

    protected bool IsState(AnimatorStateInfo stateInfo, EPlayerState state)
    {
        return stateInfo.IsName(state.ToString());
    }

    public bool IsState(EPlayerState state)
    {
        if (animator == null)
        {
            return false;
        }

        return IsState(animator.GetCurrentAnimatorStateInfo(0), state);
    }

    public bool IsEndCurrentState(EPlayerState state)
    {
        if (animator == null)
        {
            Debug.LogWarning("animator is Null");
            return false;
        }

        // 다른 애니메이션이 재생 중
        if (!IsState(state))
        {
            return false;
        }

        return (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        
    }
    #endregion

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
            {
                boosterCount = 3;
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
