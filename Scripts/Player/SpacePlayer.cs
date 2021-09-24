using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FollowNum
{
    One,Two,Three
}

//플레이어 움직임,죽음설정,데미지입혔을 때 설정등
public class SpacePlayer : CCompo
{
    public GameObject[] Follower;
    public FollowNum Number;

    private SpriteRenderer Sprite;

    private void Start()
    {
        PlayerManager.instance.IsDead = false;
        PlayerManager.instance.SPl = GetComponent<SpacePlayer>();
        PlayerManager.instance.Invisibly = false;
        Sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(PlayerManager.instance.IsDead == false)
        {
            Move();
            PlayerManager.instance.PlayerDir = this.gameObject.transform.position; //캐릭터 중심의 좌표
        }
    }
    //모바일 버전, pc버전 다르게 이동하기
    private void Move()
    {

#if UNITY_STANDALONE
        //이 게임의(어플리케이션) 플랫폼이 pc버전이었을 때
        PcMove();
#elif UNITY_ANDROID
        //이 게임의(어플리케이션) 플랫폼 안드로이드라면
        AndroidMove();
        
        
#endif
        BlockMove();
    }

    public void TakeDamage(int amout)
    {
        PlayerManager.instance.Hp -= amout;
        UIManager.instace.PlayerHpSlider.value -= amout;

        Invisble();
        if (PlayerManager.instance.Hp <= 0)
        {
            Die();
        }

        if (GameManager.instance.IsEffMute == 0)
        {
            SoundManager.instance.EffectAudio.transform.GetChild(1).GetComponent<AudioSource>().Stop();
        }
        else
        {
            if (!SoundManager.instance.EffectAudio.transform.GetChild(1).GetComponent<AudioSource>().isPlaying)
            {
                SoundManager.instance.EffectAudio.transform.GetChild(1).GetComponent<AudioSource>().Play();
            }
        }
    }
    //플레이어가 죽었을 때
    private void Die()
    {
        PlayerManager.instance.IsDead = true;
        for (int i = 0; i < EnemyManager.instance.EnemyList.Count; i++)
        {
            EnemyManager.instance.EnemyList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < PoolingManager.instance.Playerbullets.Count; i++)
        {
            PoolingManager.instance.Playerbullets[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < PoolingManager.instance.Enemybullets.Count; i++)
        {
            PoolingManager.instance.Enemybullets[i].gameObject.SetActive(false);
        }
        CancelInvoke(); //반복되고 있는 invoke를 취소함
        GameManager.instance.CoinCount += GameManager.instance.Score / 10;

        GameManager.instance.Coin();
        GameManager.instance.BestScoreSave();

        DeadEffect(transform.position, Vector3.zero);

        Function.LateCallFunc(this, 1f, (CCompo) =>
        {
            gameObject.SetActive(false);

            //씬을 결과창으로 이동함
            UIManager.instace.PlayerDead();
        });
    }

    private void BlockMove() //게임 화면상으로 나가지 못하게 막음
    {
        //화면 왼쪽 하단의 월드 좌표를 뷰포트에서 얻기
        Vector2 min = PlayerManager.instance.PlayerCamera.ViewportToWorldPoint(new Vector2(0, 0));

        //화면 오른쪽 상단의 월드좌표를 뷰포트에서 얻기
        Vector2 max = PlayerManager.instance.PlayerCamera.ViewportToWorldPoint(new Vector2(1, 1));

        Vector2 playerpos = transform.position;

        //플레이어의 위치를 화면(카메라의 절두체)에 맞게 제한시킴
        playerpos.x = Mathf.Clamp(playerpos.x, min.x, max.x);
        playerpos.y = Mathf.Clamp(playerpos.y, min.y, max.y); //mathf.clamp playerpos의 값의 최소값 최대값에 사이로 제한

        transform.position = playerpos;

        if (GameManager.instance.IsEffMute == 0)
        {
            SoundManager.instance.EffectAudio.transform.GetChild(5).GetComponent<AudioSource>().Stop();
        }
        else
        {
            if (!SoundManager.instance.EffectAudio.transform.GetChild(5).GetComponent<AudioSource>().isPlaying)
            {
                SoundManager.instance.EffectAudio.transform.GetChild(5).GetComponent<AudioSource>().Play();
            }
        }
    }

    void DeadEffect(Vector2 tr, Vector3 rot)
    {
        GameObject PlayerExplosion = Instantiate(PlayerManager.instance.DeadFealx, tr, Quaternion.identity);

        PlayerExplosion.transform.parent = this.gameObject.transform;
    }


    private void AndroidMove()
    {
        //만약 터치카운트(터치한 갯수 멀티터치라면 카운트 = 2 그리고 터치한 손가락이 움직이고 있다면
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector3 pos = Input.GetTouch(0).position; //백터 pos에 터치한 손가락 포지션을 가져옴

            Vector3 wordldPos = Camera.main.ScreenToWorldPoint(pos);

            pos.z = transform.position.z;

            transform.position = Vector3.MoveTowards(transform.position, pos, PlayerManager.instance.Speed * Time.deltaTime);
        }
    }

    private void PcMove()
    {
        if (Input.GetMouseButton(0)) //좌클릭 마우스 버튼이 눌렸을 경우
        {
            Vector3 mousePosition = PlayerManager.instance.PlayerCamera.ScreenToWorldPoint(Input.mousePosition);
            // mousePosition이라는 좌표에 게임화면 상에 마우스 포인터가 위치한 좌표를 월드 공간으로 바꾸어서 나온 값을 대입
            mousePosition.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, mousePosition, PlayerManager.instance.Speed * Time.deltaTime); //스크립트가 있는 오브젝트의 위치가 게임화면에 마우스포인터가 위치한 공간으로 30 * Time.deltaTime 속도 만큼 이동시킴
        }
    }

    private void Invisble()
    {
        PlayerManager.instance.Invisibly = true;
        //무적 이펙트 => 투명
        Sprite.color = new Color(1, 1, 1, 0.5f);

        Invoke("UnInvisble", 1.2f);
    }
    
    private void UnInvisble()
    {
        PlayerManager.instance.Invisibly = false;

        Sprite.color = new Color(1, 1, 1, 1);
    }

    public void AddFollower()
    {
       switch(Number)
        {
            case FollowNum.One:
                if(PlayerManager.instance.Power == 3)
                {
                    Follower[0].SetActive(true);
                }
                break;
            case FollowNum.Two:
                if(PlayerManager.instance.Power == 3)
                {
                    Follower[0].SetActive(true);
                }
                else if(PlayerManager.instance.Power == 4)
                {
                    Follower[1].SetActive(true);
                }
                break;
            case FollowNum.Three:
                if(PlayerManager.instance.Power == 2)
                {
                    Follower[0].SetActive(true);
                }
                else if(PlayerManager.instance.Power == 3)
                {
                    Follower[1].SetActive(true);
                }
                else if(PlayerManager.instance.Power == 4)
                {
                    Follower[2].SetActive(true);
                }
                break;
        }
    }
}