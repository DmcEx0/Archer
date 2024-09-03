using UnityEngine;

namespace Archer.Presenters
{
    public interface IGeneratable
    {
        public Transform GeneratingPoint { get; }
    }
}