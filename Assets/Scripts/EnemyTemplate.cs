using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyTemplate : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyStatus
    {
        public Sprite sprite;   //적 이미지
        public float MoveSpeed;    //이속
        public float Hp;      //피
        public float Gold;     //돈
    }
}
