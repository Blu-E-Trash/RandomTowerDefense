using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    private TowerWeapon towerWeapon;

    private void Awake()
    {
        towerWeapon = GetComponentInParent<TowerWeapon>();
    }

    private void OnTriggerEnter2D(Collider2D collision) //이거는 검사용으로 냅두기
    {
        if (!collision.CompareTag("Enemy"))
        {
            return;
        }

        //Movement2D movement2D = collision.GetComponent<Movement2D>();
        //이속 = 이속 - 이속 * 감속률
        //4 = 5 - 5 * 0.2;
        //movement2D.MoveSpeed -= movement2D.MoveSpeed * towerWeapon.Slow;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            return;
        }
        collision.GetComponent<Movement2D>().ResetMoveSpeed();
    }
}
