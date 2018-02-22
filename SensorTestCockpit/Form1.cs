using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SensorTestCockpit
{
    public partial class Form1 : Form
    {
        private DataMerger merger;
        public bool isRecording;

        public Form1()
        {
            InitializeComponent();
            isRecording = false;
            merger = new DataMerger(this);
            merger.InitializeSensors();
        }

        private void OnKinectPaint(object sender, PaintEventArgs e)
        {
            if (merger == null)
                return;

            merger.PaintKinectData(sender, e);
        }

        private void OnMyoPaint(object sender, PaintEventArgs e)
        {
            if (merger == null)
                return;

            merger.PaintMyoData(sender, e);
        }

        private void OnPaintLeap(object sender, PaintEventArgs e)
        {
            if (merger == null)
                return;

            merger.PaintLeapData(sender, e);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (merger == null)
                return;

            merger.CloseDataMerger();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // start listening for Myo data
            merger.StartChannel();
        }

        private void OnStatusPaint(object sender, PaintEventArgs e)
        {
            if (merger == null)
                return;

            merger.PaintStatusData(sender, e);
        }
    }
}
