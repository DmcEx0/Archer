using IJunior.TypedScenes;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MeinMenuView : MonoBehaviour
{
    [SerializeField] private EquipmentListSO _itemsData;

    [Space]
    [SerializeField] private TMP_Text _textCoin;
    [SerializeField] private Button _startButton;

    [Space, Header("Equipment")]
    [SerializeField] private EquipmentBigIconView _equpmentBigIcon;
    [SerializeField] private EquipmentSmallIconView _selectArrowButton;
    [SerializeField] private EquipmentSmallIconView _selectWeaponButton;
    [SerializeField] private GameObject _weaponScrollView;
    [SerializeField] private GameObject _arrowScrollView;
    [SerializeField] private Transform _arrowContainerScrollView;
    [SerializeField] private Transform _weaponContainerScrollView;

    [Space, Header("Settings")]
    [SerializeField] private Button _settingsButton;
    [SerializeField] private SettingsWindowView _settingsWindowView;

    private EquipmentListView _equipmentListView;

    private Config _config;
    private LevelManager _levelManager;

    private WeaponDataSO _currentWeaponData;
    private ArrowDataSO _currentArrowData;

    private int _weaponIndex = 0;
    private int _arrowIndex = 0;

    public event UnityAction<EquipmentDataSO> EquipmentChenged;

    public SettingsWindowView SettingsWindowView => _settingsWindowView;

    private void Awake()
    {
        _equipmentListView = GetComponent<EquipmentListView>();
        _levelManager = new();
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(StartGame);
        _settingsButton.onClick.AddListener(OnShowSettnigsWindow);

        _equipmentListView.EquipmentSelected += OnShowBigIconEquipment;

        _selectWeaponButton.EquipmentSelected += OpenEquipmentsWindowShow;
        _selectArrowButton.EquipmentSelected += OpenEquipmentsWindowShow;

        _equpmentBigIcon.EquipmentSelected += OnEquipmentSelected;

        _equpmentBigIcon.WindowClose += IsEnabledUIElements;
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(StartGame);
        _settingsButton.onClick.RemoveListener(OnShowSettnigsWindow);

        _equipmentListView.EquipmentSelected -= OnShowBigIconEquipment;

        _selectWeaponButton.EquipmentSelected -= OpenEquipmentsWindowShow;
        _selectArrowButton.EquipmentSelected -= OpenEquipmentsWindowShow;

        _equpmentBigIcon.EquipmentSelected -= OnEquipmentSelected;

        _equpmentBigIcon.WindowClose -= IsEnabledUIElements;
    }

    private void Start()
    {
        _equipmentListView.Render(_weaponContainerScrollView, _itemsData.WeaponsData);
        _equipmentListView.Render(_arrowContainerScrollView, _itemsData.ArrowsData);

        //_currentWeaponData = _itemsData.WeaponsData[_weaponIndex];
        _currentWeaponData = _itemsData.WeaponsData.FirstOrDefault(w => w.ID == PlayerData.Instance.CrossbowID);

        //_currentArrowData = _itemsData.ArrowsData[_arrowIndex];
        _currentArrowData = _itemsData.ArrowsData.FirstOrDefault(a => a.ID == PlayerData.Instance.ArrowID);

        EquipmentChenged?.Invoke(_currentWeaponData);
        EquipmentChenged?.Invoke(_currentArrowData);

        RenderEquimpemnt(_currentWeaponData);
        RenderEquimpemnt(_currentArrowData);
    }

    private void RenderEquimpemnt(EquipmentDataSO equipmentData)
    {
        if (equipmentData is WeaponDataSO)
            _selectWeaponButton.Render(equipmentData);

        else if (equipmentData is ArrowDataSO)
            _selectArrowButton.Render(equipmentData);
    }

    private void StartGame()
    {
        _config = new Config(_currentWeaponData, _currentArrowData);

        //_levelManager.LoadNextLevel();
        Level1.Load();
    }

    private void OnShowSettnigsWindow()
    {
        _settingsWindowView.gameObject.SetActive(true);
    }

    private void OpenEquipmentsWindowShow(EquipmentDataSO equipmentDataSO)
    {
        if (equipmentDataSO.Presenter is WeaponPresenter)
        {
            EnabledSelectedScrollView(_weaponScrollView, _arrowScrollView);
        }

        else if (equipmentDataSO.Presenter is ArrowPresenter)
        {
            EnabledSelectedScrollView(_arrowScrollView, _weaponScrollView);
        }
    }

    private void EnabledSelectedScrollView(GameObject selectedScrollView, GameObject secondScrollView)
    {
        if (selectedScrollView.activeSelf == true)
        {
            selectedScrollView.SetActive(false);

            return;
        }

        if (secondScrollView.activeSelf == true)
        {
            secondScrollView.SetActive(false);
        }

        selectedScrollView.SetActive(true);
    }

    private void OnShowBigIconEquipment(EquipmentDataSO equipmentData)
    {
        _equpmentBigIcon.gameObject.SetActive(true);
        _equpmentBigIcon.Render(equipmentData);

        _arrowScrollView.SetActive(false);

        IsEnabledUIElements(false);
    }

    private void IsEnabledUIElements(bool enabled)
    {
        if (enabled == false)
        {
            _arrowScrollView.gameObject.SetActive(enabled);
            _weaponScrollView.gameObject.SetActive(enabled);
        }

        _selectArrowButton.gameObject.SetActive(enabled);
        _selectWeaponButton.gameObject.SetActive(enabled);

        _startButton.gameObject.SetActive(enabled);
    }

    private void OnEquipmentSelected(EquipmentDataSO equipmentData)
    {
        if (equipmentData is WeaponDataSO)
        {
            _currentWeaponData = equipmentData as WeaponDataSO;
            PlayerData.Instance.CrossbowID = _currentWeaponData.ID;

            RenderSelectedEquipment(equipmentData, _weaponScrollView);
        }

        else if (equipmentData is ArrowDataSO)
        {
            _currentArrowData = equipmentData as ArrowDataSO;
            PlayerData.Instance.ArrowID = _currentArrowData.ID;

            RenderSelectedEquipment(equipmentData, _arrowScrollView);
        }
    }

    private void RenderSelectedEquipment(EquipmentDataSO equipmentData, GameObject scrollView)
    {
        if (equipmentData.WasBought)
        {
            EquipmentChenged?.Invoke(equipmentData);

            RenderEquimpemnt(equipmentData);

            scrollView.SetActive(false);
        }
    }
}