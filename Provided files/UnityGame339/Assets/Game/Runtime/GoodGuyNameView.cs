using Game339.Shared;
using Game339.Shared.Models;
using TMPro;

namespace Game.Runtime
{
    using System;
    using UnityEngine;

    [Serializable]
    public class ObservableString : ObservableValue<string>, ISerializationCallbackReceiver
    {
        [SerializeField] private string initialValue;

        public void OnAfterDeserialize()  => Value = initialValue;
        public void OnBeforeSerialize()   => initialValue = Value;
    }
    
    public class GoodGuyNameView : ObserverMonoBehaviour
    {
        private static GameState GameState => ServiceResolver.Resolve<GameState>();

        public TextMeshProUGUI thisIsMyLabel;

        protected override void Subscribe() => GameState.GoodGuy.Name.ChangeEvent += OnGoodGuyNameChange;

        protected override void Unsubscribe() => GameState.GoodGuy.Name.ChangeEvent -= OnGoodGuyNameChange;

        private void OnGoodGuyNameChange(string newName)
        {
            thisIsMyLabel.text = newName;
        }
    }
}
