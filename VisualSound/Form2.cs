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
    public partial class Form2 : Form
    {

        public Form2 instance;

        MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        MMDevice device;

        bool showValue, isAlwaysOne;
        float multiplier;
        int mode, sizeMultiplier;

        bool dragging = false;
        Point dragCursorPoint;
        Point dragFormPoint;

        float[] volumes = new float[8];

        PictureBox[] arrowPictures = new PictureBox[8];
        Label[] labels = new Label[8];

        
        int[,] imageCenterPos = new int[8, 2];

        public Form2()
        {
            InitializeComponent();
            instance = this;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (Form1.instance.isBigger) sizeMultiplier = 2;
            else sizeMultiplier = 1;
            this.Size = new Size(228 * sizeMultiplier, 228 * sizeMultiplier);
            device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);

            showValue = Form1.instance.showValue;
            multiplier = Form1.instance.multiplier;
            mode = Form1.instance.mode;
            isAlwaysOne = Form1.instance.isAlwaysOne;

            imageCenterPos[0, 0] = 44 * sizeMultiplier;
            imageCenterPos[0, 1] = 44 * sizeMultiplier;

            imageCenterPos[1, 0] = 114 * sizeMultiplier;
            imageCenterPos[1, 1] = 44 * sizeMultiplier;

            imageCenterPos[2, 0] = 184 * sizeMultiplier;
            imageCenterPos[2, 1] = 44 * sizeMultiplier;

            imageCenterPos[3, 0] = 44 * sizeMultiplier;
            imageCenterPos[3, 1] = 114 * sizeMultiplier;

            imageCenterPos[4, 0] = 184 * sizeMultiplier;
            imageCenterPos[4, 1] = 114 * sizeMultiplier;

            imageCenterPos[5, 0] = 44 * sizeMultiplier;
            imageCenterPos[5, 1] = 184 * sizeMultiplier;

            imageCenterPos[6, 0] = 114 * sizeMultiplier;
            imageCenterPos[6, 1] = 184 * sizeMultiplier;

            imageCenterPos[7, 0] = 184 * sizeMultiplier;
            imageCenterPos[7, 1] = 184 * sizeMultiplier;

            arrowPictures[0] = pictureBox_LU;
            arrowPictures[1] = pictureBox_CU;
            arrowPictures[2] = pictureBox_RU;
            arrowPictures[3] = pictureBox_LC;
            arrowPictures[4] = pictureBox_RC;
            arrowPictures[5] = pictureBox_LD;
            arrowPictures[6] = pictureBox_CD;
            arrowPictures[7] = pictureBox_RD;

            labels[0] = label1;
            labels[1] = label2;
            labels[2] = label3;
            labels[3] = label4;
            labels[4] = label5;
            labels[5] = label6;
            labels[6] = label7;
            labels[7] = label8;

            pictureBox1.Location = new Point((228 * sizeMultiplier)/2 - 16, (228 * sizeMultiplier) / 2 - 16);

            modeInitialize();


            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < device.AudioMeterInformation.PeakValues.Count; i++)
            {
                volumes[i] = (float)device.AudioMeterInformation.PeakValues[i] * multiplier;
                if (volumes[i] > 1) volumes[i] = 1.0f;
                if (isAlwaysOne == true) volumes[i] = 1.0f;
            }

            ImagePlace();
        }

        private void ImagePlace()
        {
            int[] size = {0,0,0,0,0,0,0,0};
            for (int i = 0; i < device.AudioMeterInformation.PeakValues.Count; i++)
            {
                int temp = link(mode, i);
                size[temp] = (int)(volumes[i] * 64) * sizeMultiplier;
                arrowPictures[temp].Size = new Size(size[temp], size[temp]);
                arrowPictures[temp].Location = new Point(imageCenterPos[temp, 0] - size[temp]/2,
                    imageCenterPos[temp, 1] - size[temp]/2);
                if (showValue)
                {
                    labels[temp].Text = string.Format("{0:0.00}", Math.Round(volumes[i], 2));
                }
            }
        }

        private int link(int mode, int number)
        {
            if (mode == 1)
            {
                if (number == 0) return 3;
                else if (number == 1) return 4;
                else return -1;
            }
            else if (mode == 2)
            {
                if (number == 0) return 0;
                else if (number == 1) return 2;
                else if (number == 2) return 5;
                else if (number == 3) return 7;
                else return -1;
            }
            else if (mode == 3)
            {
                if (number == 0) return 0;
                else if (number == 1) return 1;
                else if (number == 2) return 2;
                else if (number == 3) return 6;
                else if (number == 4) return 3;
                else if (number == 5) return 4;
                else return -1;
            }
            else if (mode == 4)
            {
                if (number == 0) return 0;
                else if (number == 1) return 2;
                else if (number == 2) return 1;
                else if (number == 3) return 6;
                else if (number == 4) return 5;
                else if (number == 5) return 7;
                else if (number == 6) return 3;
                else if (number == 7) return 4;
                else return -1;
            }
            else
            {
                return 1;
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.instance.startButton.Text = "Start";
        }

        private void modeInitialize()
        {
            for (int i = 0; i < 8; i++)
            {
                arrowPictures[i].Visible = true;
                arrowPictures[i].Location = new Point(imageCenterPos[i, 0] - 32,
                imageCenterPos[i, 1] - 32);
                labels[i].Text = "";
            }
            if (mode == 1)
            {
                arrowPictures[0].Visible = false;
                arrowPictures[1].Visible = false;
                arrowPictures[2].Visible = false;
                arrowPictures[5].Visible = false;
                arrowPictures[6].Visible = false;
                arrowPictures[7].Visible = false;
            }
            else if (mode == 2)
            {
                arrowPictures[1].Visible = false;
                arrowPictures[3].Visible = false;
                arrowPictures[4].Visible = false;
                arrowPictures[6].Visible = false;
            }
            else if (mode == 3)
            {
                arrowPictures[5].Visible = false;
                arrowPictures[6].Visible = false;
                arrowPictures[7].Visible = false;
            }
            else if (mode == 4)
            {
                arrowPictures[6].Visible = false;
            }
            else
            {
                arrowPictures[0].Visible = false;
                arrowPictures[2].Visible = false;
                arrowPictures[3].Visible = false;
                arrowPictures[4].Visible = false;
                arrowPictures[5].Visible = false;
                arrowPictures[6].Visible = false;
                arrowPictures[7].Visible = false;
            }

            if(isAlwaysOne)
            {
                for (int i = 0; i < 8; i++)
                {
                    arrowPictures[i].Visible = true;
                    arrowPictures[i].Location = new Point(imageCenterPos[i, 0] - 32,
                    imageCenterPos[i, 1] - 32);
                    labels[i].Text = "";
                }
            }
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        public void moveImageTransparency(bool isVisible)
        {
            if (isVisible)
                pictureBox1.Visible = true;
            else
                pictureBox1.Visible = false;

        }
    }
}
