using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : CCompo
{
    private float Speed;
    Vector3 pos;
    int Score;

    private void Start()
    {
        ItemManager.instance.ItemStat("PowerUp");
        Speed = 15f;
        pos = transform.position;
        Score = ItemManager.instance.Score;
    }
    
    private void Update()
    {
       
        pos.y -= Speed * Time.deltaTime;
        transform.position = pos;
    }

    //오브젝트가 카메라에 의해서 더이상 안보이는 경우 오브젝트를 꺼줌
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    // 충돌 처리
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.Score += Score;
            UIManager.instace.ScoreAddValue(GameManager.instance.Score);

            if (PlayerManager.instance.MaxPower > PlayerManager.instance.Power)
            {
                PlayerManager.instance.Power += ItemManager.instance.power;
                PlayerManager.instance.SPl.AddFollower();
            }
            if (GameManager.instance.IsEffMute == 0)
            {
                SoundManager.instance.EffectAudio.transform.GetChild(3).GetComponent<AudioSource>().Stop();
            }
            else
            {
                if (!SoundManager.instance.EffectAudio.transform.GetChild(3).GetComponent<AudioSource>().isPlaying)
                {
                    SoundManager.instance.EffectAudio.transform.GetChild(3).GetComponent<AudioSource>().Play();
                }
            }
            gameObject.SetActive(false);
        }
    }
}