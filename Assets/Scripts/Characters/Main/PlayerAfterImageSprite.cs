using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField]
    private float _activeTime = 0.1f;
    private float _timeActivated;
    private float _alpha;

    [SerializeField] 
    private float _alphaSet = 0.8f;
    private float _alphaMultiplier = 0.85f;

    private Transform _player;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _playerSpriteRenderer;

    private Color _color;

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerSpriteRenderer = _player.GetComponentInChildren<SpriteRenderer>();

        _alpha = _alphaSet;
        _spriteRenderer.sprite = _playerSpriteRenderer.sprite;
        transform.position = _player.transform.position;
        transform.rotation = _player.transform.rotation;
        if (_player.transform.localScale.x > 0f) {
            transform.transform.localScale = new Vector3(0.35f, 0.35f, 1);
        }
        else
        {
            transform.transform.localScale = new Vector3(-0.35f, 0.35f, 1);
        }
        _timeActivated = Time.time;
    }

    private void Update()
    {
        _alpha *= _alphaMultiplier;
        _color = new Color(1f,1f, 1f, _alpha);
        _spriteRenderer.color = _color;

        if(Time.time > (_timeActivated+ _activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        } 
    }
}
