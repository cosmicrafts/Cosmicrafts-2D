using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleColorControl : MonoBehaviour {

	SpriteRenderer[] sprites;
	Image[] images;

	public float animSpeedR = 3f;
	public float animSpeedG = 4f;
	public float animSpeedB = 5f;
	public float animSpeedA = 6f;

	public Sprite[] textures;
	public Sprite[] texturesRGB;
	public Sprite[] texturesALPHA;

	public Image texPreviewRGB;
	public Image texPreviewALPHA;
    public Image colorPreview;

	int texIndex;

	float r,g,b,a;

	// Use this for initialization
	void Start () {
		sprites = GetComponentsInChildren<SpriteRenderer>();
		images = GetComponentsInChildren<Image>();
		r = 0.5f;
		g = 0.5f;
		b = 0.5f;
		a = 1f;
		texIndex = 0;
		SetTexture();
	}

	void Update () {
		//r =  GetSinValue(animSpeedR);
		//g =  GetSinValue(animSpeedG);
		//b =  GetSinValue(animSpeedB);
		//a =  GetSinValue(animSpeedA);
		SetColor(new Color(r,g,b,a));
	}

	float GetSinValue(float speed) {
		return 0.5f + 0.5f * Mathf.Sin(Time.time * speed);
	}

	public void SetHue(float value) {
		r = value;
	}

	public void SetSaturation(float value) {
		g = value;
	}

	public void SetValue(float value) {
		b = value;
	}

	public void SetAlpha(float value) {
		a = value;
	}

	public void ChangeTexture(int change) {
		texIndex += change;

		if (texIndex >= textures.Length) {
			texIndex = 0;
		}
		else if (texIndex < 0) {
			texIndex = textures.Length -1;
		}

		SetTexture();
	}


	void SetColor(Color color) {
		for (int i = 0; i < sprites.Length; i++) 
			sprites[i].color = color;

		for (int i = 0; i < images.Length; i++) 
			images[i].color = color;

        colorPreview.color = color;
    }

	void SetTexture() {
		var texture = textures[texIndex];

		for (int i = 0; i < sprites.Length; i++) 
			sprites[i].sprite = texture;

		for (int i = 0; i < images.Length; i++) 
			images[i].sprite = texture;

		texPreviewRGB.sprite = texturesRGB[texIndex];
		texPreviewALPHA.sprite = texturesALPHA[texIndex];
	}

}
