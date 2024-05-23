using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSysytem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves; // ���� ���������� ��� ���̺� ����
    [SerializeField]
    private EnemySpawner enemySpawner;
    private int currentWaveIndex = -1;//���� ���̺� �ε���

    //���̺� ���� ����� ���� Get������Ƽ(���� ���̺�, �� ���̺�)
    public int CurrentWave => currentWaveIndex+1;//������ 0�̱� ������ +1
    public int MaxWave => waves.Length;

    public void StartWave()
    {
        //���� �ʿ� ���� ����, wave�� ����������
        if(enemySpawner.EnemyList.Count == 0 && currentWaveIndex<waves.Length-1)
        {
            //�ε����� ������ -1�̱� ������ ���̺� �ε��� ������ ���� ���� ��
            currentWaveIndex++;
            //EnemySpawner�� StartWave()�Լ� ȣ��. ���� ���̺� ���� ����
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
    }
}

[System.Serializable]
public struct Wave
{
    public float spawnTime;//���̺� �� ���� �ֱ�
    public int maxEnemyCount; //�� ���� ����
    public GameObject[] enemyPrefabs; //���̺� �� ���� ����
}