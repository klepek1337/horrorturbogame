namespace FpsHorrorKit
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    public class ItemMethodReferences : MonoBehaviour
    {
        [Tooltip("Her bir item için özel bir event atanabilir.")]
        [SerializeField] private ItemEventPair[] itemEventPairs; // Item ve event çiftleri

        [Serializable]
        public class ItemEventPair
        {
            public Item item;
            public UnityEvent onUseItem;
        }

        private void OnEnable()
        {
            foreach (ItemEventPair pair in itemEventPairs)
            {
                if (pair.item != null && pair.onUseItem != null)
                {
                    pair.item.useItemAction += pair.onUseItem.Invoke;
                }
            }
        }

        private void OnDisable()
        {
            foreach (ItemEventPair pair in itemEventPairs)
            {
                if (pair.item != null && pair.onUseItem != null)
                {
                    pair.item.useItemAction -= pair.onUseItem.Invoke;
                }
            }
        }
    }
}