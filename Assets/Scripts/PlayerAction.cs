using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float Speed;
    public GameManager manager;

    float h;
    float v;
    bool isHorizonMove;
    bool isADown;
    //현재 바라보고 있는 방향 값을 가진 변수
    Vector2 dirVec;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame 
    void Update()
    {
        h = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        v = manager.isAction ? 0 : Input.GetAxisRaw("Vertical");

        //Check Botton Down & Up 
        //PC+Moblie
        bool hDown = manager.isAction ? false : Input.GetButtonDown("Horizontal");
        bool vDown = manager.isAction ? false : Input.GetButtonDown("Vertical");
        bool hUp = manager.isAction ? false : Input.GetButtonUp("Horizontal");
        bool vUp = manager.isAction ? false : Input.GetButtonUp("Vertical");

        //Check Horizontal Move
        if (hDown || vUp)
            isHorizonMove = true;
        else if (vDown || hUp)
            isHorizonMove = false;

        if (vDown && v == 1)
            dirVec = Vector2.up;
        else if (vDown && v == -1)
            dirVec = Vector2.down;
        else if (hDown && h == -1)
            dirVec = Vector2.left;
        else if (hDown && h == 1)
            dirVec = Vector2.right;
    }
    void FixedUpdate()
    {
        //Move
        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) :
            new Vector2(0, v);
        rigid.velocity = moveVec * 4;
        
    }
}