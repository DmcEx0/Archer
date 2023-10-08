using Archer.Model;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private ItemsDataSO _itemsData;
    [SerializeField] private PresenterFactory _factory;
    [SerializeField] Transform _playerPosition;
    [SerializeField] Transform _ememyPosition;

    [SerializeField] private LineRenderer _lineRenderer;

    private PlayerSpawner _playerSpawner;
    private EnemySpawner _enemySpawner;

    private Health _playerHealth;
    private Health _enemyHealth;

    private void Awake()
    {
        _playerSpawner = new PlayerSpawner(_factory);
        _enemySpawner = new EnemySpawner(_playerPosition.position, _factory);

        _playerHealth = new Health(200);
        _enemyHealth = new Health(100);
    }

    private void Start()
    {
        ArrowConfigSO arrowConfig = Config.Instance.ArrowConfig;
        WeaponConfigSO weaponConfig = Config.Instance.WeaponConfig;

        int randomIndexWeaponConfig = Random.Range(0, _itemsData.WeaponsConfigs.Count);
        int randomIndexArrowConfig = Random.Range(0, _itemsData.ArrowsConfigs.Count);

        _playerSpawner.Spawn(_playerHealth, weaponConfig, arrowConfig, _playerPosition);
        _factory.CreatePoolOfPresenters(arrowConfig.Presenter);

        _enemySpawner.Spawn(_enemyHealth, _itemsData.WeaponsConfigs[randomIndexWeaponConfig], _itemsData.ArrowsConfigs[randomIndexArrowConfig], _ememyPosition);
        _factory.CreatePoolOfPresenters(_itemsData.ArrowsConfigs[randomIndexArrowConfig].Presenter);

        HealthBar playerHealthBar = _playerSpawner.CharacterTemplate.GetComponentInChildren<HealthBar>();
        HealthBar enemyHealthBar = _enemySpawner.CharacterTemplate.GetComponentInChildren<HealthBar>();

        playerHealthBar.Init(_playerHealth);
        enemyHealthBar.Init(_enemyHealth);
    }

    private void Update()
    {
        _playerSpawner.Update(Time.deltaTime);
        _enemySpawner.Update(Time.deltaTime);
    }
}