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
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private Text textUpgradeCost;
    [SerializeField]
    private Text textSellCost;
    [SerializeField]
    private TowerAttackRange towerAttackRange;
    [SerializeField]
    private Button buttonUpgrade;
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
        //타워 오브젝트 주변에 표시되는 공격범위 Sprite On
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }
    public void OffPanel()
    {
        //타워 정보 판넬 off
        gameObject.SetActive(false);
        //타워 공격 범위 Spriteo off
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData()
    {
        if(currentTower.WeaponType == WeaponType.Cannon||currentTower.WeaponType == WeaponType.Wizard || currentTower.WeaponType == WeaponType.Archor|| currentTower.WeaponType == WeaponType.Sword) {
            imageTower.rectTransform.sizeDelta = new Vector2(22, 19);
            textDamage.text = "Damage:" + currentTower.Damage+"+"+"<color=red>"+currentTower.AddedDamage.ToString("F1")+"</color>";
        }
        imageTower.sprite = currentTower.TowerSprite;
        //textDamage.text = "Damage:" + currentTower.Damage;
        textRate.text = "Rate"+currentTower.Rate;                   //공속
        textRange.text = "Range"+currentTower.Range;                //범위
        textLevel.text = "Level"+ currentTower.Level;               //렙
        textUpgradeCost.text = currentTower.UpgradeCost.ToString(); //업글 비용
        textSellCost.text = currentTower.SellCost.ToString();       //팔기면 얼마나
        //업그레이드가 불가능해지면 버튼 비활성화
        buttonUpgrade.interactable = currentTower.Level< currentTower.MaxLevel? true: false;
    }

    public void OnClickEventTowerUpgrade()
    {
        //타워 업그레이드 시도
        bool isSuccess = currentTower.Upgrade();

        if (isSuccess == true)
        {
            //타워가 업그레이드 되었기 때문에 타워 정보 갱신
            UpdateTowerData();
            //타워 주변에 보이는 공격 범위도 갱신
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
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
