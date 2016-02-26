﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DomeMaster : MonoBehaviour {
	public string shaderProperty = "_GlobalDome";
	public TextureEvent OnUpdateCubemap;

	public int lod = 10;

	Camera _attachedCam;
	RenderTexture _cube;

	void OnEnable() {
		_attachedCam = GetComponent<Camera> ();
	}
	void Update() {
		var res = (1 << Mathf.Clamp(lod, 1, 13));
		if (_cube == null || _cube.width != res) {
			Debug.LogFormat ("Create Cubemap {0}x{1}", res, res);
			DestroyImmediate (_cube);
			_cube = new RenderTexture (res, res, 24);
			_cube.filterMode = FilterMode.Bilinear;
			_cube.isCubemap = true;
			_cube.generateMips = false;
			_cube.Create ();
			OnUpdateCubemap.Invoke (_cube);
		}
		//_attachedCam.CopyFrom (Camera.main);
		//_attachedCam.depth -= 1;
		_attachedCam.enabled = false;
		_attachedCam.RenderToCubemap (_cube);
		Shader.SetGlobalTexture (shaderProperty, _cube);
	}

	[System.Serializable]
	public class TextureEvent : UnityEngine.Events.UnityEvent<Texture> {}
}