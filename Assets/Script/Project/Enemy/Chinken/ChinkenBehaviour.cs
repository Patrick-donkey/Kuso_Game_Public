using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace RiverCrab
{
    public class ChinkenBehaviour : MonoBehaviour
    {
        [SerializeField, BoxGroup("敵人移動參數")]
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

        [SerializeField, BoxGroup("發射雞蛋參數")]
        float CD;
        bool shotCD;
        [SerializeField, BoxGroup("發射雞蛋參數")]
        GameObject Egg;
        [SerializeField, BoxGroup("發射雞蛋參數")]
        Transform EggSpawn;

        Transform player;
        Transform tr;
        EnemyChinken EC;
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
            EC = GetComponent<EnemyChinken>();
            player = GameObject.Find("Player").transform;
            tr = transform.parent;

        }

        private void OnDrawGizmos()
        {
            Vector3 pos = transform.position;
            pos += transform.right * attackOffset.x;
            pos += transform.up * attackOffset.y;

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(pos, attackRange);

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
            if (transform == null||Vector2.Distance(transform.position, movePos.position) < 0.1f) return;
            transform.eulerAngles = movePos.position.x > transform.position.x ? (new Vector3(0, 0, 0)) : (new Vector3(0, 180, 0));
        }

        void AttackFlip()
        {
            if (player.transform==null||Vector2.Distance(transform.position, player.position) < 0.1f) return;
            transform.eulerAngles = player.position.x > transform.position.x ? (new Vector3(0, 0, 0)) : (new Vector3(0, 180, 0));
        }

        void ShotFlip()
        {
            if (player.transform==null||Vector2.Distance(transform.position, player.position) < 0.1f) return;
            transform.eulerAngles = player.position.x > transform.position.x ? (new Vector3(0, 180, 0)) : (new Vector3(0, 0, 0));
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
            if (EC.hit) return true;            
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
            if (EC.hit) return false;
            if (!CheckGround()) return false;
            Collider2D playerfound = Physics2D.OverlapBox(transform.position - foundPosY, foundRange, 0, attackMask);
            if (playerfound != null)
            {
                return true;
            }
            else return false;
        }

        public bool AttackFound()
        {
            rb.MovePosition(rb.position);
            if (Vector2.Distance(player.position, rb.position) <= attackFound)
            {
                AttackFlip();
                return true;
            }
            return false;
        }

        public bool EggShot()
        {
            rb.MovePosition(rb.position);
            if (shotCD) return false;
            ShotFlip();
            StartCoroutine(Shot());
            return true;
        }

        IEnumerator Shot()
        {
            shotCD = true;
            Instantiate(Egg, EggSpawn.position, Quaternion.identity);
            yield return new WaitForSeconds(CD);
            shotCD = false;
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
        #endregion
    }
}

