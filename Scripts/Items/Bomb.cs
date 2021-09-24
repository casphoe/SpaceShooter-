using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float Speed;
    Vector3 pos;

    int Score;

    // Start is called before the first frame update
    void Start()
    {
        ItemManager.instance.ItemStat("bomb");
        Speed = 18f;
        Score = ItemManager.instance.Score;
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        pos.y -= Speed * Time.deltaTime;
        transform.position = pos;
    }

    //오브젝트가 카메라에 의해서 더이상 안보이는 경우 오브젝트를 꺼줌
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) //충돌 처리
    {
        if(collision.gameObject.tag == "Player") //Player태그를 가진 오브젝트와 충돌 되었을 때
        {
            GameManager.instance.Score += Score;
            UIManager.instace.ScoreAddValue(GameManager.instance.Score);
            UIManager.instace.Special.interactable = true;
            if (PlayerManager.instance.MaxSpecialCount > PlayerManager.instance.SpecialCount)
            {
                PlayerManager.instance.SpecialCount++;
            }
            UIManager.instace.SpecialCountText.text = "" + PlayerManager.instance.SpecialCount;
            if (GameManager.instance.IsEffMute == 0)
            {
                SoundManager.instance.EffectAudio.transform.GetChild(7).GetComponent<AudioSource>().Stop();
            }
            else
            {
                if (!SoundManager.instance.EffectAudio.transform.GetChild(7).GetComponent<AudioSource>().isPlaying)
                {
                    SoundManager.instance.EffectAudio.transform.GetChild(7).GetComponent<AudioSource>().Play();
                }
            }
            gameObject.SetActive(false);
        }
    }
}
