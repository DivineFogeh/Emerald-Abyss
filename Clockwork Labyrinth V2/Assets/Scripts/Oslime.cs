using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Oslime : Enemy
{
    [SerializeField] private float chaseDistance;
    [SerializeField] private float stunDuration;

    float timer;

    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Oslime_Idle);
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
        if (PlayerController.Instance == null) return; // Avoid null reference

        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Oslime_Idle:

                rb.velocity = new Vector2(0, 0);
                if (_dist < chaseDistance)
                {
                    ChangeState(EnemyStates.Oslime_Chase);
                }
                break;

            case EnemyStates.Oslime_Chase:
                Vector2 direction = (PlayerController.Instance.transform.position - transform.position).normalized;
                rb.velocity = direction * speed;
                FlipOslime();

                if(_dist > chaseDistance)
                {
                    ChangeState(EnemyStates.Oslime_Idle);
                }
                break;

            case EnemyStates.Oslime_Stunned:
                timer += Time.deltaTime;

                if (timer > stunDuration)
                {
                    timer = 0;
                    ChangeState(EnemyStates.Oslime_Idle);
                }
                break;
            case EnemyStates.Oslime_Death:
                Death(Random.Range(5,10));
                break;
        }
    }

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);

        if (health > 0)
        {
            ChangeState(EnemyStates.Oslime_Stunned);
            rb.velocity = Vector2.zero; // Stop movement during stun
        }
        else
        {
            ChangeState(EnemyStates.Oslime_Death);
        }
    }

    protected override void Death(float _destroyTime)
    {
        rb.gravityScale = 12;
        base.Death(_destroyTime);
    }

    protected override void ChangeCurrentAnimation()
    {
        anim.SetBool("Idle", GetCurrentEnemyState == EnemyStates.Oslime_Idle);
        anim.SetBool("Idle", GetCurrentEnemyState == EnemyStates.Oslime_Chase);
        anim.SetBool("Idle", GetCurrentEnemyState == EnemyStates.Oslime_Stunned);

        if(currentEnemyState == EnemyStates.Oslime_Death)
        {
            anim.SetTrigger("Death");
        }

    }

    void FlipOslime()
    {
        if (PlayerController.Instance != null)
        {
            sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;
        }
    }
}
