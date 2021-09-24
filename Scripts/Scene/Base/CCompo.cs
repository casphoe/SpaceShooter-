using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCompo : MonoBehaviour
{
    /*
     *  public 변수를 선언하면 inspector 창에 변수명과 입력 상자가 생기는데 
     *  HideInInspector를 정의하고 변수를 선언하고 public이라도 inspector창에 변수명과 입력 상자가 사라짐
     */
    [HideInInspector] public Rigidbody2D m_Rid = null;
    [HideInInspector] public RectTransform m_Rect = null;

    [HideInInspector]
    public GameObject[] PlayerBullet = new GameObject[4];

    public virtual void Awake()
    {
        m_Rid = this.GetComponent<Rigidbody2D>();

        /*
         * as 키워드 부모 클래스의 자료형에서 자식 클래스의 자료형으로 변환 시킬 때 안전하게 변환이 가능하도록
         * 기능을 수행하는 역활을 한다.(즉, 자식 클래스의 자료형으로 변환이 가능 할 경우 변환이 된 결과를 변환하며, 만약 변환이 불가능 할 경우에는 null값이 반환 된다.)
         */
        m_Rect = this.transform as RectTransform;
    }
}
