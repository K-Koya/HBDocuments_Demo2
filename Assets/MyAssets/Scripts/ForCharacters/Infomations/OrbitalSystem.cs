using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalSystem : MonoBehaviour
{
    #region �����o
    [SerializeField, Tooltip("���񒆌p�|�C���g�̂����A���[���h���ʕ����Ɉʒu������̂̔z��ԍ�")]
    sbyte _WayPointIndexFoward = 0;

    [SerializeField, Tooltip("���񒆌p�|�C���g�̂����A���[���h�w������Ɉʒu������̂̔z��ԍ�")]
    sbyte _WayPointIndexBack = 6;

    [SerializeField, Tooltip("���񒆌p�|�C���g�̂����A���[���h�E�����Ɉʒu������̂̔z��ԍ�")]
    sbyte _WayPointIndexRight = 3;

    [SerializeField, Tooltip("���񒆌p�|�C���g�̂����A���[���h�������Ɉʒu������̂̔z��ԍ�")]
    sbyte _WayPointIndexLeft = 9;

    [SerializeField, Tooltip("���񂳂��邽�߂̒��p�|�C���g(�擪�ƌ���Ń��[�v�ł���悤�ɐݒ肷��)")]
    Transform[] _WayPoints = null;

    #endregion

    #region �v���p�e�B
    /// <summary>true : ��`����Ă���</summary>
    public bool IsDefined { get => _WayPoints != null || _WayPoints.Length > 0; }
    /// <summary>���[���h���ʕ������擾</summary>
    public Vector3 GetForward { get => _WayPoints[_WayPointIndexFoward].position; }
    /// <summary>���[���h�w��������擾</summary>
    public Vector3 GetBack { get => _WayPoints[_WayPointIndexBack].position; }
    /// <summary>���[���h�E�������擾</summary>
    public Vector3 GetRight { get => _WayPoints[_WayPointIndexRight].position; }
    /// <summary>���[���h���������擾</summary>
    public Vector3 GetLeft { get => _WayPoints[_WayPointIndexLeft].position; }


    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        if(_WayPoints == null || _WayPoints.Length < 1)
        {
            _WayPoints = GetComponentsInChildren<Transform>();
        }
    }

    /// <summary>from�ɍł��߂�WayPoint���擾����</summary>
    /// <param name="from">����W</param>
    public Vector3 GetClosist(Vector3 from)
    {
        sbyte index = 0;
        float sqrDistance = float.MaxValue;
        for (sbyte i = 0; i < _WayPoints.Length; i++)
        {
            float val = Vector3.SqrMagnitude(from - _WayPoints[i].position);
            if (val < sqrDistance)
            {
                sqrDistance = val;
                index = i;
            }
        }

        return _WayPoints[index].position;
    }

    /// <summary>from�ɍł��߂������玟��WayPoint���擾����</summary>
    /// <param name="from">����W</param>
    public Vector3 GetClosistAfter(Vector3 from)
    {
        sbyte index = 0;
        float sqrDistance = float.MaxValue;
        for (sbyte i = 0; i < _WayPoints.Length; i++)
        {
            float val = Vector3.SqrMagnitude(from - _WayPoints[i].position);
            if (val < sqrDistance)
            {
                sqrDistance = val;
                index = i;
            }
        }

        return _WayPoints[index - 1 > _WayPoints.Length ? 0 : index + 1].position;
    }

    /// <summary>from�ɍł��߂��������O��WayPoint���擾����</summary>
    /// <param name="from">����W</param>
    public Vector3 GetClosistBefore(Vector3 from)
    {
        sbyte index = 0;
        float sqrDistance = float.MaxValue;
        for (sbyte i = 0; i < _WayPoints.Length; i++)
        {
            float val = Vector3.SqrMagnitude(from - _WayPoints[i].position);
            if (val < sqrDistance)
            {
                sqrDistance = val;
                index = i;
            }
        }

        return _WayPoints[index - 1 < 1 ? _WayPoints.Length - 1 : index - 1].position;
    }
}
