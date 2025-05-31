using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace NvidiaFrequencyLocker
{
    public partial class Form1 : Form
    {
        // Do not include quotes in the path
        const string PATH2SMI = @"C:\Windows\System32\nvidia-smi.exe";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string min = numericUpDown1.Value.ToString();
            string max = numericUpDown2.Value.ToString();

            this.RunNvidiaSMI($"-lgc {min},{max}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.RunNvidiaSMI("-rgc");
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown2.Minimum = Math.Max(300, numericUpDown1.Value);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.Maximum = numericUpDown2.Value;
        }

        /// <summary>
        /// Runs nvidia-smi directly without using cmd.exe
        /// </summary>
        private void RunNvidiaSMI(string arguments)
        {
            try
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = PATH2SMI,
                        Arguments = arguments,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (String.IsNullOrWhiteSpace(error))
                {
                    LogInvokeResult($">> {output}", Color.Lime);
                }
                else
                {
                    LogInvokeResult($">> {error}", Color.Red);
                }
            }
            catch (Exception ex)
            {
                LogInvokeResult($">> {ex.Message}", Color.Red);
            }
        }

        public void LogInvokeResult(string text, Color color)
        {
            richTextBox1.SuspendLayout();
            richTextBox1.SelectionColor = color;
            richTextBox1.AppendText(text);
            richTextBox1.ScrollToCaret();
            richTextBox1.ResumeLayout();
        }
    }
}
