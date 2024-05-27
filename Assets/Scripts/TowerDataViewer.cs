using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditorInternal;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;
    [SerializeField]
    private TextMeshProUGUI textDamage;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textRange;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    private PlayerHp playerHp;

    private TowerWeapon currentTower;
    
    private void Awake()
    {
        OffPanel();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            OffPanel();
        }
    }
    public void OnPanel(Transform towerWeapon)
    {
        //출력해야하는 타워 정보를 받아와서 저장
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        //타워 정보 판텔 on
        gameObject.SetActive(true);
        //타워 정보를 갱신
        UpdateTowerData();

    }
    public void OffPanel()
    {
        //타워 정보 판넬 off
        gameObject.SetActive(false);

    }

    private void UpdateTowerData()
    {
        textDamage.text = "Damage:" + currentTower.Damage+"+"+"<color=red>"+currentTower.AddedDamage.ToString("F1")+"</color>";
        imageTower.sprite = currentTower.TowerSprite;
        textRate.text = "Rate"+currentTower.Rate;                   //공속
        textRange.text = "Range"+currentTower.Range;                //범위
    }
    public void OnClickEventTowerUpgrade()
    {
        //타워 업그레이드 시도
        bool isSuccess = currentTower.Upgrade();

        if (isSuccess == true)
        {
            //타워가 업그레이드 되었기 때문에 타워 정보 갱신
            UpdateTowerData();
        }
        else {
            //타워 업그레이드에 필요한 비용 부족하다고 출력
            systemTextViewer.PrintText(SystemType.Money);
        }
    }
    public void OnClickEventTowerSell()
    {
        //타워 판매 
        currentTower.Sell();
        //타워가 사라져서 panel, 공격범위 off
        OffPanel();
    }
}
