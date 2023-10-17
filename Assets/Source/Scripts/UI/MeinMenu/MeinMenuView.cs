using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MeinMenuView : MonoBehaviour
{
    [SerializeField] private EquipmentListSO _itemsData;

    [Space]
    [SerializeField] private TMP_Text _textCoin;
    [SerializeField] private EquipmentBigIconView _equpmentBigIcon;
    [SerializeField] private EquipmentSmallIconView _currentArrowView;
    [SerializeField] private EquipmentSmallIconView _currentWeaponView;
    [SerializeField] private Button _startButton;
    [SerializeField] private GameObject _weaponScrollView;
    [SerializeField] private GameObject _arrowScrollView;
    [SerializeField] private Transform _arrowContainerScrollView;
    [SerializeField] private Transform _weaponContainerScrollView;

    private EquipmentListView _equipmentListView;

    private Config _config;

    private WeaponDataSO _currentWeaponData;
    private ArrowDataSO _currentArrowData;

    private int _weaponIndex = 0;
    private int _arrowIndex = 0;

    public event UnityAction<WeaponDataSO> WeaponChanged;
    public event UnityAction<ArrowDataSO> ArrowChanged;

    private void Awake()
    { 
        _equipmentListView = GetComponent<EquipmentListView>();
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(StartGame);

        _equipmentListView.EquipmentSelected += OnShowBigIconEquipment;

        _currentWeaponView.EquipmentSelected += OpenEquipmentsWindowShow;
        _currentArrowView.EquipmentSelected += OpenEquipmentsWindowShow;
        
        _equpmentBigIcon.EquipmentSelected += OnEquipmentSelected;
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(StartGame);

        _equipmentListView.EquipmentSelected -= OnShowBigIconEquipment;

        _currentWeaponView.EquipmentSelected -= OpenEquipmentsWindowShow;
        _currentArrowView.EquipmentSelected -= OpenEquipmentsWindowShow;

        _equpmentBigIcon.EquipmentSelected -= OnEquipmentSelected;
    }

    private void Start()
    {
        _equipmentListView.Render(_weaponContainerScrollView, _itemsData.WeaponsData);
        _equipmentListView.Render(_arrowContainerScrollView, _itemsData.ArrowsData);

        _currentWeaponData = _itemsData.WeaponsData[_weaponIndex];
        _currentArrowData = _itemsData.ArrowsData[_arrowIndex];

        WeaponChanged?.Invoke(_currentWeaponData);
        ArrowChanged?.Invoke(_currentArrowData);

        RenderEquimpemnt(_currentWeaponData);
        RenderEquimpemnt(_currentArrowData);
    }

    private void RenderEquimpemnt(WeaponDataSO weaponData)
    {
        _currentWeaponView.Render(weaponData);
    }
    private void RenderEquimpemnt(ArrowDataSO arrowData)
    {
        _currentArrowView.Render(arrowData);
    }

    private void StartGame()
    {
        _config = new Config(_currentWeaponData, _currentArrowData);

        SceneManager.LoadScene("Game");
    }

    private void OpenEquipmentsWindowShow(EquipmentDataSO equipmentDataSO)
    {
        if (equipmentDataSO.Presenter is WeaponPresenter) 
        {
            if (_weaponScrollView.gameObject.activeSelf == true)
            {
                _weaponScrollView.gameObject.SetActive(false);

                return;
            }

            if (_arrowScrollView.gameObject.activeSelf == true)
            {
                _arrowScrollView.gameObject.SetActive(false);
            }

            _weaponScrollView.gameObject.SetActive(true);
        }

        else if (equipmentDataSO.Presenter is ArrowPresenter)
        {
            if (_arrowScrollView.gameObject.activeSelf == true)
            {
                _arrowScrollView.gameObject.SetActive(false);

                return;
            }

            if (_weaponScrollView.gameObject.activeSelf == true)
            {
                _weaponScrollView.gameObject.SetActive(false);
            }

            _arrowScrollView.gameObject.SetActive(true);
        }
    }

    private void OnShowBigIconEquipment(EquipmentDataSO equipmentData)
    {
        _equpmentBigIcon.gameObject.SetActive(true);
        _equpmentBigIcon.Render(equipmentData);
    }

    private void OnEquipmentSelected(EquipmentDataSO equipmentData)
    {
        if (equipmentData is WeaponDataSO)
        {
            WeaponDataSO nextWeaponData = equipmentData as WeaponDataSO;

            if (nextWeaponData.WasBought)
            {
                WeaponChanged?.Invoke(nextWeaponData);

                RenderEquimpemnt(nextWeaponData);

                _weaponScrollView.gameObject.SetActive(false);
            }
        }

        else if (equipmentData is ArrowDataSO)
        {

            ArrowDataSO nextEquipmentData = equipmentData as ArrowDataSO;

            if (nextEquipmentData.WasBought)
            {
                ArrowChanged?.Invoke(nextEquipmentData);

                RenderEquimpemnt(nextEquipmentData);

                _arrowScrollView.gameObject.SetActive(false);
            }
        }
    }
}