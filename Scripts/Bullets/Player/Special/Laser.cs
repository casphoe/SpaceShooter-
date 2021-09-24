using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;


public class Laser : CCompo
{
    private ParticleSystem SpecialLaser;
    private bool ParticleSystemPlaying => SpecialLaser.isPlaying;
    /*
     * get만 있으면 줄이는 것이 가능
     * =>
     * private bool ParticleSystemPlaying
     * {
     * get
     * {
     * 
     * }
     * }
     *
     */
    private Vector2 pos;

    public float damage;

    private void Start()
    {
        SpecialLaser = transform.GetChild(0).GetComponent<ParticleSystem>();

        PlayParticleSystem();

        Function.LateCallFunc(this, 1.0f, (CCompo) =>
        {
            gameObject.SetActive(false);
        });

        damage = 0.2f;
    }

    
    private void PlayParticleSystem()
    {
        if(!ParticleSystemPlaying)
        {
            SpecialLaser.Play();
        }
        else
        {
            SpecialLaser.Stop();
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {        
        pos = PlayerManager.instance.PlayerDir;
        transform.position = pos;
    }
    //충돌이 되고 있을 때=> 박스콜라이더 안에 오브젝트가 게속 머물고 있으면 매 프레임마다 실행됨
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().EnemyDamage(damage);
        }
        else if(collision.CompareTag("boss"))
        {
            collision.gameObject.GetComponent<boss>().TakeDamage(damage);
        }
        else if(collision.CompareTag("EnemyBullet"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}