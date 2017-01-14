using System;
using System.IO;

namespace SubnauticaBatchCuller
{
    public struct batch
    {
        public int x, y, z;
        public bool selected;
    }

    public partial class CullerForm
	{
		DirectoryInfo batchCacheDir;
        FileInfo[] batchFiles;
        string[] batchStrings, savePaths;
        batch[] batchList;

        // Search for save folders
        public void saveSearch(string path)
        {
            try
            {
                savePaths = Directory.GetDirectories(path + subnauticaGameDir, "slot*");
                mainPanel.batchSelection.AppendText("Saves found:");

                foreach (string v in savePaths)
                {
                    mainPanel.batchSelection.AppendText(v + "\n\n");
                    mainPanel.batchSelection.ScrollToCaret();
                }

                getSaveNames();
            }
            catch
            {
                mainPanel.batchSelection.AppendText("[ERROR] STEAMAPPS FOLDER NOT FOUND!\n\n");
                mainPanel.batchSelection.ScrollToCaret();
            }
        }

        // Get save folder name from path
        private void getSaveNames()
        {
            string[] saveNames = new string[savePaths.Length];

            for (int a = 0; a < savePaths.Length; a++)
            {
                for (int b = 0; b < savePaths[a].Length; b++)
                {
                    if (savePaths[a][b] == '\\') saveNames[a] = "";
                    else saveNames[a] += savePaths[a][b];
                }
            }

            mainPanel.selectSave.Enabled = true;
            mainPanel.selectSave.Items.Clear();
            mainPanel.selectSave.Items.AddRange(saveNames);
        }

        // Search for batch files
        public void batchSearch(string path)
        {
            try
            {
                batchCacheDir = new DirectoryInfo(path + @"\CellsCache");
                batchFiles = batchCacheDir.GetFiles("baked-batch-cells-" + "*.bin");
                batchStrings = new string[batchFiles.Length];
                mainPanel.batchSelection.AppendText("Batches found: " + batchFiles.Length + "\n");

                for (int a = 0; a < batchFiles.Length; a++)
                {
                    string workspace = batchFiles[a].Name;
                    for (int b = 18; b < workspace.Length - 4; b++) batchStrings[a] += workspace[b];
                }

                getBatchList();
            }
            catch
            {
                mainPanel.batchSelection.AppendText("[ERROR] CELLSCACHE FOLDER NOT FOUND!\n\n");
                mainPanel.batchSelection.ScrollToCaret();
            }
        }

        // Convert batch strings to int coordinates
        private void getBatchList()
        {
            string[] pos;
            batchList = new batch[batchStrings.Length];

            for (int a = 0; a < batchStrings.Length; a++)
            {
                int n = 0;
                pos = new string[3];

                for (int b = 0; b < batchStrings[a].Length; b++)
                {
                    if (batchStrings[a][b] != '-') pos[n] += batchStrings[a][b];
                    else n++;
                }

                batchList[a].x = Int32.Parse(pos[0]);
                batchList[a].z = Int32.Parse(pos[1]);
                batchList[a].y = Int32.Parse(pos[2]);
                batchList[a].selected = false;
            }

            getBatchZRange();
            updateGrid();
        }

        // Find z-range for explored batches
        private void getBatchZRange()
        {
            int zMin = 0, zMax = 0;

            foreach (batch v in batchList)
            {
                if (zMin == 0) zMin = v.z;
                else if (zMin > v.z) zMin = v.z;

                if (zMax == 0) zMax = v.z;
                else if (zMax < v.z) zMax = v.z;
            }

            mainPanel.seaLevel.Minimum = zMin;
            mainPanel.seaLevel.Maximum = zMax;
            mainPanel.seaLevel.Value = zMin;
            mainPanel.seaLevel.Enabled = true;
        }

        // Find explored batches
        public void updateGrid()
        {
            for (int a = 0; a < grid.GetLength(0); a++)
            {
                for (int b = 0; b < grid.GetLength(1); b++)
                {
                    foreach (batch v in batchList)
                    {
                        if (v.x == a && v.y == b && v.z == mainPanel.seaLevel.Value)
                        {
                            if (!grid[a, b].Enabled) grid[a, b].Enabled = true;

                            if (!v.selected)
                            {
                                grid[a, b].pointSelected = false;
                                grid[a, b].BackColor = exploredColor;
                            }
                            else
                            {
                                grid[a, b].pointSelected = true;
                                grid[a, b].BackColor = selectedColor;
                            }

                            tooltips[a, b].SetToolTip(grid[a, b], v.x + " " + v.z + " " + v.y);

                            break;
                        }
                        else if (grid[a, b].Enabled)
                        {
                            grid[a, b].Enabled = false;
                            grid[a, b].BackColor = noColor;
                        }
                    }
                }
            }
        }
	}
}