using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeFileManager : MonoBehaviour
{
    [SerializeField] private Grid<LifeCell> LifeGrid;

    private string path = Directory.GetCurrentDirectory() + "\\Patterns\\";
    private string file = "test.txt";

    // Saves a .txt file containing pattern information as described here:
    // https://conwaylife.com/wiki/Plaintext
    public void Save(string file)
    {
        LifeGrid = gameObject.GetComponent<GameOfLife>().GetGrid();

        string fullPath = path + file;
        if (!File.Exists(fullPath))
        {
            for (int y = 0; y < LifeGrid.GetHeight(); y++)
            {
                string lifeStr = "";
                for (int x = 0; x < LifeGrid.GetWidth(); x++)
                    lifeStr += LifeGrid.GetGridObject(x, y).ToPlaintextChar();

                using (StreamWriter sw = File.AppendText(fullPath))
                    sw.WriteLine(lifeStr);
            }
        }
    }

    // Loads a .txt file containing pattern information as described here:
    // https://conwaylife.com/wiki/Plaintext
    public void Load(string file)
    {
        LifeGrid = gameObject.GetComponent<GameOfLife>().GetGrid();

        string fullPath = path + file;
        if (File.Exists(fullPath))
        {
            using (StreamReader sr = File.OpenText(fullPath))
            {
                string lifeStr;
                int y = 0;
                while ((lifeStr = sr.ReadLine()) != null)
                {
                    // Skip name and comments
                    if (lifeStr[0] == '!')
                        continue;

                    // Prepare the grid
                    // NOTE: Current implementation only supports grids big enough to fit the pattern
                    // This will NOT resize the grid being used!
                    for(int x = 0; x < lifeStr.Length; x++)
                        if (lifeStr[x] == '.')
                            LifeGrid.GetGridObject(x, y).Death();
                        else if (lifeStr[x] == 'O')
                            LifeGrid.GetGridObject(x, y).Birth();

                    y += 1;
                }
            }
        }
    }

    public void SetFilename(string filename)
    {
        file = filename + ".txt";
    }

    private int LineCount(string path)
    {
        int count = 0;
        using (StreamReader reader = File.OpenText(path))
            while (reader.ReadLine() != null)
                count++;

        return count;
    }
}
