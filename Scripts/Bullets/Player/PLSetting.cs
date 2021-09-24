using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLSetting : MonoBehaviour
{
    public int Damage;
    public float Speed;

    Vector3 pos;

    Vector2 dir;

    // Start is called before the first frame update
    void Start()
    {
        Speed = 18f;

        if (GameManager.instance.IsEffMute == 0)
        {
            SoundManager.instance.EffectAudio.transform.GetChild(0).GetComponent<AudioSource>().Stop();
        }
        else
        {
            if (!SoundManager.instance.EffectAudio.transform.GetChild(0).GetComponent<AudioSource>().isPlaying)
            {
                SoundManager.instance.EffectAudio.transform.GetChild(0).GetComponent<AudioSource>().Play();
            }
        }
    }

    //스피드 만큼 위로 올라감
    void Update()
    {
        transform.Translate(Vector3.up * Speed * Time.deltaTime); //Translate 월드,로컬을 자동으로 구분해서 처리해줌
    }

    //오브젝트가 카메라에 의해서 더이상 안보이는 경우 오브젝트를 꺼줌
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().EnemyDamage(Damage);
            gameObject.SetActive(false);
        }
        else if(collision.CompareTag("boss"))
        {
            collision.gameObject.GetComponent<boss>().TakeDamage(Damage);
            gameObject.SetActive(false);
        }
    }
}
