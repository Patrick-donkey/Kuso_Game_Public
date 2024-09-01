using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace RiverCrab
{
    public class WarriorBehaviour : MonoBehaviour
    {
        [SerializeField,BoxGroup("敵人移動參數")]
        float EnemySpd;
        [SerializeField, BoxGroup("敵人移動參數")]
        float wait;
        [SerializeField, BoxGroup("敵人移動參數")]
        float newWait;
        [SerializeField, BoxGroup("敵人移動參數")]
        bool stopPatrol;
        [SerializeField, BoxGroup("敵人移動參數")]
        Vector2 foundRange;
        [SerializeField, BoxGroup("敵人移動參數")]
        Vector3 foundPosY;

        [SerializeField, BoxGroup("攻擊參數")]
        int damage;
        [SerializeField, BoxGroup("攻擊參數")]
        float attackFound;
        [SerializeField, BoxGroup("攻擊參數")]
        float attackRange = 1f;
        [SerializeField, BoxGroup("攻擊參數")]
        Vector3 attackOffset;
        [SerializeField, BoxGroup("攻擊參數")]
        LayerMask attackMask;

        [SerializeField, BoxGroup("突擊參數")]
        float heavyDamage;
        [SerializeField, BoxGroup("突擊參數")]
        float heavyAttackRange;
        [SerializeField, BoxGroup("突擊參數")]
        float height;
        [SerializeField, BoxGroup("突擊參數")]
        float duration;
        [SerializeField, BoxGroup("突擊參數")]
        float elapsedTime;               
        [SerializeField, BoxGroup("突擊參數")]
        float CD;
        [SerializeField, BoxGroup("突擊參數")]
        bool heavyAttackCD;
        [SerializeField, BoxGroup("突擊參數")]
        Vector3 heavyAttackOffset;
        [SerializeField, BoxGroup("突擊參數")]
        Vector2 KnockBack;
        Vector2 startPos;
        Vector2 endPos;
        Vector2 endPosNow;

        int playerMask;        
        int enemyMask;        
        Transform player;
        Transform tr;        
        EnemyWarrior EW;
        Rigidbody2D rb;
        CapsuleCollider2D c2d;
        [SerializeField, BoxGroup("敵人獲取組件")]
        Transform leftPos;
        [SerializeField, BoxGroup("敵人獲取組件")]
        Transform rightPos;
        [SerializeField, BoxGroup("敵人獲取組件")]
        Transform movePos;
        #region 方法
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            c2d = GetComponent<CapsuleCollider2D>();
            EW = GetComponent<EnemyWarrior>();
            player = GameObject.Find("Player").transform;
            tr = transform.parent;

            playerMask = LayerMask.NameToLayer("Player");
            enemyMask = LayerMask.NameToLayer("Enemy");            
        }

        private void OnDrawGizmos()
        {
            Vector3 pos = transform.position;
            pos += transform.right * attackOffset.x;
            pos += transform.up * attackOffset.y;

            Vector3 Hpos = transform.position;
            Hpos += transform.right * heavyAttackOffset.x;
            Hpos += transform.up * heavyAttackOffset.y;

            Gizmos.color = Color.white;        
            Gizmos.DrawWireSphere(pos, attackRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Hpos, new Vector2(heavyAttackRange,1f));
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position - foundPosY, foundRange);
        }
        void OnDestroy()
        {
            Destroy(tr.gameObject);
        }
