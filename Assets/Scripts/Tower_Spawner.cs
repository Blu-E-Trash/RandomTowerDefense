using UnityEditor;
using UnityEngine;
using System.Collections;

public class Tower_Spawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate;
    [SerializeField]
    private EnemySpawner enemySpawner;// ���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ����..
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    private bool isOnTowerButton = false;
    private int towerType;//Ÿ�� �Ӽ�
    
    public void ReadyToSpawnTower()
    {
        //Ÿ���� �������� ����
        int type = Random.Range(0, 4);
        towerType = type;
        //��ư�� �ߺ��ؼ� ������ ���� ����
        if(isOnTowerButton == true)
        {
            Debug.Log("��ư ������ ����");
            return;
        }
        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        //���� ���� ������ �Ǽ� x
        if (30 > playerGold.CurrentGold)
        {
            //�� ��� �����´ٰ� ���
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }
        //�Ǽ� ��ư �����ٰ� ����
        isOnTowerButton = true;
        systemTextViewer.PrintText(SystemType.Lcation);
        //Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnTowerCancelSystem");
    }
    public void SpawnTower(Transform tileTransform)
    {
        //�Ǽ� ��ư�� ������ ���� Ÿ�� �Ǽ� ����
        if(isOnTowerButton == false)
        {
            return;
        }
        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        ////1. Ÿ���� �Ǽ��� ��ŭ ���� ������ Ÿ�� �Ǽ� x
        //if (towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        //{
        //    systemTextViewer.PrintText(SystemType.Money);
        //    return;
        //}
        Tile tile = tileTransform.GetComponent<Tile>();

        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        //2. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �Ǽ��Ǿ� ������ Ÿ�� �Ǽ� x
        if(tile.IsBuildTower == true )
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }
        //�ٽ� Ÿ�� �Ǽ� ��ư�� ������ Ÿ���� ���鵵�� ���� ����
        isOnTowerButton = false;
        //Ÿ���� �Ǽ��Ǿ� �����Ƿ� ����
        tile.IsBuildTower = true;
        //Ÿ�� �Ǽ��� �ʿ��� ��常ŭ ����
        playerGold.CurrentGold -= 30;
        //������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�(Ÿ�Ϻ�Ÿ z�� -1�� ��ġ�� ��ġ)
        Vector3 positison = tileTransform.position + Vector3.back;
        switch (towerType)
        {
            case 0:
                systemTextViewer.PrintText(SystemType.Archor);
                break;
            case 1:
                systemTextViewer.PrintText(SystemType.Cannon); break;
            case 2:
                systemTextViewer.PrintText(SystemType.Sword); break;
            case 3:
                systemTextViewer.PrintText(SystemType.Wizard); break;
        }
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab,positison,Quaternion.identity);
        //Ÿ�� ���⿡ enemySpawner ���� ����
        clone.GetComponent<TowerWeapon>().Setup(this,enemySpawner, playerGold,tile);

        //Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ����
        StopCoroutine("OnTowerCancelSystem");
    }
    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;
                break;
            }

            yield return null;
        }
    }
}
