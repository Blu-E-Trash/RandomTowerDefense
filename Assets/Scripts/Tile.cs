using UnityEngine;

public class Tile : MonoBehaviour
{
    //타일에 타워가 건설되어 있는지 검사
    public bool IsBuildTower { set; get; }

    private void Awake()
    {
        IsBuildTower = false;
    }
}
