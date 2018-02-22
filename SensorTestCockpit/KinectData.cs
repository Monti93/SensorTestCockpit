using CCT.NUI.Core;
using CCT.NUI.Core.Clustering;
using CCT.NUI.Core.Shape;
using CCT.NUI.KinectSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SensorTestCockpit
{
    class KinectData : IMeasurable, System.Collections.IEnumerable
    {
        public const int PIXEL_SIZE = 260;

        private IDataSourceFactory dataSourceFactory;
        private ClusterShapeDataSource handDataSource;
        private Form1 mainForm;
        private Shape shape;
        //public int[,] pixels { get; private set; }
        public bool noPixelChange {get; private set; }

        public KinectData(Form1 mainForm)
        {
            this.mainForm = mainForm;
            //this.pixels = new int[PIXEL_SIZE, PIXEL_SIZE];
            this.noPixelChange = true;
            
            IDataSourceFactory dataSourceFactory = new SDKDataSourceFactory();

            handDataSource = new ClusterShapeDataSource(dataSourceFactory.CreateClusterDataSource(new ClusterDataSourceSettings())); 

            handDataSource.NewDataAvailable += new NewDataHandler<ShapeCollection>(HandDataSource_NewDataAvailable);
            handDataSource.Start();
        }

        private void HandDataSource_NewDataAvailable(ShapeCollection data)
        {
            foreach (Shape shape in data.Shapes)
            {
                if (shape.Contour.Points.ElementAt(0).X < 170 || shape.Contour.Points.ElementAt(0).X > 430)
                    continue; //this shape is either no hand or not in correct position

                this.shape = shape;
                if (shape.PointCount > 0)
                    noPixelChange = false;

                /*
                this.ClearPixels();
                
                int posX;
                int posY;
                
                foreach (CCT.NUI.Core.Point point in shape.Points)
                {
                    posX = GetThresholdedValue(point.X);
                    posY = GetThresholdedValue(point.Y);
                    pixels[posX, posY] = 1;
                    noPixelChange = false;
                }
                */
                if (mainForm.isRecording)
                    return;

                var panel = mainForm.Controls.Find("kinectPanel", false)[0];
                if (panel == null)
                    return;
                
                panel.Invoke((MethodInvoker)delegate
                {
                    panel.Invalidate();
                });
            }
        }

        private int GetThresholdedValue(float val)
        {
            //Transform point values from kinect to array positions
            int returnVal = (int)val - 170;
            if (returnVal > 259)
            {
                returnVal = 259;
            }
            if (returnVal < 0)
            {
                returnVal = 0;
            }
            return returnVal;
        }

        /*
        private void ClearPixels()
        {
            for (int i = 0; i < pixels.GetLength(0); ++i)
            {
                for (int k = 0; k < pixels.GetLength(1); ++k)
                {
                    pixels[i, k] = 0;
                }
            }
        }
        */
        public string GetCsv()
        {

            StringBuilder returnString = new StringBuilder(10000);
            foreach (int pixelCoord in this)
            {
                returnString.AppendFormat("{0},", pixelCoord.ToString());
            }
            returnString.Remove(returnString.Length - 1, 1); //cut off last ,
            return returnString.ToString();

            /*
            //pixel[0,0],pixel[0,1],...,pixel[159,159]
            string returnString = this.pixels[0,0].ToString();
            for(int i=0; i<this.pixels.GetLength(0); ++i)
            {
                for (int k = 0; k < this.pixels.GetLength(1); ++k)
                {
                    returnString = string.Join(",", new String[] {returnString, this.pixels[i, k].ToString()});
                }
            }
            return returnString;
            */
        }

        public void CloseKinect()
        {
            handDataSource.Stop();
        }

        public IEnumerator GetEnumerator()
        {
            int posX;
            int posY;

            foreach (CCT.NUI.Core.Point point in shape.Points)
            {
                posX = GetThresholdedValue(point.X);
                posY = GetThresholdedValue(point.Y);
                //pixels[posX, posY] = 1;
                noPixelChange = false;
                yield return posX * PIXEL_SIZE + posY;
            }
        }
    }
}
