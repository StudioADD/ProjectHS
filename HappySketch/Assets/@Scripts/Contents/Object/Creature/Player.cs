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
using Data;
using System.Data.Common;
using UnityEditor.Profiling.Memory.Experimental;


public enum EPlayerState
{
    None,
    Idle,
    Move,
    Swimming,
    MoveSwimming,

    JumpUp,
    Jump,
    Landing,

    Run,
    Hit,
    Dizz,
    GoBack, // 기절후 회복

    LeftCollect, //수집
    RightCollect, //수집

    Dead
}

public class Player : Creature
{
    [SerializeField]
    private EStageType stageType;

    [SerializeField]
    private GameObject[] Body = new GameObject[3];




    [SerializeField, ReadOnly] private JPlayerData data = null;

    [SerializeField, ReadOnly]
    private float inputTime = 0f; // 입력시간 

    [SerializeField, ReadOnly]
    private float moveDistance = 5f;

    private float moveSpeed = 5f;
    private float inputCooldown = 0.5f;



    private float boosterTimer = -1f; // 부스터 현재시간

    [SerializeField, ReadOnly]
    private float hitTime = -1; // hit시간

    [SerializeField]
    private ETeamType _teamType;

    public virtual ETeamType TeamType
    {
        get { return _teamType; }
        set
        {
            if (value == _teamType) return;
            _teamType = value;
        }
    }
    [SerializeField, ReadOnly]
    private List<GameObject> items = new List<GameObject>();

    [SerializeField, ReadOnly]
    private int trackNum = 2; // 현재 트랙 위치
    [SerializeField, ReadOnly]
    private bool IsJump = false;

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

            if (_playerState != EPlayerState.Move && _playerState == value)
            {
                return;
            }


            bool isChangeState = true;
            switch (value)
            {
                case EPlayerState.Idle: isChangeState = IdleStateCondition(); break;
                case EPlayerState.Move: isChangeState = MoveStateCondition(); break;
                case EPlayerState.Swimming: isChangeState = SwimmingStateCondition(); break;
                case EPlayerState.MoveSwimming: isChangeState = MoveSwimmingStateCondition(); break;
                case EPlayerState.JumpUp: isChangeState = JumpUpStateCondition(); break;
                case EPlayerState.Jump: isChangeState = JumpStateCondition(); break;
                case EPlayerState.Landing: isChangeState = JumpStateCondition(); break;
                case EPlayerState.Hit: isChangeState = HitStateCondition(); break;
                case EPlayerState.Dizz: isChangeState = DizzStateCondition(); break;
                case EPlayerState.GoBack: isChangeState = GoBackStateCondition(); break;
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
                case EPlayerState.Swimming: SwimmingStateExit(); break;
                case EPlayerState.MoveSwimming: MoveSwimmingStateExit(); break;
                case EPlayerState.JumpUp: JumpUpStateExit(); break;
                case EPlayerState.Jump: JumpStateExit(); break;
                case EPlayerState.Landing: LandingStateExit(); break;
                case EPlayerState.Hit: HitStateExit(); break;
                case EPlayerState.Dizz: DizzStateExit(); break;
                case EPlayerState.GoBack: GoBackStateExit(); break;
                case EPlayerState.LeftCollect: LeftCollectStateExit(); break;
                case EPlayerState.RightCollect: RightCollectStateExit(); break;
                case EPlayerState.Run: RunStateExit(); break;

            }

            _playerState = value;
            //PlayAnimation(value);
            animator.SetTrigger(value.ToString() + "_Trigger");

