using System;
using System.Linq;
using Archer.Data;
using Archer.Presenters;
using IJunior.TypedScenes;
using Lean.Localization;
using Archer.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Archer.UI
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Leaderboard _leaderboard;
        [SerializeField] private EquipmentListSO _equipmentsData;

        [Space] [SerializeField] private TMP_Text _textCoin;
        [SerializeField] private Button _startButton;

        [Space, Header("Equipment")] [SerializeField]
        private EquipmentBigIconView _equipmentBigIcon;

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
        [SerializeField] private Button _leaderboardButton;

        private EquipmentListView _equipmentListView;

        private WeaponDataConfig _currentWeaponData;
        private ArrowDataConfig _currentArrowData;

        public event Action<EquipmentDataConfig> EquipmentChanged;

        private void Awake()
        {
            _equipmentListView = GetComponent<EquipmentListView>();
        }

        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartGame);

            _equipmentListView.EquipmentSelected += OnShowBigIconEquipment;

            _selectWeaponButton.EquipmentSelected += OnOpenEquipmentsWindowShow;
            _selectArrowButton.EquipmentSelected += OnOpenEquipmentsWindowShow;

            _equipmentBigIcon.EquipmentSelected += OnEquipmentSelected;

            _equipmentBigIcon.Opening += OnEnabledUIElements;
            _menuSettingsWindowView.Opening += OnEnabledUIElements;
            _leaderboardView.Opening += OnEnabledUIElements;
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartGame);

            _equipmentListView.EquipmentSelected -= OnShowBigIconEquipment;

            _selectWeaponButton.EquipmentSelected -= OnOpenEquipmentsWindowShow;
            _selectArrowButton.EquipmentSelected -= OnOpenEquipmentsWindowShow;

            _equipmentBigIcon.EquipmentSelected -= OnEquipmentSelected;

            _equipmentBigIcon.Opening -= OnEnabledUIElements;
            _menuSettingsWindowView.Opening -= OnEnabledUIElements;
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

            EquipmentChanged?.Invoke(_currentWeaponData);
            EquipmentChanged?.Invoke(_currentArrowData);

            RenderEquipment(_currentWeaponData);
            RenderEquipment(_currentArrowData);
        }

        public void OnEnabledUIElements(bool enabled)
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
            _leaderboardButton.gameObject.SetActive(enabled);
        }

        private void RenderEquipment(EquipmentDataConfig equipmentData)
        {
            if (equipmentData is WeaponDataConfig)
                _selectWeaponButton.Render(equipmentData);

            else if (equipmentData is ArrowDataConfig)
                _selectArrowButton.Render(equipmentData);
        }

        private void OnStartGame()
        {
            if (PlayerData.Instance.TutorialIsComplete == false)
            {
                Tutorial.Load();
                return;
            }

            LevelSwitcher.LoadNextLevel();
        }

        private void OnOpenEquipmentsWindowShow(EquipmentDataConfig equipmentDataConfig)
        {
            if (equipmentDataConfig.Presenter is WeaponPresenter)
            {
                EnabledSelectedScrollView(_weaponScrollView, _arrowScrollView);
            }

            else if (equipmentDataConfig.Presenter is ArrowPresenter)
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

        private void OnShowBigIconEquipment(EquipmentDataConfig equipmentData)
        {
            _equipmentBigIcon.gameObject.SetActive(true);
            _equipmentBigIcon.Render(equipmentData);

            _arrowScrollView.SetActive(false);
        }

        private void OnEquipmentSelected(EquipmentDataConfig equipmentData)
        {
            if (equipmentData is WeaponDataConfig)
            {
                _currentWeaponData = equipmentData as WeaponDataConfig;
                PlayerData.Instance.CrossbowID = _currentWeaponData.ID;

                RenderSelectedEquipment(equipmentData, _weaponScrollView);
            }

            else if (equipmentData is ArrowDataConfig)
            {
                _currentArrowData = equipmentData as ArrowDataConfig;
                PlayerData.Instance.ArrowID = _currentArrowData.ID;

                RenderSelectedEquipment(equipmentData, _arrowScrollView);
            }
        }

        private void RenderSelectedEquipment(EquipmentDataConfig equipmentData, GameObject scrollView)
        {
            if (equipmentData.WasBought)
            {
                EquipmentChanged?.Invoke(equipmentData);

                RenderEquipment(equipmentData);

                scrollView.SetActive(false);
            }
        }
    }
}