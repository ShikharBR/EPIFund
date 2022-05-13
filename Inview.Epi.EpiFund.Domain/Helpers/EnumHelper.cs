using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.Helpers
{
    public static class EnumHelper
    {
        public static string GetEnumDescription(System.Enum value)
        {
            try
            {

                FieldInfo fi = value.GetType().GetField(value.ToString());
                if (fi != null)
                {
                    DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attributes != null && attributes.Length > 0)
                        return attributes[0].Description;
                    else
                        return value.ToString();
                }
                return value.ToString();
            }
            catch
            {
                return "";
            }
        }
        public static string GetAbbreviation(this AssetType assetType)
        {
            switch (assetType)
            {
                case AssetType.Retail:
                    return "Ret";
                case AssetType.Office:
                    return "Off";
                case AssetType.MultiFamily:
                    return "MF";
                case AssetType.Industrial:
                    return "Ind";
                case AssetType.MHP:
                    return "MHP";
                case AssetType.ConvenienceStoreFuel:
                    return "FS";
                case AssetType.Medical:
                    return "Med";
                case AssetType.MixedUse:
                    return "Mix";
                case AssetType.Hotel:
                    return "Hot";
                case AssetType.FracturedCondominiumPortfolio:
                    return "FCP";
                case AssetType.ParkingGarageProperty:
                    return "PG";
                case AssetType.MiniStorageProperty:
                    return "MS";
                case AssetType.Land:
                    return "Land";
                case AssetType.SecuredPaper:
                    return "Note";
                case AssetType.Other:
                    return "OT";
                default:
                    return EnumHelper.GetEnumDescription(assetType);
            }
        }

        public static string GetAssetTypeShorthand(AssetType assetType)
        {
            switch (assetType)
            {
                case AssetType.Retail:
                    return "Retail";
                case AssetType.Office:
                    return "Office";
                case AssetType.MultiFamily:
                    return "Multi Family";
                case AssetType.Industrial:
                    return "Industrial";
                case AssetType.MHP:
                    return "MHP";
                case AssetType.ConvenienceStoreFuel:
                    return "Fuel Service Retail";
                case AssetType.Medical:
                    return "Medical";
                case AssetType.MixedUse:
                    return "Mixed Use";
                case AssetType.Hotel:
                    return "Hotel";
                case AssetType.FracturedCondominiumPortfolio:
                    return "Fractured Condominium";
                case AssetType.ParkingGarageProperty:
                    return "Parking Garage";
                case AssetType.MiniStorageProperty:
                    return "Mini-Storage";
                case AssetType.Land:
                    return "Land";
                case AssetType.SecuredPaper:
                    return "Note";
                case AssetType.Other:
                    return "Other";
                default:
                    return EnumHelper.GetEnumDescription(assetType);
            }
        }

        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(System.Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = System.Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)System.Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }

    }

    
}
