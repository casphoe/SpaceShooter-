using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpace
{
    public ParticleSystem LeftGunfx, RightGunFx, CentralGunFx;
}

public class NormalSpace
{
    public GameObject LeftPosition, MidPosition, RightPosition;
}

public class HighSpace
{
    public GameObject LeftGunPosition, MidGunPosition, RightGunPosition;
}

public class PlayerShooting : MonoBehaviour
{

    private BasicSpace basic;
    private NormalSpace Normal;
    private HighSpace High;
    public float PowerShootTimer;

    private void Start()
    {
        if (GameManager.instance.PS == 1)
        {
            basic = new BasicSpace(); //클래스 객체 생성하고 할당 시킴
            PoolingManager.instance.PlayerBulletManager(60); 
            BasicSetting();
            PowerShootTimer = 2f;
            InvokeRepeating("BasicShoot", 2f, PowerShootTimer); //BasicShoot이라는 함수를 2초 지연시키고 실행 한 다음 1.3초마다 반복해서 실행 시킴
        }
        else if(GameManager.instance.PS == 2)
        {
            Normal = new NormalSpace();
            PoolingManager.instance.PlayerPowerfullBulletManager(70);
            PoolingManager.instance.PlayerMissle(15);
            NormalPlayerSetting();
            PowerShootTimer = 1.5f;
            InvokeRepeating("NormalShoot", 2f, PowerShootTimer);
            InvokeRepeating("MissleShoot", 8f, 5f);
        }
        else
        {
            High = new HighSpace();
            PowerShootTimer = 1.5f;
            HighSetting();
            PoolingManager.instance.PlayerMissle(20);
            PoolingManager.instance.PlayerPowerfullBulletManager(70);
            InvokeRepeating("HighShoot", 2f, PowerShootTimer);
            InvokeRepeating("PowerMissleShoot", 8f, 4.5f);
        }
    }

    private void BasicShoot()
    {
        switch(PlayerManager.instance.Power)
        {
            case 1:
                PoolingManager.instance.BulletCreate(transform.GetChild(2).gameObject.transform.position, Vector3.zero);
                basic.CentralGunFx.Play();
                break;
            case 2:
                PowerShootTimer = 1.7f;
                PoolingManager.instance.BulletCreate(transform.GetChild(0).gameObject.transform.position, Vector3.zero);
                basic.LeftGunfx.Play();
                PoolingManager.instance.BulletCreate(transform.GetChild(1).gameObject.transform.position, Vector3.zero);
                basic.RightGunFx.Play();
                break;
            case 3:
                PowerShootTimer = 1.4f;
                PoolingManager.instance.BulletCreate(transform.GetChild(2).gameObject.transform.position, Vector3.zero);
                PoolingManager.instance.BulletCreate(transform.GetChild(0).gameObject.transform.position, Vector3.zero);
                basic.LeftGunfx.Play();
                PoolingManager.instance.BulletCreate(transform.GetChild(1).gameObject.transform.position, Vector3.zero);
                basic.RightGunFx.Play();
                break;
            case 4:
                PowerShootTimer = 1f;
                PoolingManager.instance.BulletCreate(transform.GetChild(2).gameObject.transform.position, Vector3.zero);
                PoolingManager.instance.BulletCreate(transform.GetChild(0).gameObject.transform.position, Vector3.zero);
                basic.LeftGunfx.Play();
                PoolingManager.instance.BulletCreate(transform.GetChild(1).gameObject.transform.position, Vector3.zero);
                basic.RightGunFx.Play();
                break;
        }
    }

