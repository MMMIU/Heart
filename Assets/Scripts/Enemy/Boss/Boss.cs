using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    public class Boss : MonoBehaviour
    {
        public UnityEvent OnBossDefeated;

        [Header("Boss settings")]
        [SerializeField]
        private int blobsToSpawnWhenBossDies = 5;
        [SerializeField]
        private int HP = 100;
        [SerializeField]
        private bool isHurtable = true;
        [SerializeField]
        private bool isAlive = true;

        public void SetBossTriggered(bool value)
        {
            spawnBlobs = value;
        }

        [Space]
        [Header("Blobs settings")]
        [SerializeField]
        private bool spawnBlobs = true;
        [SerializeField]
        private List<EnemyAI> differentTypesOfEnemies = new List<EnemyAI>();
        [SerializeField]
        private int maxBlobsToSpawn = 5;

        private List<IBlob> blobs = new();

        [Space]
        [Header("Blobs spawn settings")]
        [SerializeField]
        private GameObject blobParentGO;
        [SerializeField]
        private Vector2 blobSpawnOffset = new Vector2(0.0f, 2.0f);
        [SerializeField]
        private float minTimeBetweenBlobs = 2.0f;
        [SerializeField]
        private float minThrowDistance = 5.0f;
        [SerializeField]
        private float maxThrowDistance = 10.0f;
        [SerializeField]
        private float minThrowAngle = 30.0f;
        [SerializeField]
        private float maxThrowAngle = 60.0f;


        [SerializeField]
        private GameObject playerStartCombatDialogueTrigger;

        [SerializeField]
        private GameObject playerLowHp1DialogueTrigger;

        [SerializeField]
        private GameObject playerLowHp2DialogueTrigger;

        private GameObject player;
        private PlayerMovement playerMovement;


        private float bossAliveTime = 0.0f;
        private float nextSpawnTime = 0.0f;
        private Vector3 bossFinalPosition;

        private void CalcNextSpawnTime()
        {
            //minTimeBetweenBlobs += bossAliveTime / 100;
            nextSpawnTime = bossAliveTime + minTimeBetweenBlobs;
        }

        private bool BossCompletenessCheck()
        {
            // check blobs
            if (differentTypesOfEnemies.Count <= 0)
            {
                Debug.LogError("Blob prefabs is empty");
                return false;
            }
            if (maxBlobsToSpawn <= 0)
            {
                Debug.LogError("Max blobs to spawn is less than or equal to 0");
                return false;
            }
            return true;
        }

        private void Awake()
        {
            if (!BossCompletenessCheck())
            {
                // Quit editor
                Debug.LogError("Boss compromised! Quiting...");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }

            // Get player
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player not found");
            }
            playerMovement = player.GetComponent<PlayerMovement>();
            // hook player lowHp1
            playerMovement.OnPlayerLowHP1.AddListener(PlayerLowHp1);
            // hook player lowHp2
            playerMovement.OnPlayerLowHP2.AddListener(PlayerLowHp2);

            maxThrowDistance = (player.transform.position.x - transform.position.x) / 2;
            minThrowDistance = maxThrowDistance / 2;
            nextSpawnTime = minTimeBetweenBlobs;
        }

        private void OnDisable()
        {
            OnBossDefeated.RemoveAllListeners();
            // remove playerMovement listeners
            playerMovement.OnPlayerLowHP1.RemoveListener(PlayerLowHp1);
            playerMovement.OnPlayerLowHP2.RemoveListener(PlayerLowHp2);
        }

        // player lowHp1 conversation
        IEnumerator PlayerStartCombat(float time)
        {
            yield return new WaitForSeconds(time);
            if (playerStartCombatDialogueTrigger != null)
            {
                playerStartCombatDialogueTrigger?.SetActive(true);
            }
        }

        public void PlayerLowHp1()
        {
            if (playerLowHp1DialogueTrigger != null)
            {
                playerLowHp1DialogueTrigger?.SetActive(true);
            }
        }

        public void PlayerLowHp2()
        {
            if (playerLowHp2DialogueTrigger != null)
            {
                playerLowHp2DialogueTrigger?.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!spawnBlobs)
            {
                return;
            }

            bossAliveTime += Time.deltaTime;
            if (bossAliveTime >= nextSpawnTime)
            {
                if (blobs.Count < maxBlobsToSpawn)
                {
                    SpawnBlob(true);
                    // call PlayerStartCombat after 1s
                    StartCoroutine(PlayerStartCombat(1.0f));
                }
                CalcNextSpawnTime();
            }

        }

        private void SpawnBlob(bool bindToBoss)
        {
            Vector2 targetPos = new(transform.position.x, transform.position.y);
            targetPos.x += Random.Range(minThrowDistance, maxThrowDistance);

            Vector2 startPos = new Vector2(transform.position.x, transform.position.y) + blobSpawnOffset;

            // generate random blob
            int blobIndex = Random.Range(0, differentTypesOfEnemies.Count);
            EnemyAI randomEnemy = differentTypesOfEnemies[blobIndex];

            var blobGO = Instantiate(randomEnemy, isAlive ? transform.position : bossFinalPosition, Quaternion.identity, blobParentGO.transform);

            float angle = Random.Range(minThrowAngle, maxThrowAngle);

            Vector2 speed = CalculateVelocity2D(startPos, targetPos, angle, isAlive);

            blobGO.GetComponent<Rigidbody2D>().velocity = speed;

            IBlob blob = blobGO.GetComponent<IBlob>();
            if (bindToBoss)
            {
                // bind blob to boss
                blobGO.GetComponent<BlobNotify>().OnBlobDefeated.AddListener(() => RemoveBlob(blob));
                // add blob to blobs
                blobs.Add(blob);
                blobGO.name = "Blob " + blobs.Count;
            }

        }

        public void TakeDamage(int damage)
        {
            if (!isHurtable)
            {
                Debug.Log("Boss is not defeatable");
                return;
            }

            Debug.Log("Boss took " + damage + " damage");

            HP -= damage;

            if (HP <= 0)
            {
                isAlive = false;
                // kill all blobs
                foreach (var blob in blobs)
                {
                    blob.TakeDamage(10086);
                }

                Debug.Log("Boss defeated");
                bossFinalPosition = transform.position;
                spawnBlobs = false;
                // spawn final blobs after 1s
                StartCoroutine(SpawnFinalBlobs(1.0f));

                OnBossDefeated?.Invoke();
            }
        }

        IEnumerator SpawnFinalBlobs(float time)
        {
            yield return new WaitForSeconds(time);
            // spawn final blobs
            for (int i = 0; i < blobsToSpawnWhenBossDies; ++i)
            {
                SpawnBlob(false);
            }
        }

        public bool SetBossDefeatable()
        {
            return isHurtable;
        }

        public void SetBossDefeatable(bool isDefeatable)
        {
            isHurtable = isDefeatable;
        }

        public void SetBossHP(int hp)
        {
            HP = hp;
        }

        public void RemoveBlob(IBlob blob)
        {
            if (!isAlive)
            {
                return;
            }
            blobs.Remove(blob);
            CalcNextSpawnTime();
        }

        // calculate initial velocity to reach target
        private Vector2 CalculateVelocity2D(Vector2 start, Vector2 target, float angle, bool facePlayer = true)
        {

            // define gravity
            float gravity = Physics2D.gravity.magnitude;

            // define distance
            Vector2 distance = target - start;
            float distanceX = distance.x;
            float distanceY = distance.y;

            // define angle
            float angleRad = angle * Mathf.Deg2Rad;

            // calculate initial velocity
            float velocity = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY) / (Mathf.Sin(2 * angleRad) / gravity);

            // calculate velocity components
            float velocityX = Mathf.Sqrt(velocity) * Mathf.Cos(angleRad);
            float velocityY = Mathf.Sqrt(velocity) * Mathf.Sin(angleRad);

            // create and return velocity vector
            if (facePlayer)
            {
                return new Vector2(target.x < start.x ? -velocityX : velocityX, velocityY);
            }
            // get rand of -1 or 1
            int direction = Random.value < 0.5f ? -1 : 1;
            return new Vector2(direction * velocityX, velocityY);
        }
    }
}