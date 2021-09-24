using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 샷건 씩 공격,부채꼴 총알 발사
 * 원형태 전체 공격
 */
public enum Patten
{
    None, FireFoward, ShotGunAttack, ContinuityAttack, CircleAllAttack
}

public class BossManager : CCompo
{
    private Dictionary<string, Boss> Boss;

    public float Hp;
    public int Score;
    public bool IsBossCreate = false;

    public GameObject BossHpUI;
    public GameObject BossCreatePosition;

    public Vector2 BossDir;

    public static BossManager instance;
    public boss EB;
    public Patten BossPatten;

    public int CurrentPattenCount;
    public int[] MaxPattenCount = new int[4];
    public int PattenIndex;

    private GameObject Stage1;
    private GameObject Stage2;
    private GameObject Stage3;
    private List<float> BossCreatGameTime = new List<float>(); //이동한 거리값에 따라서 보스를 생성시킴(스테이지 진행 시간에 따라서)
    private float Timer;

    public override void Awake()
    {
        base.Awake();
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            return;
        }
        Stage1 = Resources.Load<GameObject>("Prefabs/OneStageBoss");
        Stage2 = Resources.Load<GameObject>("Prefabs/TwoStageBoss");
        Stage3 = Resources.Load<GameObject>("Prefabs/ThreeStageBoss");
    }

    private void Start()
    {
        IsBossCreate = false;
        for(int i = 1; i <= 3; i++)
        {
            BossCreatGameTime.Add(40.0f * i); //스테이지 1은 120초, 2은 240초 3은 360초
        }

        if(GameManager.instance.StageNum == 1)
        {
            Timer = BossCreatGameTime[0];
        }
        else if(GameManager.instance.StageNum == 2)
        {
            Timer = BossCreatGameTime[1];
        }
        else
        {
            Timer = BossCreatGameTime[2];
        }

        Invoke("CreateBoss", Timer); //CreateBoss Timer 시간동안 지연시키는 역활을 함 => Timer에 설정된 시간후 함수 CreateBoss 실행함

    }


    //GameManager에서 설정된 bossdata 함수에 key값을 string 이름으로 정해놔서 키값을 통해서 값을 받아옴
    public void BossSetting(string Name)
    {
        Boss = GameManager.instance.BossMGR();

        Boss bosses;

        Boss.TryGetValue(string.Format("{0}", Name), out bosses);

        Hp = bosses.BossHp;
        Score = bosses.AddScore;
    }


    public void CreateBoss()
    {
        GameObject Boss;

        UIManager.instace.WaringText.gameObject.SetActive(true);

        Function.LateCallFunc(this, 1.5f, (CCompo) =>
        {
            UIManager.instace.WaringText.gameObject.SetActive(false);
            if (PlayerManager.instance.IsDead == false)
            {
                switch (GameManager.instance.StageNum) //스테이지
                {
                    case 1:
                        Boss = Instantiate(Stage1, BossCreatePosition.transform.position, Quaternion.identity);
                        IsBossCreate = true;
                        EnemyManager.instance.EnemyList.Add(Boss);
                        break;
                    case 2:
                        Boss = Instantiate(Stage2, BossCreatePosition.transform.position, Quaternion.identity);
                        IsBossCreate = true;
                        EnemyManager.instance.EnemyList.Add(Boss);
                        break;
                    case 3:
                        Boss = Instantiate(Stage3, BossCreatePosition.transform.position, Quaternion.identity);
                        IsBossCreate = true;
                        EnemyManager.instance.EnemyList.Add(Boss);
                        break;
                }
            }

        });

        

    }


}
