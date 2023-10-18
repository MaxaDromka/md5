using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace md5
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void CalculateChecksumButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilePathTextBox.Text))
            {
                MessageBox.Show("Please select a file.");
                return;
            }

            string filePath = FilePathTextBox.Text;
            string checksum = CalculateMD5Checksum(filePath);

            ChecksumTextBox.Text = checksum;

            SaveChecksumToFile(filePath, checksum);
        }

        private void VerifyChecksumButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilePathTextBox.Text) || string.IsNullOrWhiteSpace(ChecksumTextBox.Text))
            {
                MessageBox.Show("Please select a file and calculate its checksum first.");
                return;
            }

            string filePath = FilePathTextBox.Text;
            string storedChecksum = File.ReadAllText(Path.ChangeExtension(filePath, "md5")).Trim().ToLowerInvariant();
            string calculatedChecksum = ChecksumTextBox.Text.Trim().ToLowerInvariant();

            if (storedChecksum == calculatedChecksum)
            {
                MessageBox.Show("Checksum verified successfully. The file has not been modified.");
            }
            else
            {
                MessageBox.Show("Checksum verification failed. The file may have been modified.");
            }
        }


        private string CalculateMD5Checksum(string filePath)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(filePath))
            {
                byte[] hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        private void SaveChecksumToFile(string filePath, string checksum)
        {
            string checksumFilePath = Path.ChangeExtension(filePath, "md5");

            File.WriteAllText(checksumFilePath, checksum);

            MessageBox.Show($"Checksum saved to {checksumFilePath}");
        }
    }
}
