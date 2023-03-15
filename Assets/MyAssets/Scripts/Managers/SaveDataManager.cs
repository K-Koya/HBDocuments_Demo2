using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SaveDataManager : Singleton<SaveDataManager>
{
    /// <summary>�Z�[�u�f�[�^��ۑ�����CSV�t�@�C���t�H���_�̃p�X</summary>
    const string SAVE_CSV_PATH = "CSV/Save/";

    /// <summary>���[�h�����Z�[�u�ԍ�</summary>
    byte _DataNumber = 1;

    /// <summary>�L�����N�^�[���Ƃ̃Z�[�u�f�[�^</summary>
    public class ForCharacter : ICSVDataConverter
    {
        /// <summary>�Z�[�u�f�[�^���[�h�Ɏg���L�����N�^�[��</summary>
        string _Name = string.Empty;

        /// <summary>true : �f�[�^�̕ύX������</summary>
        public bool _IsChanged = false;

        /// <summary>����ς݂Ńf�b�L�ɑg�ݍ��߂�R�}���h</summary>
        public List<ushort> _UnlockedCommands = null;

        /// <summary>���ۂɃf�b�L�ɑg�ݍ��܂�Ă���R�}���h</summary>
        public List<ushort> _DeckCommands = null;

        /// <summary>�Z�[�u�f�[�^���[�h�Ɏg���L�����N�^�[��</summary>
        public string Name { get => _Name; }

        /// <summary>�L�����N�^�[���Ƃ̃Z�[�u�f�[�^</summary>
        /// <param name="name">�L�����N�^�[��</param>
        public ForCharacter(string name)
        {
            _Name = name;
            _UnlockedCommands = new List<ushort>();
            _DeckCommands = new List<ushort>();
        }

        public void CSVToMembers(List<string[]> csv)
        {
            _UnlockedCommands.Clear();
            string[] dataLine = csv[0];
            for(int i = 1; i < dataLine.Length; i++)
            {
                if(dataLine[i] != null && dataLine[i].Length > 0)
                {
                    _UnlockedCommands.Add(ushort.Parse(dataLine[i]));
                }
            }

            _DeckCommands.Clear();
            dataLine = csv[1];
            for (int i = 1; i < dataLine.Length; i++)
            {
                if (dataLine[i] != null && dataLine[i].Length > 0)
                {
                    _DeckCommands.Add(ushort.Parse(dataLine[i]));
                }
            }
        }

        public List<string> MembersToCSV()
        {
            List<string> returnal= new List<string>();
            StringBuilder sb = new StringBuilder();
            sb.Append(',');
            for (int i = 0; i < _UnlockedCommands.Count; i++)
            {
                sb.Append(_UnlockedCommands[i]);
                sb.Append(',');
            }
            returnal.Add(sb.ToString());

            sb.Clear();
            sb.Append(',');
            for (int i = 0; i < _DeckCommands.Count; i++)
            {
                sb.Append(_DeckCommands[i]);
                sb.Append(',');
            }
            returnal.Add(sb.ToString());

            return returnal;
        }
    }
    /// <summary>�v���C�A�u���L�����N�^�[�̃Z�[�u�f�[�^�i�[��</summary>
    Dictionary<string, ForCharacter> _forCharacters = null;



    protected override void Awake()
    {
        //�V�[�����܂����ŗ��p����
        IsDontDestroyOnLoad = true;
        base.Awake();

        _forCharacters = new Dictionary<string, ForCharacter>();

        ForCharacter kana = new ForCharacter("Kana");
        _forCharacters.Add(kana.Name, kana);

        LoadData(1);
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(_forCharacters["Kana"].Name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>�Ή�����Z�[�u�f�[�^�����[�h����</summary>
    /// <param name="number">�f�[�^�ԍ�</param>
    public void LoadData(byte number)
    {
        _DataNumber = number;
        string commonPath = SAVE_CSV_PATH + _DataNumber + '/';

        _forCharacters["Kana"].CSVToMembers(CSVIO.LoadCSV(commonPath + _forCharacters["Kana"].Name));
    }

    /// <summary>�Ή�����L�����N�^�[�̃f�b�L�f�[�^���擾���郁�\�b�h</summary>
    /// <param name="name">�Ή��L�����N�^�[��</param>
    /// <returns>�R�}���hID���X�g</returns>
    public IReadOnlyList<ushort> GetDeckedCommand(string name)
    {
        return _forCharacters[name]._DeckCommands;
    }

    /// <summary>�Ή�����L�����N�^�[�̉���ς݃R�}���h��ID���擾���郁�\�b�h</summary>
    /// <param name="name">�Ή��L�����N�^�[��</param>
    /// <returns>�R�}���hID���X�g</returns>
    public IReadOnlyList<ushort> GetUnlockedCommand(string name)
    {
        return _forCharacters[name]._UnlockedCommands;
    }

    /// <summary>�Ή�����L�����N�^�[�̃f�b�L�f�[�^���㏑�����郁�\�b�h</summary>
    /// <param name="name">�Ή��L�����N�^�[��</param>
    /// <param name="list">�R�}���hID���X�g</param>
    public void SetDeckedCommand(string name, List<ushort> list)
    {
        _forCharacters[name]._DeckCommands = list;
        _forCharacters[name]._IsChanged = true;
    }

    /// <summary>�Ή�����L�����N�^�[�̉���ς݃R�}���h���㏑�����郁�\�b�h</summary>
    /// <param name="name">�Ή��L�����N�^�[��</param>
    /// <param name="list">�R�}���hID���X�g</param>
    public void SetUnlockedCommand(string name, List<ushort> list)
    {
        _forCharacters[name]._UnlockedCommands = list;
        _forCharacters[name]._IsChanged = true;
    }
}
