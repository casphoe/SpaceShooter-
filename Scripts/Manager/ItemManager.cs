using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 생성 및 화면에서 안보이면 오브젝트를 꺼주는 기능
public class ItemManager : CCompo
{

    private Dictionary<string, ItemManger> itmes;

    private List<GameObject> Items = new List<GameObject>();

    private GameObject PowerUp;
    private GameObject Barrier;
    private GameObject boomb;
    private float RandomItemX;

    public int Score;
    public int power;
    


    public static ItemManager instance;

    private new void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            return;
        }
        PowerUp = Resources.Load<GameObject>("Prefabs/PowerUp");
        boomb = Resources.Load<GameObject>("Prefabs/Bomb");
        Barrier = Resources.Load<GameObject>("Prefabs/Invisbly");
    }

    // GameManager에 설정된 아이템 설정값을 받아옴
    public void ItemStat(string Name)
    {
        itmes = GameManager.instance.itemMGR();

        ItemManger Item;
        itmes.TryGetValue(string.Format("{0}", Name), out Item);

        Score = Item.Score;
        power = Item.power;
    }
    //게임이 시작할 때 코류틴을 실행
    private void Start()
    {
        InvokeRepeating("PowerUpCreation", 3f, 3f);
        InvokeRepeating("BombCreation", 10f, 10f);

        InvokeRepeating("BarrierCreation", 6f, 6f);
    }

    void PowerUpCreation() //코류틴 : 어떠한 작업을 처리할 때 필요에 따라 시간 간격을 두고 작업을 처리할 수 있도록 도와주는 함수
    {
        RandomItemX = Random.Range(-42f, 42f);
        GameObject power = Instantiate(PowerUp, new Vector2(RandomItemX, PlayerManager.instance.PlayerCamera.ViewportToWorldPoint(Vector2.up).y - 3f), Quaternion.identity);
        Items.Add(PowerUp);
        power.transform.parent = this.gameObject.transform;

    }


    void BombCreation()
    {
        RandomItemX = Random.Range(-42f, 42f);

        GameObject Special = Instantiate(boomb, new Vector2(RandomItemX, PlayerManager.instance.PlayerCamera.ViewportToWorldPoint(Vector2.up).y - 3f), Quaternion.identity);
        Items.Add(boomb);

        Special.transform.parent = this.gameObject.transform;
    }

    
    void BarrierCreation()
    {
        RandomItemX = Random.Range(-42f, 42f);

        GameObject Bar = Instantiate(Barrier, new Vector2(RandomItemX, PlayerManager.instance.PlayerCamera.ViewportToWorldPoint(Vector2.up).y - 3f), Quaternion.identity);

        Items.Add(Barrier);

        Bar.transform.parent = this.gameObject.transform;
    }
}