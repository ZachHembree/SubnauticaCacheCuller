using System.Drawing;
using System.Windows.Forms;

namespace SubnauticaBatchCuller
{       
    public partial class CullerForm : Form
    {
        batchSelector selector;
        batchMainPanel mainPanel;

        public string savePath;
        const string subnauticaGameDir = @"\steamapps\common\Subnautica\SNAppData\SavedGames\";
        ToolTip[,] tooltips;
        gridButton[,] grid;
        Color selectedColor = Color.FromArgb(215, 200, 70, 70);
        Color exploredColor = Color.FromArgb(215, 70, 160, 70);
        Color noColor = Color.FromArgb(0, 0, 0, 0);

        public CullerForm()
        {
            // Form
            SuspendLayout();
            Name = "Subnautica Batch Culler";
            Text = "Subnautica Batch Culler";
            Icon = Properties.Resources.Experimental_Icon;
            ClientSize = new Size(1024, 776);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            BackgroundImageLayout = ImageLayout.None;
            ResumeLayout(false);

            // Control Panel
            mainPanel = new batchMainPanel(this, subnauticaGameDir, 776, 16, Width - 760 - 48, Height - 70);
            Controls.Add(mainPanel);

            // Selector Grid
            selector = new batchSelector(this, 16, 16, 744, 24);
            selector.BackgroundImage = Properties.Resources.subnauticaMap_noLegend;
            selector.selectedColor = selectedColor;
            selector.exploredColor = exploredColor;
            selector.BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(selector);
			tooltips = selector.tooltips;
            grid = selector.grid;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CullerForm));
            this.SuspendLayout();
            // 
            // CullerForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CullerForm";
            this.ResumeLayout(false);

        }
    }
}