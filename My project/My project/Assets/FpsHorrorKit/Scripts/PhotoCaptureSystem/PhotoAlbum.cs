namespace FpsHorrorKit
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PhotoAlbum", menuName = "ScriptableObjects/PhotoAlbum")]
    public class PhotoAlbum : ScriptableObject
    {
        public List<Sprite> photos = new List<Sprite>();

        // Fotoğraf ekler ve kaydeder
        public void AddPhoto(Sprite photo)
        {
            photos.Add(photo);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this); // ScriptableObject değişikliklerini işaretler
            UnityEditor.AssetDatabase.SaveAssets(); // Değişiklikleri kaydeder
#endif
        }
    }
}