using System.Collections.Generic;
using System.Linq;
using Agava.YandexGames;
using Archer.Model;
using UnityEngine;

public class PresenterFactory : MonoBehaviour
{
    [SerializeField] private Presenter _playerTempalte;
    [SerializeField] private List<Presenter> _enemiesTemplates;

    private readonly int _poolCount = 30;

    private List<ObjectPool<Presenter>> _pools = new List<ObjectPool<Presenter>>();

    public Presenter CreatePlayer(Character player)
    {
        return CreatePresenter(_playerTempalte, player);
    }

    public Presenter CreateEnemy(Character enemy)
    {
        int randomEnemy = Random.Range(0, _enemiesTemplates.Count);

        return CreatePresenter(_enemiesTemplates[randomEnemy], enemy);
    }

    public Presenter CreateWeapon(Presenter weaponTemplate, Weapon weapon)
    {
        return CreatePresenter(weaponTemplate, weapon);
    }

    public Presenter CreateArrow(Presenter arrowPresenter, Arrow arrow)
    {
        return GetPresenterInPool(arrowPresenter, arrow);
    }

    public void CreatePoolOfPresenters(Presenter template)
    {
        if (_pools.Contains(_pools.FirstOrDefault(p => p.FirstElement == template)))
            return;

        ObjectPool<Presenter> pool = new ObjectPool<Presenter>(template, _poolCount, this.transform);
        _pools.Add(pool);
    }

    private Presenter CreatePresenter(Presenter template, Transformable model)
    {
        Presenter presenter = Instantiate(template, model.Position, model.Rotation);
        presenter.Init(model);

        return presenter;
    }

    private Presenter GetPresenterInPool(Presenter template, Transformable model)
    {
        ObjectPool<Presenter> currentPool = _pools.FirstOrDefault(pool => pool.FirstElement == template);
        Presenter presenter = currentPool.GetFreeElement();

        presenter.Init(model);

        return presenter;
    }
}