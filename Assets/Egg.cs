using Sirenix.OdinInspector;
using UnityEngine;

namespace RiverCrab
{
    public class Egg : MonoBehaviour
    {
        [SerializeField, BoxGroup("突擊參數")]
        float damage;
        [SerializeField, BoxGroup("突擊參數")]
        float height;
        [SerializeField, BoxGroup("突擊參數")]
        float duration;
        [SerializeField, BoxGroup("突擊參數")]
        float elapsedTime;
        Vector2 startPos;
        Vector2 endPos;
        Vector2 endPosNow;
        Rigidbody2D rb;
        Transform player;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.Find("Player").GetComponent<Transform>();

            startPos = rb.position;
            endPos = player.position;
            elapsedTime = 0;
            float ObjSpin = (endPos.x<startPos.x) ? (-360f) : (360f);
            rb.angularVelocity = ObjSpin;
        }

        // Update is called once per frame
        void Update()
        {
            Eggthrow();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() == "UnityEngine.BoxCollider2D")
            {
                Destroy(gameObject);
                collision.gameObject.GetComponent<PlayerStatus>().TakeDamage(damage);
            }
            if (collision.gameObject.CompareTag("Ground")||collision.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }

        private void Eggthrow()
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            Vector3 currentPos = CalculateParabola(startPos, endPosNow, height, t);
            rb.MovePosition(currentPos);

            if (t < 0.5f)
            {
                endPosNow = player.position;
            }
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
            //    Vector2 travelDirection = end - start;
            //    Vector2 result = start + t * travelDirection;
            //    result.y += (-parabolicT * parabolicT + 1) * height;

            //    Vector2 tangent = Vector3.Lerp(start, end, t);
            //    result.y = Mathf.Lerp(tangent.y, result.y, Mathf.Abs(parabolicT));
            //    return result;
            //}
        }
    }
}
