using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
* enum(열겨형) : 여러 개 상수를 선언할 때 단순히 종류를 구별하기 위해 사용
* - 장점 : 
* 코드가 단순해지며 가독성이 좋아짐
* 상수이기 때문에 if,switch 문에서 사용 가능
* 
*/
public enum Player //플레이어 이미지를 클릭했을 때 스탯을 알 수 있게 하고 그 플레이어를 strat버튼을 누르면 선택한 플레이어로 시작할 수 있게 하기 위해서 설정
{
    none,basic, normal, power
};

public class SelectPlayer : CCompo, IPointerDownHandler,IPointerUpHandler,IPointerClickHandler
{
    public Player select;

    public void OnPointerDown(PointerEventData eventData)
    {       
       if(select == Player.basic)
        {
            PlayerUI.instance.PlayerUISetting.gameObject.SetActive(true);
            PlayerUI.instance.PlayerStat("basic");
            GameManager.instance.PS = 1;
        }
       else if(select == Player.normal)
        {
            PlayerUI.instance.PlayerUISetting.gameObject.SetActive(true);
            PlayerUI.instance.PlayerStat("Normal");
            GameManager.instance.PS = 2;
        }
       else if(select == Player.power)
        {
            PlayerUI.instance.PlayerUISetting.gameObject.SetActive(true);
            PlayerUI.instance.PlayerStat("High");
            GameManager.instance.PS = 3;
        }


         if(GameManager.instance.IsEffMute == 0)
         {
             SoundManager.instance.EffectAudio.transform.GetChild(4).GetComponent<AudioSource>().Stop();
         }
         else
         {
             if (!SoundManager.instance.EffectAudio.transform.GetChild(4).GetComponent<AudioSource>().isPlaying)
             {
                SoundManager.instance.EffectAudio.transform.GetChild(4).GetComponent<AudioSource>().Play();
             }
         }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerUI.instance.PlayerUISetting.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //더블 클릭 시
        if(eventData.clickCount == 2)
        {
            if(select == Player.power)
            {
                if(GameManager.instance.IsCreate[1] == 1)
                {
                    //그 플레이어 정보와 함께 게임 씬을 넘어가야함=> 플레이 게임 씬
                    Function.LateCallFunc(this, 1.5f, (a_componet) =>
                    {
                        SceneLoader.instance.LoadScene(CDefine.SCENE_NAME_SPACESHOOT_Stage1);
                    });
                }
                else
                {
                    return;
                }
            }
            else if(select == Player.normal)
            {
                if (GameManager.instance.IsCreate[0] == 1)
                {
                    //그 플레이어 정보와 함께 게임 씬을 넘어가야함=> 플레이 게임 씬
                    Function.LateCallFunc(this, 1.5f, (a_componet) =>
                    {
                        SceneLoader.instance.LoadScene(CDefine.SCENE_NAME_SPACESHOOT_Stage1);
                    });
                }
                else
                {
                    return;
                }
            }
            else if(select == Player.basic)
            {
                //그 플레이어 정보와 함께 게임 씬을 넘어가야함=> 플레이 게임 씬
                Function.LateCallFunc(this, 1.5f, (a_componet) =>
                {
                    SceneLoader.instance.LoadScene(CDefine.SCENE_NAME_SPACESHOOT_Stage1);
                });
            }
        }
    }
}