            switch (value)
            {
                case EPlayerState.Idle: IdleStateEnter(); break;
                case EPlayerState.Move: MoveStateEnter(); break;
                case EPlayerState.Swimming: SwimmingStateEnter(); break;
                case EPlayerState.MoveSwimming: MoveSwimmingStateEnter(); break;
                case EPlayerState.JumpUp: JumpUpStateEnter(); break;
                case EPlayerState.Jump: JumpStateEnter(); break;
                case EPlayerState.Landing: LandingStateEnter(); break;
                case EPlayerState.Hit: HitStateEnter(); break;
                case EPlayerState.Dizz: DizzStateEnter(); break;
                case EPlayerState.GoBack: GoBackStateEnter(); break;
                case EPlayerState.LeftCollect: LeftCollectStateEnter(); break;
                case EPlayerState.RightCollect: RightCollectStateEnter(); break;
                case EPlayerState.Run: RunStateEnter(); break;
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
        stageType = Managers.Game.GetCurrStageType();
        SetScale();
        trackNum = 2;
        targetPosition = beforePosition = transform.position;
        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        stageType = (EStageType)templateID;
        // data = Managers.Data.PlayerDict[(int)stageType];
        switch (stageType)
        {
            case EStageType.CollectingCandy:
                PlayerState = EPlayerState.Run;
                //임시 스테이지 endpoint로
                targetPosition = transform.position;
                targetPosition.z += 1000;
                break; // 추후 스테이지2 나오면  바꿔야함
            default:
                PlayerState = EPlayerState.Idle;
                break;
        }
        IsPlayerInputControll = true;
    }
    public virtual void SetScale()
    {
        switch (stageType)
        {
            case EStageType.SharkAvoidance:
                this.gameObject.transform.localScale = new Vector3(2, 2, 2);
                break;
            default:
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                break;
        }
    }
    #region SharkAvoidanceStage Event
    Action onAddBoosterItem;
    Func<bool> onUseBoosterItem;
    public void ConnectSharkAvoidanceStage(Action onAddBoosterItem, Func<bool> onUseBoosterItem)
    {
        this.onAddBoosterItem = onAddBoosterItem;
        this.onUseBoosterItem = onUseBoosterItem;
    }
    #endregion

    #region CollectingCandyStage Event
    Action<ECandyItemType> onCollectCandyItem;
    Action<bool> onChangeScoreBuff;
    public void ConnectCollectingCandyStage(Action<ECandyItemType> onCollectCandyItem, Action<bool> onChangeScoreBuff)
    {
        this.onCollectCandyItem = onCollectCandyItem;
        this.onChangeScoreBuff = onChangeScoreBuff;
    }
    #endregion

    #region CrossingBridgeStage Event
    /// <summary>
    /// Id, IsLeft, TargetPos
    /// </summary>
    Func<int, bool, Vector3> getJumpTargetPos;
    Action onAddGoggleItem;
    Func<bool> onUseGoggleItem;

    public void ConnectCrossingBridgeStage(Func<int, bool, Vector3> getJumpTargetPos, Action onAddGoggleItem, Func<bool> onUseGoggleItem)
    {
        this.getJumpTargetPos = getJumpTargetPos;
        this.onAddGoggleItem = onAddGoggleItem;
        this.onUseGoggleItem = onUseGoggleItem;
    }
    #endregion

    #region Input

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

    private Vector2 moveDirection = Vector2.zero;


    private void ConnectInputActions(bool isConnect)
    {
        UnConnectInputActions();
        switch (stageType)
        {

            case EStageType.SharkAvoidance: SharkAvoidanceConnectInputActions(isConnect); break;
            case EStageType.CollectingCandy: Stage2ConnectInputActions(isConnect); break;
            case EStageType.CrossingBridge: Stage3ConnectInputActions(isConnect); break;
                // 스테이지 추가
        }
    }
    private void UnConnectInputActions()
    {
        Managers.Input.OnArrowKeyEntered -= OnArrowKeySharkAvoidance;
        Managers.Input.OnSpaceKeyEntered -= OnBoosterKeySharkAvoidance;

        Managers.Input.OnWASDKeyEntered -= OnArrowKeySharkAvoidance;
        Managers.Input.OnEndKeyEntered -= OnBoosterKeySharkAvoidance;

        Managers.Input.OnArrowKeyEntered -= OnArrowKeyStage2;
        Managers.Input.OnSpaceKeyEntered -= OnJumpKey;

        Managers.Input.OnWASDKeyEntered -= OnArrowKeyStage2;
        Managers.Input.OnEndKeyEntered -= OnJumpKey;

        Managers.Input.OnNum1KeyEntered -= OnHit;
        Managers.Input.OnNum1KeyEntered += OnHit;
    }

    #region SharkAvoidance

    public void SharkAvoidanceConnectInputActions(bool isConnect)
    {
        if (isConnect)
        {
            if (TeamType == ETeamType.Left)
            {
                Managers.Input.OnWASDKeyEntered += OnArrowKeySharkAvoidance;
                Managers.Input.OnSpaceKeyEntered += OnBoosterKeySharkAvoidance;
            }
            else
            {
                Managers.Input.OnArrowKeyEntered += OnArrowKeySharkAvoidance;
                Managers.Input.OnEndKeyEntered += OnBoosterKeySharkAvoidance;
            }
        }
    }

