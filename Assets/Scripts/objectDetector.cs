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
    private Transform hitTransform = null;//마우스 클릭으로 선택한 오브젝트 임시 저장

    private void Awake()
    {
        //"MaomCamera" 태그를 가지고 있는 오브젝트 탐색 후 Camera컴포넌트 정보 전달
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();와 동일
        mainCamera = Camera.main;
        Debug.Log("레이케스트로 넘어옴");
    }

    private void Update()
    {
        //마우스가 UI에 머물러 있을 때는 아래 코드가 실행되지 않도록 함
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            Debug.Log("UI에 마우스 있음");
            return;
        }

        //마우스 왼쪽 버튼을 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            //카메라 위치에서 화면의 마우스 위치를 관통하는 광선 생성
            //ray.origin: 광선의 시작 위치(=카메라 위치)
            //ray.direction : 광선의 진행방향
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //2D 모니터를 통해 3D 월드의 오브젝트를 마우스로 선택하는 방법
            //광선에 부딪히는 오브젝트를 검출해서 hit에 저장
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;
                if (hit.transform.CompareTag("Tower"))
                {
                    Debug.Log("타워 클릭");
                    //타워면 타워 정보 호출
                    towerDataViewer.OnPanel(hit.transform);
                }
                //광선에 부딪힌 오브젝트의 태그가 "Tile"이면
                else if (hit.transform.CompareTag("Tile"))
                {
                    Debug.Log(hit.transform.gameObject);
                    //타워를 생성하는 SpawnTower() 호출
                    towerSpawner.SpawnTower(hit.transform);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //마우스를 눌렀을 때 선택한 오브젝트가 없거나 선택한 오브젝트가 타워가 아니면
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
            {
                //타워 패털 비활성화
                towerDataViewer.OffPanel();
            }

            hitTransform = null;
        }
    }
}