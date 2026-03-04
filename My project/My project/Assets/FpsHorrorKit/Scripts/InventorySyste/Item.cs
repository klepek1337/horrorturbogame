namespace FpsHorrorKit
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        [Header("Item Properties")]
        public string itemName;
        public float energyLevel; // fener için pil yüzdesi, silah için şarjordeki mermi sayısı gibi değerler için
        public Sprite icon;
        public bool isStackable;
        public int maxStackSize;

        [Header("Bool Control")]
        public bool hasItem;
        public bool canUseItem;
        public bool isUsingItem;
        public bool isEnergyEnough;
        public Action useItemAction;
    }
}
