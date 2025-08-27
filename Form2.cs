using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DarrenLee.Media;


namespace K8055Demo

{
    public partial class Form2 : Form
    {
        //_____________Kamera________________
        int count = 0;
        string datnamstr;
        Camera myCamera = new Camera();
        //_____________Steuerung_____________

        int Data1, Data2;
        int n = 0;
        float clicks = 0;
        int startwert = 0;
        int endwert = 0;
        int counteroneold = 0;
        int countertwoold = 0;
        int counteronenew = 0;
        int countertwonew = 0;
        int counteroneoldtime = 0;
        int countertwooldtime = 0;
        int counteronenewtime = 0;
        int countertwonewtime = 0;
        int counteronetimediff = 0;
        int countertwotimediff = 0;
        float avrgspeed = 0;
        float counteronenewspeed = 0;
        float countertwonewspeed = 0;
        int timeCs = 0;

        public Form2()
        {
            InitializeComponent();

            GetInfo();
            //bild();
            myCamera.OnFrameArrived += MyCamera_OnFrameArrived;

        }

        [DllImport("k8055d.dll")]
        public static extern int OpenDevice(int CardAddress);

        [DllImport("k8055d.dll")]
        public static extern void CloseDevice();

        [DllImport("k8055d.dll")]
        public static extern int ReadAnalogChannel(int Channel);

        [DllImport("k8055d.dll")]
        public static extern void ReadAllAnalog(ref int Data1, ref int Data2);

        [DllImport("k8055d.dll")]
        public static extern void OutputAnalogChannel(int Channel, int Data);

        [DllImport("k8055d.dll")]
        public static extern void OutputAllAnalog(int Data1, int Data2);

        [DllImport("k8055d.dll")]
        public static extern void ClearAnalogChannel(int Channel);

        [DllImport("k8055d.dll")]
        public static extern void SetAllAnalog();

        [DllImport("k8055d.dll")]
        public static extern void ClearAllAnalog();

        [DllImport("k8055d.dll")]
        public static extern void SetAnalogChannel(int Channel);

        [DllImport("k8055d.dll")]
        public static extern void WriteAllDigital(int Data);

        [DllImport("k8055d.dll")]
        public static extern void ClearDigitalChannel(int Channel);

        [DllImport("k8055d.dll")]
        public static extern void ClearAllDigital();

        [DllImport("k8055d.dll")]
        public static extern void SetDigitalChannel(int Channel);

        [DllImport("k8055d.dll")]
        public static extern void SetAllDigital();

        [DllImport("k8055d.dll")]
        public static extern bool ReadDigitalChannel(int Channel);

        [DllImport("k8055d.dll")]
        public static extern int ReadAllDigital();

        [DllImport("k8055d.dll")]
        public static extern int ReadCounter(int CounterNr);

        [DllImport("k8055d.dll")]
        public static extern void ResetCounter(int CounterNr);

        [DllImport("k8055d.dll")]
        public static extern void SetCounterDebounceTime(int CounterNr, int DebounceTime);

        [DllImport("k8055d.dll")]
        public static extern int Version();

        [DllImport("k8055d.dll")]
        public static extern int SearchDevices();

        [DllImport("k8055d.dll")]
        public static extern int SetCurrentDevice(int lngCardAddress);



        private void GetInfo()
        {
            var cameraDevices = myCamera.GetCameraSources();
            var cameraResolutions = myCamera.GetSupportedResolutions();

            foreach (var d in cameraDevices)
                comboBox1.Items.Add(d);
            
            foreach (var r in cameraResolutions)
                comboBox2.Items.Add(r);

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;

        }


        private void MyCamera_OnFrameArrived(object source, FrameArrivedEventArgs e)
        {
            Image img = e.GetFrame();
            pictureBox1.Image = img;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            myCamera.ChangeCamera(comboBox1.SelectedIndex);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            myCamera.Start(comboBox2.SelectedIndex);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            myCamera.Stop();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            string filename = datnamstr + @"\" + "Image" + count.ToString();
            myCamera.Capture(filename);
            count++;             
        }

