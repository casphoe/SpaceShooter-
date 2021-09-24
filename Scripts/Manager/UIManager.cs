using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//플레이어 선택 화면창에 게임 매니저에서 입력 받은 플레이어 데이터(능력치)를 화면상에 표시하게 함
public class UIManager : CCompo
{
    public Slider PlayerHpSlider;
    public Text ScoreText;
    public Button Special;
    public GameObject PaseUI;
    public Slider BGMSlider;
    public Slider EffectSlider;
    public GameObject BarrierBt;
    public Text StageText;
    public Text WaringText;
    public static UIManager instace;

    public Text SpecialCountText;
    public GameObject Sc;

    private Image SpecialImage;

    private new void Awake()
    {
        if(instace == null)
        {
            instace = this;
        }
        else if (instace != null)
        {
            return;
        }
    }

    private void Start()
    {
        Time.timeScale = 1;
        PlayerHpSlider.maxValue = PlayerManager.instance.Hp;
        PlayerHpSlider.value = PlayerManager.instance.Hp;

        ScoreText.text = "Score " + GetScoreText(GameManager.instance.Score).ToString();

        SpecialCountText = Special.transform.GetChild(0).GetComponent<Text>(); //자식개체의 텍스트 스크립트를 찾음
        SpecialImage = Special.gameObject.GetComponent<Image>();
        PaseUI.gameObject.SetActive(false);
        SpecialCountText.text = "" + PlayerManager.instance.SpecialCount;
        StageText.text = "Stage : " + GameManager.instance.StageNum;

        BarrierBt.SetActive(false);

        BGMSlider.value = GameManager.instance.IsBGMMute;
        EffectSlider.value = GameManager.instance.IsEffMute;

        SoundManager.instance.BackGroundAudio.volume = BGMSlider.value / 4;

        for (int i = 0; i < SoundManager.instance.EffectAudioList.Count; i++)
        {
            SoundManager.instance.EffectAudio.transform.GetChild(i).GetComponent<AudioSource>().volume = EffectSlider.value;
        }
    }

    IEnumerator CoolTime(float Cool)
    {
        while(SpecialImage.fillAmount > 0)
        {
            //Time.smoothDeltaTime :  Time.deltaTime보다 안정된 값을 가져옴
            SpecialImage.fillAmount -= 1 * Time.smoothDeltaTime / Cool;
            yield return null;
        }
        SpecialImage.fillAmount = 1;
        yield break;
    }

    public void HPImageChange(int amount)
    {
        PlayerHpSlider.value -= amount;
    }

    public void ScoreAddValue(int value)
    {
        ScoreText.text = "Score " + GetScoreText(value).ToString();      
    }

    private string GetScoreText(int value) //천의 단위를 표현
    {
        return string.Format("{0:#,###}",value);

    }

    public void SpecialButtonClick()
    {
        PlayerManager.instance.SpecialCount -= 1;
        SpecialCountText.text = "" + PlayerManager.instance.SpecialCount;
        // 필살기 생성
        PlayerManager.instance.CreateSpecial(new Vector2(PlayerManager.instance.PlayerDir.x, PlayerManager.instance.PlayerDir.y + 2f), Vector3.zero);

        StartCoroutine(CoolTime(5f));

        if(PlayerManager.instance.SpecialCount <= 0)
        {
            Special.interactable = false;
        }
    }

    //일시 정지 기능
    public void PasueButton()
    {
        PaseUI.gameObject.SetActive(true);
        Time.timeScale = 0; //게임화면을 멈추는 기능 
    }

    public void BarrierButton()
    {
        BarrierBt.SetActive(false);
        PlayerManager.instance.CreateBarrier(new Vector2(PlayerManager.instance.PlayerDir.x, PlayerManager.instance.PlayerDir.y), Vector3.zero);
    }

    public void RestartButton()
    {
        if(GameManager.instance.StageNum == 1)
        {
            /*
             * Time.timeScale = 0; 이 된상태에서 씬을 불려오기 때문에 무조건 Time.timeScale = 1;한다음 로드 씬을 하던가 start문에 Time.timeScale = 1;를 하면 됨
             */
            SceneLoader.instance.LoadScene(CDefine.SCENE_NAME_SPACESHOOT_Stage1);
        }
        else if(GameManager.instance.StageNum == 2)
        {
            SceneLoader.instance.LoadScene(CDefine.SCENE_NAME_SPACESHOOT_Stage2);
        }
        else if(GameManager.instance.StageNum == 3)
        {
            SceneLoader.instance.LoadScene(CDefine.SCENE_NAME_SPACESHOOT_Stage3);
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        GameManager.instance.StageNum = 0;
        SceneLoader.instance.LoadScene(CDefine.SCENE_NAME_SPACESHOOT_STARTSCENE);
    }

    public void Exit()
    {
        Application.Quit(); //게임을 끔
    }

    public void QuitPauseButton()
    {
        PaseUI.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void PlayerDead()
    {
        SceneLoader.instance.LoadScene(CDefine.SCENE_NAME_SPACESHOOT_RESULT);
    }

    public void StageClear()
    {
        Sc.SetActive(true);
        Sc.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = string.Format("Stage {0} Clear",GameManager.instance.StageNum);

        //스테이지 클리어 음악 실행
        if (GameManager.instance.IsEffMute == 0)
        {
            SoundManager.instance.EffectAudio.transform.GetChild(8).GetComponent<AudioSource>().Stop();
        }
        else
        {
            if (!SoundManager.instance.EffectAudio.transform.GetChild(8).GetComponent<AudioSource>().isPlaying)
            {
                SoundManager.instance.EffectAudio.transform.GetChild(8).GetComponent<AudioSource>().Play();
            }
        }

        Function.LateCallFunc(this, 1f, (a_componet) =>
        {
            GameManager.instance.StageNum += 1;

            //스테이지 클리어 음악 정지

            if(GameManager.instance.StageNum == 2)
            {
                SceneLoader.instance.LoadScene(CDefine.SCENE_NAME_SPACESHOOT_Stage2);
            }
            else if(GameManager.instance.StageNum == 3)
            {
                SceneLoader.instance.LoadScene(CDefine.SCENE_NAME_SPACESHOOT_Stage3);
            }
            else if(GameManager.instance.StageNum == 4)
            {
                SceneLoader.instance.LoadScene(CDefine.SCENE_NAME_SPACESHOOT_RESULT);
            }
        });
    }

    public void BGMVolume()
    {
        SoundManager.instance.BackGroundAudio.volume = BGMSlider.value / 2;

        if(BGMSlider.value > 0)
        {
            GameManager.instance.IsBGMMute = 1;
        }
        else
        {
            GameManager.instance.IsBGMMute = 0;
        }
    }

    public void EffectVolume()
    {

        for(int i = 0; i < SoundManager.instance.EffectAudioList.Count; i++)
        {
            SoundManager.instance.EffectAudio.transform.GetChild(i).GetComponent<AudioSource>().volume = EffectSlider.value;
        }

        if(EffectSlider.value > 0)
        {
            GameManager.instance.IsEffMute = 1;
        }
        else
        {
            GameManager.instance.IsEffMute = 0;
        }
    }
}