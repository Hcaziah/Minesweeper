using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardController : MonoBehaviour {
	[SerializeField] private GameObject tilePrefab;
	public GameObject[,] tiles;
	public Vector2 tileSize, boardSize;
	public int numTilesX = 10;
	public int numTilesY = 10;
	public int numTiles = 0;
	public int numberMines = 10;
	public Vector2Int[] mineArray;
	public int numFlags = 0;

	private Vector2Int[] neighborPoints = new Vector2Int[] {
			new Vector2Int(-1, -1),
			new Vector2Int(-1,  0),
			new Vector2Int(-1,  1),
			new Vector2Int(0, -1),
			new Vector2Int(0,  1),
			new Vector2Int(1, -1),
			new Vector2Int(1,  0),
			new Vector2Int(1,  1)
		};

	void Start() {
		SetupBoard();
	}

	// Update is called once per frame
	void Update() {
		// Count the number of flagged tiles
		CountFlags();
		ClearEmptyTiles();
	}

	void SetupBoard() {
		numTiles = numTilesX * numTilesY;
		boardSize = transform.GetComponent<Renderer>().bounds.size;
		tileSize = new Vector2(boardSize.x / (numTilesX), boardSize.y / (numTilesY));

		// Initiate cellArray
		tiles = new GameObject[numTilesX, numTilesY];

		Vector2 startingPosition = new Vector2((boardSize.x / 2) - (tileSize.x / 2), (boardSize.y / 2) - (tileSize.y / 2));

		int numberXLines = numTilesX + 1;
		int numberYLines = numTilesY + 1;

		// Create tile grid of tile Gameobjects
		for (int x = 0; x < numTilesX; x++) {
			for (int y = 0; y < numTilesY; y++) {
				// Instantiate a new tile at the current position
				GameObject tile = Instantiate(tilePrefab, new Vector2((x * tileSize.x) - startingPosition.x, (y * tileSize.y) - startingPosition.y), Quaternion.identity);
				tile.transform.parent = transform;
				// Scale the tile so that it takes up the entire board
				tile.transform.localScale = new Vector2(1f / numTilesX, 1f / numTilesY);
				// Inform tile of its position
				tile.GetComponent<TileController>().tilePos = new Vector2Int(x, y);
				// Store the tile in the cells array
				tiles[x, y] = tile;
			}
		}

		// Place mines
		int numMines = numberMines;
		// Initate mineArray
		mineArray = new Vector2Int[numberMines];

		while (numMines > 0) {
			// Choose a random position on the board
			int x = Random.Range(0, numTilesX);
			int y = Random.Range(0, numTilesY);

			if (!tiles[x, y].GetComponent<TileController>().isMine) {
				// Place a mine on the tile at the chosen position
				tiles[x, y].GetComponent<TileController>().isMine = true;
				// Add mines location to mineArray, reverse order because why not.
				mineArray[numMines - 1] = new Vector2Int(x, y);

				for (int i = 0; i < neighborPoints.Length; i++) {
					Vector2Int point = neighborPoints[i];
					// Increment the number of adjacent mines for each neighboring tile
					if (point.x + x >= 0 && point.x + x < numTilesX && point.y + y >= 0 && point.y + y < numTilesY)
						tiles[point.x + x, point.y + y].GetComponent<TileController>().numAdjacentMines++;
				}
				numMines--;
			}
		}
	}
	void CountFlags() {
		int sum = 0;
		for (int x = 0; x < numTilesX; x++) {
			for (int y = 0; y < numTilesY; y++) {
				if (tiles[x, y].GetComponent<TileController>().isFlagged) sum++;
				if (sum >= numberMines) break; // Break out of the loop if we've found all the mines (no need to keep counting)
			}
		}
		numFlags = sum;
	}

	// Set numTiles and board size

	// Clear out empty tiles (tiles with no adjacent mines)
	void ClearEmptyTiles() {
		// Loop through all the cells in the grid
		for (int x = 0; x < numTilesX; x++) {
			for (int y = 0; y < numTilesY; y++) {
				// Get the TileController component of the current cell
				TileController currentTile = tiles[x, y].GetComponent<TileController>();

				// Check if the current cell is revealed, not a mine, and has no adjacent mines
				if (currentTile.isRevealed && !currentTile.isMine && currentTile.numAdjacentMines == 0) {
					// Loop through the neighboring points of the current cell
					for (int i = 0; i < neighborPoints.Length; i++) {
						Vector2Int point = neighborPoints[i];

						// Calculate the position of the neighboring cell
						int neighborX = x + point.x;
						int neighborY = y + point.y;

						// Check if the neighboring cell is within the bounds of the grid
						if (neighborX >= 0 && neighborX < numTilesX && neighborY >= 0 && neighborY < numTilesY) {
							// Get the TileController component of the neighboring cell
							TileController neighborCell = tiles[neighborX, neighborY].GetComponent<TileController>();

							// Check if the neighboring cell is not a mine and has at least one adjacent mine
							if (!neighborCell.isMine && neighborCell.numAdjacentMines >= 0) {
								// Set the neighboring cell as revealed
								neighborCell.isRevealed = true;
							}
						}
					}
				}
			}
		}
	}

	// Check if the player has won
	public bool CheckWin() {
		// Loop through all the cells in the grid
		for (int x = 0; x < numTilesX; x++) {
			for (int y = 0; y < numTilesY; y++) {
				// Get the TileController component of the current cell
				TileController currentTile = tiles[x, y].GetComponent<TileController>();

				// Check if the current cell is not a mine and is not revealed
				if (!currentTile.isMine && !currentTile.isRevealed) {
					// The player has not won yet
					return false;
				}
			}
		}
		// The player has won
		return true;
	}
	public bool CheckLose() {
		// Loop through all the cells in the grid
		for (int x = 0; x < numTilesX; x++) {
			for (int y = 0; y < numTilesY; y++) {
				// Get the TileController component of the current cell
				TileController currentTile = tiles[x, y].GetComponent<TileController>();

				// Check if the current cell is a mine and is revealed
				if (currentTile.isMine && currentTile.isRevealed) {
					// The player has lost
					return true;
				}
			}
		}
		// The player has not lost
		return false;
	}
}

