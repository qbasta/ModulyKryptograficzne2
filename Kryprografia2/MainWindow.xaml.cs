using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kryprografia2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LFSR lfsr;
        private bool isRunning;

        public MainWindow()
        {
            InitializeComponent();
            isRunning = false;
            StartButton.Click += StartButton_Click;
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                isRunning = false;
                StartButton.Content = "Start";
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
    }


    public class LFSR
    {
        private string state;
        private int[] taps;

        public LFSR(string seed, int[] taps)
        {
            this.state = seed;
            this.taps = taps;
        }

        public int Next()
        {
            int feedback = 0;
            for (int i = 0; i < taps.Length; i++)
            {
                feedback += int.Parse(state[taps[i]].ToString());
            }
            feedback %= 2;
            state = state.Substring(1) + feedback.ToString();
            return feedback;
        }
    }
}