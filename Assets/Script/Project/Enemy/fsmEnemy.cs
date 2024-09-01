using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace RiverCrab
{
    public class FsmEnemy : MonoBehaviour
    {
        [SerializeField, BoxGroup("敵人移動參數")]
        float EnemySpd;
        [SerializeField, BoxGroup("敵人移動參數")]
        float wait;
        [SerializeField, BoxGroup("敵人移動參數")]
        float newWait;
        [SerializeField, BoxGroup("敵人移動參數")]
        bool stopPatrol;
        [SerializeField]
        LayerMask Ground;

        [SerializeField, BoxGroup("敵人攻擊參數")]
        float damage;
        [SerializeField, BoxGroup("敵人攻擊參數")]
        float attackFound;
        [SerializeField, BoxGroup("敵人攻擊參數")]
        float attackRange = 1f;
        [SerializeField, BoxGroup("敵人攻擊參數")]
        Vector3 attackOffset;
        [SerializeField, BoxGroup("敵人攻擊參數")]
        Vector2 attackFoundOffset;
        [SerializeField]
        LayerMask attackMask;

        [SerializeField, BoxGroup("突擊參數")]
        float chargeDamage;
        [SerializeField, BoxGroup("突擊參數")]
        float height = 5f;
        [SerializeField, BoxGroup("突擊參數")]
        float duration = 2f;
        [SerializeField, BoxGroup("突擊參數")]
        float elapsedTime = 0f;
        [SerializeField, BoxGroup("突擊參數")]
        float chargeCoolDown;
        [SerializeField]
        bool chargeCD;
        Vector2 startPos;
        Vector2 endPos;

        #region 組件
        ObjStateNow idle;
        ObjStateNow run;
        ObjStateNow charge;


        int playerMask;
        int enemyMask;
        Animator animator;
        Transform player;
        Transform parent;
        Rigidbody2D rb;
        CapsuleCollider2D c2d;
        public Transform leftPos;
        public Transform rightPos;
        public Transform movePos;
        EnemyCanine EC;
        StateMachine stateMachine;
        #endregion

        #region 方法
        void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            c2d = GetComponent<CapsuleCollider2D>();
            EC = GetComponent<EnemyCanine>();
            player = GameObject.Find("Player").transform;
            playerMask = LayerMask.NameToLayer("Player");
            enemyMask = LayerMask.NameToLayer("Enemy");

            parent = transform.parent;

            stateMachine = new StateMachine();
            idle = new ObjStateNow
            (
                () => { },
                () =>
                {
                    if (EC.dead)
                    {
                        animator.SetTrigger("Death");
                    }

                    if (EC.hit)
                    {
                        animator.SetTrigger("Hit");
                        animator.ResetTrigger("Hit");
                    }
                    SwitchToRun();
                },
                () => { }
            );
            run = new ObjStateNow
            (
                () => { },

                () =>
                {
                    if (EC.dead)
                    {
                        animator.SetTrigger("Death");
                    }

                    if (EC.hit)
                    {
                        animator.SetTrigger("Hit");
                        animator.ResetTrigger("Hit");
                    }

                    if (PlayerFound())
                    {
                        if (!chargeCD)
                        {
                            if (Charge())
                            {
                                animator.SetBool("Charge", true);
                                stateMachine.ChangeState(charge);
                            }
                        }
                        AttackFound();                        
                    }
                    else
                    {
                        Patrol();
                    }
                },

                () => { }
            );
            charge = new ObjStateNow
            (
                () => { },
                () =>
                {
                    if (EC.dead)
                    {
                        animator.SetTrigger("Death");
                    }

                    if (EC.hit)
                    {
                        animator.SetTrigger("Hit");
                        animator.ResetTrigger("Hit");
                    }

                    if (ChargePos())
                    {
                        stateMachine.ChangeState(idle);
                    }
                },
                () => 
                {
                    
                }
            );
            stateMachine.ChangeState(run);
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.UpdateState();
        }
        private void OnDrawGizmos()
        {
            Vector3 pos = transform.position;
            pos += transform.right * attackOffset.x;
            pos += transform.up * attackOffset.y;


            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(pos, attackRange);
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position - new Vector3(0, 0f, 0), attackFoundOffset);
        }

        void OnDestroy()
        {
            animator.ResetTrigger("Death");
            Destroy(parent.gameObject);
        }
        #endregion

        #region 死亡
        void Dead()
        {
            Destroy(gameObject);
        }
        #endregion

        #region 翻轉
        void Flip()
        {
            if (transform == null || Vector2.Distance(transform.position, movePos.position) < 0.1f) return;
            transform.eulerAngles = movePos.position.x > transform.position.x ? (new Vector3(0, 0, 0)) : (new Vector3(0, 180, 0));
        }

        void AttackFlip()
        {
            if (player.position == null || Vector2.Distance(transform.position, player.position) < 0.1f) return;
            transform.eulerAngles = player.position.x > transform.position.x ? (new Vector3(0, 0, 0)) : (new Vector3(0, 180, 0));
        }
        #endregion

        #region 巡邏
        bool CheckGround()
        {
            if (c2d.IsTouchingLayers(Ground))
            {
                return true;
            }
            return false;
        }
        void Patrol()
        {
            if (!CheckGround()) return;
            else if (stopPatrol) return;
            else if (EC.hit) return;

            newWait += Time.fixedDeltaTime;
            Vector3 patrol = Vector2.MoveTowards(rb.position, movePos.position, EnemySpd * Time.fixedDeltaTime);
            rb.MovePosition(patrol);

            if (Vector2.Distance(rb.position, movePos.position) < 0.1f && newWait < wait)
            {
                animator.SetBool("Run", false);
                stateMachine.ChangeState(idle);
            }
            if (newWait >= wait)
            {
                newWait = 0;
                movePos.position = RandomPos();
            }

        }

        Vector2 RandomPos()
        {
            Vector2 randomPos = new Vector2(Random.Range(leftPos.position.x, rightPos.position.x), rb.position.y);
            return randomPos;
        }

        bool SwitchToRun()
        {
            newWait += Time.fixedDeltaTime;
            if (newWait >= wait)
            {
                newWait = 0;
                animator.SetBool("Run", true);
                stateMachine.ChangeState(run);
            }
            return false;
        }
        #endregion

        #region 尋找玩家
        bool PlayerFound()
        {
            if (!CheckGround()) return false;
            else if (EC.hit) return false;
            else if (stopPatrol) return false;

            Collider2D playerfound = Physics2D.OverlapBox(transform.position - new Vector3(-1, 0f, 0), attackFoundOffset, 0, attackMask);
            if (playerfound != null)
            {
                AttackFlip();
                return true;
            }
            else
            {
                Flip();
                return false;
            }
        }

        void AttackFound()
        {
            if (stopPatrol) return;
            
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            if (Vector2.Distance(player.position, rb.position) <= attackFound)
            {
                rb.MovePosition(transform.position);
                animator.SetTrigger("Attack");
                StartCoroutine(ATKreset());
            }
            else
            {
                Vector2 newPos = Vector2.MoveTowards(rb.position, target, EnemySpd * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
        }

        IEnumerator ATKreset()
        {
            yield return null;
            animator.ResetTrigger("Attack");
        }

        bool Charge()
        {
            if (Vector2.Distance(transform.position, player.position) <= 4f) return false;

            StartCoroutine(ChargeCD());
            startPos = rb.position;
            stopPatrol = true;
            return true;
        }

        bool ChargePos()
        {
            float t = elapsedTime / duration;

            //到達點位並重製
            if (t >= 1.0f)
            {
                c2d.enabled = true;
                rb.velocity = Vector2.zero;
                stopPatrol = false;
                animator.SetBool("Charge", false);
                return true;
            }

            elapsedTime += Time.deltaTime;

            Vector3 currentPos = CalculateParabola(startPos, endPos, height, t);
            rb.MovePosition(currentPos);

            if (t < 0.5f)
            {
                endPos = new Vector2(player.position.x, rb.position.y);
                c2d.enabled = false;
            }

            return false;
        }

        Vector3 CalculateParabola(Vector2 start, Vector2 end, float height, float t)
        {
            float parabolicT = t * 2 - 1;

            //if (Mathf.Abs(start.y - end.y) < 0.1f)
            //{
            //平拋
            Vector2 travelDirection = end - start;
            Vector2 result = start + t * travelDirection;
            result.y += (-parabolicT * parabolicT + 1) * height;
            return result;
            //}
            //else
            //{
            //    // 斜拋
            //    Vector3 travelDirection = end - start;
            //    Vector3 result = start + t * travelDirection;
            //    result.y += (-parabolicT * parabolicT + 1) * height;

            //    Vector3 tangent = Vector3.Lerp(start, end, t);
            //    result.y = Mathf.Lerp(tangent.y, result.y, Mathf.Abs(parabolicT));
            //    return result;
            //}
        }

        IEnumerator ChargeCD()
        {
            chargeCD = true;
            yield return new WaitForSeconds(chargeCoolDown);
            chargeCD = false;
        }
        #endregion

        #region 攻擊判斷
        public void AttackCheck()
        {
            Vector3 pos = transform.position;
            pos += transform.right * attackOffset.x;
            pos += transform.up * attackOffset.y;

            Collider2D attackCollider = Physics2D.OverlapCircle(pos, attackRange, attackMask);
            if (attackCollider != null && StatusUi.Hpcurrrent > 0)
            {
                attackCollider.GetComponent<PlayerStatus>().TakeDamage(damage);
            }
        }

        public void ChargeAttackCheck()
        {
            Vector3 pos = transform.position;
            pos += transform.right * attackOffset.x;
            pos += transform.up * attackOffset.y;

            Collider2D attackCollider = Physics2D.OverlapCircle(pos, attackRange, attackMask);
            if (attackCollider != null && StatusUi.Hpcurrrent > 0)
            {
                attackCollider.GetComponent<PlayerStatus>().TakeDamage(chargeDamage);
            }
        }
        #endregion
    }
}