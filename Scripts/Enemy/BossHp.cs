using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHp : MonoBehaviour
{
    public Camera uiCamera; //ui카메라를 담을 변수
    public Canvas canvas; //캔버스를 담을 변수

    private RectTransform rectParent;
    private RectTransform rectHp;
    public Transform BossTr;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;

        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = gameObject.GetComponent<RectTransform>();
    }

    private void LateUpdate() //update 이후 실행함
    {
        var screenPos = Camera.main.WorldToScreenPoint(BossTr.position + new Vector3(0f, 9.5f, 0f));
        var localpos = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localpos);
        //스크린좌표에서 캔버스에서 사용할 수 있는 좌표로 변경

        rectHp.localPosition = localpos;
    }
}
