using UnityEngine;
using System.Collections;

public class ColorSwapper : MonoBehaviour {
	
	public float swapTime = 5f;
	
	private string[] colorsList;
	private int currentColorIndex = 0;
	
	// Use this for initialization
	void Start () {
		colorsList = System.Enum.GetNames (typeof(BlockColors));
		StartCoroutine (SwapColorEnumerator ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//TODO: Probably animte the color changes, also get rid of box colliders for the necessary color
	IEnumerator SwapColorEnumerator () {
		while (true) {
			currentColorIndex++;
			print (currentColorIndex);
			currentColorIndex %= colorsList.Length;
			BlockColors curColor = (BlockColors)System.Enum.Parse(typeof(BlockColors), colorsList[currentColorIndex]);
			Camera.main.backgroundColor = ToColor((int)curColor);
			yield return new WaitForSeconds(swapTime);
		}
	}
	
	
	public Color32 ToColor(int HexVal)
	{
		byte R = (byte)((HexVal >> 16) & 0xFF);
		byte G = (byte)((HexVal >> 8) & 0xFF);
		byte B = (byte)((HexVal) & 0xFF);
		return new Color32(R, G, B, 255);
	}
}
