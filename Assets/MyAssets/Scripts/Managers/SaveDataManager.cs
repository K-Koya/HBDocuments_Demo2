using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SaveDataManager : Singleton<SaveDataManager>
{
    /// <summary>セーブデータを保存するCSVファイルフォルダのパス</summary>
    const string SAVE_CSV_PATH = "CSV/Save/";

    /// <summary>ロードしたセーブ番号</summary>
    byte _DataNumber = 1;

    /// <summary>キャラクターごとのセーブデータ</summary>
    public class ForCharacter : ICSVDataConverter
    {
        /// <summary>セーブデータロードに使うキャラクター名</summary>
        string _Name = string.Empty;

        /// <summary>true : データの変更が発生</summary>
        public bool _IsChanged = false;

        /// <summary>解放済みでデッキに組み込めるコマンド</summary>
        public List<ushort> _UnlockedCommands = null;

        /// <summary>実際にデッキに組み込まれているコマンド</summary>
        public List<ushort> _DeckCommands = null;

        /// <summary>セーブデータロードに使うキャラクター名</summary>
        public string Name { get => _Name; }

        /// <summary>キャラクターごとのセーブデータ</summary>
        /// <param name="name">キャラクター名</param>
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
    /// <summary>プレイアブルキャラクターのセーブデータ格納庫</summary>
    Dictionary<string, ForCharacter> _forCharacters = null;



    protected override void Awake()
    {
        //シーンをまたいで利用する
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


    /// <summary>対応するセーブデータをロードする</summary>
    /// <param name="number">データ番号</param>
    public void LoadData(byte number)
    {
        _DataNumber = number;
        string commonPath = SAVE_CSV_PATH + _DataNumber + '/';

        _forCharacters["Kana"].CSVToMembers(CSVIO.LoadCSV(commonPath + _forCharacters["Kana"].Name));
    }

    /// <summary>対応するキャラクターのデッキデータを取得するメソッド</summary>
    /// <param name="name">対応キャラクター名</param>
    /// <returns>コマンドIDリスト</returns>
    public IReadOnlyList<ushort> GetDeckedCommand(string name)
    {
        return _forCharacters[name]._DeckCommands;
    }

    /// <summary>対応するキャラクターの解放済みコマンドのIDを取得するメソッド</summary>
    /// <param name="name">対応キャラクター名</param>
    /// <returns>コマンドIDリスト</returns>
    public IReadOnlyList<ushort> GetUnlockedCommand(string name)
    {
        return _forCharacters[name]._UnlockedCommands;
    }

    /// <summary>対応するキャラクターのデッキデータを上書きするメソッド</summary>
    /// <param name="name">対応キャラクター名</param>
    /// <param name="list">コマンドIDリスト</param>
    public void SetDeckedCommand(string name, List<ushort> list)
    {
        _forCharacters[name]._DeckCommands = list;
        _forCharacters[name]._IsChanged = true;
    }

    /// <summary>対応するキャラクターの解放済みコマンドを上書きするメソッド</summary>
    /// <param name="name">対応キャラクター名</param>
    /// <param name="list">コマンドIDリスト</param>
    public void SetUnlockedCommand(string name, List<ushort> list)
    {
        _forCharacters[name]._UnlockedCommands = list;
        _forCharacters[name]._IsChanged = true;
    }
}