    public void OnArrowKeySharkAvoidance(Vector2 value)
    {
        Debug.LogWarning(value);
        if (isInputRock)
        {
            return;
        }
        if (value.x == 0 && value.y == 0)
        {
            return;
        }
        if (value.y < 0)
        {
            return;
        }
        moveDirection = value;

        if (PlayerState == EPlayerState.Idle)
        {
            if (value.x == 0)
            {
                if (value.y > 0 && inputTime >= inputCooldown)
                {
                    inputTime = 0f;
                    PlayerState = EPlayerState.Swimming;
                }
            }
            else if (value.x != 0)
            {
                if (value.y > 0)
                    PlayerState = EPlayerState.MoveSwimming;
                else
                    PlayerState = EPlayerState.Move;
            }
        }
        else
        {
            PlayerState = EPlayerState.MoveSwimming;
        }



    }

    public void OnBoosterKeySharkAvoidance()
    {
        if (isInputRock)
        {
            return;
        }
        if (onUseBoosterItem())
        {
            boosterTimer = 0;
            inputCooldown = 0.25f;
        }

    }
    #endregion

    #region stage2
    public void Stage2ConnectInputActions(bool isConnect)
    {
        if (isConnect)
        {
            if (TeamType == ETeamType.Left)
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

    public void Stage3ConnectInputActions(bool isConnect)
    {
        if (isConnect)
        {
            if (TeamType == ETeamType.Left)
            {
                Managers.Input.OnArrowKeyEntered += OnArrowKeyStage3;
                Managers.Input.OnSpaceKeyEntered += OnJumpKey;
            }
            else
            {
                Managers.Input.OnWASDKeyEntered += OnArrowKeyStage3;
                Managers.Input.OnSpaceKeyEntered += OnJumpKey;
            }
        }

    }

    public void OnArrowKeyStage3(Vector2 value)
    {

        if (value.x > 0)
        {
            // 오른쪽 점프 위치 
            //targetPostion = 우 
        }
        else if (value.x < 0)
        {
            // 왼쪽 점프 위치 
            //targetPostion = 좌
        }


    }

    #endregion
    public void OnJumpKey()
    {
        if (isInputRock)
        {
            return;
        }

        PlayerState = EPlayerState.JumpUp;
    }
    #endregion

    #region PlayerState

    #region Idle
    protected virtual bool IdleStateCondition()
    {


        return true;
    }

    protected virtual void IdleStateEnter()
    {

    }

    protected virtual void UpdateIdleState()
    {
    }

    protected virtual void IdleStateExit()
    {

    }
    #endregion

    #region Hit
    public void OnHit()
    {

        switch (stageType)
        {
            case EStageType.CollectingCandy:
                StartCoroutine(CoBlinkingEffect());

                break;
            case EStageType.SharkAvoidance:
                PlayerState = EPlayerState.Hit;
                break;

        }

    }
    protected virtual bool HitStateCondition()
    {
        if (boosterTimer >= 0)
        {
            return false;
        }
        if (PlayerState == EPlayerState.Dizz || PlayerState == EPlayerState.GoBack)
            return false;
        IsJump = false;

        return true;
    }

    protected virtual void HitStateEnter()
    {
        InitRigidVelocityY();
        isInputRock = true;
        hitTime = 0;




        // 뒤로 밀려나기
        targetPosition = new Vector3(beforePosition.x, beforePosition.y, beforePosition.z);
        beforePosition = transform.position;

        targetPosition.z -= 3f;


    }

    protected virtual void UpdateHitState()
    {
        Movement();
        hitTime += Time.deltaTime;
        if (transform.position == targetPosition)
        {
            PlayerState = EPlayerState.Dizz;
        }
    }

    protected virtual void HitStateExit()
    {


    }
    //점멸효과

    protected IEnumerator CoBlinkingEffect()
    {
        float time = 0f;
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while (time <= 3f)
        {
            foreach (GameObject go in Body)
            {
                go.SetActive(!go.activeSelf);
            }
            yield return wait;
        }
        foreach (GameObject go in Body)
        {
            go.SetActive(true);
        }
    }

    #endregion

    #region Dizz


    protected virtual bool DizzStateCondition()
    {

        return true;
    }

    protected virtual void DizzStateEnter()
    {



    }

    protected virtual void UpdateDizzState()
    {
        hitTime += Time.deltaTime;
        if (hitTime >= 2.5f)
        {
            if (stageType == EStageType.CollectingCandy) // 추후 변경
                PlayerState = EPlayerState.Run;
            else
                PlayerState = EPlayerState.GoBack;

        }
    }

    protected virtual void DizzStateExit()
    {

        isInputRock = false;
    }

    #endregion

    #region GoBack


    protected virtual bool GoBackStateCondition()
    {
        if (PlayerState != EPlayerState.Dizz)
            return false;
        return true;
    }

    protected virtual void GoBackStateEnter()
    {
        targetPosition = transform.position + new Vector3(0, 0, 3f);
        beforePosition = transform.position;


    }

    protected virtual void UpdateGoBackState()
    {
        Movement();
        hitTime += Time.deltaTime;
        if (hitTime >= 3f)
        {
            hitTime = -1;
            if (stageType == EStageType.CollectingCandy) // 추후 변경
                PlayerState = EPlayerState.Run;
            else
                PlayerState = EPlayerState.Idle; // 기절 상태 종료
        }
    }

    protected virtual void GoBackStateExit()
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
            moveDirection.y *= moveDistance;
        }
        Debug.LogWarning(moveDirection);
        Debug.LogWarning(transform.position);
        beforePosition = transform.position;
        targetPosition = transform.position + new Vector3(moveDirection.x * moveDistance, 0, moveDirection.y);
        Debug.LogWarning(targetPosition);
    }

