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
        //����ؾ��ϴ� Ÿ�� ������ �޾ƿͼ� ����
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        //Ÿ�� ���� ���� on
        gameObject.SetActive(true);
        //Ÿ�� ������ ����
        UpdateTowerData();

    }
    public void OffPanel()
    {
        //Ÿ�� ���� �ǳ� off
        gameObject.SetActive(false);

    }

    private void UpdateTowerData()
    {
        textDamage.text = "Damage:" + currentTower.Damage+"+"+"<color=red>"+currentTower.AddedDamage.ToString("F1")+"</color>";
        imageTower.sprite = currentTower.TowerSprite;
        textRate.text = "Rate"+currentTower.Rate;                   //����
        textRange.text = "Range"+currentTower.Range;                //����
    }
    public void OnClickEventTowerUpgrade()
    {
        //Ÿ�� ���׷��̵� �õ�
        bool isSuccess = currentTower.Upgrade();

        if (isSuccess == true)
        {
            //Ÿ���� ���׷��̵� �Ǿ��� ������ Ÿ�� ���� ����
            UpdateTowerData();
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
