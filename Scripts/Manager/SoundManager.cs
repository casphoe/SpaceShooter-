using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioClip BGM;
    public AudioClip Laser;
    public AudioClip Hit;
    public AudioClip GameOver;
    public AudioClip PowerUp;
    public AudioClip Click;
    public AudioClip Engine;
    public AudioClip Experision;
    public AudioClip PickUP;
    public AudioClip StageClear;
    public AudioClip Buy;
    public AudioClip Missle;
    public AudioClip BossIntro;

    public List<AudioClip> EffectAudioList;

    public AudioSource BackGroundAudio;
    public AudioSource EffectAudio;

    public override void Awake()
    {
        base.Awake();
        EffectAudioList = new List<AudioClip>();
        /*
         * UnassignedReferenceException: The variable BackGroundAudio of SoundManager has not been assigned. You probably need to assign the BackGroundAudio variable of the SoundManager script in the inspector.
         * 
         * 맨처음에 실행될 씬에 사운드 매니저를 추가한 다음 다음으로 넘어갈 씬에 사운드 매니저를 따로 만들어나서 오류가 남
         * 사운드 매니저를 싱글톤처럼 인스턴트 하기 때문에 미리 넘어갈 씬에 사운드 매니저가 총 두개 있어서 한 개는 null로 나타나서 못 찾아져서 오류가 발생
         */

        EffectAudioList.Add(Laser);
        EffectAudioList.Add(Hit);
        EffectAudioList.Add(GameOver);
        EffectAudioList.Add(PowerUp);
        EffectAudioList.Add(Click);
        EffectAudioList.Add(Engine);
        EffectAudioList.Add(Experision);
        EffectAudioList.Add(PickUP);
        EffectAudioList.Add(StageClear);
        EffectAudioList.Add(Buy);
        EffectAudioList.Add(Missle);
        EffectAudioList.Add(BossIntro);

        for (int i = 0; i < EffectAudioList.Count; i++)
        {
            EffectAudio.gameObject.transform.GetChild(i).GetComponent<AudioSource>().clip = EffectAudioList[i];
        }

        if(BackGroundAudio.clip != null)
        {
            BGM = BackGroundAudio.clip;
        }
        else
        {
            BackGroundAudio.clip = BGM;
        }

        if (GameManager.instance.IsBGMMute == 0)
        {
            BackGroundAudio.Stop();
        }
        else
        {
            if (!BackGroundAudio.isPlaying)
            {
                BackGroundAudio.Play();
            }            
        }

        if(GameManager.instance.IsEffMute == 0)
        {
            for (int i = 0; i < EffectAudioList.Count; i++)
            {
                EffectAudio.gameObject.transform.GetChild(i).GetComponent<AudioSource>().Stop();
            }
        }
    }
}