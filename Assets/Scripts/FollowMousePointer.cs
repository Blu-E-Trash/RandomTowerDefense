using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMousePointer : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        transform.position = mainCamera.ScreenToWorldPoint(position);
        //z위치를 0으로 설정
        transform.position = new Vector3(transform.position.x,transform.position.y, 0);
    }
}