        public void Bild()
        {
            string filename = datnamstr + @"\" + "Image" + count.ToString();
            myCamera.Capture(filename);
            count++;
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.Description = "+++ Ordner auswählen +++";
            fbd.ShowNewFolderButton = true;

            if(fbd.ShowDialog() == DialogResult.OK)
            {
                datnamstr = @fbd.SelectedPath;               
                button1.Enabled = true;
                button2.BackColor = Color.LightGreen;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int device = OpenDevice(0);
            if (device == 0)
            {
                button3.BackColor = Color.LightGreen;
                button10.Enabled = true;

            }
            else
            {
                button3.BackColor = Color.Red;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetAllDigital();        
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearAllDigital();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ClearAllAnalog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ResetCounter(1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ResetCounter(2);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            aGauge1.Value = trackBar1.Value;
            label10.Text = trackBar1.Value.ToString();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            aGauge2.Value = trackBar2.Value;
            label11.Text = trackBar2.Value.ToString();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            aGauge3.Value = trackBar3.Value;
            label12.Text = trackBar3.Value.ToString();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            aGauge4.Value = (trackBar4.Value / 10.0f);
            label13.Text = (trackBar4.Value / 10.0f).ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {

            Bild();

            timer1.Interval = trackBar3.Value + 1;
            //Rampe

            startwert = trackBar1.Value;
            endwert = trackBar2.Value;
            // Start und Endwerte

            ResetCounter(1);
            ResetCounter(2);
            clicks = trackBar4.Value * 0.407f;
            // Endbedingung Länge

            timer1.Start();
            // Laufroutine starten

            timeCs = 0;
            counteroneold = 0;
            countertwoold = 0;
            counteronenew = 0;
            countertwonew = 0;
            counteroneoldtime = 0;
            countertwooldtime = 0;
            counteronenewtime = 0;
            countertwonewtime = 0;
            counteronetimediff = 0;
            countertwotimediff = 0;
            avrgspeed = 0;
            // counteroneoldspeed = 0;
            // countertwooldspeed = 0;
            counteronenewspeed = 0;
            countertwonewspeed = 0;
            timer2.Start();
            // Geschwindigkeitsmessung starten
            
        }

        
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (startwert >= endwert)
            {

                OutputAnalogChannel(1, startwert);
                progressBar1.Value = startwert;
                label5.Text = (startwert).ToString();
                aGauge5.Value = (ReadCounter(1) / 4.06f);
                label19.Text = (ReadCounter(1) / 4.06f).ToString("00.0");
                TextBox1.Text = ReadCounter(1).ToString();
                TextBox2.Text = ReadCounter(2).ToString();  

                if (ReadCounter(1) > clicks)
                {
                    startwert = -1;
                    ClearAllAnalog();
                    timer1.Stop();
                    progressBar1.Value = 0;
                    label5.Text = (0).ToString();
                }
                startwert--;

            }
            else
            {
                ClearAllAnalog();
                timer1.Stop();
                progressBar1.Value = 0;
                label5.Text = (0).ToString();
            }

        }



        private void timer2_Tick(object sender, EventArgs e)
        {
            timeCs++;

            if (ReadCounter(1) != counteroneold)
            {
                counteronenew = ReadCounter(1);
                counteronenewtime = timeCs;
                counteronetimediff = counteronenewtime - counteroneoldtime;
                counteronenewspeed = (24.50f / counteronetimediff);
                label20.Text = (counteronenewspeed).ToString("0.00");
                counteroneold = counteronenew;
                counteroneoldtime = counteronenewtime;
                // avrgspeed = (counteronenewspeed + countertwonewspeed) / 2;
                // aGauge6.Value = ((int) avrgspeed) * 1000;
            }
            if (ReadCounter(2) != countertwoold)
            {
                countertwonew = ReadCounter(2);
                countertwonewtime = timeCs;
                countertwotimediff = countertwonewtime - countertwooldtime;
                countertwonewspeed = (24.50f / countertwotimediff);
                label21.Text = (countertwonewspeed).ToString("0.00");
                countertwoold = countertwonew;
                countertwooldtime = countertwonewtime;
                avrgspeed = (counteronenewspeed + countertwonewspeed) / 2;
                aGauge6.Value = (avrgspeed * 1000);
            }
        }

    }
}