    private void NormalShoot()
    {
        
        switch(PlayerManager.instance.Power)
        {
            case 1:
                PoolingManager.instance.PowerBulletCreate(Normal.MidPosition.transform.position, Vector3.zero);
                break;
            case 2:
                PowerShootTimer = 1.2f;
                PoolingManager.instance.PowerBulletCreate(Normal.MidPosition.transform.position, Vector3.zero);
                PoolingManager.instance.PowerBulletCreate(Normal.LeftPosition.transform.position, Vector3.zero);
                PoolingManager.instance.PowerBulletCreate(Normal.RightPosition.transform.position, Vector3.zero);               
                break;
            case 3:
                PowerShootTimer = 1f;               
                PoolingManager.instance.PowerBulletCreate(Normal.MidPosition.transform.position, Vector3.zero);
                PoolingManager.instance.PowerBulletCreate(Normal.LeftPosition.transform.position, Vector3.zero);
                PoolingManager.instance.PowerBulletCreate(Normal.RightPosition.transform.position, Vector3.zero);
                break;
            case 4:
                PowerShootTimer = 0.8f;                
                PoolingManager.instance.PowerBulletCreate(Normal.MidPosition.transform.position, Vector3.zero);
                PoolingManager.instance.PowerBulletCreate(Normal.LeftPosition.transform.position, Vector3.zero);
                PoolingManager.instance.PowerBulletCreate(Normal.RightPosition.transform.position, Vector3.zero);
                break;
        }
    }

    private void HighShoot()
    {
        switch(PlayerManager.instance.Power)
        {
            case 1:
                PoolingManager.instance.PowerBulletCreate(High.MidGunPosition.transform.position, Vector3.zero);
                break;
            case 2:
                PowerShootTimer = 1.2f;
                PoolingManager.instance.PowerBulletCreate(High.MidGunPosition.transform.position, Vector3.zero);
                PoolingManager.instance.PowerBulletCreate(High.LeftGunPosition.transform.position, Vector3.zero);
                PoolingManager.instance.PowerBulletCreate(High.RightGunPosition.transform.position, Vector3.zero);
                break;
            case 3:
                PowerShootTimer = 1f;
                PoolingManager.instance.PowerBulletCreate(High.MidGunPosition.transform.position, Vector3.zero);
                PoolingManager.instance.PowerBulletCreate(High.LeftGunPosition.transform.position, Vector3.zero);
                PoolingManager.instance.PowerBulletCreate(High.RightGunPosition.transform.position, Vector3.zero);
                break;
            case 4:
                PowerShootTimer = 0.8f;
                PoolingManager.instance.PowerBulletCreate(High.MidGunPosition.transform.position, Vector3.zero);
                PoolingManager.instance.PowerBulletCreate(High.LeftGunPosition.transform.position, Vector3.zero);
                PoolingManager.instance.PowerBulletCreate(High.RightGunPosition.transform.position, Vector3.zero);
                break;
        }
    }

    private void NormalPlayerSetting()
    {
        Normal.LeftPosition = this.gameObject.transform.GetChild(2).gameObject;
        Normal.MidPosition = this.gameObject.transform.GetChild(3).gameObject;
        Normal.RightPosition = this.gameObject.transform.GetChild(4).gameObject;
    }

    private void BasicSetting()
    {
        //GetChild(인덱스) : 이 오브젝트의 인덱스 숫자에 있는 자식 오브젝트의 접근
        basic.LeftGunfx = this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        basic.RightGunFx = this.gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        basic.CentralGunFx = this.gameObject.transform.GetChild(2).GetComponent<ParticleSystem>();
    }

    private void HighSetting()
    {
        High.LeftGunPosition = this.gameObject.transform.GetChild(2).gameObject;
        High.MidGunPosition = this.gameObject.transform.GetChild(3).gameObject;
        High.RightGunPosition = this.gameObject.transform.GetChild(4).gameObject;
    }

    private void MissleShoot()
    {
        PoolingManager.instance.PlayerMissleCreate(Normal.MidPosition.transform.position, Vector3.zero);
    }

    public void PowerMissleShoot()
    {
        PoolingManager.instance.PlayerMissleCreate(High.MidGunPosition.transform.position, Vector3.zero);
    }

   
}
