using UnityEngine;

[CreateAssetMenu]
public class ArrowConfigSO : ScriptableObject
{
    [SerializeField] private ArrowPresenter _presenter;
    [SerializeField] private int _damage;

    public ArrowPresenter Presenter => _presenter;
    public int Damage => _damage;
}