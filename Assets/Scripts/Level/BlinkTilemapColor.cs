using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class BlinkTilemapColor : MonoBehaviour
{
	public float Interval;

	public Color Color1;
	public Color Color2;

	private Tilemap _tilemap;

	private float _nextStateChange;
	private bool _isColor1;

	private void Start()
	{
		_tilemap = GetComponent<Tilemap>();
		_tilemap.color = Color1;
		_isColor1 = true;
		_nextStateChange = Time.time + Interval;
	}

	private void Update()
	{
		if (Time.time > _nextStateChange)
		{
			_tilemap.color = _isColor1 ? Color2 : Color1;
			_isColor1 = !_isColor1;
			_nextStateChange += Interval;
		}
	}
}
