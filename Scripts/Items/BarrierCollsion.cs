using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierCollsion : CCompo
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.Score += transform.parent.GetComponent<Barrier>().Score;
            UIManager.instace.ScoreAddValue(GameManager.instance.Score);
            UIManager.instace.BarrierBt.SetActive(true);
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
            gameObject.transform.parent.gameObject.SetActive(false); //부모를 꺼버림
        }
    }
}
