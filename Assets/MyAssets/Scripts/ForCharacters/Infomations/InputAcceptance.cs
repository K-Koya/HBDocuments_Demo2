/// <summary>����ۏ�Ԃ��Ǘ�����N���X</summary>
[System.Serializable]
public class InputAcceptance
{
    /// <summary>true : ���͈ړ����󂯕t����</summary>
    public bool _Move = true;

    /// <summary>true : ���͂ŃW�����v���󂯕t����</summary>
    public bool _Jump = true;

    /// <summary>true : �V�t�g�X���C�h�i�Z��������j���󂯕t����</summary>
    public bool _ShiftSlide = true;

    /// <summary>true : �����O�g���b�v�i����������j���󂯕t����</summary>
    public bool _LongTrip = true;

    /// <summary>true : �K�[�h���󂯕t����</summary>
    public bool _Gurad = true;

    /// <summary>true : �ʏ�R���{���󂯕t����</summary>
    public bool _ComboNormal = true;

    /// <summary>true : �R���{�t�B�j�b�V�����󂯕t����</summary>
    public bool _ComboFinish = false;
}
