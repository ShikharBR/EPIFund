using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace Inview.Epi.EpiFund.Business
{
    public class Helper : DbContext
    {
        public virtual List<HoldingCompanyList> GetHoldingCompany(ManageHoldingCompanyModel model)
        {

            try
            {
                DataTable dataTable = new DataTable();
                var cs = ConfigurationManager.ConnectionStrings["EPIRepository"].ConnectionString;

                using (SqlConnection Connection = new SqlConnection(cs))
                {
                    // Open connection
                    Connection.Open();

                    // Create command from params / SP
                    SqlCommand cmd = new SqlCommand("GetHoldingCompany", Connection);

                    // Add parameters
                        cmd.Parameters.Add("@HCName", SqlDbType.VarChar).Value = model.HCName;

                    if (model.ISRA)
                        cmd.Parameters.Add("@ISRA", SqlDbType.Bit).Value = model.ISRA;

                        cmd.Parameters.Add("@HCEmail", SqlDbType.VarChar).Value = model.HCEmail;
                        cmd.Parameters.Add("@HCFirstName", SqlDbType.VarChar).Value = model.HCFirstName;
                        cmd.Parameters.Add("@HCLastName", SqlDbType.VarChar).Value = model.HCLastName;
                        cmd.Parameters.Add("@LinkedInurl", SqlDbType.VarChar).Value = model.LinkedInurl;
                        cmd.Parameters.Add("@Facebookurl", SqlDbType.VarChar).Value = model.Facebookurl;
                        cmd.Parameters.Add("@Instagramurl", SqlDbType.VarChar).Value = model.Instagramurl;
                        cmd.Parameters.Add("@Twitterurl", SqlDbType.VarChar).Value = model.Twitterurl;


                    //add asset filter
                        cmd.Parameters.Add("@AssetNumber", SqlDbType.VarChar).Value = model.AssetNumber;
                        cmd.Parameters.Add("@AssetName", SqlDbType.VarChar).Value = model.AssetName;
                        cmd.Parameters.Add("@AddressLine1", SqlDbType.VarChar).Value = model.AddressLine1;
                        cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = model.City;
                        cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = model.State;
                        cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar).Value = model.ZipCode;
                        cmd.Parameters.Add("@ApnNumber", SqlDbType.VarChar).Value = model.ApnNumber;

                    if(model.IsPaper)
                        cmd.Parameters.Add("@IsPaper", SqlDbType.Bit).Value = model.IsPaper;

                        cmd.Parameters.Add("@County", SqlDbType.VarChar).Value = model.County;

                                       

                    cmd.CommandType = CommandType.StoredProcedure;

                    // Make datatable for conversion
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dataTable);
                    da.Dispose();

                    // Close connection
                    Connection.Close();
                }

                // Convert to list of T
                var retVal = ConvertToList<HoldingCompanyList>(dataTable);
                return retVal;
            }
            catch (SqlException e)
            {                
                return new List<HoldingCompanyList>();
            }

        }

        public virtual List<OperatingCompanyList> GetOperatingCompany(ManageOperatingCompanyModel model)
        {

            try
            {
                DataTable dataTable = new DataTable();
                var cs = ConfigurationManager.ConnectionStrings["EPIRepository"].ConnectionString;

                using (SqlConnection Connection = new SqlConnection(cs))
                {
                    // Open connection
                    Connection.Open();

                    // Create command from params / SP
                    SqlCommand cmd = new SqlCommand("GetOperatingCompany", Connection);

                    // Add parameters
                    cmd.Parameters.Add("@OCName", SqlDbType.VarChar).Value = model.OCName;

                    cmd.Parameters.Add("@OCEmail", SqlDbType.VarChar).Value = model.OCEmail;
                    cmd.Parameters.Add("@OCFirstName", SqlDbType.VarChar).Value = model.OCFirstName;
                    cmd.Parameters.Add("@OCLastName", SqlDbType.VarChar).Value = model.OCLastName;
                    cmd.Parameters.Add("@LinkedInurl", SqlDbType.VarChar).Value = model.LinkedInurl;
                    cmd.Parameters.Add("@Facebookurl", SqlDbType.VarChar).Value = model.Facebookurl;
                    cmd.Parameters.Add("@Instagramurl", SqlDbType.VarChar).Value = model.Instagramurl;
                    cmd.Parameters.Add("@Twitterurl", SqlDbType.VarChar).Value = model.Twitterurl;


                    //add asset filter
                    cmd.Parameters.Add("@AssetNumber", SqlDbType.VarChar).Value = model.AssetNumber;
                    cmd.Parameters.Add("@AssetName", SqlDbType.VarChar).Value = model.AssetName;
                    cmd.Parameters.Add("@AddressLine1", SqlDbType.VarChar).Value = model.AddressLine1;
                    cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = model.City;
                    cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = model.State;
                    cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar).Value = model.ZipCode;
                    cmd.Parameters.Add("@ApnNumber", SqlDbType.VarChar).Value = model.ApnNumber;

                    if (model.IsPaper)
                        cmd.Parameters.Add("@IsPaper", SqlDbType.Bit).Value = model.IsPaper;

                    cmd.Parameters.Add("@County", SqlDbType.VarChar).Value = model.County;

                    cmd.CommandType = CommandType.StoredProcedure;

                    // Make datatable for conversion
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dataTable);
                    da.Dispose();

                    // Close connection
                    Connection.Close();
                }

                // Convert to list of T
                var retVal = ConvertToList<OperatingCompanyList>(dataTable);
                return retVal;
            }
            catch (SqlException e)
            {
                return new List<OperatingCompanyList>();
            }

        }

        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var issuein = "";
            var issueVal = "";

            try // Necesarry unfotunately.
            {
                var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();

                var properties = typeof(T).GetProperties();

                return dt.AsEnumerable().Select(row =>
                {
                    var objT = Activator.CreateInstance<T>();

                    foreach (var pro in properties)
                    {
                        if (columnNames.Contains(pro.Name))
                        {
                            issuein = pro.Name;

                            if (row[pro.Name].GetType() == typeof(System.DBNull))
                                pro.SetValue(objT, null, null);
                            else
                            {
                                issueVal = row[pro.Name].ToString();
                                pro.SetValue(objT, row[pro.Name], null);
                                
                            }
                        }
                    }

                    return objT;
                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to write data to list. Often this occurs due to type errors (DBNull, nullables), changes in SP's used or wrongly formatted SP output.");
                Console.WriteLine("ConvertToList Exception: " + e.ToString());
                return new List<T>();
            }
        }


    }
}
