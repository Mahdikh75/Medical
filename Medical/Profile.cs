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
using UK.CO.Chrisjenx.Calligraphy;
using Android.Graphics;
using System.Threading.Tasks;

namespace Medical
{
    [Activity(Label = "Profile",ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Profile : Activity
    {
        ImageView ImgVMain;
        EditText Name, Age, Tall, Weight;
        Spinner SpSex, SpActive;
        ISharedPreferences Isp;ISharedPreferencesEditor EditIsp;
        int SexHaman = -1; string ValueActive = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.ThemeaMain);
            SetContentView(Resource.Layout.Profile);

            Isp = GetSharedPreferences("Profile", FileCreationMode.WorldReadable);
            EditIsp = Isp.Edit();

            ImgVMain = (ImageView)FindViewById(Resource.Id.ProfileImageViewNA);
            Name = (EditText)FindViewById(Resource.Id.ProfileEditTextName);
            Age = (EditText)FindViewById(Resource.Id.ProfileEditTextAge);
            Tall = (EditText)FindViewById(Resource.Id.ProfileEditTextTall);
            Weight = (EditText)FindViewById(Resource.Id.ProfileEditTextWeight);

            SpSex = (Spinner)FindViewById(Resource.Id.ProfileSpinnerSex);
            SpActive = (Spinner)FindViewById(Resource.Id.ProfileSpinnerActive);

            SpSex.ItemSelected += SpSex_ItemSelected;
            SpActive.ItemSelected += SpActive_ItemSelected;

            ArrayAdapter<string> AdapterSex = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, new string[] { "--", "مرد", "زن" });
            SpSex.Adapter = AdapterSex;

            string[] ItemActive = new string[] { "--", "بی حرکت", "کمی فعال", "متوسط فعال", "فعال", "بسیار فعال" };
            ArrayAdapter<string> AdapterActive = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, ItemActive);
            SpActive.Adapter = AdapterActive;

            // Get Data To as Perfeneces
            GetDataPf();
            //
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                ISharedPreferences isp = GetSharedPreferences("Profile", FileCreationMode.WorldReadable);
                string name = isp.GetString("Name", null);
                string action = isp.GetString("Active", null);
                int age = isp.GetInt("Age", 0);
                int tall = isp.GetInt("Tall", 0);
                int weight = isp.GetInt("Weight", 0);
                int sex = isp.GetInt("Sex", -1);

