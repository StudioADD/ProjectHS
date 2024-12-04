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
using UnityEditor;


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

    Fall, // 추락

    Run,
    Hit,
    Dizz,
    GoBack, // 기절후 회복

    LeftCollect, //수집
    RightCollect, //수집

    Victory, // 승리

    Dead
}

public class Player : Creature
{
    static float stage2Time = 90f;

    [SerializeField]
    private EStageType stageType;

    [SerializeField]
    private GameObject[] Body = new GameObject[3];


    [SerializeField, ReadOnly] private JPlayerData data = null;

    [SerializeField, ReadOnly]
    private float inputTime = 0f; // 입력시간 

    [SerializeField, ReadOnly]
    private float moveDistance = 5f;
    [SerializeField, ReadOnly]
    private float moveSpeed = 5f;
    private float inputCooldown = 0.5f;

    [SerializeField, ReadOnly]
    private PlayerEffectObject StunEffect = null;

    [SerializeField, ReadOnly]
    private PlayerEffectObject BuffEffect = null;


    private float boosterTimer = -1f; // 부스터 현재시간

    [SerializeField, ReadOnly]
    private float hitTimer = -1; // hit시간

    private float hitTime;

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
    //[SerializeField, ReadOnly]
    //private List<Candy> items = new List<Candy>();

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

