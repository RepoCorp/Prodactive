using System;
using System.Collections.Generic;

namespace ProdactiveMovil.Database
{
    public static class StepEntryManager
    {

        static StepEntryManager()
        {
        }

        public static StepEntry GetStepEntry(DateTime time)
        {
            return StepEntryRepositoryADO.GetStepEntry(time);
        }

        public static IList<StepEntry> GetStepEntries()
        {
            return new List<StepEntry>(StepEntryRepositoryADO.GetStepEntries());
        }

        public static int SaveStepEntry(StepEntry item)
        {
            return StepEntryRepositoryADO.SaveStepEntry(item);
        }

        public static int DeleteStepEntry(int id)
        {
            return StepEntryRepositoryADO.DeleteStepEntry(id);
        }
    }
}