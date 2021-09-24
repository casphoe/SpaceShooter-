using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

/*
 * where 키워드 제네릭 형식 지정자에 명시할 수 있는 타입을  자료형을 제한하는 역활을 한다.
 * (즉, 기본적으로 제너릭에는 모든 자료형을 명시 할 수 있는 where 키워드를 사용하면 특정 자료형에만 동작 하는 제너릭을 만드는 것이 가능)
 * 
 * where 키워드 사용하는 방법
 * :
 * - where T : class                            => 참조 형식의 자료형으로 제한
 * - where T : struct                           => 값 형식의 자료형으로 제한
 * - where T : someClass or someInterface       => 해당 클래스/인터페이스를 직접 또는 간접적으로 상속하는 자료형으로 제한
 */

// 싱글턴
public class Singleton<T> : CCompo where T : Singleton<T>
{
    public static T m_instance = null;

    public static T instance
    {
        get
        {
            if(m_instance == null)
            {
                /*
                 * typeof 키워드는 명시된 자료형의 정보를 지니고 있는 Type 객체를 반환하는 역활을 한다.
                 * Type 객체는 특정 자료형에 대한 정보를 모두 지니고 있기 때문에 실행 중에 특정 자료형에 특정 함수가 
                 * 있는 지 없는 지에 대한 판단 하는 것도 가능하며 호출 또한 가능하다.
                 * 
                 * PS.
                 * Type 객체를 쓰기 위해서는 System.Reflection 네임 스페이스를 포함
                 * 
                 *
                 */
                var Cgameobject = new GameObject(typeof(T).ToString());
                /*
                 * AddComponet 함수는 객체에 지정된 컴포넌트를 추가하는 역활을 수행한다.
                 * (즉, 실행 중에 특정 객체에 원하는 컴포넌트를 필요한 만큼 추가하는 것이 가능하다.)
                 */
                m_instance = Cgameobject.AddComponent<T>();


                /*
                 * DontDestroyOnLoad 함수는 씬이 변경 되었을 때 특정 객체를 제거하지 않고 유지하는 역활을 수행한다.
                 * (즉, 유니티는 기본적으로 씬이 변경 되면 이전 씬에 있던 모든 객체를 제거)
                 * 
                 */
                DontDestroyOnLoad(Cgameobject);
            }
            return m_instance;
        }
    }
    //인스턴스를 생성
    public static T creatinstance()
    {
        return Singleton<T>.instance;
    }

    public override void Awake()
    {
        base.Awake();

        m_instance = this as T;

        DontDestroyOnLoad(this.gameObject);
    }
}
