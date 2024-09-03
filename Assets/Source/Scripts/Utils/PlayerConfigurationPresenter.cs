using Agava.YandexGames;
using UnityEngine;

public class PlayerConfigurationPresenter : MonoBehaviour
{
    [SerializeField] private MeinMenuView _meinMenuView;

    [Space]
    [SerializeField] private PlayerPresenter _playerPresenter;
    [SerializeField] private Transform _rightHand;
    [SerializeField] private Transform _endPointPosition;

    [Space]
    [SerializeField] private AudioDataSO _audioData;
    [SerializeField] private AudioSource _SFXAudioSource;
    [SerializeField] private AudioSource _musicAudioSource;

    private WeaponDataSO _currentWeaponData;
    private ArrowDataSO _currentArrowData;

    private WeaponPresenter _currentWeaponTemplate;
    private ArrowPresenter _currentArrowTemplate;

    private Vector3 _startPlayerPosition;

    private void OnEnable()
    {
        _meinMenuView.EquipmentChenged += OnEquipmentSelected;
    }

    private void OnDisable()
    {
        _meinMenuView.EquipmentChenged -= OnEquipmentSelected;
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
        _playerPresenter.AnimationController.PlaySitIdle();

        _audioData.Init(_SFXAudioSource, _musicAudioSource);
        _audioData.Play(Sounds.MeinMenu);
    }

    private void Update()
    {
        if (_playerPresenter.AnimationController.IsTakenPosition == true)
            return;

        _playerPresenter.transform.position = _playerPresenter.AnimationController.TakenPosition(_startPlayerPosition, _endPointPosition.position, Time.deltaTime);
    }

    private Presenter CreatePresenter(EquipmentDataSO equipmentData, Transform spawnPoint)
    {
        return Instantiate(equipmentData.Presenter, spawnPoint.position, spawnPoint.rotation);
    }

    private void OnEquipmentSelected(EquipmentDataSO equipmentData)
    {
        if (equipmentData is WeaponDataSO)
            OnWeaponPresenterChanged(equipmentData as WeaponDataSO);
        else if (equipmentData is ArrowDataSO)
            OnArrowPresenterChanged(equipmentData as ArrowDataSO);
    }

    private void DestroyCurrentPresenter(Presenter currentTemplate)
    {
        if (currentTemplate != null)
            Destroy(currentTemplate.gameObject);
    }

    private void OnWeaponPresenterChanged(WeaponDataSO weaponData)
    {
        DestroyCurrentPresenter(_currentWeaponTemplate);

        _currentWeaponData = weaponData;
        _currentWeaponTemplate = CreatePresenter(_currentWeaponData, _rightHand) as WeaponPresenter;
        _currentWeaponTemplate.transform.SetParent(_rightHand);
    }

    private void OnArrowPresenterChanged(ArrowDataSO arrowData)
    {
        DestroyCurrentPresenter(_currentArrowTemplate);

        _currentArrowData = arrowData;
        _currentArrowTemplate = CreatePresenter(_currentArrowData, _currentWeaponTemplate.ArrowSlot) as ArrowPresenter;
        _currentArrowTemplate.transform.SetParent(_rightHand);
    }
}