using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;
    private float baseMoveSpeed;

    /*public float MoveSpeed => moveSpeed;*/ // moveSpeed ������ ������Ƽ
    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }
    private void Awake()
    {
        baseMoveSpeed = moveSpeed;
    }
    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
    public void ResetMoveSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }
}