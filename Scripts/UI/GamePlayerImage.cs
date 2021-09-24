using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayerImage : CCompo
{
    private Image BasicImage;

    public Sprite[] PlayerImage = new Sprite[3]; //플레이어마다 게임화면 쌍 플레이어 이미지를 바꾸기위해서 스프라이트 이미지를 설정

    private void Start()
    {
        BasicImage = GetComponent<Image>(); //이미지 스크립트를 받아옴

        if(GameManager.instance.PS == 1)
        {
            BasicImage.sprite = PlayerImage[0];
        }
        else if(GameManager.instance.PS == 2)
        {
            BasicImage.sprite = PlayerImage[1];
        }
       else
        {
            BasicImage.sprite = PlayerImage[2];
        }
    }

}
