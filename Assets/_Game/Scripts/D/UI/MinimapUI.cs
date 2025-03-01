using LitMotion;
using UnityEngine;
using UnityEngine.UI;
namespace D
{
    public class MinimapUI : MonoBehaviour
    {
        [SerializeField] private Button zoomButton;
        [SerializeField] private RectTransform zoomMinimap;
        [SerializeField] private Camera cam;
        [SerializeField] private float duration = 0.5f;

        MotionHandle zoomMotion;
        MotionHandle moveMotion;
        MotionHandle zoomMinimapMotion;
        MotionHandle moveMinimapMotion;

        private void Start()
        {
            zoomButton.onClick.AddListener(OnClick);

            zoomMotion = LMotion.Create(0, 0, 0).RunWithoutBinding();
            moveMotion = LMotion.Create(0, 0, 0).RunWithoutBinding();
        }

        private bool isOpen = false;
        private void OnClick()
        {
            isOpen = !isOpen;
            if (isOpen)
            {
                if (GameManager.Instance.playerData.CurrentFloor % 10 == 0)
                return;
                Open();
            }
            else
            {
                Close();
            }
        }

        private void Open()
        {
            CancelMotion();
            Debug.Log("Open");
            Vector2Int gridSize = GameManager.Instance.GridSize();
            int maxSize = Mathf.Max(gridSize.x, gridSize.y);

            zoomMinimapMotion = LMotion.Create(zoomMinimap.sizeDelta, new Vector2(1000, 1000), duration)
                .WithEase(Ease.InCubic)
                .Bind(zoomMinimap, (x, t) => t.sizeDelta = x);

            //zoomMinimap.pivot = new Vector2(0.5f, 0.5f);

            moveMinimapMotion = LMotion.Create(zoomMinimap.localPosition, new Vector3(-500, 500, 0), duration)
                .WithEase(Ease.InCubic)
                .Bind(zoomMinimap, (x, t) => t.localPosition = x);

            zoomMotion = LMotion.Create(cam.orthographicSize, maxSize / 2, duration)
                .WithEase(Ease.InCubic)
                .Bind(cam, (x, cam) => cam.orthographicSize = x);
            //cam.orthographicSize = maxSize / 2;

            cam.transform.parent = null;

            Vector3 center = GameManager.Instance.MapCenter();
            center.z = -10;
            moveMotion = LMotion.Create(cam.transform.position, center, duration)
                .WithEase(Ease.InCubic)
                .Bind(cam, (x, t) => t.transform.position = x);
            //cam.transform.position = center;
        }

        private void Close()
        {
            CancelMotion();

            cam.transform.parent = Player.Instance.transform;

            zoomMinimapMotion = LMotion.Create(zoomMinimap.sizeDelta, new Vector2(256, 256), duration)
                .WithEase(Ease.InCubic)
                .Bind(zoomMinimap, (x, t) => t.sizeDelta = x);

            //zoomMinimap.pivot = new Vector2(0, 1);

            moveMinimapMotion = LMotion.Create(zoomMinimap.localPosition, new Vector3(-910, 440, 0), duration)
                .WithEase(Ease.InCubic)
                .Bind(zoomMinimap, (x, t) => t.localPosition = x);

            moveMotion = LMotion.Create(cam.transform.localPosition, new Vector3(0, 0, -10), 0.5f)
                .WithEase(Ease.InCubic)
                .Bind(cam, (x, t) => t.transform.localPosition = x);

            zoomMotion = LMotion.Create(cam.orthographicSize, 10, 0.5f)
                .WithEase(Ease.InCubic)
                .Bind(cam, (x, t) => t.orthographicSize = x);
            /*cam.orthographicSize = 10;
            cam.transform.localPosition = new Vector3(0, 0, -10);*/
        }

        private void CancelMotion()
        {
            if (moveMotion.IsPlaying()) moveMotion.Cancel();
            if (zoomMotion.IsPlaying()) zoomMotion.Cancel();
            if (zoomMinimapMotion.IsPlaying()) zoomMinimapMotion.Cancel();
            if (moveMinimapMotion.IsPlaying()) moveMinimapMotion.Cancel();
        }
    }
}
