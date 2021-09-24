using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Assertion failed on expression: 'task.rasterData.vertexBuffer == NULL'
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr)
파티클을 실행하거나 클릭하면 에러가 남 => 유니티 버전 3.5버전이하에서 나타나는 오류라고 함

또는 파티클과 라인랜더러 둘다 랜더링 컴포넌트이기 때문에 다른 객체로 컴포넌트를 분리시켜줘야 함
 */

public class BossLaser : CCompo
{

    public int damage;
    public Transform StartLinePostion;
    public Transform endLinePostion;

    private LineRenderer Line;

    void Start()
    {
        Line = GetComponent<LineRenderer>();

        Line.SetColors(Color.red, Color.yellow);
        Line.SetWidth(3f, 3f);

        //라인렌더러 처음위치와 나중위치 설정
        Line.SetPosition(0, StartLinePostion.position);
        Line.SetPosition(1, endLinePostion.position);

        Invoke("CancelLineRender", 0.2f);

        Function.LateCallFunc(this, 0.8f, (CCompo) =>
        {
            gameObject.SetActive(false);
        });
        damage = 1;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        { 
            if(PlayerManager.instance.IsBarrier == true)
            {
                if(PlayerManager.instance.Invisibly == false)
                {
                    PlayerManager.instance.BH.TakeDamage();
                }
            }
            else
            {
                if (PlayerManager.instance.Invisibly == false)
                {
                    PlayerManager.instance.SPl.TakeDamage(damage);
                }
            }
        }
    }

    private void CancelLineRender()
    {
        Line.SetWidth(0f, 0f);
    }
}  