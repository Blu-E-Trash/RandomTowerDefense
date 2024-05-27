using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoint : MonoBehaviour
{
    [SerializeField]
    private int currentPoint = 0;

    public int CurrentPoint
    {
        set => currentPoint = Mathf.Max(0, value);
        get => currentPoint;
    }
}