using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorTestCockpit
{
    class LeapData : IMeasurable
    {
        private Controller controller;
        private Frame frame;

        public Vector[] fingerDirections { private set; get; }
        public Vector[,] boneDirections { private set; get; }
        public Vector handDirection { private set; get; }

        public LeapData()
        {
            controller = new Controller();
            fingerDirections = new Vector[5];
            boneDirections = new Vector[5, 4];
        }

        public void retrieveHandData()
        {
            clearValues();
            frame = controller.Frame();
            foreach (Hand hand in frame.Hands)
            {
                if (!hand.IsRight)
                {
                    continue;
                }
                handDirection = hand.Direction;

                foreach (Finger finger in hand.Fingers)
                {
                    if (finger.Type != Finger.FingerType.TYPE_UNKNOWN)
                    {
                        fingerDirections[(int)finger.Type] = finger.Direction;
                        foreach (Bone bone in finger.bones)
                        {
                            if (bone.Type != Bone.BoneType.TYPE_INVALID)
                            {
                                boneDirections[(int)finger.Type, (int)bone.Type] = bone.Direction;
                            }
                        }
                    }
                }
            }
        }

        public string GetCsv()
        {
            // handDirection(x,y,z), from thumb to pinky: fingerDirection(x,y,z) and then from metacarpal to distal: boneDirection(x,y,z)

            retrieveHandData();

            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49},{50},{51},{52},{53},{54},{55},{56},{57},{58},{59},{60},{61},{62},{63},{64},{65},{66},{67},{68},{69},{70},{71},{72},{73},{74}",
                Math.Round(handDirection.x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(handDirection.y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(handDirection.z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_THUMB].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_THUMB].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_THUMB].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                //boneDirections[(int)Finger.FingerType.TYPE_THUMB, (int)Bone.BoneType.TYPE_METACARPAL].x, Always null
                //boneDirections[(int)Finger.FingerType.TYPE_THUMB, (int)Bone.BoneType.TYPE_METACARPAL].y, Always null
                //boneDirections[(int)Finger.FingerType.TYPE_THUMB, (int)Bone.BoneType.TYPE_METACARPAL].z, Always null
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_THUMB, (int)Bone.BoneType.TYPE_PROXIMAL].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_THUMB, (int)Bone.BoneType.TYPE_PROXIMAL].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_THUMB, (int)Bone.BoneType.TYPE_PROXIMAL].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_THUMB, (int)Bone.BoneType.TYPE_INTERMEDIATE].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_THUMB, (int)Bone.BoneType.TYPE_INTERMEDIATE].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_THUMB, (int)Bone.BoneType.TYPE_INTERMEDIATE].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_THUMB, (int)Bone.BoneType.TYPE_DISTAL].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_THUMB, (int)Bone.BoneType.TYPE_DISTAL].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_THUMB, (int)Bone.BoneType.TYPE_DISTAL].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_INDEX].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_INDEX].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_INDEX].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_INDEX, (int)Bone.BoneType.TYPE_METACARPAL].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_INDEX, (int)Bone.BoneType.TYPE_METACARPAL].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_INDEX, (int)Bone.BoneType.TYPE_METACARPAL].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_INDEX, (int)Bone.BoneType.TYPE_PROXIMAL].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_INDEX, (int)Bone.BoneType.TYPE_PROXIMAL].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_INDEX, (int)Bone.BoneType.TYPE_PROXIMAL].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_INDEX, (int)Bone.BoneType.TYPE_INTERMEDIATE].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_INDEX, (int)Bone.BoneType.TYPE_INTERMEDIATE].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_INDEX, (int)Bone.BoneType.TYPE_INTERMEDIATE].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_INDEX, (int)Bone.BoneType.TYPE_DISTAL].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_INDEX, (int)Bone.BoneType.TYPE_DISTAL].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_INDEX, (int)Bone.BoneType.TYPE_DISTAL].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_MIDDLE].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_MIDDLE].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_MIDDLE].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_MIDDLE, (int)Bone.BoneType.TYPE_METACARPAL].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_MIDDLE, (int)Bone.BoneType.TYPE_METACARPAL].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_MIDDLE, (int)Bone.BoneType.TYPE_METACARPAL].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_MIDDLE, (int)Bone.BoneType.TYPE_PROXIMAL].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_MIDDLE, (int)Bone.BoneType.TYPE_PROXIMAL].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_MIDDLE, (int)Bone.BoneType.TYPE_PROXIMAL].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_MIDDLE, (int)Bone.BoneType.TYPE_INTERMEDIATE].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_MIDDLE, (int)Bone.BoneType.TYPE_INTERMEDIATE].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_MIDDLE, (int)Bone.BoneType.TYPE_INTERMEDIATE].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_MIDDLE, (int)Bone.BoneType.TYPE_DISTAL].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_MIDDLE, (int)Bone.BoneType.TYPE_DISTAL].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_MIDDLE, (int)Bone.BoneType.TYPE_DISTAL].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_RING].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_RING].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_RING].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_RING, (int)Bone.BoneType.TYPE_METACARPAL].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_RING, (int)Bone.BoneType.TYPE_METACARPAL].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_RING, (int)Bone.BoneType.TYPE_METACARPAL].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_RING, (int)Bone.BoneType.TYPE_PROXIMAL].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_RING, (int)Bone.BoneType.TYPE_PROXIMAL].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_RING, (int)Bone.BoneType.TYPE_PROXIMAL].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_RING, (int)Bone.BoneType.TYPE_INTERMEDIATE].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_RING, (int)Bone.BoneType.TYPE_INTERMEDIATE].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_RING, (int)Bone.BoneType.TYPE_INTERMEDIATE].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_RING, (int)Bone.BoneType.TYPE_DISTAL].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_RING, (int)Bone.BoneType.TYPE_DISTAL].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_RING, (int)Bone.BoneType.TYPE_DISTAL].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_PINKY].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_PINKY].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(fingerDirections[(int)Finger.FingerType.TYPE_PINKY].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_PINKY, (int)Bone.BoneType.TYPE_METACARPAL].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_PINKY, (int)Bone.BoneType.TYPE_METACARPAL].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_PINKY, (int)Bone.BoneType.TYPE_METACARPAL].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_PINKY, (int)Bone.BoneType.TYPE_PROXIMAL].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_PINKY, (int)Bone.BoneType.TYPE_PROXIMAL].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_PINKY, (int)Bone.BoneType.TYPE_PROXIMAL].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_PINKY, (int)Bone.BoneType.TYPE_INTERMEDIATE].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_PINKY, (int)Bone.BoneType.TYPE_INTERMEDIATE].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_PINKY, (int)Bone.BoneType.TYPE_INTERMEDIATE].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_PINKY, (int)Bone.BoneType.TYPE_DISTAL].x, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_PINKY, (int)Bone.BoneType.TYPE_DISTAL].y, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")),
                Math.Round(boneDirections[(int)Finger.FingerType.TYPE_PINKY, (int)Bone.BoneType.TYPE_DISTAL].z, 2).ToString(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
            );            
        }

        private void clearValues()
        {
            handDirection = new Vector(0, 0, 0);
            for (int i = 0; i < fingerDirections.Length; ++i)
            {
                fingerDirections[i] = new Vector(0, 0, 0);
            }
            for (int i = 0; i < boneDirections.GetLength(0); ++i)
            {
                for (int k = 0; k < boneDirections.GetLength(1); ++k)
                {
                    boneDirections[i, k] = new Vector(0, 0, 0);
                }
            }
        }

    }
}
