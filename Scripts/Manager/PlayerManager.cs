using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//게임 씬이 시작할 때 플레이어 선택 화면에서 선택된 플레이어가 게임화면에 생성 되어야하고 스텟도 받아와야함
public class PlayerManager : CCompo
{
    List<GameObject> PlayerObject = new List<GameObject>();
    private Dictionary<string, PlayerDB> PLAYERS;
    private GameObject ob;
    private GameObject SpeicalLaser;
    private GameObject barrier;
    public GameObject DeadFealx;

    public GameObject basic;
    public GameObject Normal;
    public GameObject High;

    public GameObject Bullet;

    public GameObject playerTranstform; 

    public int Hp;
    public int Power;
    public int MaxPower;
    public int SpecialCount;
    public int MaxSpecialCount;
    public float Speed;

    public bool IsDead; //플레이어가 죽었는지 살았는지 확인
    public bool IsBarrier; //배리어가 생성되었는지 안되었는지 확인
    public bool Invisibly;
    public Vector2 PlayerDir;

    public Camera PlayerCamera;
    public static PlayerManager instance;
    public BarrierHp BH;
    public SpacePlayer SPl;
   
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
        CreatePlayer(playerTranstform.transform);
        SpeicalLaser = Resources.Load<GameObject>("Prefabs/LaserFire");
        barrier = Resources.Load<GameObject>("Prefabs/Barrier");
    }

    private void Start()
    {
        SoundManager.instance.BackGroundAudio.Stop();
        if (GameManager.instance.StageNum == 1)
        {
            SoundManager.instance.BackGroundAudio.clip = SoundManager.instance.BackGroundAudio.gameObject.GetComponent<BGM>().StageBgm[0];

            if (!SoundManager.instance.BackGroundAudio.isPlaying)
            {
                SoundManager.instance.BackGroundAudio.Play();
            }
        }
        else if (GameManager.instance.StageNum == 2)
        {
            SoundManager.instance.BackGroundAudio.clip = SoundManager.instance.BackGroundAudio.gameObject.GetComponent<BGM>().StageBgm[1];

            if (!SoundManager.instance.BackGroundAudio.isPlaying)
            {
                SoundManager.instance.BackGroundAudio.Play();
            }
        }
        else if(GameManager.instance.StageNum == 3)
        {
            SoundManager.instance.BackGroundAudio.clip = SoundManager.instance.BackGroundAudio.gameObject.GetComponent<BGM>().StageBgm[2];

            if (!SoundManager.instance.BackGroundAudio.isPlaying)
            {
                SoundManager.instance.BackGroundAudio.Play();
            }
        }
    }


    public void CreatePlayer(Transform tr)
    {

        if(GameManager.instance.PS == 1)
        {
            PlayerObject.Add(basic);
            playerstat("basic");
            ob = Instantiate(basic, tr.position, Quaternion.identity);
        }
        else if(GameManager.instance.PS == 2)
        {
            PlayerObject.Add(Normal);
            playerstat("Normal");
            ob = Instantiate(Normal, tr.position, Quaternion.identity);
            ob.transform.rotation = Quaternion.Euler(0, 0, 180);

        }
        else if(GameManager.instance.PS == 3)
        {
            PlayerObject.Add(High);
            playerstat("High");
            ob = Instantiate(High, tr.position, Quaternion.identity);
            ob.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    private void playerstat(string Name)
    {
        PLAYERS = GameManager.instance.PlayerMGR();

        PlayerDB players;
        PLAYERS.TryGetValue(string.Format("{0}", Name), out players); //GameManager에 클래스인 playerdata함수에 설정된 데이터 값을 받아옴

        Hp = players.Hp;
        Power = players.power;
        MaxPower = players.MP;

        SpecialCount = players.SpecialCount;
        MaxSpecialCount = players.MaxSpeical;
        Speed = players.PlayerSpeed;
    }

    public void CreateSpecial(Vector2 tr, Vector3 rot)
    {
        GameObject Laser = Instantiate(SpeicalLaser, tr, Quaternion.identity);
    }

    public void CreateBarrier(Vector2 tr, Vector3 rot)
    {
        GameObject br = Instantiate(barrier, tr, Quaternion.identity);
    }

    
}