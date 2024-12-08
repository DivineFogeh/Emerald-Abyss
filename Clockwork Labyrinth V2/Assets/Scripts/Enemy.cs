using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;

    [SerializeField] public float speed;

    [SerializeField] public float damage;

    [SerializeField] AudioClip hurtSound;

    protected float recoilTimer;
    [HideInInspector] public Rigidbody2D rb;
    protected SpriteRenderer sr;
    public Animator anim;
    protected AudioSource audioSource;

    protected enum EnemyStates
    {
       //Gslime 
        Gslime_Idle,
        Gslime_Flip,

        //Oslime
        Oslime_Idle,
        Oslime_Chase,
        Oslime_Stunned,
        Oslime_Death,

        //THK
        THK_Stage1,
        THK_Stage2,
        THK_Stage3,
        THK_Stage4
    }
    protected EnemyStates currentEnemyState;

    protected virtual EnemyStates GetCurrentEnemyState
    {
        get { return currentEnemyState; }
        set
        {
            if(currentEnemyState != value)
            {
                currentEnemyState = value;

                ChangeCurrentAnimation();
            }
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (GameManager.Instance.gameIsPaused) return;

        if(isRecoiling)
        {
            if(recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
        else
        {
            UpdateEnemyStates();
        }
    }

    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        if(!isRecoiling)
        {
            audioSource.PlayOneShot(hurtSound);
            rb.velocity = _hitForce * recoilFactor * _hitDirection;
            isRecoiling = true;
        }
    }
    protected virtual void OnCollisionStay2D(Collision2D _other)
    {
        if(_other.gameObject.CompareTag("Player") && !PlayerController.Instance.pState.invincible && health > 0)
        {
            Attack();
            if(PlayerController.Instance.pState.alive)
            {
                PlayerController.Instance.HitStopTime(0, 5, 0.5f);
            }            
        }
    }

    protected virtual void Death(float _destroyTime)
    {
        Destroy(gameObject, _destroyTime);
    }

    protected virtual void UpdateEnemyStates() { }
    protected virtual void ChangeCurrentAnimation() { }

    protected void ChangeState(EnemyStates _newState)
    {
        GetCurrentEnemyState = _newState;
    }

    protected virtual void Attack()
    {
        PlayerController.Instance.TakeDamage(damage);
    }
    
}
