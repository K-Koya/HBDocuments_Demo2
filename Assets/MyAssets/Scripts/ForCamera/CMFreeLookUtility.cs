using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CMFreeLookUtility : MonoBehaviour
{
    /// <summary>���Y��CinemachineFreeLook</summary>
    CinemachineFreeLook _CM = default;

    // Start is called before the first frame update
    void Start()
    {
        _CM = GetComponent<CinemachineFreeLook>();
        SeekPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_CM.Follow || !_CM.LookAt)
        {
            SeekPlayer();
        }
    }

    /// <summary>����L�����N�^�[�̈ʒu����Cinemachine�ɔ��f</summary>
    void SeekPlayer()
    {
        PlayerMove player = FindObjectOfType<PlayerMove>();

        if(player)
        {
            _CM.Follow = player.transform;
            _CM.LookAt = player.EyePoint ? player.EyePoint : player.transform;
        }
    }
}
