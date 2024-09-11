using Archer.Audio;
using Archer.Data;
using Archer.Presenters;
using Archer.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Archer.Utils
{
    public class PlayerConfigurationPresenter : MonoBehaviour
    {
        [SerializeField] private MainMenuView _mainMenuView;

        [Space] [SerializeField] private PlayerPresenter _playerPresenter;
        [SerializeField] private Transform _rightHand;
        [SerializeField] private Transform _endPointPosition;

        [Space] [SerializeField] private AudioDataConfig _audioData;
        [SerializeField] private AudioSource _sfxAudioSource;
        [SerializeField] private AudioSource _musicAudioSource;

        private WeaponDataConfig _currentWeaponData;
        private ArrowDataConfig _currentArrowData;

        private WeaponPresenter _currentWeaponTemplate;
        private ArrowPresenter _currentArrowTemplate;

        private Vector3 _startPlayerPosition;

        private void OnEnable()
        {
            _mainMenuView.EquipmentChanged += OnEquipmentSelected;
        }

        private void OnDisable()
        {
            _mainMenuView.EquipmentChanged -= OnEquipmentSelected;
        }

        private void Awake()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        YandexGamesSdk.GameReady();
#endif
        }

        private void Start()
        {
            _startPlayerPosition = _playerPresenter.transform.position;
            _playerPresenter.AnimationHandler.PlaySitIdle();

            _audioData.Init(_sfxAudioSource, _musicAudioSource);
            _audioData.Play(Sounds.MeinMenu);
        }

        private void Update()
        {
            if (_playerPresenter.AnimationHandler.IsTakenPosition == true)
                return;

            _playerPresenter.transform.position =
                _playerPresenter.AnimationHandler.GetTakenPosition(_startPlayerPosition, _endPointPosition.position,
                    Time.deltaTime);
        }

        private Presenter CreatePresenter(EquipmentDataConfig equipmentData, Transform spawnPoint)
        {
            return Instantiate(equipmentData.Presenter, spawnPoint.position, spawnPoint.rotation);
        }

        private void OnEquipmentSelected(EquipmentDataConfig equipmentData)
        {
            if (equipmentData is WeaponDataConfig)
                OnWeaponPresenterChanged(equipmentData as WeaponDataConfig);
            else if (equipmentData is ArrowDataConfig)
                OnArrowPresenterChanged(equipmentData as ArrowDataConfig);
        }

        private void DestroyCurrentPresenter(Presenter currentTemplate)
        {
            if (currentTemplate != null)
                Destroy(currentTemplate.gameObject);
        }

        private void OnWeaponPresenterChanged(WeaponDataConfig weaponData)
        {
            DestroyCurrentPresenter(_currentWeaponTemplate);

            _currentWeaponData = weaponData;
            _currentWeaponTemplate = CreatePresenter(_currentWeaponData, _rightHand) as WeaponPresenter;
            _currentWeaponTemplate.transform.SetParent(_rightHand);
        }

        private void OnArrowPresenterChanged(ArrowDataConfig arrowData)
        {
            DestroyCurrentPresenter(_currentArrowTemplate);

            _currentArrowData = arrowData;
            _currentArrowTemplate =
                CreatePresenter(_currentArrowData, _currentWeaponTemplate.ArrowSlot) as ArrowPresenter;
            _currentArrowTemplate.transform.SetParent(_rightHand);
        }
    }
}