using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public enum AssetType
    {
        [Description("Retail")]
        Retail = 1,

        [Description("Office")]
        Office = 2,

        [Description("MF")]
        MultiFamily = 3,

        [Description("Indus")]
        Industrial = 4,

        [Description("MHP")]
        MHP = 5,

        [Description("Fuel/S")]
        ConvenienceStoreFuel = 6,

        [Description("Med")]
        Medical = 7,

        [Description("Mixed Use Commercial Property")]
        MixedUse = 8,

        //[Description("Commercial")]
        //Commercial = 9,

        [Description("Other")]
        Other = 10,

        [Description("Resort/Hotel/Motel Property")]
        Hotel = 11,

        [Description("Single Tenant Property (All Type)")]
        SingleTenantProperty = 12,

        [Description("Fractured Condominium Portfolio's")]
        FracturedCondominiumPortfolio = 13,

        [Description("Mini-Storage Property")]
        MiniStorageProperty = 14,

        [Description("Parking Garage Property")]
        ParkingGarageProperty = 15,

        [Description("Secured CRE Paper")]
        SecuredPaper = 16,

        [Description("Land")]
        Land = 17

    }
}
