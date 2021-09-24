using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : CCompo
{
    private Dictionary<string, PlayerDB> PLAYERS;

    public Text PName;
    public Text Hp;
    public Text power;
    public Text Speed;

    public Slider HPSlider;
    public Slider PowerSlider;
    public Slider SpeedSlider;

    public GameObject Player1;
    public GameObject Player2;

    public Sprite Player1Image;
    public Sprite Player2Image;

    public Sprite SoundOff;
    public Sprite SoundOn;

    public GameObject PlayerUISetting;
    public GameObject Coin;
    public GameObject Coinlack;
    public GameObject SoundImage;

    public GameObject Star;

    public bool IsMute = false;

    public static PlayerUI instance;

    //private GameObject SoundManager;
    private bool ParticleSystemPlaying => Star.GetComponent<ParticleSystem>().isPlaying;

    private GameObject StarParticle;

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
    }

    private void Start()
    {
        Coin.transform.GetChild(0).GetComponent<Text>().text = "Coin : " + GameManager.instance.CoinCount;
        int Player1Set = PlayerPrefs.GetInt("Player1");
        int Player2Set = PlayerPrefs.GetInt("Player2");
        GameManager.instance.IsCreate[0] = Player1Set;
        GameManager.instance.IsCreate[1] = Player2Set;
        if (GameManager.instance.CoinCount >= 500)
        {
            if(GameManager.instance.IsCreate[0] == 0)
            {
                CreateStar(Player1.transform);
            }
            
            if(GameManager.instance.IsCreate[1] == 0)
            {
                CreateStar(Player2.transform);
            }
        }
        else if(GameManager.instance.CoinCount >= 300 && GameManager.instance.CoinCount < 500 && GameManager.instance.IsCreate[0] == 0)
        {
            CreateStar(Player1.transform);
        }
        
        if(Player1Set > 0)
        {
            Player1.GetComponent<Image>().sprite = Player1Image;
            Player1.GetComponent<Button>().enabled = false; //버튼이 더이상 안눌리게 함
        }
        
        if(Player2Set > 0)
        {
            Player2.GetComponent<Image>().sprite = Player2Image;
            Player2.GetComponent<Button>().enabled = false;
        }

        if (GameManager.instance.IsBGMMute == 1 || GameManager.instance.IsEffMute == 1)
        {
            SoundImage.GetComponent<Image>().sprite = SoundOn;
            IsMute = false;
        }
        else
        {
            SoundImage.GetComponent<Image>().sprite = SoundOff;
            IsMute = true;
        }
    }


    public void PlayerStat(string Name) //이름 값으로 플레이어 데이터값을 받아옴
    {
        PLAYERS = GameManager.instance.PlayerMGR();

        PlayerDB players;
        PLAYERS.TryGetValue(string.Format("{0}", Name), out players); //GameManager에 클래스인 playerdata함수에 설정된 데이터 값을 받아옴

        PName.text = "Name : "  + players.Name;
        Hp.text = "" + players.Hp;
        power.text = "" + players.power;
        Speed.text = "" + players.PlayerSpeed;

        HPSlider.value = players.Hp;
        PowerSlider.value = players.power;
        SpeedSlider.maxValue = 35;
        SpeedSlider.value = players.PlayerSpeed;

        HPSlider.maxValue = players.Hp;
    }

    public void PlayerNormalButton()
    {
        if (GameManager.instance.CoinCount < 300 && GameManager.instance.IsCreate[0] == 0)
        {
            Coinlack.gameObject.SetActive(true);
            int count = 300 - GameManager.instance.CoinCount;
            Coinlack.transform.GetChild(0).GetComponent<Text>().text = "동전이 : " + count +  " 만큼 부족하다";
            Invoke("CancelCoinLack", 1.2f);
        }
        else
        {
            Player1.GetComponent<Image>().sprite = Player1Image;
            Player1.GetComponent<Button>().enabled = false; //버튼을 비활성화 함
            GameManager.instance.CoinCount -= 300;
            GameManager.instance.Coin();
            Coin.transform.GetChild(0).GetComponent<Text>().text = "Coin : " + GameManager.instance.CoinCount;
            GameManager.instance.IsCreate[0] = 1;
            PlayerPrefs.SetInt("Player1", GameManager.instance.IsCreate[0]);
            Player1.transform.GetChild(0).gameObject.SetActive(false);
            if (GameManager.instance.IsEffMute == 0)
            {
                SoundManager.instance.EffectAudio.transform.GetChild(9).GetComponent<AudioSource>().Stop();
            }
            else
            {
                if (!SoundManager.instance.EffectAudio.transform.GetChild(9).GetComponent<AudioSource>().isPlaying)
                {
                    SoundManager.instance.EffectAudio.transform.GetChild(9).GetComponent<AudioSource>().Play();
                }
            }

            Player1.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void PlayerPowerButton()
    {
        if(GameManager.instance.CoinCount < 500 && GameManager.instance.IsCreate[1] == 0)
        {
            Coinlack.gameObject.SetActive(true);
            int count = 500 - GameManager.instance.CoinCount;
            Coinlack.transform.GetChild(0).GetComponent<Text>().text = "동전이 : " + count + " 만큼 부족하다";
            Invoke("CancelCoinLack", 1.2f);
        }
        else
        {
            Player2.GetComponent<Image>().sprite = Player2Image;
            Player2.GetComponent<Button>().enabled = false; 
            GameManager.instance.CoinCount -= 500;
            GameManager.instance.Coin();
            Coin.transform.GetChild(0).GetComponent<Text>().text = "Coin : " + GameManager.instance.CoinCount;
            GameManager.instance.IsCreate[1] = 1;
            PlayerPrefs.SetInt("Player2", GameManager.instance.IsCreate[1]);
            Player2.transform.GetChild(0).gameObject.SetActive(false);
            if (GameManager.instance.IsEffMute == 0)
            {
                SoundManager.instance.EffectAudio.transform.GetChild(9).GetComponent<AudioSource>().Stop();
            }
            else
            {
                if (!SoundManager.instance.EffectAudio.transform.GetChild(9).GetComponent<AudioSource>().isPlaying)
                {
                    SoundManager.instance.EffectAudio.transform.GetChild(9).GetComponent<AudioSource>().Play();
                }
            }

            Player2.transform.GetChild(0).gameObject.SetActive(false);

            if(GameManager.instance.CoinCount < 300)
            {
                Player1.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void CancelCoinLack()
    {
        Coinlack.gameObject.SetActive(false);
    }

    private void CreateStar(Transform tr)
    {
        StarParticle = Instantiate(Star, tr.position, Quaternion.identity);
        StarParticle.transform.SetParent(tr, false); 
        StarParticle.transform.position = tr.position;
        PlayerLockerChange();
    }

    private void PlayerLockerChange()
    {
        if (!ParticleSystemPlaying)
        {
            Star.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            Star.GetComponent<ParticleSystem>().Stop();
        }
    }

    public void SoundISMute()
    {
        if(IsMute)
        {
            SoundImage.GetComponent<Image>().sprite = SoundOn;
            GameManager.instance.IsBGMMute = 1;
            GameManager.instance.IsEffMute = 1;
            if (!SoundManager.instance.BackGroundAudio.isPlaying)
            {
                SoundManager.instance.BackGroundAudio.Play();
            }
        }
        else
        {
            SoundImage.GetComponent<Image>().sprite = SoundOff;
            GameManager.instance.IsBGMMute = 0;
            GameManager.instance.IsEffMute = 0;
            SoundManager.instance.BackGroundAudio.Stop();

            for (int i = 0; i < SoundManager.instance.EffectAudioList.Count; i++)
            {
                SoundManager.instance.EffectAudio.gameObject.transform.GetChild(i).GetComponent<AudioSource>().Stop();
            }
        }
        IsMute = !IsMute;
        PlayerPrefs.SetInt("Sound", GameManager.instance.IsBGMMute);
        PlayerPrefs.SetInt("EffectSound", GameManager.instance.IsEffMute);
    }
}