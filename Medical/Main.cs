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
using Java.Lang;
using Android.Content.PM;
using Android.Media;
using UK.CO.Chrisjenx.Calligraphy;
using Android.Preferences;

namespace Medical
{
    [Activity(Label = "Health & Medical", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Icon = "@drawable/MedicalApp")]
    public class Amain : Activity
    {
        bool PfSound, PfVibrator;
        MediaPlayer sound; Vibrator vibrator;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.ThemeaMain);
            SetContentView(Resource.Layout.Main);

            GridView GvMain = (GridView)FindViewById(Resource.Id.gridView1);
            List<TypeGridLV> list = new List<TypeGridLV>();
            list.Add(new TypeGridLV() { IdImg = Resource.Drawable.AgSetpCounter, Text = "سلامتی - تندرستی" });
            list.Add(new TypeGridLV() { IdImg = Resource.Drawable.AgHR, Text = "ضربان قلب" });
            list.Add(new TypeGridLV() { IdImg = Resource.Drawable.AgWatchSmart, Text = "ساعت هوشمند" });
            list.Add(new TypeGridLV() { IdImg = Resource.Drawable.AgCycle, Text = "دوچرخه سواری" });
            list.Add(new TypeGridLV() { IdImg = Resource.Drawable.AgCBP, Text = "کنترل وضعیت بدن" });
            list.Add(new TypeGridLV() { IdImg = Resource.Drawable.AgAlarm, Text = "یادآور دارو - فعالیت" });
            list.Add(new TypeGridLV() { IdImg = Resource.Drawable.AgMsg, Text = "ماساژور" });
            list.Add(new TypeGridLV() { IdImg = Resource.Drawable.AgTest, Text = "شاخص ها" });
            list.Add(new TypeGridLV() { IdImg = Resource.Drawable.gvTagZ, Text = "تغذیه و سلامت" });
            GvMain.Adapter = new GridCustomMain(this, list);
            GvMain.ItemClick += Gv_ItemClick;

