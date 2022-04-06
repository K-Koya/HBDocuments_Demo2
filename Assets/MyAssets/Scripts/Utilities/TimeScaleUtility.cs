using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary> ���Ԍo�ߑ��x�𐧌䂷�� </summary>
[RequireComponent(typeof(Timekeeper))]
public class TimeScaleUtility : Singleton<TimeScaleUtility>
{
    /// <summary> ���ׂẴO���[�o���N���b�N�𐧌䂷��^�C���L�[�p�[ </summary>
    Timekeeper _Tk = default;

    [Header("�ȉ��A�h�������o�[")]
    [SerializeField, Tooltip("�o�^����Ă���Globalclock���������ɃA�T�C��")]
    GlobalclockName _GlobalclockName = new GlobalclockName();

    // Start is called before the first frame update
    void Start()
    {
        _Tk = GetComponent<Timekeeper>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary> �o�^����Ă���Globalclock�����܂Ƃ߂�\���� </summary>
    [Serializable]
    public struct GlobalclockName
    {
        #region �����o�[
        [SerializeField] string _Root;
        [SerializeField] string _Pausable;
        [SerializeField] string _FreeLookCamera;
        [SerializeField] string _Character;
        [SerializeField] string _Allies;
        [SerializeField] string _Player;
        [SerializeField] string _Friend;
        [SerializeField] string _Enemy;
        [SerializeField] string _Unpausable;
        #endregion

        #region �v���p�e�B
        public string Root { get => _Root; }
        public string Pausable { get => _Pausable; }
        public string FreeLookCamera { get => _FreeLookCamera; }
        public string Character { get => _Character; }
        public string Allies { get => _Allies; }
        public string Player { get => _Player; }
        public string Friend { get => _Friend; }
        public string Enemy { get => _Enemy; }
        public string Unpausable { get => _Unpausable; }
        #endregion
    }
}
