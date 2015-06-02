using UnityEngine;
using System.Collections;

public class ColorSwapper : MonoBehaviour {

	public int bpm = 60;
	public float swapTime = 5f;
	public bool swapping = true;
	
	private string[] colorsList;
	private int currentColorIndex = 0;

	private BlockColors prevColor;

	private Transform whiteBlocksHolder;
	private Transform blackBlocksHolder;
	
	// Use this for initialization
	void Start () {
		colorsList = System.Enum.GetNames (typeof(BlockColors));
		swapTime = bpm / 60.0f;
		whiteBlocksHolder = GameObject.Find ("WhiteBlocks").transform;
		blackBlocksHolder = GameObject.Find ("BlackBlocks").transform;
		StartCoroutine (SwapColorEnumerator ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//TODO: Probably animte the color changes, also get rid of box colliders for the necessary color
	IEnumerator SwapColorEnumerator () {
		while (true && swapping) {
			//Re-enable previous color blocks
			EnableBlockColliders(prevColor);

			currentColorIndex++;
			currentColorIndex %= colorsList.Length;
			BlockColors curColor = (BlockColors)System.Enum.Parse(typeof(BlockColors), colorsList[currentColorIndex]);
			prevColor = curColor;
			Camera.main.backgroundColor = ToColor((int)curColor);

			//Disable current color blocks
			DisableBlockColliders(curColor);
			yield return new WaitForSeconds(swapTime);
		}
	}

	void EnableBlockColliders (BlockColors color) {
		ToggleBlockColliders(color, true);
	}

	void DisableBlockColliders (BlockColors color) {
		ToggleBlockColliders(color, false);
	}

	void ToggleBlockColliders (BlockColors color, bool enable) {
		switch (color) {
		case BlockColors.Black:
			for(int i = 0; i < blackBlocksHolder.childCount; i++) {
				Transform block = blackBlocksHolder.GetChild(i);
				block.gameObject.SetActive(enable);
			}
			break;
		case BlockColors.White:
			for(int i = 0; i < whiteBlocksHolder.childCount; i++) {
				Transform block = whiteBlocksHolder.GetChild(i);
				block.gameObject.SetActive(enable);
			}
			break;
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
