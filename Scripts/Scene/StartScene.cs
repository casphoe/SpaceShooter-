using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : Scene
{
    // 초기화
    public override void Awake()
    {
        base.Awake();
        /*
       * 람다 => 이름이 없는 함수를 의미하며 해당 함수는 일반적으로 한번 사용하고 버릴 일회성 함수가 필요 할 때
       * 주로 사용 c# 언어에서 람다를 사용하기 위해서 반드시 델리게이트가 필요
       *  (int a_nLhs, int a_nRhs) => a_nLhs + a_nRhs
       * 
       * - (int a_nLhs, int a_nRhs) => {return a_nLhs + a_nRhs; };
       * 
       * - c# 언어의 람다는 {} (중괄호 연산자)의 여부에 따라서 단일행(식 형식) 람다와 문 형식의 람다로 구분
       * 또한 람다는 매개 변수의 자료형을 생략하는 것이 가능
       * (즉, 람다 함수 매개 변수의 자료형을 컴파일러에게 하여금 추측하게 만드는 것이 가능하다)
       */
        Function.LateCallFunc(this, 2.5f, (a_componet) =>
        {
            SceneLoader.instance.LoadScene(CDefine.SCENE_NAME_SPACESHOOT_PLAYERSELECT);
        });
    }
}
