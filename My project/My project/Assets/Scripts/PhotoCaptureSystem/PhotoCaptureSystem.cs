namespace FpsHorrorKit
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class PhotoCaptureSystem : MonoBehaviour
    {
        public static PhotoCaptureSystem Instance { get; private set; }

        [Header("Inventory Item")]
        public Item photoCameraItem;

        [Header("Camera Settings")]
        public Camera photoCamera; // Fotoğraf çeken kamera
        public RenderTexture renderTexture; // Fotoğrafın çekileceği RenderTexture
        public PhotoAlbum photoAlbum; // Fotoğrafları depolayacağımız ScriptableObject

        [Header("UI Elements")]
        public GameObject photoUIPanel; // Fotoğrafın göründüğü panel
        public Image displayImage; // Fotoğrafın gösterildiği UI Image elementi
        public Image currentDisplayImage; // Fotoğrafı ilk çekişte gösterilecek UI Image elementi
        public Button nextPhotoButton; // Bir sonraki fotoğrafı göstermek için kullanılacak buton
        public Button previousPhotoButton; // Bir onceki fotoğrafı göstermek için kullanılacak buton

        private FpsController _fpsController;

        private int photoIndex = 0; // Fotoğrafın sırasını tutacak bir degisken
        private bool isShowPhoto = false;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            _fpsController = FindAnyObjectByType<FpsController>();
        }
        private void Start()
        {
            photoAlbum.photos.Clear();
            nextPhotoButton.onClick.AddListener(() => ShowPhoto(photoIndex + 1, true));
            previousPhotoButton.onClick.AddListener(() => ShowPhoto(photoIndex - 1, true));
        }
        private void Update()
        {
            if (photoCameraItem.hasItem && photoCameraItem.canUseItem)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    OpenAlbum();
                }
            }
        }
        public void OpenAlbum()
        {
            isShowPhoto = !isShowPhoto;
            _fpsController.isInteracting = isShowPhoto;
            ItemUsageSystem.Instance.isAlbumActive = isShowPhoto;

            if (isShowPhoto)
            {
                InteractCameraSettings.Instance.Interacting();
                InteractCameraSettings.Instance.ShowCursor();
                ItemUsageSystem.Instance.cameraFrameUI.SetActive(false);
            }
            else
            {
                InteractCameraSettings.Instance.NotInteracting();
                InteractCameraSettings.Instance.HideCursor();
                ItemUsageSystem.Instance.cameraFrameUI.SetActive(true);
            }
            ShowPhoto(0, isShowPhoto);
        }

        public void CapturePhoto()
        {
            // Fotoğraf çekme işlemi
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = renderTexture;

            photoCamera.targetTexture = renderTexture;
            photoCamera.Render();

            Texture2D photo = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            photo.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            photo.Apply();

            RenderTexture.active = currentRT;
            photoCamera.targetTexture = null;

            // Fotoğrafı Sprite olarak dönüştür
            Sprite photoSprite = Sprite.Create(photo, new Rect(0, 0, photo.width, photo.height), new Vector2(0.5f, 0.5f));

            // Fotoğrafı ScriptableObject'e kaydet
            photoAlbum.AddPhoto(photoSprite);

            // UI Image elementinde göster
            if (currentDisplayImage != null)
            {
                currentDisplayImage.gameObject.SetActive(true);
                currentDisplayImage.sprite = photoSprite;
                StartCoroutine(DelayedShowPhoto(0.5f));
            }
        }

        public void ShowPhoto(int index, bool isShow)
        {
            photoUIPanel.gameObject.SetActive(isShow);
            if (index >= 0 && index < photoAlbum.photos.Count)
            {
                displayImage.sprite = photoAlbum.photos[index];
                photoIndex = index;
            }
        }
        IEnumerator DelayedShowPhoto(float delay)
        {
            yield return new WaitForSeconds(delay);
            currentDisplayImage.gameObject.SetActive(false);
        }
    }
}