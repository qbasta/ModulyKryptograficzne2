using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kryprografia2
{
    public partial class MainWindow : Window
    {
        private LFSR lfsr;
        private bool isRunning;

        public MainWindow()
        {
            InitializeComponent();
            isRunning = false;
        }

        public class LFSR
        {
            private string seed;
            private int[] taps;
            private int currentIndex;

            public LFSR(string seed, int[] taps)
            {
                this.seed = seed;
                this.taps = taps;
                this.currentIndex = 0;
            }

            public int Next()
            {
                int result = seed[currentIndex] - '0';
                int tapResult = 0;
                foreach (int tap in taps)
                {
                    tapResult ^= (seed[(currentIndex + seed.Length - tap) % seed.Length] - '0');
                }
                currentIndex = (currentIndex + 1) % seed.Length;
                seed = seed.Remove(currentIndex, 1).Insert(currentIndex, tapResult.ToString());
                return result;
            }

            public void Reset()
            {
                currentIndex = 0;
                seed = seed.Remove(currentIndex, 1).Insert(currentIndex, "0");
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                isRunning = false;
                StartButton.Content = "Szyfruj";
                return;
            }
            isRunning = true;
            StartButton.Content = "Stop";
            string seed = SeedTextBox.Text;
            string tapsInput = TapsTextBox.Text;
            int[] taps = tapsInput.Split(',').Select(int.Parse).ToArray();
            lfsr = new LFSR(seed, taps);
            while (isRunning)
            {
                await Task.Delay(50);
                OutputTextBox.AppendText(lfsr.Next().ToString());
            }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            string ciphertext = OutputTextBox.Text;
            string seed = SeedTextBox.Text;
            string tapsInput = TapsTextBox.Text;
            int[] taps = tapsInput.Split(',').Select(int.Parse).ToArray();
            lfsr = new LFSR(seed, taps);
            string plaintext = "";
            for (int i = 0; i < ciphertext.Length; i++)
            {
                int c = (int)ciphertext[i] - 48;
                plaintext += (lfsr.Next() ^ c).ToString();
            }
            OutputTextBox.Text = plaintext;
        }
    }
}


    