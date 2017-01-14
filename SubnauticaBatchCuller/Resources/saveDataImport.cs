using System;
using System.IO;

namespace SubnauticaBatchCuller
{
    public partial class CullerForm
    {
        public void getJSON()
        {
            string[] json, saveData;
            int saveDataLen = 0, saveDataPos = 0;

            json = File.ReadAllLines(savePath + @"\gameinfo.json");

            foreach(char v in json[0])
            {
                if (v == ',') saveDataLen++;
            }

            saveData = new string[saveDataLen + 1];

            foreach (char v in json[0])
            {
                if (v != '{' && v != '}')
                {
                    if (v == ',') saveDataPos++;
                    else saveData[saveDataPos] += v;
                }
            }

            mainPanel.batchSelection.AppendText("Save Information:\n\n");
            mainPanel.batchSelection.ScrollToCaret();

            foreach (string v in saveData)
            {
                mainPanel.batchSelection.AppendText(v + "\n");
                mainPanel.batchSelection.ScrollToCaret();
            }

            mainPanel.batchSelection.AppendText("\n");
            mainPanel.batchSelection.ScrollToCaret();
        }

    }
}