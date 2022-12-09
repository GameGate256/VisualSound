using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.CoreAudioApi;

namespace VisualSound
{
    public partial class debugForm : Form
    {
        public debugForm instance;

        MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        MMDevice device;

        float multiplier;
        int mode;
        string l2, l4;
        public debugForm()
        {
            InitializeComponent();
            instance = this;
        }

        private void debugForm_Load(object sender, EventArgs e)
        {
            device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            multiplier = Form1.instance.multiplier;
            mode = Form1.instance.mode;
            timer1.Start();

            label5.Text = "multiplier: " + multiplier + "x";
            label6.Text = "mode: " + modeToTxt(mode);
        }
        private void debugForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.instance.startButton.Enabled = true;
        }

        private string modeToTxt(int mode)
        {
            if (mode == 0) return "mono";
            else if (mode == 1) return "stereo";
            else if (mode == 2) return "4 channel";
            else if (mode == 3) return "5.1 curround";
            else if (mode == 4) return "7.1 surround";
            else return "Unknown";
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            l2 = "";
            l4 = "";

            for (int i = 0; i < device.AudioMeterInformation.PeakValues.Count; i++)
            {
                l2 += string.Format("{0:0.00}",
                    Math.Round(device.AudioMeterInformation.PeakValues[i], 2)) + "\n";
                l4 += string.Format("{0:0.00}",
                    Math.Round(device.AudioMeterInformation.PeakValues[i], 2) * multiplier) + "\n";
            }
            label2.Text = l2;
            label4.Text = l4;
        }
    }
}
