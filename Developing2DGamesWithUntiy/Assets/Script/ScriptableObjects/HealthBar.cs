using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    /// <summary>
    /// 플레이어 프리팹이 참조하는 스크립팅 가능한 오브젝트인 HitPoints애셋을 가리키는 참조
    /// </summary>
    public HitPoints hitPoints;
    [HideInInspector]
    public Player character;
    public Image meterImage;
    public Text hpText;
    float maxHitPoints;

    private void Start()
    {
        maxHitPoints = character.maxHitPoints;
    }

    private void Update()
    {
        if(character != null)
        {
            meterImage.fillAmount = hitPoints.value / maxHitPoints;
            hpText.text = "HP:" + (meterImage.fillAmount * 100);
        }
    }

}