#endregion

        #region 死亡
        public void Dead()
        {
            Destroy(gameObject);
        }
        #endregion

        #region 翻轉
        public void Flip()
        {
            if (Vector2.Distance(transform.position, movePos.position) < 0.1f) return;
            transform.eulerAngles = movePos.position.x > transform.position.x ? (new Vector3(0, 0, 0)) : (new Vector3(0, 180, 0));
        }

        void AttackFlip()
        {
            if (Vector2.Distance(transform.position, player.position) < 0.1f) return;
            transform.eulerAngles = player.position.x > transform.position.x ? (new Vector3(0, 0, 0)) : (new Vector3(0, 180, 0));
        }
        #endregion
        #region 巡邏
        bool CheckGround()
        {
            if (c2d.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                return true;
            }
            return false;
        }

        public bool Patrol()
        {
            if (EW.hit) return true;
            if (stopPatrol) return true;
            if (!CheckGround()) return true;

            Flip();

            newWait += Time.fixedDeltaTime;
            Vector3 patrol = Vector2.MoveTowards(rb.position, movePos.position, EnemySpd * Time.fixedDeltaTime);
            rb.MovePosition(patrol);

            if (Vector2.Distance(rb.position, movePos.position) < 0.1f && newWait < wait)
            {
                return true;
            }
            if (newWait >= wait)
            {
                newWait = 0;
                movePos.position = RandomPos();
            }
            return false;
        }

        Vector2 RandomPos()
        {
            Vector2 randomPos = new Vector2(Random.Range(leftPos.position.x, rightPos.position.x), rb.position.y);
            return randomPos;
        }

        public bool SwitchToRun()
        {
            newWait += Time.fixedDeltaTime;
            if (newWait >= wait)
            {
                return true;
            }
            return false;
        }

        #endregion
        #region 尋找玩家
        public bool PlayerFound()
        {
            if (EW.hit) return false;
            if (!CheckGround()) return false;
            Collider2D playerfound = Physics2D.OverlapBox(transform.position -foundPosY, foundRange, 0, attackMask);
            if (playerfound != null)
            {
                return true;
            }
            else return false;
        }

        public bool AttackFound()
        {
            AttackFlip();

            Vector2 target = new Vector2(player.position.x, rb.position.y);
            if (Vector2.Distance(player.position, rb.position) <= attackFound)
            {
                rb.MovePosition(transform.position);
                return true;
            }
            else
            {
                Vector2 newPos = Vector2.MoveTowards(rb.position, target, EnemySpd * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
                return false;
            }
        }

        public bool HeavyAttackFound()
        {
            if (Vector2.Distance(transform.position, player.position) <= 4f) return false;
            if (!heavyAttackCD)
            {
                startPos = rb.position;
                endPos = new Vector2(player.position.x, rb.position.y);
                stopPatrol = true;
                elapsedTime = 0;
                StartCoroutine(HeavyAttackCD());
                return true;
            }
            return false;
            //AttackFlip();
            //if (!heavyAttackCD)
            //{
            //    Vector2 target = Vector2.Lerp(rb.position,player.position+new Vector3(0,10,0),1);
            //    rb.MovePosition(target);                
            //    return true;
            //}                       
            //return false;
        }
        public bool HeavyAttackFall()
        {
            AttackFlip();

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            Vector3 currentPos = CalculateParabola(startPos, endPosNow, height, t);
            rb.MovePosition(currentPos);

            if (t < 0.5f)
            {
                endPosNow = new Vector2(player.position.x,endPos.y);
            }
            //到達點位並重製
            if (t >= 1f)
            {
                Physics2D.IgnoreLayerCollision(playerMask, enemyMask, false);
                rb.velocity = Vector2.zero;
                c2d.enabled = true;
                stopPatrol = false;
                return false;
            }
            c2d.enabled = false;
            Physics2D.IgnoreLayerCollision(playerMask, enemyMask, true);            
            return true;
            
            //瞬間跳躍
            //StartCoroutine(HeavyAttackCD());
            //if (rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
            //{
            //    Physics2D.IgnoreLayerCollision(playerMask,enemyMask, false);
            //    rb.gravityScale = 1.0f;
            //    rb.velocity = Vector2.zero;
            //    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            //    return false;
            //}
            //rb.gravityScale = 2;
            //rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            //Physics2D.IgnoreLayerCollision(playerMask,enemyMask,true);
            //return true;
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
        #endregion
        #region 攻擊判斷
        public void AttackCheck()
        {
            Vector3 pos = transform.position;
            pos += transform.right * attackOffset.x;
            pos += transform.up * attackOffset.y;

            Collider2D attackCollider = Physics2D.OverlapCircle(pos, attackRange, attackMask);
            if (attackCollider != null)
            {
                print("ATK");
                attackCollider.GetComponent<PlayerStatus>().TakeDamage(damage);
            }
        }

        public void HeavyAttackCheck()
        {
            Vector3 Hpos = transform.position;
            Hpos += transform.right * heavyAttackOffset.x;
            Hpos += transform.up * heavyAttackOffset.y;

            Collider2D heavyAttackCollider = Physics2D.OverlapBox(Hpos, new Vector2(heavyAttackRange,1f),0, attackMask);
            if (heavyAttackCollider != null)
            {
                heavyAttackCollider.GetComponent<PlayerStatus>().TakeDamage(heavyDamage);
                heavyAttackCollider.GetComponent<PlayerStatus>().KnockBack(KnockBack,transform);
            }            
        }

        IEnumerator HeavyAttackCD()
        {
            heavyAttackCD = true;
            yield return new WaitForSeconds(CD);
            heavyAttackCD = false;
        }
        #endregion
    }
}
