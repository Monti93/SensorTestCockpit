using Leap;
using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SensorTestCockpit
{
    class DataMerger
    {
        private const int BUFFER_LINES = 20;
        private const int TIMER_INTERVAL = 500;
        private const int COUNT_MAX = 4;
        private const string FILENAME = "C://Users//D059415//Documents//Uni//Master//Studienarbeit//Versuche//Abtastgeschwindigkeit//Versuch 2 Geschwindigkeiten von 1000 bis 100ms//500ms_2.csv";
        private const int TARGET_LINES = 100;

        private Form1 mainForm;
        private MyoData myoData;
        private LeapData leapData;
        private KinectData kinectData;
        private System.Timers.Timer timer;
        private string[] writeBuffer;
        private int lineCounter;
        private readonly IChannel channel;
        private readonly IHub hub;
        private int startCtr;
        private int totalLines;

        public DataMerger(Form1 mainForm)
        {
            this.mainForm = mainForm;
            myoData = new MyoData();
            leapData = new LeapData();
            kinectData = new KinectData(mainForm);

            writeBuffer = new string[BUFFER_LINES];
            lineCounter = 0;
            startCtr = -1; // kinect has not detected a hand yet
            totalLines = 0;

            channel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create()));

            hub = Hub.Create(channel);
            hub.MyoConnected += Hub_MyoConnected;
            hub.MyoDisconnected += Hub_MyoDisconnected;
        }

        public void StartChannel()
        {
            channel.StartListening();
        }

        public void InitializeSensors()
        {
            //*************************************
            //*          Initialize Myo           *
            //*************************************
            // create a hub that will manage Myo devices for us
            /*
            channel = Channel.Create(
                ChannelDriver.Create(ChannelBridge.Create(),
                MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create())));
            using (var hub = Hub.Create(channel))
            {
                // listen for when the Myo connects
                hub.MyoConnected += (sender, e) =>
                {
                    e.Myo.Vibrate(VibrationType.Long);
                    e.Myo.OrientationDataAcquired += myoData.Myo_OrientationDataAcquired;
                    e.Myo.EmgDataAcquired += myoData.Myo_EmgDataAcquired;
                    e.Myo.SetEmgStreaming(true);
                };
                
                // listen for when the Myo disconnects
                hub.MyoDisconnected += (sender, e) =>
                {
                    e.Myo.OrientationDataAcquired -= myoData.Myo_OrientationDataAcquired;
                    e.Myo.EmgDataAcquired -= myoData.Myo_EmgDataAcquired;
                    e.Myo.SetEmgStreaming(false);
                };
                */

            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(ElapsedTimeEvent);
            timer.Interval = TIMER_INTERVAL;
            //timer.Start();


        }

        private void Hub_MyoDisconnected(object sender, MyoEventArgs e)
        {
            e.Myo.OrientationDataAcquired -= myoData.Myo_OrientationDataAcquired;
            e.Myo.EmgDataAcquired -= myoData.Myo_EmgDataAcquired;
            e.Myo.SetEmgStreaming(false);
            timer.Stop();
        }

        private void Hub_MyoConnected(object sender, MyoEventArgs e)
        {
            e.Myo.Vibrate(VibrationType.Long);
            e.Myo.OrientationDataAcquired += myoData.Myo_OrientationDataAcquired;
            e.Myo.EmgDataAcquired += myoData.Myo_EmgDataAcquired;
            e.Myo.SetEmgStreaming(true);
            timer.Start();
        }


        private void ElapsedTimeEvent(object source, ElapsedEventArgs e)
        {
            if(myoData == null || leapData == null || kinectData == null || startCtr < 0)
            {
                return;
            }


            if (mainForm.isRecording)
            {
                writeBuffer[lineCounter++] = string.Format("{0},{1},{2}", myoData.GetCsv(), leapData.GetCsv(), kinectData.GetCsv());
                totalLines++;
                if (lineCounter >= BUFFER_LINES)
                {
                    // save buffered data...
                    timer.Stop();
                    if(totalLines <= TARGET_LINES)
                    {
                        WriteCsv();
                    }
                    else
                    {
                        var statusPanel2 = mainForm.Controls.Find("statusPanel", false)[0];
                        if (statusPanel2 == null)
                            return;

                        statusPanel2.Invoke((MethodInvoker)delegate
                        {
                            statusPanel2.Invalidate();
                        });
                        return;
                    }
                    lineCounter = 0;
                    startCtr = 0;
                    mainForm.isRecording = false;
                    timer.Start();
                }
                return;
            }

            //Write output to screen (trigger paint events)...

            var myoPanel = mainForm.Controls.Find("myoPanel", false)[0];
            if (myoPanel == null)
                return;

            myoPanel.Invoke((MethodInvoker)delegate
            {
                myoPanel.Invalidate();
            });

            var leapPanel = mainForm.Controls.Find("leapPanel", false)[0];
            if (leapPanel == null)
                return;

            leapPanel.Invoke((MethodInvoker)delegate
            {
                leapPanel.Invalidate();
            });

            if (GetCorrectedCount() > COUNT_MAX)
                return;

            startCtr++;
            var statusPanel = mainForm.Controls.Find("statusPanel", false)[0];
            if (statusPanel == null)
                return;

            statusPanel.Invoke((MethodInvoker)delegate
            {
                statusPanel.Invalidate();
            });

        }

        private int GetCorrectedCount()
        {
            return Convert.ToInt32(TIMER_INTERVAL / 1000.0 * startCtr);
        }

        private void WriteCsv()
        {
            using (StreamWriter writer = new StreamWriter(FILENAME, append: true)) //(StreamWriter writer = File.CreateText("newfile.txt"))
            {
                for (int i = 0; i < BUFFER_LINES; ++i)
                {
                    writer.WriteLine(writeBuffer[i]);
                }
            }
        }

        private void DrawBoarders(PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black, 2);
            e.Graphics.DrawLine(pen, 200, 200, 200, 400);
            e.Graphics.DrawLine(pen, 200, 200, 400, 200);
            e.Graphics.DrawLine(pen, 200, 400, 400, 400);
            e.Graphics.DrawLine(pen, 400, 200, 400, 400);

            pen = new Pen(Color.Red, 2);

            e.Graphics.DrawLine(pen, 170, 170, 170, 430);
            e.Graphics.DrawLine(pen, 170, 170, 430, 170);
            e.Graphics.DrawLine(pen, 170, 430, 430, 430);
            e.Graphics.DrawLine(pen, 430, 170, 430, 430);
        }

        private void DrawString(string text, float x, float y, Graphics pane)
        {
            Font drawFont = new Font("Arial", 12);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            pane.DrawString(text, drawFont, drawBrush, x, y, new StringFormat());
            drawFont.Dispose();
            drawBrush.Dispose();
        }

        public void PaintStatusData(object sender, PaintEventArgs e)
        {
            if(totalLines >= TARGET_LINES)
            {
                DrawString("Recording finished", 10, 10, e.Graphics);
                startCtr = 0;
            }
            else if (startCtr < 0)
            {
                DrawString("Place right hand in front of Kinect...", 10, 10, e.Graphics);
            }
            else if (GetCorrectedCount() < COUNT_MAX)
            {
                DrawString(string.Format("Recording in {0}", COUNT_MAX - GetCorrectedCount()), 10, 10, e.Graphics);
            }
            else
            {
                DrawString("Currenctly recording...", 10, 10, e.Graphics);
                mainForm.isRecording = true;
            }
        }

        public void PaintKinectData(object sender, PaintEventArgs e)
        {
            if (kinectData == null)
                return;
            if (kinectData.noPixelChange)
                return;
            if (startCtr < 0)
                startCtr = 0; // start recording 

            this.DrawBoarders(e);
            
            foreach(int pixelCoord in kinectData)
            {
                e.Graphics.FillRectangle((Brush)Brushes.Black, pixelCoord/KinectData.PIXEL_SIZE+170, pixelCoord%KinectData.PIXEL_SIZE + 170, 2, 2);
            }

            /*
            for (int i = 0; i < kinectData.pixels.GetLength(0); ++i)
            {
                for (int k = 0; k < kinectData.pixels.GetLength(1); ++k)
                {
                    if (kinectData.pixels[i, k] == 0)
                        continue;
                    e.Graphics.FillRectangle((Brush)Brushes.Black, i + 170, k + 170, 2, 2);
                }
            }
            */
        }

        public void PaintLeapData(object sender, PaintEventArgs e)
        {
            if (leapData == null)
                return;

            leapData.retrieveHandData();
            int height = 0;
            DrawString("-----------------------------------------------------------------------------------------------", 10, height, e.Graphics);
            height += 20;
            DrawString(string.Format("Hand Direction: {0}", leapData.handDirection), 10, height, e.Graphics);
            for (int i = 0; i < leapData.fingerDirections.Length; ++i)
            {
                height += 20;
                DrawString(string.Format("Finger Type: {0} --> Direction: {1}", (Finger.FingerType)i, leapData.fingerDirections[i]), 10, height, e.Graphics);
                for (int k = 0; k < leapData.boneDirections.GetLength(1); ++k)
                {
                    if (i == (int)Finger.FingerType.TYPE_THUMB && k == (int)Bone.BoneType.TYPE_METACARPAL)
                    {
                        continue; // always null
                    }
                    height += 20;
                    DrawString(string.Format("Bone Type: {0} --> Direction: {1}", (Bone.BoneType)k, leapData.boneDirections[i, k]), 10, height, e.Graphics);
                }
                height += 20;
                DrawString("-----------------------------------------------------------------------------------------------", 10, height, e.Graphics);
            }
        }

        public void PaintMyoData(object sender, PaintEventArgs e)
        {
            if (myoData == null)
                return;

            DrawString(string.Format("Roll: {0,5} | Pitch: {1,5} | Yaw: {2,5}", Math.Round(myoData.roll, 2), Math.Round(myoData.pitch, 2), Math.Round(myoData.yaw, 2)), 10, 10, e.Graphics);

            for(int i=0; i<8; ++i)
            {
                FlushEmgIndex(40 + i * 30, i, e.Graphics);
            }

            myoData.ClearCounter();
        }

        private void FlushEmgIndex(int height, int idx, Graphics pane)
        {
            //Console.Write("(EMG {0}) --> Max Value: {1,3} | Average: {2,3}", idx, maxEmg[idx], Math.Round(sumEmg[idx] / countEmgSets, 2));
            DrawString(string.Format("(EMG {0}) --> Max Value: {1,3} | Average: {2,3}", idx, myoData.maxEmg[idx], Math.Round(myoData.sumEmg[idx] / myoData.countEmgSets, 2)), 10, height, pane);
            myoData.ClearIndexedData(idx);
        }

        public void CloseDataMerger()
        {
            if(timer != null)
                timer.Stop();

            if(kinectData != null)
                kinectData.CloseKinect();
        }
    }
}
