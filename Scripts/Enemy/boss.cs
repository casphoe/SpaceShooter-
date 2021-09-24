using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossShootPosition
{
    public GameObject Mid, Left, Right;
}

public class boss : CCompo
{

    public float Hp;
    public int Score;
    public GameObject Experision;
    public GameObject BossHpSlider;
    public Canvas BossHpBarCanvas;

    
    private bool ParticleSystemPlaying => Experision.GetComponent<ParticleSystem>().isPlaying;
    private BossShootPosition ShootPosition;
    private Slider HpSlider;
    float RandomShootPosition;

    private void Start()
    {
        BossManager.instance.BossDir = gameObject.transform.position;
        BossManager.instance.EB = GetComponent<boss>();
        ShootPosition = new BossShootPosition();
        if (GameManager.instance.StageNum == 1)
        {
            BossSetting();
            BossManager.instance.BossSetting("OneStageBoss");
            Hp = BossManager.instance.Hp;
            Score = BossManager.instance.Score;
            SetHpBar();
            PoolingManager.instance.BossBulletManager(80);
            InvokeRepeating("CreateLaser", 0.6f, 1f);
        }
        else if(GameManager.instance.StageNum == 2)
        {
            BossManager.instance.BossSetting("TwoStageBoss");
            Hp = BossManager.instance.Hp;
            Score = BossManager.instance.Score;

            SetHpBar();
        }
        else if(GameManager.instance.StageNum == 3)
        {
            BossManager.instance.BossSetting("ThreeStageBoss");
            Hp = BossManager.instance.Hp;
            Score = BossManager.instance.Score;

            SetHpBar();
        }
        SoundManager.instance.BackGroundAudio.clip = SoundManager.instance.BackGroundAudio.gameObject.GetComponent<BGM>().BossBgm;
        if (GameManager.instance.IsBGMMute == 0)
        {
            SoundManager.instance.BackGroundAudio.Stop();
        }
        else
        {
            if (!SoundManager.instance.BackGroundAudio.isPlaying)
            {
                SoundManager.instance.BackGroundAudio.Play();
            }
        }
    }

    void SetHpBar()
    {
        BossHpBarCanvas = EnemyManager.instance.EnemyHpui.GetComponent<Canvas>();
        GameObject BHpBar = Instantiate(BossHpSlider, BossHpBarCanvas.transform);
        HpSlider = BHpBar.GetComponent<Slider>();

        var _BossBar = BHpBar.GetComponent<BossHp>();
        _BossBar.BossTr = this.gameObject.transform;

        HpSlider.maxValue = Hp;
        HpSlider.value = Hp;
    }

    public void TakeDamage(float amout)
    {
        Hp -= amout;
        HpSlider.value -= amout;
        if(Hp <= 0)
        {
            GameManager.instance.Score += Score;
            UIManager.instace.ScoreAddValue(GameManager.instance.Score);
            BossManager.instance.EB = gameObject.GetComponent<boss>();
            //폭팔 이펙트 실행
            ExpersionPartcleSystem(this.gameObject.transform.position);
            EnemyManager.instance.EnemyList.Remove(this.gameObject);
            CancelInvoke();

            /*
             * foreach문을 통해서 오브젝트를 비활성 화 시킬수 있다.
             * foreach(var Enemy in EnemyManager.instance.EnemyList)
                {
                    Enemy.SetActive(false);
                }
             *  하지만 딕셔너리를 제외한 컬랙션을 순회할 때는 for문이 더 퍼포면스가 뛰어나서 for문을 통해서 비활성화 시킴
             */
            for (int i = 0; i < EnemyManager.instance.EnemyList.Count; i++)
            {
                EnemyManager.instance.EnemyList[i].SetActive(false);
            }

            for (int i = 0; i < PoolingManager.instance.Enemybullets.Count; i++)
            {
                PoolingManager.instance.Enemybullets[i].SetActive(false);
            }

            Function.LateCallFunc(this, 1f, (CCompo) =>
            {
                gameObject.SetActive(false);


                UIManager.instace.StageClear();
            });
        }
    }

    private void CreateLaser()
    {
        RandomShootPosition = Random.Range(0, 2);

        switch(RandomShootPosition)
        {
            case 0:
                PoolingManager.instance.BossFireBulletCreate(ShootPosition.Mid.transform.position, Vector3.zero);
                break;
            case 1:
                PoolingManager.instance.BossFireBulletCreate(ShootPosition.Left.transform.position, Vector3.zero);
                break;
            case 2:
                PoolingManager.instance.BossFireBulletCreate(ShootPosition.Right.transform.position, Vector3.zero);
                break;
        }
    }


    private void ExpersionPartcleSystem(Vector3 tr)
    {
        GameObject EP = Instantiate(Experision, tr, Quaternion.identity);
        EP.transform.SetParent(gameObject.transform,false);

        if(!ParticleSystemPlaying)
        {
            Experision.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            Experision.GetComponent<ParticleSystem>().Stop();
        }
    }

    private void BossSetting()
    {
        ShootPosition.Mid = transform.GetChild(0).gameObject;
        ShootPosition.Left = transform.GetChild(1).gameObject;
        ShootPosition.Right = transform.GetChild(2).gameObject;
    }
}