using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GoogleARCore.Examples.HelloAR
{
    public class UIManager : MonoBehaviour
    {
        public Transform panelMain, panelBullet, panelGun;
        public RectTransform rectSettings;
        public InputField inputSpeed, inputAngle;
        public Image toggleArrow;
        private Vector3 localPos;
        public UnityEngine.Video.VideoClip videoClip;
        public RawImage renderTexture;
        public UnityEngine.Video.VideoPlayer videoPlayer;
        bool isShowing;

        private void Start()
        {
            isShowing = true;
        }
        public void ToggleSettings()
        {
            if (isShowing)
            {
                rectSettings.position = new Vector3(Screen.width / 2f, 0, 0);
                toggleArrow.transform.localScale = new Vector2(1, 1);
            }
            else
            {
                rectSettings.position = new Vector3(Screen.width / 2f, -Screen.height/4f, 0);
                toggleArrow.transform.localScale = new Vector2(1, -1);
            }
            isShowing = !isShowing;
        }

        public void Simulate()
        {
            rectSettings.position = new Vector3(Screen.width / 2f, -Screen.height/4f, 0);
            toggleArrow.transform.localScale = new Vector2(1, -1);
            isShowing = false;
            if (inputSpeed.text != "" && inputAngle.text != "")
                HelloARController.Instance.UpdateValues(float.Parse(inputSpeed.text), float.Parse(inputAngle.text));
        }

        public void OpenBulletPanel()
        {
            panelBullet.gameObject.SetActive(true);
        }
        public void OpenGunPanel()
        {
            panelGun.gameObject.SetActive(true);
        }
        public void CloseBulletPanel()
        {
            panelBullet.gameObject.SetActive(false);
        }
        public void CloseGunPanel()
        {
            panelGun.gameObject.SetActive(false);
        }
        public void IdentifyBullet()
        {
            videoPlayer = Camera.main.gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
            videoPlayer.playOnAwake = false;
            videoPlayer.clip = videoClip;
            videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
            //videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
            //videoPlayer.targetMaterialProperty = "_MainTex";
            StartCoroutine(IEPlayVideo());
        }
        private IEnumerator IEPlayVideo()
        {
            renderTexture.gameObject.SetActive(true);
            videoPlayer.Prepare();
            WaitForSeconds waitForSeconds = new WaitForSeconds(1);
            while (!videoPlayer.isPrepared)
            {
                yield return waitForSeconds;
                break;
            }
            renderTexture.texture = videoPlayer.texture;
            videoPlayer.Play();
            videoPlayer.loopPointReached += CloseVideo;
        }
        
        private void CloseVideo(UnityEngine.Video.VideoPlayer vp)
        {
            renderTexture.gameObject.SetActive(false);
        }
    }
}