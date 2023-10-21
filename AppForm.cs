using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace ThirdKR
{
    public interface IMyAppForm : IView
    {
        string LeftBorder { get; set; }
        string RightBorder { get; set; }
        string Step { get; set; }
        string FilePath { get; set; }
        string A { get; set; }
        bool ShowGreetings { get; set; }

        Image Image { get; }
        void Draw(GraphData graphData, string _label);

        void ClearFields();

        event EventHandler? TryDraw;
        event EventHandler? Clear;
        event EventHandler? Removelast;
        event EventHandler? OpenFile;
        event EventHandler? SaveInitialData;
        event EventHandler? NoData;
        
    }
    public partial class AppForm : Form, IMyAppForm
    {
        [Required]
        public string LeftBorder
        {
            get => textBoxLeftBorder.Text;
            set
            {
                textBoxLeftBorder.Text = value;
                this.Refresh();
            }
        }
        public string RightBorder
        {
            get => textBoxRightBorder.Text;
            set
            {
                textBoxRightBorder.Text = value;
                this.Refresh();
            }
        }
        public string A
        {
            get => textBoxA.Text;
            set
            {
                textBoxA.Text = value;
                this.Refresh();
            }
        }
        public string Step
        {
            get => textBoxStep.Text;
            set
            {
                textBoxStep.Text = value;
                this.Refresh();
            }
        }
        public string FilePath
        {
            get;
            //private
            set;
        }

        public Image Image
        {
            get
            {
                using (var ms = new MemoryStream(chart.Plot.GetImageBytes(), 0, chart.Plot.GetImageBytes().Length))
                {
                    Image image = Image.FromStream(ms, true);

                    return image;
                }
            }
        }

        public void Draw(GraphData graphData, string _label)
        {
            var scatter = chart.Plot.AddScatter(graphData.xs, graphData.ys, label: _label);

            chart.Plot.Legend();

            chart.Refresh();

            ContainDataGrid(graphData);

        }
        public AppForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            FilePath = "";
        }
        public event EventHandler? TryDraw;

        public event EventHandler? Clear;

        public event EventHandler? Removelast;

        public event EventHandler? OpenFile;

        public event EventHandler? SaveInitialData;

        public event EventHandler? NoData;


        public bool ShowGreetings { get; set; }
        public new void Show()
        {
            Application.Run(this);
        }
        private void Greetings()
        {
            MessageBox.Show("Berdiev 415 KR-3 VAR-1", "Greatings!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void ClearFields()
        {
            textBoxLeftBorder.Text = "";
            textBoxRightBorder.Text = "";
            textBoxStep.Text = "";
            textBoxA.Text = "";
            this.Refresh();
        }


        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart.Plot.Clear();
            dataGridView1.Rows.Clear();
            ClearFields();
            chart.Refresh();
            Clear?.Invoke(this, EventArgs.Empty);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (chart.Plot.GetPlottables().Length > 0)
            {

                chart.Plot.RemoveAt(chart.Plot.GetPlottables().Length - 1);
                chart.Refresh();

                Removelast?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                NoData?.Invoke(this, EventArgs.Empty);
            }
        }
        private void drawToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            TryDraw?.Invoke(this, EventArgs.Empty);

        }

        private void AppForm_Load(object sender, EventArgs e)
        {
            ShowGreetings = enableToolStripMenuItem.Checked;
            if (ShowGreetings)
                Greetings();
        }
        public void ContainDataGrid(GraphData data)
        {
            for (int i = 0; i < data.ys.Length; i++)
            {
                dataGridView1.Rows.Add(data.xs[i].ToString(), data.ys[i].ToString());
            }
        }

        private void saveDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (chart.Plot.GetPlottables().Length == 0)
            {
                NoData?.Invoke(this, EventArgs.Empty);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.OverwritePrompt = true;
            dialog.Filter = "Text file|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FilePath = dialog.FileName;
            }
            else
            {
                FilePath = "";
            }

            if (FilePath != "") SaveInitialData?.Invoke(this, EventArgs.Empty);
        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text file|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FilePath = dialog.FileName;
            }
            else
            {
                FilePath = "";
            }


            if (FilePath != "") OpenFile?.Invoke(this, EventArgs.Empty);
        }
        private void buttonDraw_Click(object sender, EventArgs e)
        {
            TryDraw?.Invoke(this, EventArgs.Empty);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (chart.Plot.GetPlottables().Length > 0)
            {

                chart.Plot.RemoveAt(chart.Plot.GetPlottables().Length - 1);
                chart.Refresh();

                Removelast?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                NoData?.Invoke(this, EventArgs.Empty);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            chart.Plot.Clear();
            ClearFields();
            chart.Refresh();
            Clear?.Invoke(this, EventArgs.Empty);
            dataGridView1.Rows.Clear();
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enableToolStripMenuItem.Checked = true;
            disableToolStripMenuItem.Checked = false;
        }

        private void disbaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enableToolStripMenuItem.Checked = false;
            disableToolStripMenuItem.Checked = true;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Greetings();
        }
        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
