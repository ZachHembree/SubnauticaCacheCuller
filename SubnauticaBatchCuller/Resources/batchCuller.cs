using System.IO;

namespace SubnauticaBatchCuller
{
    public partial class CullerForm
    {
        // Save selected batches to list
        public void getSelections()
        {
            for (int a = 0; a < grid.GetLength(0); a++)
            {
                for (int b = 0; b < grid.GetLength(1); b++)
                {
                    for (int n = 0; n < batchList.Length; n++)
                    {
                        if (batchList[n].x == a && batchList[n].y == b && batchList[n].z == mainPanel.seaLevel.Value)
                        {
                            if (grid[a, b].pointSelected) batchList[n].selected = true;
                            else batchList[n].selected = false;
                            break;
                        }
                    }
                }
            }
        }

        // Delete selected batch files
        public void deleteBatches()
        {
            for (int n = 0; n < batchList.Length; n++)
            {
                batch v = batchList[n];

                if (v.selected)
                {
                    string baked, compiled, objects;

                    baked = savePath + @"\CellsCache\baked-batch-cells-" + v.x + "-" + v.z + "-" + v.y + ".bin";
                    compiled = savePath + @"\CompiledOctreesCache\compiled-batch-" + v.x + "-" + v.z + "-" + v.y + ".optoctrees";
                    objects = savePath + @"\batch-objects-" + v.x + "-" + v.z + "-" + v.y + ".txt";

                    mainPanel.conBox.AppendText("Deleting " + v.x + " " + v.z + " " + v.y + "\n");
                    mainPanel.conBox.ScrollToCaret();
                    File.Delete(baked);
                    File.Delete(compiled);
                    File.Delete(objects);
                }
            }
        }
    }
}