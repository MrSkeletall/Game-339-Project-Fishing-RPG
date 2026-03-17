using Game339.Shared.Models;
using Game339.Shared.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class BadGuyHitItView : ObserverMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Button button;

        protected override void Subscribe()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        protected override void Unsubscribe()
        {
            button.onClick.RemoveListener(OnButtonClick);
        }

        private static void OnButtonClick()
        {
            var gameState = ServiceResolver.Resolve<GameState>();
            var damageService = ServiceResolver.Resolve<IDamageService>();
        
            var dmg = damageService.CalculateDamage(gameState.BadGuy, gameState.GoodGuy);
            damageService.ApplyDamage(gameState.GoodGuy, dmg);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.LogWarning("OnPointerEnter");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.LogWarning("OnPointerExit");
        }
    }
}
