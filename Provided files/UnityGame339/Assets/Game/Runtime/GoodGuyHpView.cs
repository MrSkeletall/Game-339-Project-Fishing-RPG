using Game339.Shared;
using Game339.Shared.Models;
using TMPro;

namespace Game.Runtime
{
    using System;
    using UnityEngine;

    [Serializable]
    public class ObservableInt : ObservableValue<int>, ISerializationCallbackReceiver
    {
        [SerializeField] private int initialValue;

        public void OnAfterDeserialize()  => Value = initialValue;
        public void OnBeforeSerialize()   => initialValue = Value;
    }

    public class GoodGuyHpView : ObserverMonoBehaviour
    {
        private static GameState GameState => ServiceResolver.Resolve<GameState>();

        public TextMeshProUGUI thisIsMyLabel;

        public bool useAlternativeMessage;

        protected override void Subscribe() => GameState.GoodGuy.Health.ChangeEvent += OnGoodGuyHealthChange;

        protected override void Unsubscribe() => GameState.GoodGuy.Health.ChangeEvent -= OnGoodGuyHealthChange;

        private void OnGoodGuyHealthChange(int health)
        {
            thisIsMyLabel.text = useAlternativeMessage
                ? "WAT?? -" + health
                : "Health: " + health;
        }
    }
}
