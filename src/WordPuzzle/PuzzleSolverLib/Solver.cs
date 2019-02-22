using System;
using System.Collections;

namespace PuzzleSolverLib
{
	public static class Solver
	{
		public struct GridPosition
		{
			public int Row;
			public int Col;

			public GridPosition(int row, int col)
			{
				Row = row;
				Col = col;
			}
		}

		public struct LetterAtPosition
		{
			public GridPosition Position { get; set; }
			public char Letter { get; set; }
			public LetterAtPosition(char letter, int row, int col)
			{
				Letter = letter;
				Position = new GridPosition(row, col);
			}
		}

		public class SearchResult
		{
			public bool Found { get; set; }
			public GridPosition StartPos { get; set; }
			public string Word { get; set; }
			public ArrayList Letters { get; set; } = new ArrayList();

			public void AddLetterMatch(char letter, int row, int col)
			{
				Letters.Add(new LetterAtPosition(letter, row, col));
			}
		}

		public static SearchResult SearchForWord(char[,] grid, string word)
		{
			// Consider every point as starting point and search 
			for (int row = 0; row < grid.GetUpperBound(0) + 1; row++)
			{
				for (int col = 0; col < grid.GetUpperBound(1) + 1; col++)
				{
					// search for the word starting at the row and column
					var result = SearchAtPosition(grid, row, col, word);

					if (result.Found)
					{
						return result;
					}
				}
			}

			return new SearchResult { Found = false };
		}

		static SearchResult SearchAtPosition(char[,] grid, int row, int col, string word)
		{
			if (grid[row, col] != word[0])
				return new SearchResult() { Found = false };

			int wordLength = word.Length;

			// Search word in all 8 directions starting from (row,col) 
			for (int searchDirection = 0; searchDirection < 8; searchDirection++)
			{
				var result = new SearchResult();
				result.Word = word;
				result.StartPos = new GridPosition(row, col);
				result.AddLetterMatch(grid[row, col], row, col);

				// get delta for direction
				var directionDelta = GetDeltaForDirection(searchDirection);

				// Initialize starting point for current direction 
				int posInWord;
				int rd = row + directionDelta.Row;
				int cd = col + directionDelta.Col;

				// First character is already checked, match remaining 
				// characters 
				for (posInWord = 1; posInWord < wordLength; posInWord++)
				{
					// If out of bound break 
					if (rd >= grid.GetUpperBound(0) + 1 || rd < 0 || cd >= grid.GetUpperBound(1) + 1 || cd < 0)
						break;

					// If not matched, break 
					if (grid[rd, cd] != word[posInWord])
						break;

					result.AddLetterMatch(grid[rd, cd], rd, cd);

					// Moving in particular direction 
					rd += directionDelta.Row;
					cd += directionDelta.Col;
				}

				// If all character matched, then value of must 
				// be equal to length of word 
				if (posInWord == wordLength)
				{
					result.Found = true;
					return result;
				}

			}
			return new SearchResult() { Found = false };
		}

		static GridPosition GetDeltaForDirection(int direction)
		{
			// grid position deltas for searching in different directions
			int[] x = { -1, -1, -1, 0, 0, 1, 1, 1 };
			int[] y = { -1, 0, 1, -1, 1, -1, 0, 1 };

			return new GridPosition(x[direction], y[direction]);
		}
	}
}
