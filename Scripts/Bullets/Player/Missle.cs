using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : CCompo
{
    public float Speed;
    public int Damage;

    Vector3 pos;
    
    private float ShotDis;
    private Vector3 LookAt;
    GameObject CloseEnemy;

    // Start is called before the first frame update
    void Start()
    {
        Speed = 6f;

        InvokeRepeating("TargetEnemy", 0.5f, 0.3f);

        Damage = 2;

        if (GameManager.instance.IsEffMute == 0)
        {
            SoundManager.instance.EffectAudio.transform.GetChild(10).GetComponent<AudioSource>().Stop();
        }
        else
        {
            if (!SoundManager.instance.EffectAudio.transform.GetChild(10).GetComponent<AudioSource>().isPlaying)
            {
                SoundManager.instance.EffectAudio.transform.GetChild(10).GetComponent<AudioSource>().Play();
            }
        }
    }

    void TargetEnemy()
    {
        if(EnemyManager.instance.EnemyList[0].gameObject != null)
        {
            ShotDis = Vector3.Distance(gameObject.transform.position, EnemyManager.instance.EnemyList[0].gameObject.transform.position);

            CloseEnemy = EnemyManager.instance.EnemyList[0].gameObject;
            foreach (GameObject ShotEnemy in EnemyManager.instance.EnemyList)
            {
                float curDist = Vector3.Distance(gameObject.transform.position, ShotEnemy.transform.position);

                if (curDist < ShotDis)
                {
                    CloseEnemy = ShotEnemy;
                    ShotDis = curDist;
                }
            }
            LookAt = CloseEnemy.transform.position - transform.position;
            float Angle = Mathf.Atan2(LookAt.y, LookAt.x) * Mathf.Rad2Deg; //각도 구하기
            transform.rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
            transform.position = Vector3.MoveTowards(transform.position, CloseEnemy.transform.position, Speed);
        }
        else
        {
            transform.Translate(Vector3.up * Speed * Time.deltaTime); //Translate 월드,로컬을 자동으로 구분해서 처리해줌
        }
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
