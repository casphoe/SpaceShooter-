using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//총알 관리
public class PoolingManager : CCompo
{

    public List<GameObject> Playerbullets = new List<GameObject>(); //총알을 담아둘 리스트를 만듬
    public List<GameObject> Enemybullets = new List<GameObject>(); //적들의 총알을 담아둘 리스트를 만듬(오브젝트 풀)
    public List<GameObject> EnemyPowerFullBullets = new List<GameObject>();
    public List<GameObject> PlayerMissles = new List<GameObject>();
    public List<GameObject> BossBullets = new List<GameObject>();
    public List<GameObject> FollowerBullets = new List<GameObject>();

    private GameObject Laser;
    private GameObject EnemyLaser;
    private GameObject EnemyFireBall;
    private GameObject BossLaser;
    private GameObject PowerFullLaser;
    private GameObject Missle;
    private GameObject BossBullet;
    private GameObject FollowerLaser;
    public GameObject BossBulletB;
    

    public static PoolingManager instance;


    private new void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            return;
        }
        Laser = Resources.Load<GameObject>("Prefabs/Player_Short_Lazer");
        EnemyLaser = Resources.Load<GameObject>("Prefabs/Enemy_Straight_Projetile");
        EnemyFireBall = Resources.Load<GameObject>("Prefabs/EnemyFireball");
        BossLaser = Resources.Load<GameObject>("Prefabs/BossLaser");
        PowerFullLaser = Resources.Load<GameObject>("Prefabs/LaserFire2");
        Missle = Resources.Load<GameObject>("Prefabs/PlayerMissle");
        BossBullet = Resources.Load<GameObject>("Prefabs/BossBullet");
        FollowerLaser = Resources.Load<GameObject>("Prefabs/Follower_Laser");
    }

    public void PlayerMissle(int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject PM = Instantiate(Missle) as GameObject;

            PM.transform.parent = PlayerManager.instance.Bullet.transform;

            PM.SetActive(false);

            PlayerMissles.Add(PM);
        }
    }

    public void PlayerPowerfullBulletManager(int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject PowerLaser = Instantiate(PowerFullLaser) as GameObject;

            PowerLaser.transform.parent = PlayerManager.instance.Bullet.transform;

            PowerLaser.SetActive(false);
            Playerbullets.Add(PowerLaser);
        }
    }

    public void PlayerBulletManager(int Count)
    {
        for(int i = 0; i < Count; i++)
        {
            //Instantiate 로 생성한 게임 오브젝트를 변수에 담고자 하기 위해서 as + 데이터 타입을 명령어 뒤에 붙여주어야함
            GameObject PL = Instantiate(Laser) as GameObject;
            PL.transform.parent = PlayerManager.instance.Bullet.transform;
            PL.SetActive(false);

            Playerbullets.Add(PL);
        }
    }

    public void FollowerBulletManager(int Count)
    {
        for(int i = 0; i < Count; i++)
        {
            GameObject FL = Instantiate(FollowerLaser) as GameObject;
            FL.transform.parent = PlayerManager.instance.Bullet.transform;
            FL.SetActive(false);

            FollowerBullets.Add(FL);
        }
    }

    public GameObject PlayerMissleCreate(Vector3 pos,Vector3 rot)
    {
        GameObject Pmissle = null;

        for(int i = 0; i < PlayerMissles.Count; i++)
        {
            if(PlayerMissles[i].activeSelf == false)
            {
                Pmissle = PlayerMissles[i];

                break;
            }
        }

        if(Pmissle == null)
        {
            /*
             * Euler angle의 각도는 x,y,z 3개 축을 기준으로 회전시키는 우리가 알고 있는 각도계이다.
             * 오브젝트를 회전하기 위해서 사용되는 transform.rotation은 Quaterion을 기반으로 한다.
             * Quaternion.Euler : Quaterion을 우리가 알고 잇는 Euler angle으로 바꾸어 사용하기 위해서 사용
             */
            GameObject newMissle = Instantiate(Missle, pos, Quaternion.Euler(rot));

            newMissle.transform.parent = PlayerManager.instance.Bullet.transform;

            PlayerMissles.Add(newMissle);

            Pmissle = newMissle;
        }

        Pmissle.SetActive(true);
        Pmissle.transform.position = pos;      
        return Pmissle;
    }

    public GameObject FollowerBulletCreate(Vector3 pos, Vector3 rot)
    {
        GameObject FBullet = null;

        for(int i = 0; i< FollowerBullets.Count; i++)
        {
            if(FollowerBullets[i].activeSelf == false)
            {
                FBullet = FollowerBullets[i];

                break;
            }
        }

        if(FBullet == null)
        {
            GameObject newFollowerLaser = Instantiate(FollowerLaser, pos, Quaternion.Euler(rot));

            newFollowerLaser.transform.parent = PlayerManager.instance.Bullet.transform;

            FollowerBullets.Add(newFollowerLaser);

            FBullet = newFollowerLaser;
        }
        FBullet.SetActive(true);
        FBullet.transform.position = pos;
        return FBullet;
    }

    public GameObject BulletCreate(Vector3 pos, Vector3 rot)
    {
        GameObject PlayerLaser = null;
        for(int i = 0; i < Playerbullets.Count; i++)
        {
            if(Playerbullets[i].activeSelf == false)
            {
                PlayerLaser = Playerbullets[i]; //비활성화 되어있는 총알을 찾아 PlayerLaser에 담아줌

                break;
            }
        }

        if(PlayerLaser == null) //PlayerLaser 오브젝트의 총알을 없을 경우 => 추가 총알 생성
        {
            GameObject newLaser = Instantiate(Laser, pos, Quaternion.Euler(rot));
            newLaser.transform.parent = PlayerManager.instance.Bullet.transform;
            Playerbullets.Add(newLaser);

            PlayerLaser = newLaser;
        }

        PlayerLaser.SetActive(true);
        PlayerLaser.transform.position = pos;
        return PlayerLaser;
    }

    public GameObject PowerBulletCreate(Vector3 pos, Vector3 rot)
    {
        GameObject PowerLaser = null;
        
        for(int i = 0; i < Playerbullets.Count; i++)
        {
            if(Playerbullets[i].activeSelf == false)
            {
                PowerLaser = Playerbullets[i];

                break;
            }
        }

        if(PowerLaser == null)
        {
            GameObject newPowerLaser = Instantiate(PowerFullLaser, pos, Quaternion.Euler(rot));

            newPowerLaser.transform.parent = PlayerManager.instance.Bullet.transform;

            Playerbullets.Add(newPowerLaser);

            PowerLaser = newPowerLaser;
        }

        PowerLaser.SetActive(true);
        PowerLaser.transform.position = pos;
        return PowerLaser;
    }


    public void EnemyBulletManager(int Count)
    {
        for(int i = 0; i < Count; i++)
        {
            GameObject EL = Instantiate(EnemyLaser) as GameObject;
            EL.transform.parent = EnemyManager.instance.EnemyBulletParent.transform;
            EL.SetActive(false);

            Enemybullets.Add(EL);
        }
    }

    public GameObject EnemyBulletCreate(Vector3 pos, Vector3 rot)
    {
        GameObject EnemyLaser = null;

        for(int i = 0; i < Enemybullets.Count; i++)
        {
            if(Enemybullets[i].activeSelf == false)
            {
                EnemyLaser = Enemybullets[i]; //비활성화 되어있는 적들의 총알을 찾아 Enemybullets이라는 리스트 배열의 담아줌

                break; //if문을 빠져나옴
            }
        }

        if(EnemyLaser == null)
        {
            GameObject newEnemyLaser = Instantiate(EnemyLaser, pos, Quaternion.Euler(rot));

            newEnemyLaser.transform.parent = EnemyManager.instance.EnemyBulletParent.transform; //생성할 적의 총알의 부모를 설정

            Enemybullets.Add(newEnemyLaser);

            EnemyLaser = newEnemyLaser;
        }

        EnemyLaser.SetActive(true); //오브젝트를 켜줌
        EnemyLaser.transform.position = pos;
        return EnemyLaser;
    }


    public void EnemyFireBulletManager(int Count)
    {
        for(int i = 0; i < Count; i++)
        {
            GameObject EF = Instantiate(EnemyFireBall) as GameObject;
            EF.transform.parent = EnemyManager.instance.EnemyBulletParent.transform;

            EF.SetActive(false);

            EnemyPowerFullBullets.Add(EF);
        }
    }
    

    public GameObject EnemyFireBulletCreate(Vector3 pos, Vector3 rot)
    {
        GameObject EnemyFireBullet = null;

        for(int i = 0; i < EnemyPowerFullBullets.Count; i++)
        {
            if(EnemyPowerFullBullets[i].activeSelf == false)
            {
                EnemyFireBullet = EnemyPowerFullBullets[i];

                break;
            }
        }

        if(EnemyFireBullet == null)
        {
            GameObject newFireBullet = Instantiate(EnemyFireBullet, pos, Quaternion.Euler(rot));

            newFireBullet.transform.parent = EnemyManager.instance.EnemyBulletParent.transform;

            EnemyPowerFullBullets.Add(newFireBullet);

            EnemyFireBullet = newFireBullet;
        }
        EnemyFireBullet.SetActive(true);
        EnemyFireBullet.transform.position = pos;


        return EnemyFireBullet;
    }

    public void BossBulletManager(int count)
    {
        for(int i = 0; i< count; i++)
        {
            GameObject BL = Instantiate(BossLaser) as GameObject;
            BL.transform.parent = EnemyManager.instance.EnemyBulletParent.transform;

            BL.SetActive(false);

            BossBullets.Add(BL);
        }
    }

    public GameObject BossFireBulletCreate(Vector3 pos, Vector3 rot)
    {
        GameObject BossLaserBullet = null;

        for(int i = 0; i < BossBullets.Count; i++)
        {
            if(BossBullets[i].activeSelf == false)
            {
                BossLaserBullet = BossBullets[i];

                break;
            }
        }

        if(BossLaserBullet == null)
        {
            GameObject newBossLaserBullet = Instantiate(BossLaserBullet, pos, Quaternion.Euler(rot));

            newBossLaserBullet.transform.parent = EnemyManager.instance.EnemyBulletParent.transform;

            BossBullets.Add(newBossLaserBullet);

            BossLaserBullet = newBossLaserBullet;
        }

        BossLaserBullet.SetActive(true);
        BossLaserBullet.transform.position = pos;
        return BossLaserBullet;
    }

    public void BossMissleManager(int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject BM = Instantiate(BossBullet) as GameObject;

            BM.transform.parent = EnemyManager.instance.EnemyBulletParent.transform;

            BM.SetActive(false);

            BossBullets.Add(BM);
        }
    }

    public GameObject BossMissleBulletCreate(Vector3 pos,Vector3 rot)
    {
        GameObject BossBullet = null;

        for(int i = 0; i < BossBullets.Count; i++)
        {
            if(BossBullets[i].activeSelf == false)
            {
                BossBullet = BossBullets[i];

                break;
            }
        }

        if(BossBullet == null)
        {
            GameObject newMissleBullet = Instantiate(BossBullet, pos, Quaternion.Euler(rot));

            newMissleBullet.transform.parent = EnemyManager.instance.EnemyBulletParent.transform;

            BossBullets.Add(newMissleBullet);

            BossBullet = newMissleBullet;
        }
        BossBullet.SetActive(true);
        BossBullet.transform.position = pos;
        return BossBullet;
    }
}