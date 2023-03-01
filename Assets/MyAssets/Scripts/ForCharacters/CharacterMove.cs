using System.Collections.Generic;
using UnityEngine;
using Chronos;


[RequireComponent(typeof(Rigidbody), typeof(CharacterParameter))]
abstract public class CharacterMove : MonoBehaviour
{
    #region 定数
    /// <summary>速度が0であるとみなす数値</summary>
    protected const float VELOCITY_ZERO_BORDER = 0.5f;
    #endregion

    #region メンバ
    /// <summary>移動向けに力をかける時の力の大きさ</summary>
    protected float _MovePower = 3.0f;

    /// <summary>結果の移動速度</summary>
    protected float _Speed = 0.0f;

    /// <summary>キャラクターの、武器を出したりして臨戦態勢になる継続タイマー</summary>
    protected float _ArmedTimer = 0.0f;

    /// <summary>True : ジャンプ直後</summary>
    protected bool _JumpFlag = false;

    /// <summary>true : 各種アクションを実施直後</summary>
    protected bool _DoAction = false;

    /// <summary>キャラクターの持つ情報</summary>
    protected CharacterParameter _Param = null;

    /// <summary>当該キャラクターの物理挙動コンポーネント</summary>
    protected RigidbodyTimeline3D _Rb = null;

    /// <summary>着地判定をするコンポーネント</summary>
    GroundChecker _GroundChecker = default;

    /// <summary>移動入力の大きさ</summary>
    protected float _MoveInputRate = 0f;

    /// <summary>かけるブレーキ力</summary>
    protected Vector3 _ForceOfBrake = Vector3.zero;

    /// <summary>移動用メソッド</summary>
    protected System.Action Movement = null;

    /// <summary>各種行動用メソッド</summary>
    protected System.Action Act = null;

    /// <summary>被ダメージメソッド</summary>
    protected System.Action Damaged = null;

    /// <summary>コマンドを格納するコンポーネント</summary>
    protected CommandHolder _CommandHolder = null;

    /// <summary>利用するアニメーションの種類</summary>
    protected AnimationKind _AnimKind = AnimationKind.NoCall;
    #endregion

