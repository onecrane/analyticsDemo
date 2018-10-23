using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class GlowObject : MonoBehaviour
{
	public Color GlowColor;
	public float LerpFactor = 10;

    public float playerRange = 3.0f;    // unity units
    public float fovRange = 15.0f;      // degrees
    private Transform player;

    public bool isFound = false;

	public Renderer[] Renderers
	{
		get;
		private set;
	}

	public Color CurrentColor
	{
		get { return _currentColor; }
	}

	private List<Material> _materials = new List<Material>();
	private Color _currentColor;
	private Color _targetColor;

	void Start()
	{
		Renderers = GetComponentsInChildren<Renderer>();

        foreach (var renderer in Renderers)
        {
            _materials.AddRange(renderer.materials);
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void ActivateGlow()
    {
        if (isFound) return;
        _targetColor = GlowColor;
        enabled = true;
    }

    private void DeactivateGlow()
    {
        _targetColor = Color.black;
        enabled = true;
    }

    /// <summary>
    /// Loop over all cached materials and update their color, disable self if we reach our target color.
    /// </summary>
    private void Update()
	{
		_currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * LerpFactor);

        if (_targetColor == GlowColor)  // Only true when the object is normally glowing - doesn't have to be active yet
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Object is found
                OnObjectFound();
            }
        }

        for (int i = 0; i < _materials.Count; i++)
		{
			_materials[i].SetColor("_GlowColor", _currentColor);
		}

		if (_currentColor.Equals(_targetColor))
		{
			enabled = false;
		}

        if (Vector3.Distance(player.position, transform.position) <= playerRange && Vector3.Angle(player.forward, transform.position - player.position) <= fovRange)
        {
            // Safe to call repeatedly since nothing is explicitly updated in the call
            ActivateGlow();
        }
        else
        {
            // As previous
            DeactivateGlow();
        }
	}

    private void OnObjectFound()
    {
        isFound = true;
        _currentColor = Color.black;
        DeactivateGlow();

    }
}
