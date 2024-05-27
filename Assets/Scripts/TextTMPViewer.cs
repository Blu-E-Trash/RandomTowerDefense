using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerHP;
    [SerializeField]
    private TextMeshProUGUI textPlayerGold;
    [SerializeField]
    private TextMeshProUGUI textWave;
    [SerializeField]
    private TextMeshProUGUI textEnemyCount;
    [SerializeField]
    private TextMeshProUGUI textPlayerPoint;
    [SerializeField]
    private PlayerHp playerHP;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private PlayerPoint playerPoint;
    [SerializeField]
    private WaveSysytem waveSystem;//웨이브 정보
    [SerializeField]
    private EnemySpawner enemySpawner;// 적 정보
    private void Update()
    {
        textPlayerHP.text = playerHP.CurrentHp + "/" + playerHP.MaxHP;
        textPlayerGold.text = playerGold.CurrentGold.ToString();
        textWave.text = waveSystem.CurrentWave+"/" + waveSystem.MaxWave;
        textEnemyCount.text = enemySpawner.CurrentEnemyCount + "/" + enemySpawner.MaxEnemyCount;
        textPlayerPoint.text = playerPoint.CurrentPoint.ToString();
    }
}