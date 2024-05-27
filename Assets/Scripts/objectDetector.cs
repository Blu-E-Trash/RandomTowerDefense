using UnityEngine;
using UnityEngine.EventSystems;

public class objectDetector : MonoBehaviour
{
    [SerializeField]
    private Tower_Spawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;//���콺 Ŭ������ ������ ������Ʈ �ӽ� ����

    private void Awake()
    {
        //"MaomCamera" �±׸� ������ �ִ� ������Ʈ Ž�� �� Camera������Ʈ ���� ����
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();�� ����
        mainCamera = Camera.main;
        Debug.Log("�����ɽ�Ʈ�� �Ѿ��");
    }

    private void Update()
    {
        //���콺�� UI�� �ӹ��� ���� ���� �Ʒ� �ڵ尡 ������� �ʵ��� ��
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            Debug.Log("UI�� ���콺 ����");
            return;
        }

        //���콺 ���� ��ư�� ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            //ī�޶� ��ġ���� ȭ���� ���콺 ��ġ�� �����ϴ� ���� ����
            //ray.origin: ������ ���� ��ġ(=ī�޶� ��ġ)
            //ray.direction : ������ �������
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //2D ����͸� ���� 3D ������ ������Ʈ�� ���콺�� �����ϴ� ���
            //������ �ε����� ������Ʈ�� �����ؼ� hit�� ����
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;
                if (hit.transform.CompareTag("Tower"))
                {
                    Debug.Log("Ÿ�� Ŭ��");
                    //Ÿ���� Ÿ�� ���� ȣ��
                    towerDataViewer.OnPanel(hit.transform);
                }
                //������ �ε��� ������Ʈ�� �±װ� "Tile"�̸�
                else if (hit.transform.CompareTag("Tile"))
                {
                    Debug.Log(hit.transform.gameObject);
                    //Ÿ���� �����ϴ� SpawnTower() ȣ��
                    towerSpawner.SpawnTower(hit.transform);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //���콺�� ������ �� ������ ������Ʈ�� ���ų� ������ ������Ʈ�� Ÿ���� �ƴϸ�
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
            {
                //Ÿ�� ���� ��Ȱ��ȭ
                towerDataViewer.OffPanel();
            }

            hitTransform = null;
        }
    }
}