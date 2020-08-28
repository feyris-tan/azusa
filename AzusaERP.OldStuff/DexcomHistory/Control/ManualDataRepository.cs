using System;
using System.Collections.Generic;
using AzusaERP.OldStuff;
using moe.yo3explorer.azusa.DexcomHistory.Entity;

namespace moe.yo3explorer.azusa.DexcomHistory.Control
{
    static class ManualDataRepository
    {
        public static IEnumerable<ManualDataEntity> GetAllValues()
        {
            AzusaContext ctx = AzusaContext.GetInstance();
            return ctx.DatabaseDriver.Dexcom_GetAllManualGlucoseValues();
        }

        public static bool HasTimestamp(DateTime dt)
        {
            return AzusaContext.GetInstance().DatabaseDriver.Dexcom_ManualGlucoseValueTestForTimestamp(dt);
        }

        public static void StoreValue(DateTime dt, short value, string unit)
        {
            AzusaContext.GetInstance().DatabaseDriver.Dexcom_ManualGlucoseValueStore(dt, value, unit);
        }

        public static void EditValue(int id, byte be, byte novorapid, byte levemir, string note)
        {
            AzusaContext.GetInstance().DatabaseDriver.Dexcom_ManualGlucoseValueUpdate(id, be, novorapid, levemir, note);
        }
    }
}
