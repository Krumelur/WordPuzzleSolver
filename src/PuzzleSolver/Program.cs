using PuzzleSolverLib;
using System;
using System.Collections.Generic;

namespace WordFinder
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Console.WriteLine("================");
				Console.WriteLine("WORD FINDER 2000");
				Console.WriteLine("================");
				Console.WriteLine("");


				// dummy up some data
				List<string> testData = new List<string>()
				{
					"OBNL",
					"FUNS",
					"FEAR",
				};

				// put our dummy data into a 2D char array
				char[,] letterGrid = new char[testData.Count, testData[0].Length];

				// populate our char array
				for (int i = 0; i < testData.Count; i++)
				{
					char[] line = testData[i].ToCharArray();

					for (int charPos = 0; charPos < line.Length; charPos++)
					{
						letterGrid[i, charPos] = line[charPos];
					}
				}


				// dump out the grid of characters to the screen
				string dividerLine = new String('-', (letterGrid.GetUpperBound(1) + 1) * 2) + "-";
				Console.WriteLine(dividerLine);

				for (int row = 0; row < letterGrid.GetUpperBound(0) + 1; row++)
				{

					for (int col = 0; col < letterGrid.GetUpperBound(1) + 1; col++)
					{
						Console.Write("|");
						Console.Write(letterGrid[row, col]);
					}
					Console.WriteLine("|");

					Console.WriteLine(dividerLine);
				}

				Console.WriteLine("");
				Console.WriteLine("");

				Search(letterGrid, "FUN");
				Search(letterGrid, "FEAR");
				Search(letterGrid, "OFF");

				Console.WriteLine("Done");
				Console.ReadLine();
			}
			// Because I felt guilty about having no error handling...
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		static void Search(char[,] letterGrid, string word)
		{
			var result = Solver.SearchForWord(letterGrid, word);
			if (result.Found)
			{
				Console.WriteLine($"Found {result.Word} at {result.StartPos.Row}, {result.StartPos.Col}");
				foreach (Solver.LetterAtPosition item in result.Letters)
				{
					Console.WriteLine($"  - '{item.Letter}' {item.Position.Row},{item.Position.Col}");
				}
			}
		}
	}
}