    protected virtual void UpdateMoveState()
    {
        Movement();
        if (transform.position == targetPosition)
        {
            Debug.LogWarning(targetPosition);
            PlayerState = EPlayerState.Idle;
        }


    }

    protected virtual void MoveStateExit()
    {

    }

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, data.MoveSpeed * Time.deltaTime);// 1 / data.inputCooldown * Time.deltaTime); // 이동속도 data로 뺄수 있게 해줄것
    }
    #endregion

    #region Swimming

    protected virtual bool SwimmingStateCondition()
    {

        if (trackNum + (int)moveDirection.x < 0 || trackNum + (int)moveDirection.x > 3)
        {
            return false;
        }


        return true;
    }

    protected virtual void SwimmingStateEnter()
    {
        trackNum += (int)moveDirection.x;
        if (moveDirection.y > 0)
        {
            moveDirection.y *= 5;
        }

        beforePosition = transform.position;
        targetPosition = transform.position + new Vector3(moveDirection.x * moveDistance, 0, moveDirection.y);

    }

    protected virtual void UpdateSwimmingState()
    {
        Movement();
        if (transform.position == targetPosition)
        {
            PlayerState = EPlayerState.Idle;
        }


    }

    protected virtual void SwimmingStateExit()
    {
        //onMoveEvent();
    }


    #endregion

    #region MoveSwimming

    protected virtual bool MoveSwimmingStateCondition()
    {


        if (trackNum + (int)moveDirection.x < 0 || trackNum + (int)moveDirection.x > 3)
        {
            return false;
        }
        if (hitTime > 0)
        {
            return false;
        }
        if (targetPosition.z + moveDirection.y == transform.position.z)
        {
            return false;
        }
        if(targetPosition.z != transform.position.z)
        {
            moveDirection.y = 0;
        }
        if(targetPosition.x != transform.position.x)
        {
            moveDirection.x = 0;
        }



        return true;
    }

    protected virtual void MoveSwimmingStateEnter()
    {
        isInputRock = true;
        float x = 0, z = 0;
        if (targetPosition.z == transform.position.z)
        {
            z = moveDirection.y * moveDistance;
        }
        else
        {
            trackNum +=(int) moveDirection.x;
            x = moveDirection.x * moveDistance;
        }



        targetPosition += new Vector3(x, 0, z);
        this.transform.forward = (targetPosition - this.transform.position).normalized;
        //Debug.LogWarning($" targetPosition : {targetPosition}");
    }

    protected virtual void UpdateMoveSwimmingState()
    {
        Movement();
        if (transform.position == targetPosition)
        {
            PlayerState = EPlayerState.Idle;
        }


    }

    protected virtual void MoveSwimmingStateExit()
    {
        isInputRock = false;
        if (targetPosition != transform.position)
        {
            transform.position = beforePosition;
            trackNum -= (int)moveDirection.x;
        }
        transform.forward = new Vector3(0, 0, 1);
        //onMoveEvent();
    }


    #endregion

    #region JumpUp
    protected virtual bool JumpUpStateCondition()
    {
        if (IsJump)
            return false;

        IsJump = true;

        return true;
    }

    protected virtual void JumpUpStateEnter()
    {



    }

    protected virtual void UpdateJumpUpState()
    {

        if (IsEndCurrentState(PlayerState))
        {
            PlayerState = EPlayerState.Jump;
        }

    }


    protected virtual void JumpUpStateExit()
    {

    }
    #endregion

    #region Jump
    protected virtual bool JumpStateCondition()
    {


        return true;
    }

    protected virtual void JumpStateEnter()
    {
        beforePosition = transform.position;
        targetPosition = transform.position + Vector3.forward * data.MoveSpeed; // 추후 이동거리로 뺄것
        if (false)
        {
            //targetPosition 받아올것
        }

        // 목표 위치로의 벡터 계산
        Vector3 direction = targetPosition - transform.position;

        // 목표까지의 수평 거리와 목표 높이를 계산
        float distance = direction.magnitude;


        // 최종적으로 물리적인 점프를 위한 속도 계산
        Vector3 velocity = direction.normalized * distance * moveDistance + Vector3.up * data.JumpPower;
        InitRigidVelocityY();
        SetRigidVelocity(velocity);


    }

    protected virtual void UpdateJumpState()
    {

        if (rigid.velocity.y == 0)
        {
            PlayerState = EPlayerState.Landing;
        }

    }


    protected virtual void JumpStateExit()
    {
        targetPosition = Vector3.zero;
    }
    #endregion

    #region Landing
    protected virtual bool LandingStateCondition()
    {

        return true;
    }

    protected virtual void LandingStateEnter()
    {
        InitRigidVelocity();


    }

    protected virtual void UpdateLandingState()
    {
        if (IsEndCurrentState(PlayerState))
            switch (stageType)
            {
                case EStageType.CrossingBridge:
                    PlayerState = EPlayerState.Idle; break;

                case EStageType.CollectingCandy: PlayerState = EPlayerState.Run; break;
            }




    }


    protected virtual void LandingStateExit()
    {

        IsJump = false;
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
        targetPosition = new Vector3(transform.position.x, transform.position.y, 1000); // 추후 트랙 끝 position 받아올것
        //Running();
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
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.01f * moveDistance); // 추후 2스테이지 data로 뺄것
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
        CollectItem(true);
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
        CollectItem(false);
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
            boosterTimeUpdate();

            switch (PlayerState)
            {
                case EPlayerState.Idle: UpdateIdleState(); break;
                case EPlayerState.Move: UpdateMoveState(); break;
                case EPlayerState.Swimming: UpdateSwimmingState(); break;
                case EPlayerState.MoveSwimming: UpdateMoveSwimmingState(); break;
                case EPlayerState.JumpUp: UpdateJumpUpState(); break;
                case EPlayerState.Jump: UpdateJumpState(); break;
                case EPlayerState.Landing: UpdateLandingState(); break;
                case EPlayerState.Hit: UpdateHitState(); break;
                case EPlayerState.Dizz: UpdateDizzState(); break;
                case EPlayerState.GoBack: UpdateGoBackState(); break;
                case EPlayerState.Run: UpdateRunState(); break;
                case EPlayerState.LeftCollect: UpdateLeftCollectState(); break;
                case EPlayerState.RightCollect: UpdateRightCollectState(); break;
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

    #region Booster

    private void boosterTimeUpdate()
    {
        if (boosterTimer >= 0)
        {
            boosterTimer += Time.deltaTime;
            if (boosterTimer >= 3)
            {
                boosterTimer = -1;
                onUseBoosterItem();
            }
        }
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
            onAddBoosterItem();
        }

    }

    protected void CollectItem(bool isLeft)
    {
        if (items.Count > 0)
            foreach (GameObject item in items)
            {
                if (item != null)
                {
                    if (isLeft && item.transform.position.x < this.transform.position.x)
                        Destroy(item); // 추후 수집 param 필요
                    else if (!isLeft && item.transform.position.x > this.transform.position.x)
                        Destroy(item);// 추후 수집 param 필요
                }
            }
    }


    public override void OnCollisionTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            items.Add(other.gameObject);
        }
    }
    public override void OnCollisionTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            items.Remove(other.gameObject);
        }
    }
    private void OnDestroy()
    {
        UnConnectInputActions();
    }

}
