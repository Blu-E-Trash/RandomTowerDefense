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
        //����ؾ��ϴ� Ÿ�� ������ �޾ƿͼ� ����
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        //Ÿ�� ���� ���� on
        gameObject.SetActive(true);
        //Ÿ�� ������ ����
        UpdateTowerData();
        //Ÿ�� ������Ʈ �ֺ��� ǥ�õǴ� ���ݹ��� Sprite On
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }
    public void OffPanel()
    {
        //Ÿ�� ���� �ǳ� off
        gameObject.SetActive(false);
        //Ÿ�� ���� ���� Spriteo off
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
        textRate.text = "Rate"+currentTower.Rate;                   //����
        textRange.text = "Range"+currentTower.Range;                //����
        textLevel.text = "Level"+ currentTower.Level;               //��
        textUpgradeCost.text = currentTower.UpgradeCost.ToString(); //���� ���
        textSellCost.text = currentTower.SellCost.ToString();       //�ȱ�� �󸶳�
        //���׷��̵尡 �Ұ��������� ��ư ��Ȱ��ȭ
        buttonUpgrade.interactable = currentTower.Level< currentTower.MaxLevel? true: false;
    }

    public void OnClickEventTowerUpgrade()
    {
        //Ÿ�� ���׷��̵� �õ�
        bool isSuccess = currentTower.Upgrade();

        if (isSuccess == true)
        {
            //Ÿ���� ���׷��̵� �Ǿ��� ������ Ÿ�� ���� ����
            UpdateTowerData();
            //Ÿ�� �ֺ��� ���̴� ���� ������ ����
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else {
            //Ÿ�� ���׷��̵忡 �ʿ��� ��� �����ϴٰ� ���
            systemTextViewer.PrintText(SystemType.Money);
        }
        
    }
    public void OnClickEventTowerSell()
    {
        //Ÿ�� �Ǹ�
        currentTower.Sell();
        //Ÿ���� ������� panel, ���ݹ��� off
        OffPanel();
    }
}
