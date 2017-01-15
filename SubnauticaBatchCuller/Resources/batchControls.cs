using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SubnauticaBatchCuller
{
    public class batchMainPanel : Panel
    {
        private CullerForm main;
        private Button execute = new Button();
        private Button getFolderDialog = new Button();
        private Button loadSave = new Button();
        private Button saveBMP = new Button();
        private FolderBrowserDialog getSave = new FolderBrowserDialog();
        public RichTextBox conBox = new RichTextBox();
        public ListBox selectSave = new ListBox();
        public TrackBar seaLevel = new TrackBar();
        private ToolTip seaLevelTip = new ToolTip();

        public int btnHeight = 24, sliderWidth = 32;
        private int sliderHeight, consoleWidth, consoleHeight, saveHeight, controlSpacing = 6;
        private string lastSave = "", selectedItem = "";
        public string subnauticaGameDir;

        public batchMainPanel(CullerForm m, string dir, int x, int y, int w, int h)
        {
            main = m;
            subnauticaGameDir = dir;
            Location = new Point(x, y);
            Size = new Size(w, h);

            sliderHeight = Height - (btnHeight * 4) - (4 * controlSpacing);
            consoleHeight = (4 * sliderHeight) / 5;
            saveHeight = sliderHeight - consoleHeight - controlSpacing;
            consoleWidth = Width - sliderWidth - controlSpacing;

            // Console Output
            conBox.ReadOnly = true;
            conBox.Width = consoleWidth;
            conBox.Height = consoleHeight;
            conBox.Location = new Point(sliderWidth + controlSpacing, 0);
            conBox.Text = "STEAM LIBRARY PATH NOT SET";
            Controls.Add(conBox);

            // Save Selection
            selectSave.Width = consoleWidth;
            selectSave.Height = saveHeight;
            selectSave.Location = new Point(sliderWidth + controlSpacing, consoleHeight + controlSpacing);
            selectSave.Items.Add("NO SAVES FOUND");
            selectSave.SelectedIndexChanged += new EventHandler(saveSelectionChanged);
            selectSave.Enabled = false;
            Controls.Add(selectSave);

            // Sealevel Trackbar
            seaLevel.AutoSize = false;
            seaLevel.Height = sliderHeight + 12;
            seaLevel.Location = new Point(0, -8);
            seaLevel.Orientation = Orientation.Vertical;
            seaLevel.Minimum = 1;
            seaLevel.Maximum = 24;
            seaLevel.ValueChanged += new EventHandler(seaLevelUpdate);
            seaLevel.Enabled = false;
            Controls.Add(seaLevel);

            // Draw Map to BMP
            saveBMP.Width = Width;
            saveBMP.Height = btnHeight;
            saveBMP.BackColor = Color.FromArgb(100, 255, 255, 255);
            saveBMP.Text = "Save Map to PNG";
            saveBMP.Location = new Point(0, Height - (btnHeight * 4) - (3 * controlSpacing));
            saveBMP.Enabled = false;
            saveBMP.Click += new EventHandler(saveBatchBMP);
            Controls.Add(saveBMP);

            // Load Save
            loadSave.Width = Width;
            loadSave.Height = btnHeight;
            loadSave.BackColor = Color.FromArgb(100, 255, 255, 255);
            loadSave.Text = "Load Save";
            loadSave.Location = new Point(0, Height - (btnHeight * 3) - (2 * controlSpacing));
            loadSave.Enabled = false;
            loadSave.Click += new EventHandler(loadSaveClick);
            Controls.Add(loadSave);

            // Steam library selection
            getFolderDialog.Width = Width;
            getFolderDialog.Height = btnHeight;
            getFolderDialog.BackColor = Color.FromArgb(100, 255, 255, 255);
            getFolderDialog.Text = "Set Steam Library";
            getFolderDialog.Location = new Point(0, Height - (btnHeight * 2) - controlSpacing);
            getFolderDialog.Click += new EventHandler(folderDialogClick);
            Controls.Add(getFolderDialog);

            // Execute
            execute.Width = Width;
            execute.Height = btnHeight;
            execute.BackColor = Color.FromArgb(100, 255, 255, 255);
            execute.Text = "Delete Selected Batches";
            execute.Location = new Point(0, Height - btnHeight);
            execute.Click += new EventHandler(cullBatches);
            execute.Enabled = false;
            Controls.Add(execute);
        }

        // Save BMP of the Batch map
        private void saveBatchBMP(object sender, EventArgs e)
        {
            if (!Directory.Exists("screenshots")) Directory.CreateDirectory("screenshots");
            Bitmap gridBMP = new Bitmap(main.selector.Width, main.selector.Height);

            main.selector.DrawToBitmap(gridBMP, new Rectangle(0, 0, main.selector.Width, main.selector.Height));
            gridBMP.Save(@"screenshots\" +selectedItem + "-Z" + seaLevel.Value + ".png", System.Drawing.Imaging.ImageFormat.Png);

            conBox.AppendText("Screenshot " + selectedItem + "-Z" + seaLevel.Value + ".png saved.\n");
        }

        // Delete selected batches
        private void cullBatches(object sender, EventArgs e)
        {
            main.deleteBatches();
        }

        // Enable load button
        private void saveSelectionChanged(object sender, EventArgs e)
        {
            ListBox l = sender as ListBox;
            if (l.SelectedIndex >= 0) loadSave.Enabled = true;
        }

        // Load selected save
        private void loadSaveClick(object sender, EventArgs e)
        {
            // Get save directory
            selectedItem = selectSave.GetItemText(selectSave.SelectedItem);
            main.savePath = getSave.SelectedPath + subnauticaGameDir + selectedItem;

            // Load save
            if (lastSave != selectedItem) conBox.AppendText("Loading save " + selectedItem + ".\n\n");         
            else conBox.AppendText("\nReloading save " + selectedItem + ".\n\n");
            conBox.ScrollToCaret();
            lastSave = selectedItem;

            // Set sealevel range
            string sealevelRange = "Sealevel: " + ((seaLevel.Value * 160) - 2800) + "m to " + ((seaLevel.Value * 160) - 2960) + "m";
            seaLevelTip.SetToolTip(seaLevel, "Z: " + seaLevel.Value + "\n" + sealevelRange);
            execute.Enabled = true;
            saveBMP.Enabled = true;
          
            main.getJSON();
            main.batchSearch(main.savePath);
        }

        // Update sealevel from slider
        private void seaLevelUpdate(object sender, EventArgs e)
        {
            string sealevelRange = "Sealevel: " + ((seaLevel.Value * 160) - 2800) + "m to " + ((seaLevel.Value * 160) - 2960) + "m";
            seaLevelTip.SetToolTip(seaLevel, "Z: " + seaLevel.Value + "\n" + sealevelRange);
            main.updateGrid();
        }

        // Open folder dialog
        private void folderDialogClick(object sender, EventArgs e)
        {
            Button b = sender as Button;

            if (getSave.ShowDialog() == DialogResult.OK)
            {
                conBox.Text = "Selected Steam Library:\n" + getSave.SelectedPath + "\n\n";
                main.saveSearch(getSave.SelectedPath);
            }
        }
    }
}