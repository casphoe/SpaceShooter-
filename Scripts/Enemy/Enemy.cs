using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    Dead,None,Shoot
}

public class Medium
{
    public GameObject Left, Center, Right;
}

public class Enemy : CCompo
{
    private Vector3 pos;
    private Vector3 half;
    private Vector3 max;
   
    private float Speed;

    private int preset = 0;
    private float RotateSpeed = 5.0f;
    private float ShootTime;
    private float timer;
    private EnemyState EnemyStateSet;
    private int MN = 0;
    private Medium Med;

    public GameObject Destruction; //폭팔 이펙트
    public Enemypreset EnemySt;
    public float Hp;
    public int Score;

    public GameObject HpImage;
    public Canvas enemyHpBarCanvas;

    private Slider HpbarSlider;
    private GameObject hpBar;
    Vector2 dir;

    private void Start()
    {
        pos = transform.position;
        ShootTime = 2f;
        timer = 0f;
        if (EnemySt == Enemypreset.Weak)
        {
            EnemyManager.instance.EnemySetting("Weak");           
            preset = 0;
            if (GameManager.instance.StageNum == 1)
            {
                Hp = EnemyManager.instance.Hp;
            }
            else if (GameManager.instance.StageNum == 2)
            {
                Hp = EnemyManager.instance.Hp + 1;
            }
            else if (GameManager.instance.StageNum == 3)
            {
                Hp = EnemyManager.instance.Hp + 2;
            }
            Score = EnemyManager.instance.Score;
        }
        else if(EnemySt == Enemypreset.Usually)
        {
            EnemyManager.instance.EnemySetting("Usually");
            preset = 1;

            if (GameManager.instance.StageNum == 1)
            {
                Hp = EnemyManager.instance.Hp;
            }
            else if (GameManager.instance.StageNum == 2)
            {
                Hp = EnemyManager.instance.Hp + 1;
            }
            else if (GameManager.instance.StageNum == 3)
            {
                Hp = EnemyManager.instance.Hp + 2;
            }
            Score = EnemyManager.instance.Score;

            Med = new Medium();
            //자식의 위치를 찾음
            Med.Center = transform.GetChild(1).gameObject; //2번째 자식의 오브젝트
            Med.Left = transform.GetChild(0).gameObject;
            Med.Right = transform.GetChild(2).gameObject;
            SetHpBar();

        }
        else
        {
            EnemyManager.instance.EnemySetting("Asteroid");
            preset = 2;
            Speed = 12f;

            Hp = EnemyManager.instance.Hp;
            Score = EnemyManager.instance.Score;
        }
        Speed = 8.5f;
        PoolingManager.instance.EnemyBulletManager(100);
        PoolingManager.instance.EnemyFireBulletManager(150);
        EnemyStateSet = EnemyState.None;
        half = PlayerManager.instance.PlayerCamera.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        max = PlayerManager.instance.PlayerCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    private void Update()
    {
        Move();
    }

    void SetHpBar()
    {
        enemyHpBarCanvas = EnemyManager.instance.EnemyHpui.GetComponent<Canvas>();
        hpBar = Instantiate(HpImage, enemyHpBarCanvas.transform);
        HpbarSlider = hpBar.GetComponent<Slider>();

        var _hpBar = hpBar.GetComponent<EnemyHp>();
        _hpBar.enemyTr = this.gameObject.transform;
        HpbarSlider.maxValue = Hp;
        HpbarSlider.value = Hp;
    }

    //적의 상태에 따라 이동하는 구간을 다르게 줌
    void Move()
    {
        //맵에 절반만큼 아래로 이동한다음 다시 위쪽으로 돌아서 이동시킴
        if (preset == 0)
        {
            switch (MN)
            {
                case 0:
                    pos.y -= Speed * Time.deltaTime;
                    transform.position = pos;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, 360.0f), RotateSpeed * Time.deltaTime);
                    Shoot();

                    if (pos.y <= half.y)
                    {
                        MN = 1;
                    }
                    break;
                case 1:
                    pos = transform.position;
                    pos.y += Speed * Time.deltaTime;
                    transform.position = pos;
                    //Quaternion.Slerp : 오브젝트 회전을 time 동안 부드러운 회전을 함
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, 180.0f), RotateSpeed * Time.deltaTime);
                    if (pos.y >= max.y)
                    {
                        MN = 0;
                    }
                    break;
            }
        }
        //맵에 절반만큼 아래로 이동할 때 위에서 아래로 z값을 변화한다음 중앙에 위치할 때 z = 0을 주어서 총알 생성 및 공격할 수 있게 함
        else if(preset == 1)
        {
            pos.y -= Speed * Time.deltaTime;
            transform.position = pos;

            if (pos.y <= half.y)
            {
                pos.y = half.y;
            }
            transform.position = pos;
            //움직임을 멈추고 그자리에서 총알을 생성 발사 함
            FireBulletShot();
        }
        else
        {
            pos = transform.position;

            pos.y -= Speed * Time.deltaTime;

            transform.position = pos;

            PlayerCollsion();
        }
    }

    public void EnemyDamage(float amout)
    {
        Hp -= amout;
        if(EnemySt == Enemypreset.Usually)
        {
            HpbarSlider.value -= amout;
        }
        if(Hp <= 0)
        {
            EnemyStateSet = EnemyState.Dead;
            Instantiate(Destruction, this.gameObject.transform.position, Quaternion.identity);
            if(EnemySt == Enemypreset.Usually)
            {
                hpBar.SetActive(false);
                EnemyManager.instance.MediumMaxNum -= 1;
            }


            /*
             * 람다 => 이름이 없는 함수(잠깐 쓰고 버리는 함수) 자료형을 생략할 수 있음 => 알아서 판단
             * 
             * c# 람다 함수의 원형
             * :
             * - (int a_nLhs, int a_nRhs) => a_nLhs + a_nRhs
             * 
             * - (int a_nLhs, int a_nRhs) => {return a_nLhs + a_nRhs; };
             * 
             * - c# 언어의 람다는 {} (중괄호 연산자)의 여부에 따라서 단일행(식 형식) 람다와 문 형식의 람다로 구분
             * 또한 람다는 매개 변수의 자료형을 생략하는 것이 가능하다.
             * (즉, 람다 함수 매개 변수의 자료형을 컴파일러에게 하여금 추측하게 만드는 것이 가능하다)
             */


            //0.5초 이후에 오브젝트를 꺼줌
            Function.LateCallFunc(this, 0.5f, (CCompo) =>
            {
                GameManager.instance.Score += Score;
                UIManager.instace.ScoreAddValue(GameManager.instance.Score);
                gameObject.SetActive(false);
                /*
                 * 죽은 적의 리스트를 제거하기 않게 되면 미사일이 가장 가까이 잇는 적의 위치에 가서 타격하는 방식인데
                 * 리스트가 그대로 있고 삭제를 안시켜주니 가까이 있는 적의 위치가 바끼지 않는 이상 똑같은 자리로 게속 이동하게 되는 현상이 발생한다.
                 */
                EnemyManager.instance.EnemyList.Remove(this.gameObject);

            });

            if (GameManager.instance.IsEffMute == 0)
            {
                SoundManager.instance.EffectAudio.transform.GetChild(6).GetComponent<AudioSource>().Stop();
            }
            else
            {
                if (!SoundManager.instance.EffectAudio.transform.GetChild(6).GetComponent<AudioSource>().isPlaying)
                {
                    SoundManager.instance.EffectAudio.transform.GetChild(6).GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    private void Shoot()
    {
        timer += Time.deltaTime;
        if(timer > ShootTime)
        {
            PoolingManager.instance.EnemyBulletCreate(transform.position, Vector3.zero);

            timer = 0f;
        }
    }

    private void FireBulletShot()
    {
        timer += Time.deltaTime;

        if(timer > ShootTime)
        {
            PoolingManager.instance.EnemyFireBulletCreate(Med.Center.transform.position, Vector3.zero);
            PoolingManager.instance.EnemyFireBulletCreate(Med.Left.transform.position, new Vector3(0, 0, -25));
            PoolingManager.instance.EnemyFireBulletCreate(Med.Right.transform.position, new Vector3(0, 0, 25));

            timer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //부딪쳤을 때 데미지를 입음
    {
        if(collision.CompareTag("Player"))
        {
            PlayerManager.instance.SPl.TakeDamage(1);
            
        }
    }

    private void PlayerCollsion()
    {
        dir = gameObject.transform.position;

        Vector2 Range = dir - PlayerManager.instance.PlayerDir;

        float distance = Range.magnitude;

        float OL = 0.5f;
        float PL = 1.0f;
        float BL = 1.0f;

        if (PlayerManager.instance.IsBarrier == true)
        {
            if (PlayerManager.instance.Invisibly == false)
            {
                if (distance < OL + BL)
                {
                    PlayerManager.instance.BH.TakeDamage();
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (distance < OL + BL)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (PlayerManager.instance.Invisibly == false)
            {
                if (distance < OL + PL)
                {
                    PlayerManager.instance.SPl.TakeDamage(1);
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (distance < OL + PL)
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