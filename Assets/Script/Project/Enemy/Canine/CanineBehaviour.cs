using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace RiverCrab
{
    public class CanineBehaviour : MonoBehaviour
    {

        [SerializeField, BoxGroup("敵人移動參數")]
        float EnemySpd;
        [SerializeField, BoxGroup("敵人移動參數")]
        float wait;
        [SerializeField, BoxGroup("敵人移動參數")]
        float newWait;
        [SerializeField, BoxGroup("敵人移動參數")]
        bool stopPatrol;

        [SerializeField, BoxGroup("敵人攻擊參數")]
        float damage;
        [SerializeField, BoxGroup("敵人攻擊參數")]
        float attackFound;
        [SerializeField, BoxGroup("敵人攻擊參數")]
        float attackRange = 1f;
        [SerializeField, BoxGroup("敵人攻擊參數")]
        Vector3 attackOffset;
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
        bool chargeCD;
        Vector2 startPos;
        Vector2 endPos;

        //獲取組件
        int playerMask;
        int enemyMask;
        Transform player;
        Rigidbody2D rb;
        CapsuleCollider2D c2d;
        EnemyCanine EC;
        Transform parent;
        public Transform leftPos;
        public Transform rightPos;
        public Transform movePos;
        #region 方法
        void Start()
        {
            player = GameObject.Find("Player").transform;
            rb = GetComponent<Rigidbody2D>();
            c2d = GetComponent<CapsuleCollider2D>();
            EC = GetComponent<EnemyCanine>();
            playerMask = LayerMask.NameToLayer("Player");
            enemyMask = LayerMask.NameToLayer("Enemy");
            attackMask = LayerMask.GetMask("Player");
            parent = transform.parent;
        }

        private void OnDrawGizmos()
        {
            Vector3 pos = transform.position;
            pos += transform.right * attackOffset.x;
            pos += transform.up * attackOffset.y;


            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(pos, attackRange);
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position - new Vector3(0, 0f, 0), new Vector2(15f, 0.4f));
        }
        
        void OnDestroy()
        {
            Destroy(parent.gameObject);
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
            if (transform == null||Vector2.Distance(transform.position, movePos.position) < 0.1f) return;
            transform.eulerAngles = movePos.position.x > transform.position.x ? (new Vector3(0, 0, 0)) : (new Vector3(0, 180, 0));
        }

        void AttackFlip()
        {
            if (player.position == null||Vector2.Distance(transform.position, player.position) < 0.1f) return;
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
            if(EC.hit) return true;
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
            if(!CheckGround()) return false;
            if(EC.hit) return false;
            Collider2D playerfound = Physics2D.OverlapBox(transform.position - new Vector3(-1, 0f, 0), new Vector2(15f, 0.4f), 0, attackMask);
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

        public bool Charge()
        {
            if (Vector2.Distance(transform.position, player.position) <= 4f) return false;
            if (!chargeCD)
            {
                startPos = rb.position;                
                stopPatrol = true;
                elapsedTime = 0;
                StartCoroutine(ChargeCD());
                return true;
            }
            return false;
        }
        public bool ChargePos()
        {
            AttackFlip();
            
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;            

            Vector3 currentPos = CalculateParabola(startPos, endPos, height, t);
            rb.MovePosition(currentPos);

            if (t < 0.5f)
            {
                endPos = new Vector2(player.position.x, rb.position.y);
            }

            //到達點位並重製
            if (t >= 1f)
            {
                t = 1.0f;
                Physics2D.IgnoreLayerCollision(playerMask, enemyMask, false);
                rb.velocity = Vector2.zero;
                stopPatrol = false;                
                return false;
            }

            Physics2D.IgnoreLayerCollision(playerMask, enemyMask, true);

            return true;
            // 當前位置  
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
            if (attackCollider != null&& StatusUi.Hpcurrrent > 0)
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
            if (attackCollider != null&& StatusUi.Hpcurrrent>0)
            {
                attackCollider.GetComponent<PlayerStatus>().TakeDamage(chargeDamage);
            }
        }
        #endregion
    }
}
