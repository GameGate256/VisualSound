using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Forms;
using NAudio.CoreAudioApi;

namespace VisualSound
{
    public partial class Form1 : Form
    {
        public static Form1 instance;

        MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        MMDevice device;

        double maxVol;
        float opacity;

        public bool showValue, isBigger, isAlwaysOne;
        public float multiplier;
        public int mode;

        public Button startButton;

        Form2 form2 = new Form2();

        public Form1()
        {
            InitializeComponent();
            instance = this;
            startButton = button1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            comboBox1.SelectedIndex = 0;
            checkBox3.Checked = true;

            multiplier = 1.0f;
            opacity = 1.0f;
            maxVol = 0.0f;

            if (device.AudioMeterInformation.PeakValues.Count == 2)
                comboBox1.SelectedIndex = 1;
            else if (device.AudioMeterInformation.PeakValues.Count == 4)
                comboBox1.SelectedIndex = 2;
            else if (device.AudioMeterInformation.PeakValues.Count == 6)
                comboBox1.SelectedIndex = 3;
            else if (device.AudioMeterInformation.PeakValues.Count == 8)
                comboBox1.SelectedIndex = 4;
            else
                comboBox1.SelectedIndex = 0;

            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start")
            {
                mode = comboBox1.SelectedIndex;

                if (checkBox2.Checked == true)
                {
                    debugForm debugform = new debugForm();
                    startButton.Enabled = false;
                    debugform.Show();
                }
                else
                {
                    form2 = new Form2();
                    form2.Show();
                    button1.Text = "Stop";
                    if (checkBox3.Checked == true)
                        form2.TransparencyKey = Color.WhiteSmoke;
                    else
                        form2.TransparencyKey = Color.Navy;
                    form2.Opacity = opacity;
                    checkBox6.Checked = false;
                }
            }
            else
            {
                form2.Close();
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SystemSounds.Exclamation.Play();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label4.Text = trackBar1.Value/100.0f + "x";
            multiplier = trackBar1.Value/100.0f;
            numericUpDown1.Value = (decimal)(trackBar1.Value / 100.0f);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double tempmaxVol = 0.0f;
            for (int i = 0; i < device.AudioMeterInformation.PeakValues.Count; i++)
            {
                if (tempmaxVol < device.AudioMeterInformation.PeakValues[i])
                {
                    tempmaxVol = device.AudioMeterInformation.PeakValues[i];
                }
            }

            if (maxVol < tempmaxVol * multiplier)
            {
                maxVol = tempmaxVol * multiplier;
            }
            label7.Text = "maxValue: " + string.Format("{0:0.00}", Math.Round(maxVol, 2));
            label5.Text = "currentVolume: " + string.Format("{0:0.00}",Math.Round(tempmaxVol,2)*multiplier);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = (int)(numericUpDown1.Value*100);
            label4.Text = trackBar1.Value / 100.0f + "x";
            multiplier = trackBar1.Value / 100.0f;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            maxVol = 0.0f;
            label7.Text = "maxValue: " + string.Format("{0:0.00}", Math.Round(maxVol, 2));
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            opacity = trackBar2.Value/100.0f;
            label9.Text = trackBar2.Value + "%";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            showValue = checkBox1.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            form2.instance.moveImageTransparency(!checkBox6.Checked);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            isAlwaysOne = checkBox5.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            isBigger = checkBox4.Checked;
        }
    }
}
