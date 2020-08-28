using System;
using System.IO;
using System.Windows.Forms;
using libazuworker;

namespace moe.yo3explorer.azusa.Utilities.BandcampImporter
{
    public partial class BandcampImportForm : Form
    {
        public BandcampImportForm()
        {
            InitializeComponent();
            shelfSelector1.TrySetShelfByName("Digitaler Content");
        }

        private void UpdateControls()
        {
            inputFolderTextBox.Text = inputFolderBrowserDialog.SelectedPath;
            outputFolderTextBox.Text = outputFolderBrowserDialog.SelectedPath;
            discographyNameTextBox.Enabled = discographyCheckbox.Checked;
            if (!discographyCheckbox.Checked)
                discographyNameTextBox.Text = string.Empty;
            goButton.Enabled = !string.IsNullOrEmpty(inputFolderBrowserDialog.SelectedPath) &&
                               !string.IsNullOrEmpty(outputFolderBrowserDialog.SelectedPath);
            if (goButton.Enabled && discographyCheckbox.Checked)
            {
                goButton.Enabled = !string.IsNullOrEmpty(discographyNameTextBox.Text);
            }
        }

        private void openFolderButton_Click(object sender, EventArgs e)
        {
            if (inputFolderBrowserDialog.ShowDialog(this) != DialogResult.OK)
                return;

            UpdateControls();
        }

        private void outputFolderOpenButton_Click(object sender, EventArgs e)
        {
            if (outputFolderBrowserDialog.ShowDialog(this) != DialogResult.OK)
                return;

            UpdateControls();
        }

        private void discographyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            BandcampImportWorker bandcampImportWorker = new BandcampImportWorker(new DirectoryInfo(this.inputFolderBrowserDialog.SelectedPath));
            bandcampImportWorker.Shelf = shelfSelector1.SelectedShelf;
            bandcampImportWorker.TargetFolder = new DirectoryInfo(this.outputFolderBrowserDialog.SelectedPath);
            bandcampImportWorker.IsDiscography = discographyCheckbox.Checked;
            bandcampImportWorker.DiscographyName = discographyNameTextBox.Text;
            bandcampImportWorker.Context = AzusaContext.GetInstance();

            WorkerForm workerForm = new WorkerForm(bandcampImportWorker);
            workerForm.ShowDialog(this);
            this.Close();
        }

        private void discographyNameTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }
    }
}
