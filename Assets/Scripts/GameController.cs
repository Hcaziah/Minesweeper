using UnityEngine;
using TMPro;
using System;

public class GameController : MonoBehaviour {
	[SerializeField] private GameObject board;
	[SerializeField] private Canvas endScreen;
	[SerializeField] private Canvas mainStats;
	private decimal seconds, minutes;
	private BoardControllerV2 boardController;
	public float time; // in seconds
	private bool gameRunning = false;
	// Start is called before the first frame update
	void Start() {
		boardController = board.GetComponent<BoardControllerV2>();
		gameRunning = true;
	}

	// Update is called once per frame
	void Update() {
		// Increment the timer and format the time display
		if (gameRunning) time += Time.deltaTime;

		seconds = Mathf.FloorToInt(time % 60);
		minutes = Mathf.FloorToInt(time / 60);
		mainStats.GetComponentInChildren<TMP_Text>().SetText(String.Format("{0:00}:{1:00}", minutes, seconds));

		// Update the number of remaining mines
		mainStats.GetComponentsInChildren<TMP_Text>()[1].SetText(String.Format(":{0:00}", (boardController.numTotalMines - boardController.numFlags).ToString()));
		// If the player has won, stop the timer and disable the tiles
		if (boardController.CheckWin()) {
			if (gameRunning) endState("You Win!");
			stopGame();
		}
		// If the player has lost, stop the timer and disable the tiles
		if (boardController.CheckLose()) {
			if (gameRunning) endState("You Lose!");
			stopGame();
		}
	}
	// Stop the game
	void stopGame() {
		gameRunning = false;
		for (int x = 0; x < boardController.tileArraySize.x; x++) {
			for (int y = 0; y < boardController.tileArraySize.y; y++) {
				boardController.tiles[x, y].isClickable = false;
			}
		}
	}
	void endState(String text) {
		endScreen.GetComponentInChildren<TMP_Text>().SetText(text);
		endScreen.GetComponentsInChildren<TMP_Text>()[1].SetText(String.Format("{0:00}:{1:00}", minutes, seconds));
		endScreen.GetComponentsInChildren<TMP_Text>()[2].SetText(String.Format("[{0:0}, {1:0}] with {2:00} mines", boardController.tileArraySize.x, boardController.tileArraySize.y, boardController.numTotalMines));
		endScreen.gameObject.SetActive(true);
	}
}