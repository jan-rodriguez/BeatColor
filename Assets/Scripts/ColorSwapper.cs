using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorSwapper : MonoBehaviour {

	public int bpm = 60;
	public float swapTime = 5f;
	public bool swapping = true;
	public BlockColors[] colorsList;

	public RectTransform colorIndicatorHolder;
	public GameObject colorIndicatorPrefab;
	
	private int currentColorIndex = 0;
	private BlockColors prevColor;

	//Containers for different colored blocks
	private GameObject whiteBlocksHolder;
	private GameObject blackBlocksHolder;
	
	// Use this for initialization
	void Start () {
		swapTime = bpm / 60.0f;
		whiteBlocksHolder = GameObject.Find ("WhiteBlocks");
		blackBlocksHolder = GameObject.Find ("BlackBlocks");
		PopulateColorIndicator ();
		StartCoroutine (SwapColorEnumerator ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Add the colors to the indicator bar at the bottom of the screen
	void PopulateColorIndicator () {
		for (int i =0; i < colorsList.Length; i++) {
			BlockColors color = colorsList[i];

			GameObject newIndicator = Instantiate(colorIndicatorPrefab);
			if(color != BlockColors.None) {
				newIndicator.GetComponent<Image>().color = ToColor((int)color);
			}else{
				newIndicator.GetComponent<Image>().sprite = new Sprite();
			}
			newIndicator.GetComponent<RectTransform>().SetParent(colorIndicatorHolder);
		}
	}

	//TODO: Probably animte the color changes, also get rid of box colliders for the necessary color
	IEnumerator SwapColorEnumerator () {
		while (true && swapping) {
			//Move to the next color
			currentColorIndex++;
			currentColorIndex %= colorsList.Length;
			BlockColors curColor = colorsList[currentColorIndex];
			//If the color doesn't change, don't enable the previous blocks
			if(curColor != BlockColors.None) {
				//Re-enable previous color blocks
				EnableBlockColliders(prevColor);
				prevColor = curColor;
				Camera.main.backgroundColor = ToColor((int)curColor);
				//Disable current color blocks
				DisableBlockColliders(curColor);
			}

			yield return new WaitForSeconds(swapTime);
		}
	}

	// Enable blocks of the specified color
	void EnableBlockColliders (BlockColors color) {
		ToggleBlockColliders(color, true);
	}

	// Disable blocks of the specified color
	void DisableBlockColliders (BlockColors color) {
		ToggleBlockColliders(color, false);
	}

	//Enable/disable blocks of a specific color
	void ToggleBlockColliders (BlockColors color, bool enable) {
		switch (color) {
		case BlockColors.Black:
			blackBlocksHolder.SetActive(enable);
			break;
		case BlockColors.White:
			whiteBlocksHolder.SetActive(enable);
			break;
		//Do nothing for a None color
		case BlockColors.None:
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
