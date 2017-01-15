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
            string gameMode = "";
            double gameTime = 0;

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

            string[,] parsedSaveData = new string[saveData.Length, 2];

            for (int n = 0; n < saveData.Length; n++)
            {
                int b = 0;

                foreach (char v in saveData[n])
                {
                    if (v != '"')
                    {
                        if (v != ':') parsedSaveData[n, b] += v;
                        else b++;
                    }
                }
            }
            
            for (int n = 0; n < parsedSaveData.GetLength(0); n++)
            {
                if (parsedSaveData[n, 0] == "gameTime") gameTime = Math.Round(Double.Parse(parsedSaveData[n, 1]) / 3600, 2);

                if (parsedSaveData[n, 0] == "gameMode")
                {
                    if (parsedSaveData[n, 1] == "0") gameMode = "Survival";
                }
            }
            
            mainPanel.conBox.AppendText("Save Information:\nPlay Time: " + gameTime + " hours\nGame Mode: " + gameMode + "\n");
            mainPanel.conBox.ScrollToCaret();
        }
    }
}