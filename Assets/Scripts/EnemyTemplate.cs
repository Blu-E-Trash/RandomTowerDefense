using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyTemplate : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyStatus
    {
        public Sprite sprite;   //�� �̹���
        public float MoveSpeed;    //�̼�
        public float Hp;      //��
        public float Gold;     //��
    }
}