    #region プロパティ
    /// <summary>true : 各種アクション入力があった</summary>
    public bool DoAction { get => _DoAction; }
    /// <summary>現在の行動状態</summary>
    public MotionState.StateKind State { get => _Param.State.Kind; }
    /// <summary>キャラクターの、武器を出したりして臨戦態勢になる継続タイマー</summary>
    public float ArmedTimer { get => _ArmedTimer; }
    /// <summary>True : 着地している</summary>
    public bool IsGround { get => _GroundChecker.IsGround; }
    /// <summary>重力方向</summary>
    public Vector3 GravityDirection { get => _GroundChecker.GravityDirection; }
    /// <summary>Rigidbodyのvelocityを移動方向平面に換算したもの</summary>
    public Vector3 VelocityOnPlane { get => Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection); }
    /// <summary>移動向けに力をかける時の力の大きさ</summary>
    public float MovePower { set => _MovePower = value; }
    /// <summary>結果の移動速度</summary>
    public float Speed { get => _Speed; }
    /// <summary>ジャンプ直後フラグ</summary>
    public bool JumpFlag { get => _JumpFlag; }
    /// <summary>移動方向</summary>
    public Vector3 MoveDirection { get => _Param.MoveDirection; }
    /// <summary>利用するアニメーションの種類</summary>
    public AnimationKind AnimKind { get => _AnimKind; }
    #endregion



    // Start is called before the first frame update
    protected virtual void Start()
    {
        _Param = GetComponent<CharacterParameter>();
        _GroundChecker = GetComponent<GroundChecker>();

        _CommandHolder = GetComponentInChildren<CommandHolder>();

        Timeline tl = GetComponent<Timeline>();
        _Rb = tl.rigidbody;
        _Rb.useGravity = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //timeScaleが0ならポーズ中
        if (!(_Param.Tl.timeScale > 0f)) return;

        //速度測定
        _Speed = VelocityOnPlane.magnitude;

        //操作
        Movement?.Invoke();
        Act?.Invoke();

        //臨戦態勢
        SetArmedTimer();


    }

    void FixedUpdate()
    {
        //着地しているか否かで分岐
        //着地中
        if (IsGround)
        {
            //移動力がかかっている
            if (_MoveInputRate > 0f)
            {
                //回転する
                CharacterRotation(_Param.CharacterDirection, -GravityDirection, 720f);

                //力をかける
                _Rb.AddForce(_Param.MoveDirection * _MoveInputRate * _MovePower, ForceMode.Acceleration);

                //速度(向き)を、入力方向へ設定
                _Rb.velocity = Quaternion.FromToRotation(Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection), _Param.MoveDirection) * _Rb.velocity;

                //重力をかける
                _Rb.AddForce(GravityDirection * 2f, ForceMode.Acceleration);
            }
            //移動力がなければ、現在の速度が閾値を下回った時に0にする
            else
            {
                if (VelocityOnPlane.sqrMagnitude < VELOCITY_ZERO_BORDER)
                {
                    _Rb.velocity = Vector3.Project(_Rb.velocity, -GravityDirection);
                }

                //重力をかける
                _Rb.AddForce(GravityDirection * 1f, ForceMode.Acceleration);
            }
        }
        //空中
        else
        {
            //移動力がかかっている
            if (_MoveInputRate > 0f)
            {
                //回転する
                CharacterRotation(_Param.CharacterDirection, -GravityDirection, 90f);

                //力をかける
                _Rb.AddForce(_Param.MoveDirection * _MoveInputRate * _MovePower, ForceMode.Acceleration);
            }

            //重力をかける
            _Rb.AddForce(GravityDirection * 9.8f, ForceMode.Acceleration);
        }

        //速度減衰をかける
        if (_ForceOfBrake.sqrMagnitude > 0f)
        {
            _Rb.AddForce(_ForceOfBrake, ForceMode.Acceleration);
        }
    }

    /// <summary> キャラクターを指定向きに回転させる </summary>
    /// <param name="targetDirection">目標向き</param>
    /// <param name="up">上方向（Vector.Zeroなら上方向を指定しない）</param>
    /// <param name="rotateSpeed">回転速度</param>
    protected void CharacterRotation(Vector3 targetDirection, Vector3 up, float rotateSpeed)
    {
        if (targetDirection.sqrMagnitude > 0.0f)
        {
            Vector3 trunDirection = transform.right;
            Quaternion charDirectionQuaternion = Quaternion.identity;
            if (up.sqrMagnitude > 0f) charDirectionQuaternion = Quaternion.LookRotation(targetDirection + (trunDirection * 0.001f), up);
            else charDirectionQuaternion = Quaternion.LookRotation(targetDirection + (trunDirection * 0.001f));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, charDirectionQuaternion, rotateSpeed * Time.deltaTime);
        }
    }

    /// <summary>臨戦態勢をチェック</summary>
    void SetArmedTimer()
    {
        if(State is MotionState.StateKind.ComboNormal or MotionState.StateKind.AttackCommand)
        {
            _ArmedTimer = 10f;
        }
        else if(_ArmedTimer > 0f)
        {
            _ArmedTimer -= Time.deltaTime;
            if(_ArmedTimer < 0f) _ArmedTimer = 0f;
        }
    }


    #region アニメーションイベント

    /// <summary>アニメーションイベントにて、アニメーション遷移におけるフリーズ回避のため、待機状態にする</summary>
    public void StateCallStaying()
    {
        _Param.State.Kind = MotionState.StateKind.Stay;
        _Param.State.Process = MotionState.ProcessKind.Playing;
    }

    /// <summary>アニメーションイベントにて、予備動作に入った情報を受け取る</summary>
    public void ProcessCallPreparation()
    {
        _Param.State.Process = MotionState.ProcessKind.Preparation;
    }

    /// <summary>アニメーションイベントにて、本動作に入った情報を受け取る</summary>
    public void ProcessCallPlaying()
    {
        _Param.State.Process = MotionState.ProcessKind.Playing;
    }

    /// <summary>アニメーションイベントにて、動作の空き時間になった情報を受け取る</summary>
    public void ProcessCallInterval()
    {
        _Param.State.Process = MotionState.ProcessKind.Interval;
    }

    /// <summary>アニメーションイベントにて、動作終了予定の情報を受け取る</summary>
    public void ProcessCallEndSoon()
    {
        _Param.State.Process = MotionState.ProcessKind.EndSoon;

        //コンボ手数をリセットする
        _CommandHolder.Combo.CountReset();

        //コマンドの後処理
        _CommandHolder.GetActiveSkillForPostProcess()?.PostProcess(_Param, _Rb.component, GravityDirection, ref _AnimKind);
    }

    /// <summary>アニメーションイベントにて、追加実行する指示を出す</summary>
    public void RunningCall()
    {
        _CommandHolder.Running?.Running(_Param, null, GravityDirection, Vector3.zero, ref _AnimKind);
    }

    /// <summary>アニメーションイベントにて、実行中コマンドの打ち出したいオブジェクトを攻撃情報と射出元を指定しアクティブ化する</summary>
    /// <param name="twoIds">上2桁:攻撃情報テーブルにアクセスしたいカラムID 下2桁:攻撃範囲情報にアクセスしたいカラムID</param>
    public void CommandObjectShootCall(int twoIds)
    {
        int attackInfoID = twoIds / 100;
        int attackAreaID = twoIds % 100;

        AttackPowerColumn apc = _CommandHolder.Running.GetAttackArea(attackInfoID);
        AttackInformation info = new AttackInformation(apc, _Param.Main);

        _CommandHolder.Running?.ObjectCreation(_Param, info, _Param.EmitPoints[attackAreaID].position);
    }

    /// <summary>アニメーションイベントにて、攻撃判定を開始したい旨を、攻撃情報と攻撃範囲とともに受け取る</summary>
    /// <param name="twoIds">上2桁:攻撃情報テーブルにアクセスしたいカラムID 下2桁:攻撃範囲情報にアクセスしたいカラムID</param>
    public void AttackCallStart(int twoIds)
    {
        int attackInfoID = twoIds / 100;
        int attackAreaID = twoIds % 100;

        AttackPowerColumn apc = _CommandHolder.Running.GetAttackArea(attackInfoID);
        AttackInformation info = new AttackInformation(apc, _Param.Main);

        foreach(AttackCollision atkArea in _Param.AttackAreas[attackAreaID]._AttackCollisions)
        {
            atkArea.gameObject.SetActive(true);
            atkArea.AttackInfo = info;
        }
    }

    /// <summary>アニメーションイベントにて、攻撃判定を終了したい旨を受け取る</summary>
    public void AttackCallEnd()
    {
        foreach(var atkAreas in _Param.AttackAreas)
        {
            foreach(AttackCollision ac in atkAreas._AttackCollisions)
            {
                ac.gameObject.SetActive(false);
                ac.AttackInfo = null;
            }
        }
    }

    #endregion
}
