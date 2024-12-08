using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gslime : Enemy
{
    [SerializeField] private float flipWaitTime;
    [SerializeField] private float ledgeCheckX;
    [SerializeField] private float ledgeCheckY;
    [SerializeField] private LayerMask WhatIsGround;


    float timer;

    // Start is called before the first frame update
 
    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 12f;        

    }

    protected override void Update()
    {
        base.Update();
        if(!PlayerController.Instance.pState.alive)
        {
            ChangeState(EnemyStates.Gslime_Idle);
        }
    }

    protected override void UpdateEnemyStates()
    {
        if(health <= 0)
        {
            Destroy(gameObject, 0.05f);
        }

        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Gslime_Idle:
                Vector3 _ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(ledgeCheckX,0) : new Vector3(-ledgeCheckX, 0);
                Vector2 _wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;

                if(!Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down, ledgeCheckY, WhatIsGround)
                    || Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, WhatIsGround))
                {
                    ChangeState(EnemyStates.Gslime_Flip);
                }
                
                if(transform.localScale.x > 0)
                {
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                }
                break;

            case EnemyStates.Gslime_Flip:
                timer += Time.deltaTime;

                if(timer > flipWaitTime)
                {
                    timer = 0;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                    ChangeState(EnemyStates.Gslime_Idle);
                }
                break;
        }
    }
}
