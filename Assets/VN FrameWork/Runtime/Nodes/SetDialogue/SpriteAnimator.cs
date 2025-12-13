// 12/13/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;
using System.Collections;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector
{
    public class SpriteAnimator : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private Vector3 originalPosition;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            originalPosition = transform.position;
        }

        public void PlayEffect(SetDialogueRuntimeNode.SpriteEffect effect, float speed)
        {
            switch (effect)
            {
                case SetDialogueRuntimeNode.SpriteEffect.FadeIn:
                    StartCoroutine(Fade(true, speed));
                    break;
                case SetDialogueRuntimeNode.SpriteEffect.FadeOut:
                    StartCoroutine(Fade(false, speed));
                    break;
                case SetDialogueRuntimeNode.SpriteEffect.Jump:
                    StartCoroutine(Jump(speed));
                    break;
                case SetDialogueRuntimeNode.SpriteEffect.Shake:
                    StartCoroutine(Shake(speed));
                    break;
                case SetDialogueRuntimeNode.SpriteEffect.ExitLeft:
                    StartCoroutine(MoveOut(true, speed));
                    break;
                case SetDialogueRuntimeNode.SpriteEffect.ExitRight:
                    StartCoroutine(MoveOut(false, speed));
                    break;
                case SetDialogueRuntimeNode.SpriteEffect.SlideDown:
                    StartCoroutine(SlideDown(speed));
                    break;
                case SetDialogueRuntimeNode.SpriteEffect.Bop:
                    StartCoroutine(Bop(speed));
                    break;
            }
        }

        private IEnumerator Fade(bool fadeIn, float speed)
        {
            float targetAlpha = fadeIn ? 1 : 0;
            canvasGroup.alpha = 1 - targetAlpha;
            while (Mathf.Abs(canvasGroup.alpha - targetAlpha) > 0.01f)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime * speed);
                yield return null;
            }
        }

        private IEnumerator Jump(float speed)
        {
            float jumpHeight = 50f;
            for (float i = 0; i < 1; i += Time.deltaTime * speed)
            {
                transform.position = originalPosition + new Vector3(0, Mathf.Sin(i * Mathf.PI) * jumpHeight, 0);
                yield return null;
            }
            transform.position = originalPosition;
        }

        private IEnumerator Shake(float speed)
        {
            float shakeAmount = 10f;
            for (float i = 0; i < 1; i += Time.deltaTime * speed)
            {
                transform.position = originalPosition + new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
                yield return null;
            }
            transform.position = originalPosition;
        }

        private IEnumerator MoveOut(bool left, float speed)
        {
            float exitDistance = Screen.width / 2 + 200;
            float direction = left ? -1 : 1;
            Vector3 targetPosition = originalPosition + new Vector3(exitDistance * direction, 0, 0);
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed * 1000);
                yield return null;
            }
        }

        private IEnumerator SlideDown(float speed)
        {
            float slideDistance = Screen.height / 2 + 200;
            Vector3 targetPosition = originalPosition - new Vector3(0, slideDistance, 0);
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed * 1000);
                yield return null;
            }
        }

        private IEnumerator Bop(float speed)
        {
            float bopHeight = 20f;
            Vector3 downPosition = originalPosition - new Vector3(0, bopHeight, 0);
            float duration = 0.2f / speed;

            float timer = 0;
            while (timer < duration)
            {
                transform.position = Vector3.Lerp(originalPosition, downPosition, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            timer = 0;
            while (timer < duration)
            {
                transform.position = Vector3.Lerp(downPosition, originalPosition, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            transform.position = originalPosition;
        }
    }
}