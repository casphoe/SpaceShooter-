using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageBossNUmber
{
    stage2,stage3
}
public class BossPatten : CCompo
{

    public int index;
    public int roundNum;

    public StageBossNUmber BossNumber;

    int roundNum2;
    int roundNum3;

    private void Start()
    {
        PoolingManager.instance.BossMissleManager(200);
        BossManager.instance.BossPatten = Patten.None;
        BossManager.instance.PattenIndex = -1;
        PattenRandom();
    }

    private void PattenRandom()
    {
        BossManager.instance.PattenIndex = BossManager.instance.PattenIndex == 3 ? 0 : BossManager.instance.PattenIndex + 1;
        BossManager.instance.CurrentPattenCount = 0;
        PattenAttack();
    }

    private void PattenAttack()
    {
        switch(BossManager.instance.PattenIndex)
        {
            case 0:
                BossManager.instance.BossPatten = Patten.FireFoward;
                FireVoward();
                break;
            case 1:
                BossManager.instance.BossPatten = Patten.ShotGunAttack;
                ShotGun();
                break;
            case 2:
                BossManager.instance.BossPatten = Patten.ContinuityAttack;
                ContinueShot();
                break;
            case 3:
                BossManager.instance.BossPatten = Patten.CircleAllAttack;
                AllShot(); 
                break;
        }
    }

    void FireVoward()
    {
        BossManager.instance.CurrentPattenCount++;

        PoolingManager.instance.BossFireBulletCreate(transform.position + Vector3.right * 0.5f, Vector3.zero);
        PoolingManager.instance.BossFireBulletCreate(transform.position + Vector3.right * 1.3f, Vector3.zero);
        PoolingManager.instance.BossFireBulletCreate(transform.position + Vector3.left * 0.5f, Vector3.zero);
        PoolingManager.instance.BossFireBulletCreate(transform.position + Vector3.left * 1.3f, Vector3.zero);

        if (BossManager.instance.CurrentPattenCount < BossManager.instance.MaxPattenCount[BossManager.instance.PattenIndex])
        {
            Invoke("FireVoward", 0.6f);
        }
        else
        {
            Invoke("PattenRandom", 2.5f);
        }
    }

    void ShotGun()
    {
        for (int i = 0; i < 6; i++)
        {
            PoolingManager.instance.BossFireBulletCreate(transform.position, Vector3.zero);
        }

        BossManager.instance.CurrentPattenCount++;
        
        if (BossManager.instance.CurrentPattenCount < BossManager.instance.MaxPattenCount[BossManager.instance.PattenIndex])
        {
            Invoke("ShotGun", 0.25f);
        }
        else
        {
            Invoke("PattenRandom", 2.5f);
        }
    }

    void ContinueShot()
    {
        BossManager.instance.CurrentPattenCount++;

        PoolingManager.instance.BossFireBulletCreate(transform.position, Vector3.zero);
        
        if (BossManager.instance.CurrentPattenCount < BossManager.instance.MaxPattenCount[BossManager.instance.PattenIndex])
        {
            Invoke("ContinueShot", 0.3f);
        }
        else
        {
            Invoke("PattenRandom", 2.5f);
        }
    }

    void AllShot()
    {
        if(BossNumber == StageBossNUmber.stage2)
        {
            roundNum2 = 30;
        }
        else if(BossNumber == StageBossNUmber.stage3)
        {
            roundNum3 = 40;
        }

        roundNum = BossManager.instance.CurrentPattenCount % 2 == 0 ? roundNum2 : roundNum3;

        for(index = 0; index < roundNum; index++)
        {
            PoolingManager.instance.BossFireBulletCreate(transform.position, Vector3.zero);

            GameObject bullet = Instantiate(PoolingManager.instance.BossBulletB);
            bullet.transform.position = transform.position;
            bullet.transform.parent = EnemyManager.instance.EnemyBulletParent.transform;

            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();


            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundNum), Mathf.Sin(Mathf.PI * 2 * index / roundNum));

            rigid.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);



            Vector3 rotVec = Vector3.forward * 360 * index / roundNum + Vector3.forward * 90;

            transform.Rotate(rotVec);



        }

        BossManager.instance.CurrentPattenCount++;

        if (BossManager.instance.CurrentPattenCount < BossManager.instance.MaxPattenCount[BossManager.instance.PattenIndex])
        {
            Invoke("AllShot", 0.7f);
        }
        else
        {
            Invoke("PattenRandom",3f);
        }
    }
}