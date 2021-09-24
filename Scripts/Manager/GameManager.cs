using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; //최소값,최대값을 찾기위해서 선언함
/*
 * 클래스 객체 선언할 때 원래 public으로 변수를 선언하면 인스팩터에서 값이 출력되어서 나타나야 하는데 c# 일반 클래스에서는 인스팩터에 값이 안타나진다 
 * 따라서 클래스를 인스팩터와 연결하기 위해서 System.Serializable을 사용
 */
[System.Serializable]
public class PlayerDB //플레이어 설정
{
    public string Name;
    public int Hp;
    public int power;
    public int MP;
    public int SpecialCount;
    public int MaxSpeical;
    public float PlayerSpeed;

    public void PlayerSeeting(string PlayerName, int playerHp, int PlayerWeaponPower, int MaxPower,int Count, int Max, float Speed)
    {
        Name = PlayerName;
        Hp = playerHp;
        power = PlayerWeaponPower;
        MP = MaxPower;
        SpecialCount = Count;
        MaxSpeical = Max;
        PlayerSpeed = Speed;
    }
}
[System.Serializable]
public class EnemyManger //적 설정
{
    public string EnemyName;
    public float EnemyHp;
    public int AddScore;

    public void EnemySetting(string Name, float Hp,int score)
    {
        EnemyName = Name;
        EnemyHp = Hp;
        AddScore = score;
    }
}

[System.Serializable]
public class Boss
{
    public string BossName;
    public float BossHp;
    public int AddScore;

    public void BossSetting(string Name,float Hp,int score)
    {
        BossName = Name;
        BossHp = Hp;
        AddScore = score;
    }
}

[System.Serializable]
public class ItemManger
{
    public string ItemName;
    public int Score;
    public int power;
    public int AddHp;

    public void ItemSetting(string Name,int score,int powerUp)
    {
        ItemName = Name;
        Score = score;
        power = powerUp;
    }
}

public class Score
{
    public List<string> ScoreNum = new List<string>();

    public void StringNumberAdd(int count)
    {
        for(int i = 0; i < count; i++)
        {
            ScoreNum.Add("SavedScore_" + i);
        }
    }
}

//플레이어 설정,적 설정,아이템 설정, 게임 설정등
public class GameManager : Singleton<GameManager>
{
    // 플레이어 정보가 담길 딕서너리
    public Dictionary<string, PlayerDB> Players = new Dictionary<string, PlayerDB>();

    // 적 정보가 담 길 dictionary
    public Dictionary<string, EnemyManger> Enemys = new Dictionary<string, EnemyManger>();

    // 아이템 정보가 잇는 dictionary
    public Dictionary<string, ItemManger> items = new Dictionary<string, ItemManger>();

    public Dictionary<string, Boss> Bosses = new Dictionary<string, Boss>();

    public Score Sc;

    //점수 정보가 있는 list
    public List<int> ScoreList;

    public int PS = 0;
    public int Score = 0;
    public int BestScore = 0;
    public int ListCount = 0;
    public int StageNum = 1;
    public int CoinCount = 0;
    public string[] oScoreNum;
    public int[] IsCreate = new int[2];
    public int IsBGMMute;
    public int IsEffMute;

    public Text BestScoreText;


    public override void Awake() //start문 전에 실행
    {
        base.Awake();

        PlayerData();
        EnemysData();
        ItemData();
        BossData();

        Score = 0;

        /*
         * PlayerPrefs 는 키에 해당하는 값이 없을 경우 기본값으로 넘겨준 0이 반환되고
         * 점수를 얻은 다음 게임을 다시시작하게 되었을 경우 해당하는 값이 존재하면 그 해당하는 값을 불려옴
         */
        CoinCount = PlayerPrefs.GetInt("Coin");
        IsBGMMute = PlayerPrefs.GetInt("Sound");
        IsEffMute = PlayerPrefs.GetInt("EffectSound");
        
        ScoreList = new List<int>();
        oScoreNum = new string[9];

        Sc = new Score();
        Sc.StringNumberAdd(9);
    }