            if (PfSound)
            {
                AudioManager am = GetSystemService(Context.AudioService) as AudioManager;
                int MaxVolme = (int)(am.GetStreamMaxVolume(Stream.Music) / 2) - 2;
                am.SetStreamVolume(Stream.Music, MaxVolme, VolumeNotificationFlags.PlaySound);
            }
            sound = MediaPlayer.Create(this, Resource.Raw.Click1);
            vibrator = (Vibrator)GetSystemService(Context.VibratorService);
           
        }

        private void Gv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            switch (e.Position)
            {
                case 0:
                    SoundVibrtor();
                    StartActivity(new Intent(this, typeof(Health)).SetFlags(ActivityFlags.NewTask));
                    break;
                case 1:
                    SoundVibrtor();
                    StartActivity(new Intent(this, typeof(HeartRate)).SetFlags(ActivityFlags.NewTask));
                    break;
                case 2:
                    SoundVibrtor();
                    StartActivity(new Intent(this, typeof(SmartWatchHealth)).SetFlags(ActivityFlags.NewTask));
                    break;
                case 3:
                    SoundVibrtor();
                    StartActivity(new Intent(this, typeof(RidingBike)).SetFlags(ActivityFlags.NewTask));
                    break;
                case 4:
                    SoundVibrtor();
                    StartActivity(new Intent(this, typeof(ControlBodyPosition)).SetFlags(ActivityFlags.NewTask));
                    break;
                case 5:
                    SoundVibrtor();
                    StartActivity(new Intent(this, typeof(AlarmMedicine)).SetFlags(ActivityFlags.NewTask));
                    break;
                case 6:
                    SoundVibrtor();
                    StartActivity(new Intent(this, typeof(Massagers)).SetFlags(ActivityFlags.NewTask));
                    break;
                case 7:
                    SoundVibrtor();
                    StartActivity(new Intent(this, typeof(CalculatorMedical)).SetFlags(ActivityFlags.NewTask));
                    break;
                default:
                    break;
            }

        }

        public void SoundVibrtor()
        {
            // Get data setting
            OnGetDataSetting();
            if (PfSound)
            {
                if (sound.IsPlaying)
                    sound.Stop();
                sound.Start();
            }

            if (PfVibrator)
            {
                if (vibrator.HasVibrator)
                    vibrator.Vibrate(50);
            }
            System.Threading.Thread.Sleep(200);

        }

        protected override void OnStart()
        {
            base.OnStart();
            CheckProfileApp();
        }

        public void CheckProfileApp()
        {
            try
            {
                ISharedPreferences isp = GetSharedPreferences("Profile", FileCreationMode.WorldReadable);
                string name = isp.GetString("Name", null);
                string action = isp.GetString("Active", null);
                int age = isp.GetInt("Age", 0);
                int tall = isp.GetInt("Tall", 0);
                int weight = isp.GetInt("Weight", 0);
                int sex = isp.GetInt("Sex", -1);

                if ((name == "" || action == "" || age == 0 || tall == 0 || weight == 0 || sex == -1))
                {
                    AlertDialog.Builder Msg = new AlertDialog.Builder(this);
                    Msg.SetTitle("حساب");
                    Msg.SetMessage("اطلاعات حساب خود را تکمیل کنید");
                    Msg.SetPositiveButton("تکمیل میکنم", delegate {
                        StartActivity(new Intent(this, typeof(Profile)).SetFlags(ActivityFlags.NewTask));
                    });
                    Msg.SetCancelable(false); Msg.Create(); Msg.Show();
                }

            }
            catch
            {

            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 100, 0, "تنظیمات");
            menu.Add(0, 101, 0, "پروفایل");
            menu.Add(0, 102, 0, "راهنما - درباره");
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                AlertDialog.Builder MsgBox = new AlertDialog.Builder(this);
                MsgBox.SetTitle("خروج");
                MsgBox.SetMessage("آیا از برنامه خارج می شوید ؟");
                MsgBox.SetPositiveButton("بله", delegate { Finish(); });
                MsgBox.SetNegativeButton("لغو", delegate { });
                MsgBox.SetCancelable(false);
                MsgBox.Create(); MsgBox.Show();
            }
            return base.OnKeyDown(keyCode, e);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            // Id Menus 100 & 101
            switch (item.ItemId)
            {
                case 100:
                    StartActivity(new Intent(this, typeof(Setting)).SetFlags(ActivityFlags.NewTask));
                    break;
                case 102:
                    StartActivity(new Intent(this, typeof(HelpAbout)).SetFlags(ActivityFlags.NewTask));
                    break;
                case 101:
                    StartActivity(new Intent(this, typeof(Profile)).SetFlags(ActivityFlags.NewTask));
                    break;
                default:
                    break;
            }
            return base.OnMenuItemSelected(featureId, item);
        }

        protected override void AttachBaseContext(Android.Content.Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }

        public void OnGetDataSetting()
        {
            // Tag Preferece "Sound","Vibrator"
            var PfManager = PreferenceManager.GetDefaultSharedPreferences(this);
            PfSound = PfManager.GetBoolean("Sound", true);
            PfVibrator = PfManager.GetBoolean("Vibrator", true);
        }

    }

    public class GridCustomMain : BaseAdapter
    {
        Activity context;List<TypeGridLV> list;

        public GridCustomMain (Activity mc,List<TypeGridLV> ml)
        {
            context = mc;
            list = ml;
        }

        public override int Count
        {
            get
            {
               return list.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var cd = list[position];
            var cv = context.LayoutInflater.Inflate(Resource.Layout.LayoutMGv, parent, false);

            ImageView img = cv.FindViewById<ImageView>(Resource.Id.LayGVImageViewMain);
            img.SetImageResource(cd.IdImg);

            Android.Graphics.Typeface fonts = Android.Graphics.Typeface.CreateFromAsset(context.Assets, "fonts/iran_sans.ttf");
            TextView TxtTitle = cv.FindViewById<TextView>(Resource.Id.LayGVTextViewTitle);
            TxtTitle.Typeface = fonts;
            TxtTitle.Text = cd.Text;

            return cv;
        }
    }

    public class TypeGridLV
    {
        public int IdImg { get; set; }
        public string Text { get; set; }
    }

}