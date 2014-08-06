using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Hardware;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ProdactiveMovil.Services
{
    //public class StepDetector : ISensorEventListener
    //{

    //    private readonly static String TAG = "StepDetector";
    //    private float       Limit       = 10;
    //    private float[]     LastValues  = new float[3 * 2];
    //    private float[]     Scale       = new float[2];
    //    private float       YOffset;

    //    private float[]     LastDirections  = new float[3 * 2];
    //    private float[][]   LastExtremes    = { new float[3 * 2], new float[3 * 2] };
    //    private float[]     LastDiff        = new float[3 * 2];
    //    private int         LastMatch       = -1;

    //    //private readonly List<IStepListener> ListStepListeners = new List<IStepListener>();

    //    public StepDetector()
    //    {
    //        int h       = 480; // TODO: remove this constant
    //        YOffset     = h * 0.5f;
    //        Scale[0]    = -(h * 0.5f * (1.0f / (SensorManager.StandardGravity * 2)));
    //        Scale[1]    = -(h * 0.5f * (1.0f / (SensorManager.MagneticFieldEarthMax)));
    //    }

    //    public void SetSensitivity(float sensitivity)
    //    {
    //        Limit = sensitivity; // 1.97  2.96  4.44  6.66  10.00  15.00  22.50  33.75  50.62
    //    }

    //    //public void AddStepListener(IStepListener sl)
    //    //{
    //    //    ListStepListeners.Add(sl);
    //    //}

    //    public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
    //    {

    //    }

    //    public void OnSensorChanged(SensorEvent e)
    //    {
    //        Sensor sensor = e.Sensor;
    //        lock (this)
    //        {
    //            if (sensor.Type == SensorType.Orientation)
    //            {

    //            }
    //            else
    //            {
    //                int j = (sensor.Type == SensorType.Accelerometer) ? 1 : 0;
    //                if (j == 1)
    //                {
    //                    float vSum = 0;
    //                    for (int i = 0; i < 3; i++)
    //                    {
    //                        float vv = YOffset + e.Values[i] * Scale[j];
    //                        vSum += vv;
    //                    }
    //                    int k = 0;
    //                    float v = vSum / 3;

    //                    float direction = (v > LastValues[k] ? 1 : (v < LastValues[k] ? -1 : 0));
    //                    if (direction == -LastDirections[k])
    //                    {
    //                        // Direction changed
    //                        int extType = (direction > 0 ? 0 : 1); // minumum or maximum?
    //                        LastExtremes[extType][k] = LastValues[k];

    //                        float diff = Math.Abs(LastExtremes[extType][k] - LastExtremes[1 - extType][k]);

    //                        if (diff > Limit)
    //                        {

    //                            bool isAlmostAsLargeAsPrevious = diff > (LastDiff[k] * 2 / 3);
    //                            bool isPreviousLargeEnough = LastDiff[k] > (diff / 3);
    //                            bool isNotContra = (LastMatch != 1 - extType);

    //                            if (isAlmostAsLargeAsPrevious && isPreviousLargeEnough && isNotContra)
    //                            {
    //                                Log.Info(TAG, "step");
    //                                //foreach (IStepListener stepListener in ListStepListeners)
    //                                //{
    //                                //    stepListener.OnStep();
    //                                //}
    //                                LastMatch = extType;
    //                            }
    //                            else
    //                            {
    //                                LastMatch = -1;
    //                            }
    //                        }
    //                        LastDiff[k] = diff;
    //                    }
    //                    LastDirections[k] = direction;
    //                    LastValues[k] = v;
    //                }
    //            }
    //        }
    //    }



    //    public IntPtr Handle
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    public void Dispose()
    //    {

    //    }
    //}
}