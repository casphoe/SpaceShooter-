using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaringUI : CCompo
{
    float WaringTime;

    private void Start()
    {
        if (GameManager.instance.IsEffMute == 0)
        {
            SoundManager.instance.EffectAudio.transform.GetChild(11).GetComponent<AudioSource>().Stop();
        }
        else
        {
            if (!SoundManager.instance.EffectAudio.transform.GetChild(11).GetComponent<AudioSource>().isPlaying)
            {
                SoundManager.instance.EffectAudio.transform.GetChild(11).GetComponent<AudioSource>().Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(WaringTime < 0.5f)
        {
            GetComponent<Text>().color = new Color(1, 0, 0, 1 - WaringTime);
        }
        else
        {
            GetComponent<Text>().color = new Color(1, 0, 0, WaringTime);
            if (WaringTime > 1f)
            {
                WaringTime = 0f;
            }
        }

        WaringTime += Time.deltaTime;
    }
}
