using System.Linq;
using Archer.Data;
using Archer.Presenters;
using IJunior.TypedScenes;
using Lean.Localization;
using Archer.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Archer.UI
{
    public class MeinMenuView : MonoBehaviour
    {
        [SerializeField] private Leaderboard _leaderboard;
        [SerializeField] private EquipmentListSO _equipmentsData;

        [Space] [SerializeField] private TMP_Text _textCoin;
        [SerializeField] private Button _startButton;

        [Space, Header("Equipment")] [SerializeField]
        private EquipmentBigIconView _equpmentBigIcon;

        [SerializeField] private EquipmentSmallIconView _selectArrowButton;
        [SerializeField] private EquipmentSmallIconView _selectWeaponButton;
        [SerializeField] private GameObject _weaponScrollView;
        [SerializeField] private GameObject _arrowScrollView;
        [SerializeField] private Transform _arrowContainerScrollView;
        [SerializeField] private Transform _weaponContainerScrollView;

        [Space, Header("Settings")] [SerializeField]
        private Button _settingsButton;

        [SerializeField] private SettingsWindowView _menuSettingsWindowView;

        [Space] [SerializeField] private LeaderboardView _leaderboardView;
        [SerializeField] private Button _leadernoardButton;

        private EquipmentListView _equipmentListView;

        private WeaponDataSO _currentWeaponData;
        private ArrowDataSO _currentArrowData;

        public event UnityAction<EquipmentDataSO> EquipmentChenged;

        private void Awake()
        {
            _equipmentListView = GetComponent<EquipmentListView>();
        }

        private void OnEnable()
        {
            _startButton.onClick.AddListener(StartGame);

            _equipmentListView.EquipmentSelected += OnShowBigIconEquipment;

            _selectWeaponButton.EquipmentSelected += OpenEquipmentsWindowShow;
            _selectArrowButton.EquipmentSelected += OpenEquipmentsWindowShow;

            _equpmentBigIcon.EquipmentSelected += OnEquipmentSelected;

            _equpmentBigIcon.OnOpened += EnabledUIElements;
            _menuSettingsWindowView.OnOpened += EnabledUIElements;
            _leaderboardView.OnOpened += EnabledUIElements;
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(StartGame);

            _equipmentListView.EquipmentSelected -= OnShowBigIconEquipment;

            _selectWeaponButton.EquipmentSelected -= OpenEquipmentsWindowShow;
            _selectArrowButton.EquipmentSelected -= OpenEquipmentsWindowShow;

            _equpmentBigIcon.EquipmentSelected -= OnEquipmentSelected;

            _equpmentBigIcon.OnOpened -= EnabledUIElements;
            _menuSettingsWindowView.OnOpened -= EnabledUIElements;
        }

        private void Start()
        {
            LeanLocalization.SetCurrentLanguageAll(PlayerData.Instance.CurrentLanguage);

#if UNITY_WEBGL && !UNITY_EDITOR
        _leaderboard.SetPlayerScore();
#endif
            _equipmentListView.Render(_weaponContainerScrollView, _equipmentsData.WeaponsData);
            _equipmentListView.Render(_arrowContainerScrollView, _equipmentsData.ArrowsData);

            _currentWeaponData =
                _equipmentsData.WeaponsData.FirstOrDefault(w => w.ID == PlayerData.Instance.CrossbowID);

            _currentArrowData = _equipmentsData.ArrowsData.FirstOrDefault(a => a.ID == PlayerData.Instance.ArrowID);

            EquipmentChenged?.Invoke(_currentWeaponData);
            EquipmentChenged?.Invoke(_currentArrowData);

            RenderEquimpemnt(_currentWeaponData);
            RenderEquimpemnt(_currentArrowData);
        }

        public void EnabledUIElements(bool enabled)
        {
            if (enabled == false)
            {
                _arrowScrollView.gameObject.SetActive(enabled);
                _weaponScrollView.gameObject.SetActive(enabled);
            }

            _selectArrowButton.gameObject.SetActive(enabled);
            _selectWeaponButton.gameObject.SetActive(enabled);

            _startButton.gameObject.SetActive(enabled);
            _settingsButton.gameObject.SetActive(enabled);
            _leadernoardButton.gameObject.SetActive(enabled);
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
            if (PlayerData.Instance.TutorialIsComplete == false)
            {
                Tutorial.Load();
                return;
            }

            LevelManager.LoadNextLevel();
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
}