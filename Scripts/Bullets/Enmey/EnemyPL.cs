using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EnemyPL : CCompo
{
    public int Damage;
    public float Speed;
    public Enemypreset EnemySt;


    Vector3 pos;

    Vector2 dir;

    

    private void Start()
    {
        Damage = 1;
        Speed = 10f;
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    private void Move()
    {
        if(EnemySt == Enemypreset.Usually)
        {
            transform.Translate(Vector3.down * Speed * Time.deltaTime); //Translate 내부적으로 월도 로컬 옵션 구분해서 자동으로 처리
        }
        else
        {
            pos = transform.position;

            pos.y -= Speed * Time.deltaTime; //밑으로 내러감
            transform.position = pos;
        }
    }


    private void Update()
    {
        if(PlayerManager.instance.IsDead == false)
        {
            Move();
            PlayerCollison();
        }
    }

    //적의 총알이 플레이어 총알과 충돌처리 => 원의 중심 좌표와 반경을 알면 충돌 판정이 가능
    private void PlayerCollison()
    {
        Vector2 EnemyLaserRange = this.gameObject.transform.position;
        dir = EnemyLaserRange - PlayerManager.instance.PlayerDir;

        float range = dir.magnitude; //magnitude : 백터의 길이를 반환 시킴  => 두 백터의 차를 구하고 해당 백터의 길이를 구하면 두 백터간의 거리를 구할 수 잇음 오브젝트간의 거리를 체크하기 위해서

        float EL = 0.5f; //레이저 반경
        float PL = 1.0f; //플레이어 반경
        float BL = 1.0f;
        if (PlayerManager.instance.IsBarrier == true)
        {
            if(PlayerManager.instance.Invisibly == false)
            {
                if (range < EL + BL)
                {
                    PlayerManager.instance.BH.TakeDamage();
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (range < EL + BL)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (PlayerManager.instance.Invisibly == false)
            {
                if (range < EL + PL)
                {
                    //플레이어에게 데미지를 줌
                    PlayerManager.instance.SPl.TakeDamage(1);
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (range < EL + PL)
                {
                    gameObject.SetActive(false);
                }
            }
        }

    }

   
}