            if (_playerState == EPlayerState.Dead || value == EPlayerState.None)
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
                case EPlayerState.Landing: isChangeState = LandingStateCondition(); break;
                case EPlayerState.Fall: isChangeState = FallStateCondition(); break;
                case EPlayerState.Hit: isChangeState = HitStateCondition(); break;
                case EPlayerState.Dizz: isChangeState = DizzStateCondition(); break;
                case EPlayerState.GoBack: isChangeState = GoBackStateCondition(); break;
                case EPlayerState.LeftCollect:
                case EPlayerState.RightCollect: isChangeState = CollectStateCondition(); break;
                case EPlayerState.Run: isChangeState = RunStateCondition(); break;
                case EPlayerState.Victory: isChangeState = true; break;
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
                case EPlayerState.Fall: FallStateExit(); break;
                case EPlayerState.Hit: HitStateExit(); break;
                case EPlayerState.Dizz: DizzStateExit(); break;
                case EPlayerState.GoBack: GoBackStateExit(); break;
                case EPlayerState.LeftCollect:
                case EPlayerState.RightCollect: CollectStateExit(); break;
                case EPlayerState.Run: RunStateExit(); break;

            }
            animator.ResetTrigger(_playerState.ToString() + "_Trigger");
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
                case EPlayerState.Fall: FallStateEnter(); break;
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
        InitRigidVelocity();
        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        stageType = (EStageType)templateID;
        data = Managers.Data.PlayerDict[(int)stageType];
        moveSpeed = data.MoveSpeed;
        PlayerState = EPlayerState.Idle;
        targetPosition = beforePosition = transform.position;
        switch (stageType)
        {
            case EStageType.SharkAvoidance:
                SetSharkAvoidanceEffet();
                break;
            case EStageType.CollectingCandy:
                PlayerState = EPlayerState.Run;
                //임시 스테이지 endpoint로
                targetPosition = transform.position;
                targetPosition.z = 0;

                SetCollectingCandyEffet();
                break; // 추후 스테이지2 나오면  바꿔야함
            case EStageType.CrossingBridge:

                break;
            default:

                break;

        }
        IsPlayerInputControll = true;
    }

    public void OnEndStage(bool victory)
    {
        UnConnectInputActions();
        UnConnectEvent();
        if(victory)
        {
            PlayerState = EPlayerState.Victory;
        }
        IsPlayerInputControll = false;
        if (coPlayerStateController != null)
        {
            StopCoroutine(coPlayerStateController);
            coPlayerStateController = null;
        }
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

    #region Event

    #region SharkAvoidanceStage Event
    Action onAddBoosterItem;
    Func<bool> onUseBoosterItem;
    public void ConnectSharkAvoidanceStage(Action onAddBoosterItem, Func<bool> onUseBoosterItem)
    {
        this.onAddBoosterItem -= onAddBoosterItem;
        this.onUseBoosterItem -= onUseBoosterItem;

        this.onAddBoosterItem += onAddBoosterItem;
        this.onUseBoosterItem += onUseBoosterItem;
    }
    #endregion

    #region CollectingCandyStage Event
    Action<List<ECandyItemType>> onCollectCandyItems;
    Action<bool> onChangeScoreBuff;
    public void ConnectCollectingCandyStage(Action<List<ECandyItemType>> onCollectCandyItems, Action<bool> onChangeScoreBuff)
    {
        this.onCollectCandyItems = onCollectCandyItems;
        this.onChangeScoreBuff = onChangeScoreBuff;
    }

    [SerializeField, ReadOnly]
    List<ECandyItemType> candys = new List<ECandyItemType>();
    public override void OnCollisionTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == (int)ELayer.Item)
        {
            Vector3 pos = other.transform.position;
            CandyItem candy = other.gameObject.GetComponent<CandyItem>();
            if (isleft == true && pos.x <= transform.position.x)
            {
                candys.Add(candy.CandyItemType);
                candy.OnCollected();
                if (candy.CandyItemType == ECandyItemType.StarCandyItem)
                {
                    _isCandyBuff = true;
                }
            }
            else if (isleft == false && pos.x >= transform.position.x)
            {
                candys.Add(candy.CandyItemType);
                candy.OnCollected();
                if (candy.CandyItemType == ECandyItemType.StarCandyItem)
                {
                    _isCandyBuff = true;
                }
            }

        }
    }

    
    #endregion

    #region CrossingBridgeStage Event
    Func<ETeamType, Vector3> getJumpTargetPos;
    Func<ETeamType, Vector3> getSpawnPoint;
    Action<ETeamType> onUseGoggleItem;
    Action<ETeamType, EDirection> onChangeTarget;

    public void ConnectCrossingBridgeStage(
        Func<ETeamType, Vector3> getJumpTargetPos,
        Func<ETeamType, Vector3> getSpawnPoint,
        Action<ETeamType> onUseGoggleItem,
        Action<ETeamType, EDirection> onChangeTarget)
    {
        this.getJumpTargetPos = getJumpTargetPos;
        this.getSpawnPoint = getSpawnPoint;
        this.onUseGoggleItem = onUseGoggleItem;
        this.onChangeTarget = onChangeTarget;
    }

    public void OnDropDetect()
    {

        if (getSpawnPoint != null)
            transform.position = getSpawnPoint.Invoke(TeamType);

    }
    #endregion

    protected void UnConnectEvent()
    {
        onAddBoosterItem = null;
        onUseBoosterItem = null;
        onCollectCandyItems = null;
        onChangeScoreBuff = null;
        getJumpTargetPos = null;
        getSpawnPoint = null;
        onUseGoggleItem = null;
        onChangeTarget = null;
    }
    #endregion

    #region Effect

    #region SharkAvoidance Effet
    private void SetSharkAvoidanceEffet()
    {
        StunEffect = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_EFFECT_PATH}/" + Util.EnumToString(EEffectType.StunEffect)).GetComponent<PlayerEffectObject>();
        StunEffect.transform.parent = this.transform;
        StunEffect.transform.localPosition = new Vector3(0, 1.5f, 0);
        StunEffect.StopEffect();

        BuffEffect = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_EFFECT_PATH}/" + Util.EnumToString(EEffectType.BuffEffect)).GetComponent<PlayerEffectObject>();
        BuffEffect.transform.parent = this.transform;
        BuffEffect.transform.localPosition = new Vector3(0, 0, 0);
        BuffEffect.StopEffect();
    }

    #endregion

    #region SetCollectingCandy Effet
    private void SetCollectingCandyEffet()
    {
        BuffEffect = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_EFFECT_PATH}/" + Util.EnumToString(EEffectType.BuffEffect)).GetComponent<PlayerEffectObject>();
        BuffEffect.transform.parent = this.transform;
        BuffEffect.transform.localPosition = new Vector3(0, 0f, 0);
        BuffEffect.StopEffect();
    }

    #endregion
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
        Managers.Input.OnNum1KeyEntered += test;
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

        Managers.Input.OnWASDKeyEntered -= OnArrowKeyStage2;

        Managers.Input.OnArrowKeyEntered -= OnArrowKeyStage2;

        Managers.Input.OnWASDKeyEntered -= OnArrowKeyStage3;
        Managers.Input.OnSpaceKeyEntered -= OnJumpKey;
        Managers.Input.OnFKeyEntered -= OnUseGoggleItem;

        Managers.Input.OnArrowKeyEntered -= OnArrowKeyStage3;
        Managers.Input.OnEndKeyEntered -= OnJumpKey;
        Managers.Input.OnPageDownKeyEntered -= OnUseGoggleItem;

        Managers.Input.OnNum1KeyEntered -= test;

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

        IsBoosterState = onUseBoosterItem.Invoke();

        if (IsBoosterState)
        {
            BuffEffect.PlayEffect();
            boosterTimer = 0;
            inputCooldown = 0.25f;
            moveSpeed *= 2;
            animator.speed = 2;
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
                Managers.Input.OnWASDKeyEntered += OnArrowKeyStage2;
            }
            else
            {
                Managers.Input.OnArrowKeyEntered += OnArrowKeyStage2;
                
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
                Managers.Input.OnWASDKeyEntered += OnArrowKeyStage3;
                Managers.Input.OnSpaceKeyEntered += OnJumpKey;
                Managers.Input.OnFKeyEntered += OnUseGoggleItem;
            }
            else
            {
                Managers.Input.OnArrowKeyEntered += OnArrowKeyStage3;
                Managers.Input.OnEndKeyEntered += OnJumpKey;
                Managers.Input.OnPageDownKeyEntered += OnUseGoggleItem;
            }
        }

    }

    public void OnArrowKeyStage3(Vector2 value)
    {

        if (value.x > 0)
        {
            onChangeTarget?.Invoke(TeamType, EDirection.Right);
        }
        else if (value.x < 0)
        {
            onChangeTarget?.Invoke(TeamType, EDirection.Left);
        }


    }
    public void OnJumpKey()
    {
        if (isInputRock)
        {
            return;
        }

        PlayerState = EPlayerState.JumpUp;
    }

    protected void OnUseGoggleItem()
    {
        if (onUseGoggleItem != null)
        {
            onUseGoggleItem.Invoke(TeamType);
        }

    }

    #endregion
    
    #endregion

    #region PlayerState

    #region Idle
    protected virtual bool IdleStateCondition()
    {


        return true;
    }

    protected virtual void IdleStateEnter()
    {
        beforePosition = transform.position;
    }

    protected virtual void UpdateIdleState()
    {
        if (beforePosition.y - transform.position.y > 0.1f)
        {
            PlayerState = EPlayerState.Fall;
        }
    }

    protected virtual void IdleStateExit()
    {

    }
    #endregion

    #region Hit
    //test
    private void test()
    {
        //OnEndStage(true);
        //onAddBoosterItem?.Invoke();
        //if (boosterTimer == -1)
        //{
        //    boosterTimer = 0;
        //    inputCooldown = 0.25f;
        //    moveSpeed *= 2; animator.speed *= 2;
        //}

        switch (stageType)
        {
            case EStageType.SharkAvoidance:
                onAddBoosterItem?.Invoke();
                //float test = TeamType == ETeamType.Left ? 3 : 2;
                //OnHit(test);
                break;
            case EStageType.CollectingCandy:
                //_isCandyBuff = true;
                candys.Add((ECandyItemType)UnityEngine.Random.Range(0, (int)ECandyItemType.Max));
                break;
        }


    }
    public void OnHit(float hitTime = 3f)
    {
        if (boosterTimer > 0)
            return;
        if (!StunEffect.GetIsPlay())
        {
            float speed = hitTime < 3f ? 1.7f : 1.2f;
            StunEffect?.SetDuration(hitTime, speed);
        }


        this.hitTime = hitTime;
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
        isInputRock = true;
        hitTimer = 0;
        StunEffect.PlayEffect();

        if (targetPosition.x < beforePosition.x)
            trackNum++;
        else if (targetPosition.x > beforePosition.x)
            trackNum--;

        // 뒤로 밀려나기
        targetPosition = new Vector3(beforePosition.x, beforePosition.y, transform.position.z);
        beforePosition = transform.position;

        targetPosition.z -= 1f;


    }

    protected virtual void UpdateHitState()
    {
        Movement();
        hitTimer += Time.deltaTime;
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
        hitTimer += Time.deltaTime;
        if (hitTimer >= hitTime - 0.5f) //히트시간 조절
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
        targetPosition = transform.position + new Vector3(0, 0, 1f);
        beforePosition = transform.position;


    }

    protected virtual void UpdateGoBackState()
    {
        Movement();
        hitTimer += Time.deltaTime;
        if (hitTimer >= hitTime) // 히트시간 조절
        {
            hitTimer = -1;
            if (stageType == EStageType.CollectingCandy) // 추후 변경
                PlayerState = EPlayerState.Run;
            else
                PlayerState = EPlayerState.Idle; // 기절 상태 종료
        }
    }

    protected virtual void GoBackStateExit()
    {
        StunEffect.StopEffect();
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
        beforePosition = transform.position;
        targetPosition = transform.position + new Vector3(moveDirection.x * moveDistance, 0, moveDirection.y);
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

    }

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);// 1 / data.inputCooldown * Time.deltaTime); // 이동속도 data로 뺄수 있게 해줄것
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
        if (hitTimer > 0)
        {
            return false;
        }
        if (targetPosition.z + moveDirection.y == transform.position.z)
        {
            return false;
        }
        if (targetPosition.z != transform.position.z)
        {
            moveDirection.y = 0;
        }
        if (targetPosition.x != transform.position.x)
        {
            moveDirection.x = 0;
        }
        if (moveDirection == Vector2.zero)
        {
            return false;
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
            trackNum += (int)moveDirection.x;
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
        targetPosition = transform.position + Vector3.forward * data.MoveSpeed * 2; // 추후 이동거리로 뺄것
        if (stageType == EStageType.CrossingBridge)
        {
            if (getJumpTargetPos != null)
            {
                targetPosition = getJumpTargetPos.Invoke(TeamType);
            }
        }

        this.transform.forward = (targetPosition - this.transform.position).normalized;
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
    }

    protected virtual void UpdateJumpState()
    {

        // 이동 거리와 속도 관련 변수
        float x0 = beforePosition.x;
        float x1 = targetPosition.x; // 목표 위치 x
        float z0 = beforePosition.z; // 시작 위치 z
        float z1 = targetPosition.z; // 목표 위치 z
        float distanceX = x1 - x0;
        float distanceZ = z1 - z0;    // 총 이동 거리
        float progress = Mathf.Clamp01((transform.position.z - z0) / distanceZ); // 이동 비율 (0 ~ 1)


        // 방향 이동 (선형 보간)
        float nextX = x0 + distanceX * progress;
        float nextZ = Mathf.MoveTowards(transform.position.z, z1, moveSpeed * Time.deltaTime);

        // 포물선 Y 높이 계산
        float peakHeight = Mathf.Max(beforePosition.y, targetPosition.y) + data.JumpPower; // 최고점 계산
        float nextY = Mathf.Lerp(beforePosition.y, targetPosition.y, progress)            // 기본 보간 높이
                    + Mathf.Sin(progress * Mathf.PI) * (peakHeight - Mathf.Lerp(beforePosition.y, targetPosition.y, 0.5f)); // 포물선 추가

        // 새로운 위치 계산
        Vector3 nextPosition = new Vector3(nextX, nextY, nextZ);

        float dirY = nextY - transform.position.y;

        // 위치 적용
        transform.position = nextPosition;

        if (dirY < 0 && transform.position.y - targetPosition.y < 0.1f)
        {
            if (IsRaycastHitToLayer(ELayer.Ground))
                PlayerState = EPlayerState.Landing;
            else
                PlayerState = EPlayerState.Fall;
        }




    }


    protected virtual void JumpStateExit()
    {

    }
    #endregion

    #region Landing
    protected virtual bool LandingStateCondition()
    {

        return true;
    }

    protected virtual void LandingStateEnter()
    {
    }

    protected virtual void UpdateLandingState()
    {


        if (IsEndCurrentState(PlayerState))
        {
            switch (stageType)
            {


                case EStageType.CollectingCandy: PlayerState = EPlayerState.Run; break;

                default:
                    PlayerState = EPlayerState.Idle; break;
            }
        }
        else
        {
            if (!IsRaycastHitToLayer(ELayer.Ground))
            {
                PlayerState = EPlayerState.Fall;
            }
        }
    }


    protected virtual void LandingStateExit()
    {
        //targetPosition = Vector3.zero;
        transform.forward = new Vector3(0, 0, 1);
        IsJump = false;
    }
    #endregion

    #region Fall
    protected virtual bool FallStateCondition()
    {
        if (stageType != EStageType.CrossingBridge)
            return false;

        return true;
    }

    protected virtual void FallStateEnter()
    {
        IsJump = true;
    }

    protected virtual void UpdateFallState()
    {

        if (IsRaycastHitToLayer(ELayer.Ground))
            PlayerState = EPlayerState.Landing;


    }


    protected virtual void FallStateExit()
    {
        //targetPosition = Vector3.zero;
        transform.forward = new Vector3(0, 0, 1);
        IsJump = false;
    }

    protected virtual bool IsRaycastHitToLayer(ELayer LayerName)
    {
        float distance = 3f;
        int layerMask = 1 << LayerMask.NameToLayer(Util.EnumToString(LayerName));  // 특정 레이어만 충돌 체크함\
        if (Physics.Raycast(transform.position + new Vector3(0, 2, 0), Vector3.down, distance, layerMask)) // 왜 더해줘야하는지 모르겠음
        {
            return true;
        }
        return false;
    }

    #endregion

    #region Run
    protected virtual bool RunStateCondition()
    {

        return true;
    }

    protected virtual void RunStateEnter()
    {

    }

    protected virtual void UpdateRunState(float timer)
    {
        Running(timer);

        if (transform.position == targetPosition)
        {
            PlayerState = EPlayerState.Idle;
        }


    }

    protected virtual void RunStateExit()
    {

    }
    protected void Running(float timer)
    {
        float distance = targetPosition.z - beforePosition.z;
        float progress = timer / stage2Time;
        transform.position = new Vector3(transform.position.x, transform.position.y, beforePosition.z + distance * progress);
    }
    

    #endregion

    #region Collect

    protected virtual bool CollectStateCondition()
    {
        if (PlayerState != EPlayerState.Run)
            return false;
        return true;
    }


    protected virtual void UpdateCollectState(float timer)
    {


        Running(timer);
        if (IsEndCurrentState(PlayerState))
        {
            PlayerState = EPlayerState.Run;
        }

    }

    protected virtual void CollectStateExit()
    {
        CollectItem();
        candys.Clear();
        collisionTrigger.SetActive(false);
    }
    #region Left Collect


    protected virtual void LeftCollectStateEnter()
    {
        isleft = true;
        collisionTrigger.SetActive(true);
        

    }

    #endregion

    #region Right Collect

    protected virtual void RightCollectStateEnter()
    {
        isleft = false;
        collisionTrigger.SetActive(true);
        
    }


    #endregion

    #endregion

    #endregion

    #region co
    Coroutine coPlayerStateController = null;
    protected IEnumerator CoPlayerStateController()
    {
        float timer = 0.0f;
        while (IsPlayerInputControll)
        {
            timer += Time.deltaTime;

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
                case EPlayerState.Fall: UpdateFallState(); break;
                case EPlayerState.Hit: UpdateHitState(); break;
                case EPlayerState.Dizz: UpdateDizzState(); break;
                case EPlayerState.GoBack: UpdateGoBackState(); break;
                case EPlayerState.Run: UpdateRunState(timer); break;
                case EPlayerState.LeftCollect:
                case EPlayerState.RightCollect: UpdateCollectState(timer); break;
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

    public bool IsBoosterState { get; private set; }

    private void boosterTimeUpdate()
    {
        if (boosterTimer >= 0)
        {
            boosterTimer += Time.deltaTime;
            if (boosterTimer >= 3)
            {
                IsBoosterState = false;
                boosterTimer = -1;
                moveSpeed = data.MoveSpeed;
                inputCooldown = 0.5f;
                animator.speed = 1;
                BuffEffect.StopEffect();
            }
        }
    }
    public void GetBooster()
    {
        onAddBoosterItem?.Invoke();
    }




    #endregion

    #region CandyBuff
    private bool IsCandyBuff = false;
    public bool _isCandyBuff
    {
        get { return IsCandyBuff; }
        set
        {
            if (IsCandyBuff == value)
                return;
            IsCandyBuff = value;
            onChangeScoreBuff(IsCandyBuff);
            if (IsCandyBuff && !BuffEffect.GetIsPlay())
            {
                StartCoroutine(CoCandyBuffEffect());
            }
        }
    }

    protected IEnumerator CoCandyBuffEffect()
    {
        float time = 0f;
        BuffEffect?.PlayEffect();
        while (time <= 10f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        _isCandyBuff = false;
        BuffEffect?.StopEffect();
    }

    #endregion


    bool isleft = false;
    protected void CollectItem()
    {
        if (candys.Count > 0)
        {
            onCollectCandyItems?.Invoke(candys);
        }
    }

    

    private void OnDisable()
    {
        UnConnectInputActions();
        UnConnectEvent();
    }




}
