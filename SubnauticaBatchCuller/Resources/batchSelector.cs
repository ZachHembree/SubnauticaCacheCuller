using System;
using System.Drawing;
using System.Windows.Forms;

namespace SubnauticaBatchCuller
{
    public class gridButton : Button
    {
        public int keyX, keyY;
        public bool pointSelected = false;

        public gridButton(int x, int y, int size, int g)
        {
            // Store array position
            keyX = x;
            keyY = y;

            // Visuals
            UseVisualStyleBackColor = false;
            BackColor = Color.FromArgb(0, 0, 0, 0);
            FlatStyle = FlatStyle.Flat;
            Enabled = false;

            // Size/location
            Size = new Size(size, size);
            Location = new Point((size * x), (size * ((g - 1) - y)));
        }
    }

    public class batchSelector : Panel
    {
        public ToolTip[,] tooltips;
        public gridButton[,] grid;
        public Color selectedColor;
        public Color exploredColor;
        CullerForm main;

        public batchSelector(CullerForm m, int a, int b, int s, int g)
        {
            DoubleBuffered = true;
            Location = new Point(a, b);
            tooltips = new ToolTip[g, g];
            grid = new gridButton[g, g];
            Width = s;
            Height = s;
            main = m;

            int buttonSize = s / g;

            // Grid buttons
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    grid[x, y] = new gridButton(x, y, buttonSize, g);
                    grid[x, y].Click += new EventHandler(locClick);
                    Controls.Add(grid[x, y]);

                    tooltips[x, y] = new ToolTip();
                    tooltips[x, y].SetToolTip(grid[x, y], (x + 1) + " Z " + (y + 1));
                }
            }
        }

        private void locClick(object sender, EventArgs e)
        {
            gridButton loc = sender as gridButton;

            Color c;
            bool selected = grid[loc.keyX, loc.keyY].pointSelected;

            if (selected)
            {
                c = exploredColor;
                selected = false;
            }
            else
            {
                c = selectedColor;
                selected = true;
            }

            loc.BackColor = c;
            grid[loc.keyX, loc.keyY].pointSelected = selected;
            main.getSelections();
        }
    }
}