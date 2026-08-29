using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Combat.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class OnScreenMessage : MonoBehaviour
    {
        [SerializeField] private Image imageComponent;
        [SerializeField] private TMP_Text text;
        [SerializeField] private TMP_Text nameText;
        private CanvasGroup _canvasGroup;
        
        private static float _lifetime = 8;
        private static float _fadeTime = 2;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Set(LogData data)
        {
            if(!data.icon)
                imageComponent.enabled = false;
            
            text.text = data.message;
            nameText.text = data.title;
            imageComponent.sprite = data.icon;

            StartCoroutine(nameof(Fade));
        }

        private IEnumerator Fade()
        {
            yield return new WaitForSecondsRealtime(_lifetime);
            float alpha = 1;
            
            while (alpha >= 0)
            {
                yield return new WaitForEndOfFrame();
                alpha -= Time.deltaTime / _fadeTime;
                _canvasGroup.alpha = alpha;
            }

            Destroy(gameObject);
        }
    }
}
