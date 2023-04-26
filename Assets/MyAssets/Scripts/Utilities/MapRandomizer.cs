using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapRandomizer : MonoBehaviour
{
    [SerializeField, Tooltip("�}�X�̐�(���5�}�X�ȏ�)")]
    Vector2Int _MapSize = new Vector2Int(5, 5);

    [SerializeField, Tooltip("�}�X��1�ӂ̒���")]
    float _Unit = 10f;



    [Header("�v���n�u")]
    [SerializeField, Tooltip("���ʒn�ʋ��")]
    GameObject _CommonFloor = null;

    [SerializeField, Tooltip("���i���H���")]
    GameObject _StraightRoad = null;

    [SerializeField, Tooltip("�Ȑ����H���")]
    GameObject _CurveRoad = null;


    /// <summary>���p</summary>
    public enum Compass
    {
        North = 1,
        East = 2,
        South = 3,
        West = 4,
    }


    // Start is called before the first frame update
    void Start()
    {
        /*�e�[�u��������*/
        MapCell[,] map = new MapCell[_MapSize.y, _MapSize.x];
        for (int y = 0; y < _MapSize.y; y++)
        {
            for (int x = 0; x < _MapSize.x; x++)
            {
                map[y, x] = new MapCell(false);
            }
        }

        /*���H�w��*/
        int curveTiming = _MapSize.x / 2;
        //�O���̂ǂ������X�^�[�g�ʒu�Ɏw��
        float rand = Random.value;
        Vector2Int currentPos = Vector2Int.zero;
        Compass advanceDirection = Compass.North;
        int[] advanceLimit = new int[4];
        if (rand < 0.5f)
        {
            int startX = (int)Random.Range(_MapSize.x * 0.2f, _MapSize.x * 0.6f);
            //�k������
            if (rand < 0.25f)
            {
                map[0, startX].isRoad = true;
                map[0, startX].enter = Compass.South;
                map[0, startX].exit = Compass.North;
                map[1, startX].isRoad = true;
                map[1, startX].enter = Compass.South;
                map[1, startX].exit = Compass.North;
                currentPos = new Vector2Int(startX, 1);

                advanceDirection = Compass.North;
                advanceLimit[0] = _MapSize.y;
                advanceLimit[1] = _MapSize.x - startX - 2;
                advanceLimit[2] = 0;
                advanceLimit[3] = startX - 2;
            }
            //�������
            else
            {
                map[_MapSize.y - 1, startX].isRoad = true;
                map[_MapSize.y - 1, startX].enter = Compass.North;
                map[_MapSize.y - 1, startX].exit = Compass.South;
                map[_MapSize.y - 2, startX].isRoad = true;
                map[_MapSize.y - 2, startX].enter = Compass.North;
                map[_MapSize.y - 2, startX].exit = Compass.South;
                currentPos = new Vector2Int(startX, _MapSize.y - 2);

                advanceDirection = Compass.South;
                advanceLimit[0] = 0;
                advanceLimit[1] = _MapSize.x - startX - 2;
                advanceLimit[2] = _MapSize.y;
                advanceLimit[3] = startX - 2;
            }

            curveTiming = _MapSize.y * 3 / 4;
        }
        else
        {
            int startY = (int)Random.Range(_MapSize.y * 0.2f, _MapSize.y * 0.6f);
            //��������
            if (rand < 0.75f)
            {
                map[startY, 0].isRoad = true;
                map[startY, 0].enter = Compass.West;
                map[startY, 0].exit = Compass.East;
                map[startY, 1].isRoad = true;
                map[startY, 1].enter = Compass.West;
                map[startY, 1].exit = Compass.East;
                currentPos = new Vector2Int(1, startY);

                advanceDirection = Compass.East;
                advanceLimit[0] = _MapSize.y - startY - 2;
                advanceLimit[1] = _MapSize.x;
                advanceLimit[2] = startY - 2;
                advanceLimit[3] = 0;
            }
            //��������
            else
            {
                map[startY, _MapSize.x - 1].isRoad = true;
                map[startY, _MapSize.x - 1].enter = Compass.East;
                map[startY, _MapSize.x - 1].exit = Compass.West;
                map[startY, _MapSize.x - 2].isRoad = true;
                map[startY, _MapSize.x - 2].enter = Compass.East;
                map[startY, _MapSize.x - 2].exit = Compass.West;    
                currentPos = new Vector2Int(_MapSize.x - 2, startY);

                advanceDirection = Compass.West;
                advanceLimit[0] = _MapSize.y - startY - 2;
                advanceLimit[1] = 0;
                advanceLimit[2] = startY - 2;
                advanceLimit[3] = _MapSize.x;
            }

            curveTiming = _MapSize.x * 3 / 4;
        }
        //���@��
        int loopLimit = _MapSize.x * _MapSize.y;
        for (int i = 0; i < loopLimit; i++)
        {
            bool isDetectNext = false;
            for(int k = 0; k < 40 && !isDetectNext; k++)
            {
                rand = Random.value;
                Compass goNext = advanceDirection;
                //�E��
                if(rand > 0.9f)
                {
                    if((int)advanceDirection > 2)
                    {
                        goNext = Compass.North;
                    }
                    else
                    {
                        goNext = advanceDirection + 1;
                    }
                }
                //����
                else if(rand > 0.8f)
                {
                    if ((int)advanceDirection < 1)
                    {
                        goNext = Compass.West;
                    }
                    else
                    {
                        goNext = advanceDirection - 1;
                    }
                }

                //���Ɍ���������
                switch (goNext)
                {
                    //�k������
                    case Compass.North:
                        if (!map[currentPos.y + 1, currentPos.x].isRoad && advanceLimit[0] > 0)
                        {
                            map[currentPos.y, currentPos.x].exit = Compass.North;

                            currentPos.y += 1;
                            map[currentPos.y, currentPos.x].isRoad = true;
                            map[currentPos.y, currentPos.x].enter = Compass.South;
                            isDetectNext = true;

                            advanceLimit[0]--;
                        }
                        break;
                    //�������
                    case Compass.South:
                        if (!map[currentPos.y - 1, currentPos.x].isRoad && advanceLimit[2] > 0)
                        {
                            map[currentPos.y, currentPos.x].exit = Compass.South;

                            currentPos.y -= 1;
                            map[currentPos.y, currentPos.x].isRoad = true;
                            map[currentPos.y, currentPos.x].enter = Compass.North;
                            isDetectNext = true;

                            advanceLimit[2]--;
                        }
                        break;
                    //��������
                    case Compass.East:
                        if (!map[currentPos.y, currentPos.x + 1].isRoad && advanceLimit[1] > 0)
                        {
                            map[currentPos.y, currentPos.x].exit = Compass.East;

                            currentPos.x += 1;
                            map[currentPos.y, currentPos.x].isRoad = true;
                            map[currentPos.y, currentPos.x].enter = Compass.West;
                            isDetectNext = true;

                            advanceLimit[1]--;
                        }
                        break;
                    //������
                    case Compass.West:
                        if (!map[currentPos.y, currentPos.x - 1].isRoad && advanceLimit[3] > 0)
                        {
                            map[currentPos.y, currentPos.x].exit = Compass.West;

                            currentPos.x -= 1;
                            map[currentPos.y, currentPos.x].isRoad = true;
                            map[currentPos.y, currentPos.x].enter = Compass.East;
                            isDetectNext = true;

                            advanceLimit[3]--;
                        }
                        break;
                    default: break;
                }
            }
            

            //�ǂ����}�b�v�̒[�ɓ��B���Ă���Η��E
            if (currentPos.y < 1 || _MapSize.y - 2 < currentPos.y
                || currentPos.x < 1 || _MapSize.x - 2 < currentPos.x)
            {
                //�Ō�̓��̏o�����O���Ɏw��
                switch(map[currentPos.y, currentPos.x].enter)
                {
                    case Compass.North:
                        map[currentPos.y, currentPos.x].exit = Compass.South;
                        break;
                    case Compass.East:
                        map[currentPos.y, currentPos.x].exit = Compass.West;
                        break;
                    case Compass.South:
                        map[currentPos.y, currentPos.x].exit = Compass.North;
                        break;
                    case Compass.West:
                        map[currentPos.y, currentPos.x].exit = Compass.East;
                        break;    
                    default: break;
                }

                break;
            }
        }


        /*�p�[�c�z�u*/
        GameObject pref = null;
        for(int y = 0; y < _MapSize.y; y++)
        {
            for(int x = 0; x < _MapSize.x; x++)
            {
                Vector3 forward = Vector3.forward;
                if(map[y, x].isRoad)
                {
                    //�����Əo���̈ʒu�֌W���瓹�H�̎�ނƒu�������w��
                    switch (map[y, x].enter)
                    {
                        //�k������
                        case Compass.North:
                            switch(map[y, x].exit)
                            {
                                case Compass.East:
                                    pref = _CurveRoad;
                                    break;
                                case Compass.South:
                                    pref = _StraightRoad;
                                    forward = Vector3.left;
                                    break;
                                case Compass.West:
                                    pref = _CurveRoad;
                                    forward = Vector3.left;
                                    break;
                                default: break;
                            }
                            break;
                        //��������
                        case Compass.East:
                            switch (map[y, x].exit)
                            {
                                case Compass.North:
                                    pref = _CurveRoad;
                                    break;
                                case Compass.South:
                                    pref = _CurveRoad;
                                    forward = Vector3.right;
                                    break;
                                case Compass.West:
                                    pref = _StraightRoad;
                                    break;
                                default: break;
                            }
                            break;
                        //�삪����
                        case Compass.South:
                            switch (map[y, x].exit)
                            {
                                case Compass.North:
                                    pref = _StraightRoad;
                                    forward = Vector3.left;
                                    break;
                                case Compass.East:
                                    pref = _CurveRoad;
                                    forward = Vector3.right;
                                    break;
                                case Compass.West:
                                    pref = _CurveRoad;
                                    forward = Vector3.back;
                                    break;
                                default: break;
                            }
                            break;
                        //��������
                        case Compass.West:
                            switch (map[y, x].exit)
                            {
                                case Compass.North:
                                    pref = _CurveRoad;
                                    forward = Vector3.left;
                                    break;
                                case Compass.East:
                                    pref = _StraightRoad;
                                    break;
                                case Compass.South:
                                    pref = _CurveRoad;
                                    forward = Vector3.back;
                                    break;
                                default: break;
                            }
                            break;
                        default: break;
                    }
                }
                else
                {
                    pref = _CommonFloor;
                }

                //����
                GameObject ins = Instantiate(pref);
                ins.transform.SetParent(transform);
                ins.transform.position = new Vector3(x, 0f, y) * _Unit;
                ins.transform.forward = forward;
            }
        }
    }


    /// <summary>�}�b�v1�}�X�̏��</summary>
    class MapCell
    {
        /// <summary>���̓���</summary>
        public Compass enter = Compass.North;

        /// <summary>���̏o��</summary>
        public Compass exit = Compass.South;

        /// <summary>true : ���H</summary>
        public bool isRoad = false;


        public MapCell(bool isRoad)
        {
            this.isRoad = isRoad;
        }
    }
}