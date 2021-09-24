using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPL : CCompo
{
    private Vector2 dir;
    private Rigidbody2D rigid;

    public bool IsRotate;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(IsRotate)
        {
            /*
             * 2d에서 회전하는 것은 z축을 방향으로 회전을 함
             */
            transform.Rotate(Vector3.forward * 10);
        }
        PattenMove();
        PlayerCollision();
    }

    private void PattenMove()
    {
        if(BossManager.instance.BossPatten == Patten.FireFoward)
        {
            /*
             * rigid.addforce(vector3,mode) 힘을 주어서 움직이기 위한 함수
             * 첫번째 인수인 vector에 힘의 방향과 크기를 넣어주고 두번째 인수에는 힘을주는 모드를 지정
             * 
             * forceMode.Impulse
             * :
             * 충격량을 리지드바디에 주는 모드로 충격량이랑 힘의 크기와 주는 시간을 곱한 수치
             * -> 주로 타격이나 폭팔처럼 순간적으로 힘을 나타내는데 이용
             */
            rigid.AddForce(Vector2.down * 0.2f, ForceMode2D.Impulse);
            //transform.Translate(Vector3.down * Speed * Time.deltaTime); //Translate 내부적으로 월도 로컬 옵션 구분해서 자동으로 처리
        }
        else if(BossManager.instance.BossPatten == Patten.ShotGunAttack)
        {
            Vector2 pos = transform.position;
            dir = PlayerManager.instance.PlayerDir - pos;

            Vector2 ranDir = new Vector2(Random.Range(-10f, 10f), Random.Range(0f, 7f));
            dir += ranDir;
            /*
             * 우선 백터는 크기와 방향을 가진 데이터입니다
             * 각축의 크기가 1인 벡터를 단위벡터 즉 정규화된 백터(Normalized vector)라고 부름
             * => 방향만 표시하는 백터, 오브젝트의 균일한 이동을 위해서 사용 모든 방향 벡터의 길이가 1이 되어야지 방향에 따른 이동속도 가 같아짐
             */
            rigid.AddForce(dir.normalized * 0.2f, ForceMode2D.Impulse);
        }
        else if(BossManager.instance.BossPatten == Patten.ContinuityAttack)
        {
            transform.rotation = Quaternion.identity;
            /*
             * 원의 둘레값이 많이 줄수록 빠르게 파형을 그림
             * 
             * mathf.sin : 상하 공간을 끝까지 한바뀌 돌고은 방식 : -1~ 1사이의 변환 값
             * (mathf.cos : 좌우) 
             * mathf.pi 는 원주율 : 3.14159... 
             */
            Vector2 dirVec = new Vector2(Mathf.Sin(Mathf.PI * 5 * BossManager.instance.CurrentPattenCount / BossManager.instance.MaxPattenCount[BossManager.instance.PattenIndex]), -1);

            rigid.AddForce(dirVec.normalized * 0.2f, ForceMode2D.Impulse);
        }
    }

    private void PlayerCollision()
    {
        Vector2 BossLaserRange = this.gameObject.transform.position;
        dir = BossLaserRange - PlayerManager.instance.PlayerDir;

        float range = dir.magnitude; //magnitude : 백터의 길이를 반환 시킴  => 두 백터의 차를 구하고 해당 백터의 길이를 구하면 두 백터간의 거리를 구할 수 잇음 오브젝트간의 거리를 체크하기 위해서

        float EL = 0.5f; //레이저 반경
        float PL = 1.0f; //플레이어 반경
        float BL = 1.0f;
        if (PlayerManager.instance.IsBarrier == true)
        {
            if (PlayerManager.instance.Invisibly == false)
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

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
