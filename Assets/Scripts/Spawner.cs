using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LalaFight
{
    public partial class Spawner : MonoBehaviour
    {
        //[Header("Player Prefab")]
        //[SerializeField] private Transform _playerPrefab = null;

        [Header("MapGenerator")]
        [SerializeField] private MapGenerator _mapGenerator = null;

        [Header("Enemy Prefabs")]
        [SerializeField] private Enemy _lightEnemyPrefab = null;

        [Header("Waves properties")]
        [SerializeField] private List<Wave> _waves = new List<Wave>();

        private Transform _playerTransform = null;

        private int _currentWaveIndex = -1;
        private int _enemyRemainingToSpawn;
        private int _enemyRemainingAlive;
        private float _nextSpawnTime;
        private bool _isOnNextWave = false;

        private float _timebetweenCamping = 2f;
        private float _nextCampingTime = 0f;
        private float _campingThreshold = 1.5f;
        private bool _isCamping = false;
        private Vector3 _oldPosition;

        public event System.Action OnWaveEnd;
        public event System.Action OnNextWave;
        public event System.Action<int> OnNextWaveTimer;

        private void Awake()
        {
            _playerTransform = FindObjectOfType<PlayerController>()?.transform;
        }

        private void OnEnable()
        {
            if (_playerTransform == null)
                gameObject.SetActive(false);

            _mapGenerator.OnValidPlayerPosition += RespawnPlayer;
            OnNextWave += _mapGenerator.BeginNewMapGeneration;


            _nextCampingTime = Time.time + _timebetweenCamping;
            _oldPosition = _playerTransform.position;

            if (_waves.Count > 0)
                NextWave();
        }

        private void OnDisable()
        {
            _mapGenerator.OnValidPlayerPosition -= RespawnPlayer;
        }

        private void Update()
        {
            if (_playerTransform == null || _isOnNextWave == true)
                return;
            CheckPlayerIsCamping();
            ManageEnemySpawn();
        }

        private void CheckPlayerIsCamping()
        {
            if (Time.time > _nextCampingTime)
            {
                _nextCampingTime = Time.time + _timebetweenCamping;
                _isCamping = (_playerTransform.position - _oldPosition).sqrMagnitude <= Mathf.Pow(_campingThreshold, 2);
                _oldPosition = _playerTransform.position;
            }
        }

        private void ManageEnemySpawn()
        {
            if (_waves.Count <= 0 || _currentWaveIndex == _waves.Count)
                return;

            if (Time.time > _nextSpawnTime && (_enemyRemainingToSpawn > 0 || _waves[_currentWaveIndex].infinite))
            {
                --_enemyRemainingToSpawn;
                _nextSpawnTime = Time.time + _waves[_currentWaveIndex].timeBetweenSpawn;
                StartCoroutine(InstantiateEnemy());
                return;
            }

            if (_enemyRemainingAlive == 0 && _isOnNextWave == false && _waves[_currentWaveIndex].infinite == false)
                NextWave();
        }

        private IEnumerator InstantiateEnemy()
        {
            float delayTime = _waves[_currentWaveIndex].timeBetweenSpawn;
            float tileFlashSpeed = 4f;

            Transform tileTransform;
            if (_isCamping)
                tileTransform = _mapGenerator.GetTileFromPosition(_playerTransform.position);
            else
                tileTransform = _mapGenerator.GetRandomOpenTile();

            Material tileMaterial = tileTransform.GetComponent<Renderer>().material;
            Color originalColor = tileMaterial.color;
            Color flashColor = Color.red;

            float spawnTime = 0f;

            while (spawnTime < delayTime)
            {
                tileMaterial.color = Color.Lerp(originalColor, flashColor, Mathf.PingPong(spawnTime * tileFlashSpeed, 1f));
                spawnTime += Time.deltaTime;
                yield return null;
            }

            var enemy = Instantiate(_lightEnemyPrefab, tileTransform.position + Vector3.up, Quaternion.identity);
            enemy.SetTarget(_playerTransform);
            enemy.GetComponent<HealthController>().OnDeath += OnEnemyDeath;
            enemy.SetProperties(_waves[_currentWaveIndex]);
        }

        private void NextWave()
        {
            ++_currentWaveIndex;
            if (_currentWaveIndex == _waves.Count)
            {
                print("*** DEFEATED ALL OF ENEMIES ***");
                OnWaveEnd?.Invoke();
                return;
            }

            StartCoroutine(NextWaveTimer());
        }

        private IEnumerator NextWaveTimer()
        {
            _isOnNextWave = true;

            var waitObject = new WaitForSeconds(1);
            int timer = _currentWaveIndex * 5;
            timer = Mathf.Clamp(timer, 0, 3);

            while (timer > 0)
            {
                OnNextWaveTimer?.Invoke(timer);
                yield return waitObject;
                --timer;
            }

            _enemyRemainingToSpawn = _waves[_currentWaveIndex].enemyAmount;
            _enemyRemainingAlive = _enemyRemainingToSpawn;
            _isOnNextWave = false;
            OnNextWave?.Invoke();
        }

        private void RespawnPlayer(Vector3 positionToRespawn)
        {
            _playerTransform.transform.position = positionToRespawn + Vector3.up;
        }

        private void OnEnemyDeath()
        {
            --_enemyRemainingAlive;
        }
    }
}