    void PlayerData()
    {
        PlayerDB player1 = new PlayerDB();
        player1.PlayerSeeting("basic", 6, 1,4,1,3,35f);
        Players.Add(player1.Name, player1); //이름을 키값으로 정함

        PlayerDB player2 = new PlayerDB();
        player2.PlayerSeeting("Normal", 7, 1,4,2,3,25f);
        Players.Add(player2.Name, player2);

        PlayerDB player3 = new PlayerDB();
        player3.PlayerSeeting("High", 8, 1,4,3,3,20f);
        Players.Add(player3.Name, player3);
    }

    void EnemysData()
    {
        EnemyManger enemy1 = new EnemyManger();
        enemy1.EnemySetting("Weak", 3, 5);
        Enemys.Add(enemy1.EnemyName, enemy1);

        EnemyManger enemy2 = new EnemyManger();
        enemy2.EnemySetting("Usually", 4, 10);
        Enemys.Add(enemy2.EnemyName, enemy2);

        EnemyManger enemy3 = new EnemyManger();
        enemy3.EnemySetting("Asteroid", 1, 2);
        Enemys.Add(enemy3.EnemyName, enemy3);
    }

    void ItemData()
    {
        ItemManger item1 = new ItemManger();
        item1.ItemSetting("PowerUp", 15, 1);
        items.Add(item1.ItemName, item1);

        ItemManger item2 = new ItemManger();
        item2.ItemSetting("Barrier", 20, 0);
        items.Add(item2.ItemName, item2);

        ItemManger item3 = new ItemManger();
        item3.ItemSetting("bomb", 30, 0);
        items.Add(item3.ItemName, item3);
    }

    void BossData()
    {
        Boss boss1 = new Boss();
        boss1.BossSetting("OneStageBoss", 20, 50);
        Bosses.Add(boss1.BossName, boss1);

        Boss boss2 = new Boss();
        boss2.BossSetting("TwoStageBoss", 45, 80);
        Bosses.Add(boss2.BossName, boss2);

        Boss boss3 = new Boss();
        boss3.BossSetting("ThreeStageBoss", 70, 100);
        Bosses.Add(boss3.BossName, boss3);
    }

    public Dictionary<string,PlayerDB> PlayerMGR()
    {
        return Players;
    }

    public Dictionary<string,EnemyManger> EnemyMGR()
    {
        return Enemys;
    }

    public Dictionary<string, ItemManger> itemMGR()
    {
        return items;
    }

    public Dictionary<string, Boss> BossMGR()
    {
        return Bosses;
    }

    public void BestScoreSave()
    {
        ScoreList.Add(Score);
        ScoreList.Sort(); //리스트를 정렬시킴 오름차순으로
        
        //점수를 저장할 때 점수의 리스트가 10(bestScore 포함)개가 초과되면 10개 이하로 개수를 줄임 => 가장 작은 점수값을 제거함
        if (ScoreList.Count > 9)
        {
            ScoreList.Remove(ScoreList.IndexOf(0));
            ListCount = 9;
        }
        for (int i = 0; i < ScoreList.Count; i++)
        {
            if (Score >= BestScore)
            {
                BestScore = Score;
                PlayerPrefs.SetInt("BestScore", BestScore); //지정한 키로 int 타입의 BestScore 값을 저장
                /*
                 * 오름차순으로 정렬이 되어있어서 가장 작은 값부터 가장 큰값 순서대로 정렬이됨
                 * 따라서 score점수가 bestscore점수가 같거나 크면 최고점수에 점수를 대입시키고 기존의 점수를 삭제 해야함
                 * 
                 * ArgumentOutOfRanageException : Index was out of range라는 오류가 발생
                 * ScoreList.RemoveAt(Score); => RemoveAt은 인덱스 기반으로 제거하는 함수인데 인수가 아닌 값을 집어넣어서 오류가 발생하게 되었습니다.
                */
                ScoreList.RemoveAt(ScoreList.Count - 1); //RemoveAt 리스트의 인수값을 제거해주는 역활
                break;
            }
            else
            {
                oScoreNum[i] = string.Format("Score_", i);
                PlayerPrefs.SetInt(oScoreNum[i],ScoreList[i]);
                ListCount++;
                PlayerPrefs.SetInt("Count", ListCount);
            }
        }
    }

    public void Coin()
    {
        PlayerPrefs.SetInt("Coin", CoinCount);
    }

    public void BestScoreUI()
    {
        BestScoreText.text = string.Format("Best Score : {0}", BestScore);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    public void Exit()
    {
        PlayerPrefs.Save();
        Application.Quit(); //게임을 끔
    }
}