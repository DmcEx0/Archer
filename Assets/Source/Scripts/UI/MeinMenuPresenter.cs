using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MeinMenuPresenter : MonoBehaviour
{
    [SerializeField] private PlayerPresenter _playerPresenter;
    [SerializeField] private ItemsDataSO _itemsData;

    [SerializeField] private Button _arrowButton;
    [SerializeField] private Button _weaponButton;
    [SerializeField] private Button _startButton;

    private Config _config;

    private List<WeaponPresenter> _weaponPresenters;
    private List<ArrowPresenter> _arrowPresenters;

    private WeaponPresenter _currentWeapon;
    private ArrowPresenter _currentArrow;

    private int _weaponIndex = 0;
    private int _arrowIndex = 0;

    private void Awake()
    {
        _weaponPresenters = new List<WeaponPresenter>();
        _arrowPresenters = new List<ArrowPresenter>();
    }

    private void OnEnable()
    {
        _weaponButton.onClick.AddListener(ChooseWeapon);
        _arrowButton.onClick.AddListener(ChooseArrow);
        _startButton.onClick.AddListener(StartGame);
    }

    private void Start()
    {
        foreach (var weaponConfig in _itemsData.WeaponsConfigs)
        {
            WeaponPresenter currentWeapon = Instantiate(weaponConfig.Presenter, _playerPresenter.SpawnPoint.position, _playerPresenter.SpawnPoint.rotation);
            currentWeapon.gameObject.SetActive(false);
            _weaponPresenters.Add(currentWeapon);
        }

        _currentWeapon = _weaponPresenters[_weaponIndex];
        _currentWeapon.gameObject.SetActive(true);

        foreach (var arrowConfig in _itemsData.ArrowsConfigs)
        {
            ArrowPresenter currentArrow = Instantiate(arrowConfig.Presenter, _currentWeapon.SpawnPoint.position, _currentWeapon.SpawnPoint.rotation);
            currentArrow.gameObject.SetActive(false);
            _arrowPresenters.Add(currentArrow);
        }

        _currentArrow = _arrowPresenters[_arrowIndex];
        _currentArrow.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _weaponButton.onClick.RemoveListener(ChooseWeapon);
        _arrowButton.onClick.RemoveListener(ChooseArrow);
        _startButton.onClick.RemoveListener(StartGame);
    }

    private void StartGame()
    {
        _config = new Config(_itemsData.WeaponsConfigs[_weaponIndex], _itemsData.ArrowsConfigs[_arrowIndex]);

        SceneManager.LoadScene("Game");
    }

    private void ChooseWeapon()
    {
        _currentWeapon.gameObject.SetActive(false);
        _weaponIndex++;
        if (_weaponIndex == _weaponPresenters.Count)
            _weaponIndex = 0;

        _currentWeapon = _weaponPresenters[_weaponIndex];
        _currentWeapon.gameObject.SetActive(true) ;
    }

    private void ChooseArrow()
    {
        _currentArrow.gameObject.SetActive(false);
        _arrowIndex++;

        if (_arrowIndex == _arrowPresenters.Count)
            _arrowIndex = 0;

        _currentArrow = _arrowPresenters[_arrowIndex];
        _currentArrow.gameObject.SetActive(true) ;
    }
}