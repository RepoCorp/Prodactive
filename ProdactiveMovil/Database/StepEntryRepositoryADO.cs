using System;
using Android.Util;

namespace ProdactiveMovil.Database
{
    public class StepEntryRepositoryADO
    {
        internal static StepEntry GetStepEntry(DateTime time)
        {
            Log.Debug("GetStepEntry", String.Format("Time {0}", time));
            return new StepEntry();
        }

        internal static int GetStepEntries()
        {
            return 0;
        }

        internal static int SaveStepEntry(StepEntry item)
        {
            Log.Debug("SaveStepEntry", String.Format("Time {0} - Steps {1}", item.Date, item.Steps));
            return 1;
        }

        internal static int DeleteStepEntry(int id)
        {
            return 1;
        }
    }
}