                if (name != "" && action != "" && age != 0 && tall != 0 && weight != 0 && sex != -1)
                {
                    //StartActivity(new Intent(this, typeof(Main)).SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask));
                    Finish();
                }
                else
                {
                    keyCode = Keycode.A;
                    Toast.MakeText(this, "لطفا همه ی فیلدها را پر کنید", ToastLength.Long).Show();
                }
            }
            return base.OnKeyDown(keyCode, e);
        }

        private void SpActive_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            switch (e.Parent.SelectedItem.ToString())
            {
                case "بی حرکت":
                    ValueActive = "1.25";
                    break;
                case "کمی فعال":
                    ValueActive = "1.375";
                    break;
                case "متوسط فعال":
                    ValueActive = "1.55";
                    break;
                case "فعال":
                    ValueActive = "1.725";
                    break;
                case "بسیار فعال":
                    ValueActive = "1.9";
                    break;
                default:
                    ValueActive = "0";
                    break;
            }
        }

        private void SpSex_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            switch (e.Parent.SelectedItem.ToString())
            {
                case "مرد":
                    SexHaman = 0;
                    break;
                case "زن":
                    SexHaman = 1;
                    break;
                default:
                    SexHaman = -1;
                    break;
            }
        }

        public void GetDataPf()
        {
            Isp = GetSharedPreferences("Profile", FileCreationMode.WorldReadable);

            Name.Text = Isp.GetString("Name", "");
            Age.Text = Isp.GetInt("Age", 0) != 0 ? Isp.GetInt("Age", 0).ToString() : "";
            Tall.Text = Isp.GetInt("Tall", 0) != 0 ? Isp.GetInt("Tall", 0).ToString() : "";
            Weight.Text = Isp.GetInt("Weight", 0) != 0 ? Isp.GetInt("Weight", 0).ToString() : "";
            //Sp Sex
            int TfSex = Isp.GetInt("Sex", -1);
            string[] ItemSex = new string[3];
            switch (TfSex)
            {
                case -1:
                    ItemSex = new string[] { "--", "مرد", "زن" };
                    break;
                case 0:
                    ItemSex = new string[] { "مرد", "زن" };
                    ImgVMain.SetImageResource(Resource.Drawable.Man);
                    break;
                case 1:
                    ItemSex = new string[] {  "زن", "مرد" };
                    ImgVMain.SetImageResource(Resource.Drawable.Girl);
                    break;
                default:
                    break;
            }
            ArrayAdapter<string> AdapterSex = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, ItemSex);
            SpSex.Adapter = AdapterSex;
            //Sp Active
            string TfActive = Isp.GetString("Active", "0");
            string[] ItemActive = new string[6];
            switch (TfActive)
            {
                case "1.25":
                    ItemActive = new string[] { "بی حرکت", "کمی فعال", "متوسط فعال", "فعال", "بسیار فعال" };
                    break;
                case "1.375":
                    ItemActive = new string[] { "کمی فعال", "بی حرکت", "متوسط فعال", "فعال", "بسیار فعال" };
                    break;
                case "1.55":
                    ItemActive = new string[] { "متوسط فعال", "بی حرکت", "کمی فعال", "فعال", "بسیار فعال" };
                    break;
                case "1.725":
                    ItemActive = new string[] { "فعال", "بی حرکت", "کمی فعال", "متوسط فعال", "بسیار فعال" };
                    break;
                case "1.9":
                    ItemActive = new string[] { "بسیار فعال", "بی حرکت", "کمی فعال", "متوسط فعال", "فعال" };
                    break;
                default:
                    ItemActive = new string[] { "--", "بی حرکت", "کمی فعال", "متوسط فعال", "فعال", "بسیار فعال" };
                    break;
            }
            
            ArrayAdapter<string> AdapterActive = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, ItemActive);
            SpActive.Adapter = AdapterActive;

        }

        public void SetDataPF()
        {
            Toast toast = new Toast(this);
            toast.Duration = ToastLength.Short;
            toast.SetGravity(GravityFlags.Center, 0, 0);
            View view = View.Inflate(this, Resource.Layout.ProfileCSToast, null);
            TextView TxtToast = (TextView)view.FindViewById(Resource.Id.ProfileSCToastTextViewTS);
            toast.View = view;

            if (Name.Text != "" && Age.Text != "" && Tall.Text != "" && Weight.Text != "" && SexHaman !=-1 && ValueActive != "")
            {
                Isp = GetSharedPreferences("Profile", FileCreationMode.WorldReadable);
                EditIsp = Isp.Edit();

                EditIsp.PutString("Name", Name.Text);
                EditIsp.PutInt("Age", int.Parse(Age.Text));
                EditIsp.PutInt("Tall", int.Parse(Tall.Text));
                EditIsp.PutInt("Weight", int.Parse(Weight.Text));
                EditIsp.PutInt("Sex", SexHaman);
                EditIsp.PutString("Active", ValueActive);
                EditIsp.Commit();
                TxtToast.Text = "  مشخصات ثبت شد  ";

                toast.Show();
                StartActivity(new Intent(this, typeof(Profile)).SetFlags(ActivityFlags.ClearTop));

            }
            else
            {
                TxtToast.Text = "  لطفا همه فیلدها را پر کنید  "; toast.Show();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 201, 0, "حذف").SetIcon(Resource.Drawable.DelPF).SetShowAsAction(ShowAsAction.Always);
            menu.Add(0, 200, 0, "ثبت").SetIcon(Resource.Drawable.SetDataOK).SetShowAsAction(ShowAsAction.Always);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            if (item.ItemId == 200)
            {
                SetDataPF();
            }
            else if (item.ItemId == 201)
            {
                AlertDialog.Builder MsBox = new AlertDialog.Builder(this);
                MsBox.SetTitle("حذف حساب");
                MsBox.SetMessage("آیا میخواهید حساب جاری را حذف کنید ؟");
                MsBox.SetPositiveButton("حذف", delegate
                {
                    Isp = GetSharedPreferences("Profile", FileCreationMode.WorldReadable);
                    EditIsp = Isp.Edit();
                    EditIsp.Clear(); EditIsp.Commit();
                    Toast toast = new Toast(this);
                    toast.Duration = ToastLength.Short;
                    toast.SetGravity(GravityFlags.Center, 0, 0);
                    View view = View.Inflate(this, Resource.Layout.ProfileCSToast, null);
                    TextView TxtToast = (TextView)view.FindViewById(Resource.Id.ProfileSCToastTextViewTS);
                    TxtToast.Text = "  حساب حذف شد  ";
                    toast.View = view;
                    toast.Show();
                    StartActivity(new Intent(this, typeof(Profile)).SetFlags(ActivityFlags.ClearTop));
                });
                MsBox.SetNegativeButton("لغو", delegate { });
                MsBox.Create();MsBox.Show(); 
            }
            return base.OnMenuItemSelected(featureId, item);
        }

        protected override void AttachBaseContext(Android.Content.Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }
    }
}