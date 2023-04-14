using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	[SerializeField] private TMP_InputField XInput;
	[SerializeField] private TMP_InputField YInput;
	[SerializeField] private TMP_InputField NumInput;
	[SerializeField] private Button startButton;
	[SerializeField] private TextMeshProUGUI errorText;

	// Start is called before the first frame update
	void Start() {
		// Add listeners to input fields
		XInput.onValueChanged.AddListener(delegate { CheckInput(); });
		YInput.onValueChanged.AddListener(delegate { CheckInput(); });
		NumInput.onValueChanged.AddListener(delegate { CheckInput(); });

		// Add listener to start button
		startButton.onClick.AddListener(delegate { StartGame(); });
		startButton.interactable = false;
	}
	void CheckInput() {
		// Check if all input fields have a value
		if (XInput.text != "" && YInput.text != "" && NumInput.text != "") {
			startButton.interactable = true;
		} else {
			startButton.interactable = false;
		}
	}

	void StartGame() {
		// Get input values
		int numTilesX = int.Parse(XInput.text);
		int numTilesY = int.Parse(YInput.text);
		int numMines = int.Parse(NumInput.text);

		// Check if input is valid
		if (numTilesX * numTilesY <= numMines) {
			Debug.Log("invalid input");
			// Flash error text
			StartCoroutine(errorFunc());
		} else {
			// Start game
			Debug.Log("Starting game with x: " + numTilesX + ", y: " + numTilesY + ", num: " + numMines);
			PlayerPrefs.SetInt("numTilesX", numTilesX);
			PlayerPrefs.SetInt("numTilesY", numTilesY);
			PlayerPrefs.SetInt("numMines", numMines);
			SceneManager.LoadScene("game", LoadSceneMode.Single);
		}
	}
	// Flash error text
	IEnumerator errorFunc() {
		for (int i = 0; i < 4; i++) {
			errorText.enabled = !errorText.enabled;
			yield return new WaitForSeconds(0.5f);
		}
	}
}
