using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ResultManager : CCompo
{
    public GameObject NumberText;
    public GameObject NumberTextParent;

    public GameObject InputScoreObject;
    public GameObject InputScoreObjectParent;
    public List<int> Score;
    public GameObject GameOverPanel;
    public GameObject ResultObject;
    public GameObject StageClear;

    private Text InputScore;

    //시작할 때 text를 생성하고 
    private void Start()
    {
        if(GameManager.instance.StageNum > 3)
        {
            StageClear.SetActive(true);

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

            Function.LateCallFunc(this, 0.6f, (CCompo) =>
            {
                StageClear.SetActive(false);

            });
        }
        else
        {
            GameOverPanel.SetActive(true);

            Function.LateCallFunc(this, 0.6f, (CCompo) =>
            {
                GameOverPanel.SetActive(false);

            });
        }

        PlayerPrefs.GetInt("Count", GameManager.instance.ListCount);

        if (GameManager.instance.IsEffMute == 0)
        {
            SoundManager.instance.EffectAudio.transform.GetChild(2).GetComponent<AudioSource>().Stop();
        }
        else
        {
            if (!SoundManager.instance.EffectAudio.transform.GetChild(2).GetComponent<AudioSource>().isPlaying)
            {
                SoundManager.instance.EffectAudio.transform.GetChild(2).GetComponent<AudioSource>().Play();
            }
        }

        //지정 시간 뒤에 호출되는 로직
        Function.LateCallFunc(this, 1.2f, (CCompo) =>
        {
            ResultObject.SetActive(true);

            NumberText.GetComponent<Text>().text = "순위 : 1";
            for (int i = 1; i <= 10; i++)
            {
                CreateNumberText(new Vector3(NumberTextParent.transform.position.x, NumberTextParent.transform.position.y - 45 * i));
                NumberText.GetComponent<Text>().text = "순위 : " + (i + 1);
            }

            if (PlayerPrefs.HasKey("BestScore"))
            {
                CreateInputSystem(InputScoreObjectParent.transform.position);
                Score.Add(PlayerPrefs.GetInt("BestScore"));

                InputScoreObject.transform.GetChild(0).GetComponent<Text>().text = "BestScore : " + Score[0].ToString();
            }

            for (int i = 0; i < GameManager.instance.ListCount; i++)
            {
                CreateInputSystem(InputScoreObjectParent.transform.position);

                Score.Add(PlayerPrefs.GetInt(GameManager.instance.oScoreNum[i]));
                InputScoreObject.transform.GetChild(0).GetComponent<Text>().text = "Score : " + PlayerPrefs.GetInt(GameManager.instance.oScoreNum[i]).ToString();
            }

        });
    }

    private void CreateNumberText(Vector3 tr)
    {
        GameObject Text = Instantiate(NumberText, tr, Quaternion.identity);
        Text.transform.parent = NumberTextParent.transform;
    }

    //리스트에 점수를 담아서 score 텍스트에 점수를 뿌림 
    private void CreateInputSystem(Vector3 tr)
    {
        Score = new List<int>();
        Score.Add(GameManager.instance.BestScore);
        Score.AddRange(GameManager.instance.ScoreList);

        for(int i = 0; i < Score.Count; i++)
        {
            GameObject CreateInputScore = Instantiate(InputScoreObject, tr, Quaternion.identity);
            CreateInputScore.transform.parent = InputScoreObjectParent.transform;
            InputScore = CreateInputScore.transform.GetChild(0).GetComponent<Text>();

            InputScore.text = string.Format("Score : {0}", Score[i]);
          
            
        }
    }

    public void Retry()
    {
        Function.LateCallFunc(this, 0.8f, (CCompo) =>
        {
            SceneLoader.instance.LoadScene(CDefine.SCENE_NAME_SPACESHOOT_PLAYERSELECT);

        });
    }
}
