using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Preferences;

namespace Medical
{
    class DataBasePreferences
    {
        int CountTotal;string KeyNameDb;
        ISharedPreferences Isp;ISharedPreferencesEditor EditIsp;

        public DataBasePreferences(string NameDataBase, int Total, Context context)
        {
            CountTotal = Total;
            KeyNameDb = NameDataBase;
            Isp = context.GetSharedPreferences(NameDataBase, FileCreationMode.WorldReadable);
            EditIsp = Isp.Edit();
        }

        public void SetData(string[] item)
        {

        }

        public string [] GetData()
        {
            string[] item = new string[CountTotal];



            return item;
        }



    }
}