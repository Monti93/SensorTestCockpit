using MyoSharp.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorTestCockpit
{
    class MyoData : IMeasurable
    {
        public double roll { get; private set; }
        public double pitch { get; private set; }
        public double yaw { get; private set; }
        public int countEmgSets { get; private set; }
        public double[] maxEmg { get; private set; }
        public double[] sumEmg { get; private set; }

        public MyoData()
        {
            roll = 0;
            pitch = 0;
            yaw = 0;
            countEmgSets = 0;
            maxEmg = new double[8];
            sumEmg = new double[8];
        }

        public void Myo_OrientationDataAcquired(object sender, OrientationDataEventArgs e)
        {
            roll = e.Roll;
            pitch = e.Pitch;
            yaw = e.Yaw;
        }

        public void Myo_EmgDataAcquired(object sender, EmgDataEventArgs e)
        {
            ++countEmgSets;

            var data0 = Math.Abs(e.EmgData.GetDataForSensor(0));
            var data1 = Math.Abs(e.EmgData.GetDataForSensor(1));
            var data2 = Math.Abs(e.EmgData.GetDataForSensor(2));
            var data3 = Math.Abs(e.EmgData.GetDataForSensor(3));
            var data4 = Math.Abs(e.EmgData.GetDataForSensor(4));
            var data5 = Math.Abs(e.EmgData.GetDataForSensor(5));
            var data6 = Math.Abs(e.EmgData.GetDataForSensor(6));
            var data7 = Math.Abs(e.EmgData.GetDataForSensor(7));

            sumEmg[0] += data0;
            sumEmg[1] += data1;
            sumEmg[2] += data2;
            sumEmg[3] += data3;
            sumEmg[4] += data4;
            sumEmg[5] += data5;
            sumEmg[6] += data6;
            sumEmg[7] += data7;

            maxEmg[0] = data0 > maxEmg[0] ? data0 : maxEmg[0];
            maxEmg[1] = data1 > maxEmg[1] ? data1 : maxEmg[1];
            maxEmg[2] = data2 > maxEmg[2] ? data2 : maxEmg[2];
            maxEmg[3] = data3 > maxEmg[3] ? data3 : maxEmg[3];
            maxEmg[4] = data4 > maxEmg[4] ? data4 : maxEmg[4];
            maxEmg[5] = data5 > maxEmg[5] ? data5 : maxEmg[5];
            maxEmg[6] = data6 > maxEmg[6] ? data6 : maxEmg[6];
            maxEmg[7] = data7 > maxEmg[7] ? data7 : maxEmg[7];
        }

        public void ClearIndexedData(int idx)
        {
            sumEmg[idx] = 0;
            maxEmg[idx] = 0;
        }

        public void ClearCounter()
        {
            countEmgSets = 0;
        }

        public string GetCsv()
        {
            // roll, pitch, yaw, countEmg, maxEmg[0-7], avgEmg[0-7]
            string returnString = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}",
                Math.Round(roll, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(pitch, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(yaw, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                countEmgSets,
                maxEmg[0],
                maxEmg[1],
                maxEmg[2],
                maxEmg[3],
                maxEmg[4],
                maxEmg[5],
                maxEmg[6],
                maxEmg[7],
                Math.Round(sumEmg[0] / countEmgSets, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(sumEmg[1] / countEmgSets, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(sumEmg[2] / countEmgSets, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(sumEmg[3] / countEmgSets, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(sumEmg[4] / countEmgSets, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(sumEmg[5] / countEmgSets, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(sumEmg[6] / countEmgSets, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(sumEmg[7] / countEmgSets, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
                );

            ClearCounter();
            for (int i = 0; i < 8; ++i)
                ClearIndexedData(i);

            return returnString;
        }
    }
}
