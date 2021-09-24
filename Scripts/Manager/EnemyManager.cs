using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enemypreset
{
    Weak, Usually,Obstacle
}

public class EnemyManager : CCompo
{
    private Dictionary<string, EnemyManger> enemys;
    
    public List<GameObject> EnemyList;
    public float Hp;
    public int Score;

    public int MediumMaxNum;

    public static EnemyManager instance;

    public GameObject EnemyBulletParent;
    public GameObject EnemyHpui;

    public Camera UICamera;

    //public Vector2 HpUIPosition;


    private GameObject Enemy;
    private GameObject MediumEnemy;
    private GameObject Asteroid;

    private Vector2 tr;

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
        Enemy = Resources.Load<GameObject>("Prefabs/Enemy");
        Asteroid = Resources.Load<GameObject>("Prefabs/Asteroid");
        MediumEnemy = Resources.Load<GameObject>("Prefabs/Medium");
        EnemyList = new List<GameObject>();
    }


    private void Start()
    {       
        if(GameManager.instance.StageNum == 1)
        {
            InvokeRepeating("CreateEnemy",4f, 4f);
            InvokeRepeating("CreateMediumEnemy", 5.5f, 5.5f);
        }
        else if(GameManager.instance.StageNum == 2)
        {
            InvokeRepeating("CreateEnemy", 3f, 3f);
            InvokeRepeating("CreateMediumEnemy", 4.5f, 4.5f);
            InvokeRepeating("CreateAsteroids", 2f, 2f);
        }
        else if(GameManager.instance.StageNum == 3)
        {
            InvokeRepeating("CreateEnemy", 2f, 2f);
            InvokeRepeating("CreateMediumEnemy", 3.5f, 3.5f);
            InvokeRepeating("CreateAsteroids", 2f, 2f);
        }
    }

    private void CreateEnemy()
    {        
        float RandomX = Random.Range(-42f, 42f);
        tr = (new Vector2(RandomX, PlayerManager.instance.PlayerCamera.ViewportToWorldPoint(Vector2.up).y - 3f));
        GameObject EN = Instantiate(Enemy, tr, Quaternion.identity);
        EN.transform.parent = this.gameObject.transform;

        
        EnemyList.Add(EN);
    }

    private void CreateMediumEnemy()
    {
        if (MediumMaxNum <= 2)
        {
            float RandomX = Random.Range(-42f, 42f);
            tr = (new Vector2(RandomX, PlayerManager.instance.PlayerCamera.ViewportToWorldPoint(Vector2.up).y - 3f));
            //오브젝트 생성후
            GameObject Me = Instantiate(MediumEnemy, tr, Quaternion.identity);
            MediumMaxNum += 1;
            EnemyList.Add(Me);
        }
    }

    private void CreateAsteroids()
    {
        float RandomPoisitionX = Random.Range(-42f, 42f);
        tr = (new Vector2(RandomPoisitionX, PlayerManager.instance.PlayerCamera.ViewportToWorldPoint(Vector2.up).y - 3f));
        GameObject obstacle = Instantiate(Asteroid, tr, Quaternion.identity);

        obstacle.transform.parent = this.gameObject.transform;

        EnemyList.Add(obstacle);
    }

    public void EnemySetting(string Name)
    {
        enemys = GameManager.instance.EnemyMGR();

        EnemyManger Enemys;

        enemys.TryGetValue(string.Format("{0}", Name), out Enemys);

        Hp = Enemys.EnemyHp;
        Score = Enemys.AddScore;
    }
}
