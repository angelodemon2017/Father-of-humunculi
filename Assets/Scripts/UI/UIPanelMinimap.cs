using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelMinimap : MonoBehaviour
{
    public static UIPanelMinimap Instance;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image targetImage;
    [SerializeField] private float scale = 1;
    [SerializeField] private Button _buttonScaleAdd;
    [SerializeField] private Button _buttonScaleSub;
    [SerializeField] private float _distanceVisible;

    private float defauthRot = 45f;
    private Texture2D texture;
    private int textureWidth => (int)_rectTransform.sizeDelta.x;
    private int textureHeight => (int)_rectTransform.sizeDelta.y;

    private void Awake()
    {
        Instance = this;
        _buttonScaleAdd.onClick.AddListener(AddScale);
        _buttonScaleSub.onClick.AddListener(SubScale);
        CameraController.ChangedRot += RotatedCamera;
    }

    private void Start()
    {
        StartCoroutine(Init());
    }

    private void AddScale()
    {
        ChangeScale(true);
    }

    private void SubScale()
    {
        ChangeScale(false);
    }

    private void ChangeScale(bool isAdd)
    {
        scale += isAdd ? -0.1f : 0.1f;
        if (scale < 0.1f)
        {
            scale = 0.1f;
        }

        PaintMap();
    }

    public IEnumerator Init()
    {
        yield return new WaitForSeconds(0.1f);

        texture = new Texture2D(textureWidth, textureHeight);

        PaintMap();

        targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    private void PaintMap()
    {
        int textureWidth = texture.width;
        int textureHeight = texture.height;
        Color[] colors = new Color[textureWidth * textureHeight];

        Vector2 centerMap = new Vector2(textureWidth / 2, textureHeight / 2);
        Vector2 focusTileOffset = new Vector2(CameraController.Instance.FocusTile.x / scale - centerMap.x,
                                              CameraController.Instance.FocusTile.z / scale - centerMap.y);
        float distanceVisibleScaled = _distanceVisible / scale;

        for (int x = 0; x < textureWidth; x++)
        {
            for (int z = 0; z < textureHeight; z++)
            {
                float worldX = x + focusTileOffset.x;
                float worldZ = z + focusTileOffset.y;

                Color tempColor = Color.black;

                if (x == textureWidth / 2 && z == textureHeight / 2)
                {
                    tempColor = Color.white;
                }
                else
                {
                    int tileX = (int)(worldX * scale);
                    int tileZ = (int)(worldZ * scale);
                    var tile = GameProcess.Instance.GameWorld.GetWorldTileForMap(tileX, tileZ);

                    if (tile != null)
                    {
                        var txt = WorldViewer.Instance.GetTE(tile.Id);
                        tempColor = txt.BaseColor;

                        Vector2 currentPixel = new Vector2(x, z);
                        bool isNear = Vector2.Distance(centerMap, currentPixel) < distanceVisibleScaled;

                        if (!isNear)
                        {
                            tempColor.r *= tempColor.r;
                            tempColor.g *= tempColor.g;
                            tempColor.b *= tempColor.b;
                        }
                    }
                }

                colors[x + z * textureWidth] = tempColor;
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
    }

    private void RotatedCamera()
    {
        targetImage.rectTransform.rotation = Quaternion.Euler(0f, 0f, CameraController.Instance.CurAngl + defauthRot);
    }
}