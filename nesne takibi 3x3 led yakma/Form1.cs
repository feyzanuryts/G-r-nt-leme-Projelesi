using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO.Ports;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;
using AForge.Imaging;


namespace nesne_takibi_3x3_led_yakma
{
    public partial class Form1 : Form
    {

        private FilterInfoCollection VideoCapTureDevices;
        private VideoCaptureDevice Finalvideo;
        SerialPort ardino = new SerialPort();

        int R;
        int G;
        int B;

      

       

        public Form1()
        {
            InitializeComponent();
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.DataSource = SerialPort.GetPortNames();
            int sayi = comboBox2.Items.Count;
            if (sayi == 0)
            {
                toolStripLabel1.Text = "Port Bulunamadı.Kontrol et!!";
                comboBox2.Enabled = false;
                button4.Enabled = false;
            }

            else
            {
                toolStripLabel1.Text = sayi + "Tane Port var";
            }

            VideoCapTureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo VideoCaptureDevice in VideoCapTureDevices)
            {

                comboBox1.Items.Add(VideoCaptureDevice.Name);

            }

            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Finalvideo = new VideoCaptureDevice(VideoCapTureDevices[comboBox1.SelectedIndex].MonikerString);
            Finalvideo.NewFrame += new NewFrameEventHandler(Finalvideo_NewFrame);
            Finalvideo.DesiredFrameRate = 30;
            Finalvideo.DesiredFrameSize = new Size(360, 360);
            Finalvideo.Start();
        }

        void Finalvideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            Bitmap image1 = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = image;


            if (radioButton1.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(215, 0, 0));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                nesnebul(image1);
            }
            if (radioButton3.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();

                filter.CenterColor = new RGB(Color.FromArgb(30, 144, 255));
                filter.Radius = 100;

                filter.ApplyInPlace(image1);

                nesnebul(image1);
            }
            if (radioButton2.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();

                filter.CenterColor = new RGB(Color.FromArgb(0, 255, 0));
                filter.Radius = 100;

                filter.ApplyInPlace(image1);

                nesnebul(image1);

            }
        }

        public void nesnebul(Bitmap image)
        {

            BlobCounter blobCounter = new BlobCounter();
            blobCounter.MinWidth = 5;
            blobCounter.MinHeight = 5;
            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;

            BitmapData objectsData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            Grayscale grayscaleFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            UnmanagedImage grayImage = grayscaleFilter.Apply(new UnmanagedImage(objectsData));
            image.UnlockBits(objectsData);
            blobCounter.ProcessImage(image);
            Rectangle[] rects = blobCounter.GetObjectsRectangles();
            Blob[] blobs = blobCounter.GetObjectsInformation();
            pictureBox2.Image = image;


            foreach (Rectangle recs in rects)

            {

                if (rects.Length > 0)
                {
                    Rectangle objectRect = rects[0];

                    Graphics g = pictureBox1.CreateGraphics();
                    using (Pen pen = new Pen(Color.FromArgb(250, 0, 0), 2))
                    {
                        g.DrawRectangle(pen, objectRect);
                    }

                    int objectX = objectRect.X + (objectRect.Width / 2);
                    int objectY = objectRect.Y + (objectRect.Height / 2);




                    String area = "";


                    if (objectX < 120 && objectY < 120)
                    {
                        area = "1.Bölge";
                        serialPort1.Write("2");

                    }
                    else if ((objectX > 120 && objectX < 240) && (objectY < 120))
                    {
                        area = "2.Bölge";
                        serialPort1.Write("3");
                    }
                    else if ((objectX > 240 && objectX < 360) && (objectY < 120))
                    {
                        area = "3.Bölge";
                        serialPort1.Write("4");
                    }
                    else if ((objectX < 120) && (objectY > 120 && objectY < 240))
                    {
                        area = "4.Bölge";
                        serialPort1.Write("5");
                    }
                    else if ((objectX > 120 && objectX < 240) && (objectY > 120 && objectY < 240))
                    {
                        area = "5.Bölge";
                        serialPort1.Write("6");
                    }
                    else if ((objectX > 240 && objectX < 360) && (objectY > 120 && objectY < 240))
                    {
                        area = "6.Bölge";
                        serialPort1.Write("7");
                    }
                    else if ((objectX < 120) && (objectY > 240 && objectY < 360))
                    {
                        area = "7.Bölge";
                        serialPort1.Write("8");
                    }
                    else if ((objectX > 120 && objectX < 240) && (objectY > 240 && objectY < 360))
                    {
                        area = "8.Bölge";
                        serialPort1.Write("9");
                    }
                    else if ((objectX > 240 ) && (objectY > 240))
                    {
                        area = "9.Bölge";
                        serialPort1.Write("10");
                    }

                    g.DrawString(objectX.ToString() + "X" + objectY.ToString() + area, new Font("Arial", 12), Brushes.Red, new System.Drawing.Point(1, 1));
                    g.Dispose();
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            R = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            G = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            B = trackBar3.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Finalvideo.IsRunning)
            {
                Finalvideo.Stop();

            }

            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBox2.SelectedItem.ToString();
            serialPort1.BaudRate = 9600;
            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                toolStripLabel1.Text = comboBox2.SelectedItem.ToString() + "portuna bağlandı";

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                ardino.Close();
                toolStripLabel1.Text = "Port bağlantısı kesildi ";
            }
            catch (Exception)
            {

                toolStripLabel1.Text = "İlk önce bağlan sonra bağlantıyı kes";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Finalvideo.IsRunning)
            {
                Finalvideo.Stop();

            }
        }

    }
}

