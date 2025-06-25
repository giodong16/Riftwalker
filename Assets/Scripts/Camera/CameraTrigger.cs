using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    public static CameraTrigger Instance;

    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlin;
    public float zoomSpeed = 3f;

    private Coroutine currenCoroutine;
    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    public void ShakeCamera(float intensity, float duration)
    {
        if (currenCoroutine != null) StopCoroutine(currenCoroutine);
        currenCoroutine = StartCoroutine(Shake(intensity, duration));
    }

    private IEnumerator Shake(float intensity, float duration)
    {
        perlin.m_AmplitudeGain = intensity;

        yield return new WaitForSeconds(duration);

        perlin.m_AmplitudeGain = 0f;
    }


    public void Zoom(float size = 5)
    {
        if (currenCoroutine != null) StopCoroutine(currenCoroutine);
        currenCoroutine = StartCoroutine(ZoomTo(size));
    }


    IEnumerator ZoomTo(float targetSize)
    {
        while (Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - targetSize) > 0.01f)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
                virtualCamera.m_Lens.OrthographicSize,
                targetSize,
                Time.deltaTime * zoomSpeed
            );
            yield return null;
        }
        virtualCamera.m_Lens.OrthographicSize = targetSize;
    }
}
