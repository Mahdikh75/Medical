using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Media;
using Android.Hardware;
// Componets
using ProgressBars;
using UK.CO.Chrisjenx.Calligraphy;
using Android.Preferences;

namespace Medical
{
    [Activity(Label = "Heart Rate", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class HeartRate : Activity, ISensorEventListener,IDialogInterfaceOnClickListener
    {
        // Widget app
        ImageView ImgAT, ImgHeartRate, ImgReport, ImgVMHR;
        ProgressBars.NumberProgressBar ProNm;TableLayout TableMain;
        TextView TextCountHR, TextReport, TextVMHR;
        // Hardware 
        Vibrator vibrator; MediaPlayer SoundHR,SoundError,SoundBeep;
        Sensor dfSensor; SensorManager SensorMg;
        PowerManager PowerMg; PowerManager.WakeLock WlockApp;
        // Value Motagayer
        TypeHR typeHR = TypeHR.CshNull;
        int CountHR = 0;float ValueBefreSA = 0;float TimeUpdate = 0;

        public enum TypeHR
        {
            HeartRate, Accelerometer,CshNull
        }

        #region LifeCycle

        public void OnLoadWidget()
        {
            ProNm = (NumberProgressBar)FindViewById(Resource.Id.HeartRateNumberbarProTime);
            ProNm.Progress = 0;
            ImgAT = (ImageView)FindViewById(Resource.Id.HeartRateImageViewAniTime);
            ImgAT.Click += delegate { RunAppHRMSG(); };
            TableMain = (TableLayout)FindViewById(Resource.Id.HeartRateTableLayoutMain);

            ImgHeartRate = (ImageView)FindViewById(Resource.Id.HeartRateImageViewCountHR);
            ImgReport = (ImageView)FindViewById(Resource.Id.HeartRateImageViewReport);
            ImgVMHR = (ImageView)FindViewById(Resource.Id.HeartRateImageViewValueMain);

            TextCountHR = (TextView)FindViewById(Resource.Id.HeartRateTextViewCountHR);
            TextReport = (TextView)FindViewById(Resource.Id.HeartRateTextViewReport);
            TextVMHR = (TextView)FindViewById(Resource.Id.HeartRateTextViewVMHR);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.ThemeaMain);
            SetContentView(Resource.Layout.HeartRate);
            OnLoadWidget();

            vibrator = GetSystemService(VibratorService) as Vibrator;
            SensorMg = (SensorManager)GetSystemService(SensorService) as SensorManager;

            PowerMg = GetSystemService(PowerService) as PowerManager;
            WlockApp = PowerMg.NewWakeLock(WakeLockFlags.Full, "DoNotSleep");
            WlockApp.Acquire();

            var PfManager = PreferenceManager.GetDefaultSharedPreferences(this).GetBoolean("Sound", true);
            if (PfManager)
            {
                AudioManager AudioMg = GetSystemService(Context.AudioService) as AudioManager;
                int MaxVolme = (int)(AudioMg.GetStreamMaxVolume(Android.Media.Stream.Music) / 2);
                AudioMg.SetStreamVolume(Android.Media.Stream.Music, MaxVolme - 2, VolumeNotificationFlags.PlaySound);
            }

            SoundHR = MediaPlayer.Create(this, Resource.Raw.Beep3);
            SoundBeep = MediaPlayer.Create(this, Resource.Raw.Beep1);
            SoundError = MediaPlayer.Create(this, Resource.Raw.BeepError);

        }

        protected override void OnStart()
        {
            base.OnStart();

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            SoundBeep.Stop();
            SoundError.Stop();
            SoundHR.Stop();
            vibrator.Cancel();
            WlockApp.Release();
            SensorMg.UnregisterListener(this);
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                if (typeHR == TypeHR.Accelerometer || typeHR == TypeHR.HeartRate)
                {
                    AlertDialog.Builder Msg = new AlertDialog.Builder(this);
                    Msg.SetTitle("خروج");
                    Msg.SetMessage("برنامه در حال اجراست آیا از برنامه خارج میشوید ؟");
                    Msg.SetPositiveButton("بله", delegate { Finish(); });
                    Msg.SetNegativeButton("خیر", delegate { });
                    Msg.SetNeutralButton("لغو ضربان گیر", delegate
                    {
                        ProNm.Progress = 0;
                        ImgAT.ImageAlpha = 255;
                        typeHR = TypeHR.CshNull;
                        ImgAT.SetImageResource(Resource.Drawable.HRCardiogram);
                        ZeroValue();
                        System.Threading.Thread.Sleep(500);
                        TextCountHR.Text = "تعداد ضربان قلب";
                        TextReport.Text = "گزارش از ضربان قلب";
                        TextVMHR.Text = "محدوده ضربان قلب";
                        TableMain.Visibility = ViewStates.Visible;
                        SensorMg.UnregisterListener(this);
                    });
                    Msg.Create(); Msg.Show();
                }
            }
            return base.OnKeyDown(keyCode, e);
        }

        #endregion

        #region Menu App 

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 1003, 0, "راهنما").SetIcon(Resource.Drawable.HelpRA).SetShowAsAction(ShowAsAction.Always);
            menu.Add(0, 1001, 0, "ضربان قلب");
            menu.Add(0, 1002, 0, "مشخصات سنسور");
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 1001:
                    RunAppHRMSG();
                    break;
                case 1002:
                    SensorHR();
                    break;
                case 1003:
                    HelpHR();
                    break;
                default:
                    break;
            }
            return base.OnMenuItemSelected(featureId, item);
        }

        public void HelpHR()
        {
            View ViewMsg = View.Inflate(this, Resource.Layout.HeartRateTHHelp, null);
            TabWidget TabWidget = (TabWidget)ViewMsg.FindViewById(Android.Resource.Id.Tabs);
            TabHost Tabs = (TabHost)ViewMsg.FindViewById(Resource.Id.HeartRateTHHTabHostMain);
            Tabs.Setup();

            TabHost.TabSpec TSEdu = Tabs.NewTabSpec("Edu");
            TSEdu.SetContent(Resource.Id.HeartRateTHHLinearLayoutEdu);
            TSEdu.SetIndicator("آموزش");
            Tabs.AddTab(TSEdu);

            TabHost.TabSpec TSHelp = Tabs.NewTabSpec("Notes");
            TSHelp.SetContent(Resource.Id.HeartRateTHHLinearLayoutNotes);
            TSHelp.SetIndicator("نکات");
            Tabs.AddTab(TSHelp);

            TextView TextEdu = (TextView)ViewMsg.FindViewById(Resource.Id.HeartRateTHHTextViewEdu);
            TextView TextNotes = (TextView)ViewMsg.FindViewById(Resource.Id.HeartRateTHHTextViewNote);

            TextEdu.Text = GetString(Resource.String.Edu1) + "\n\n" + GetString(Resource.String.Edu2) + "\n\n" +
                GetString(Resource.String.Edu3) + "\n\n" + GetString(Resource.String.Edu4) + "\n\n" + GetString(Resource.String.Edu5);

            TextNotes.Text = GetString(Resource.String.HelpAppHR1) + "\n\n" + GetString(Resource.String.HelpAppHR2) + "\n\n" + GetString(Resource.String.HelpAppHR3)
                + "\n\n" + GetString(Resource.String.HelpAppHR4) + "\n\n" + GetString(Resource.String.HelpAppHR5);
            
            AlertDialog.Builder Msg = new AlertDialog.Builder(this);
            Msg.SetTitle("راهنما");
            Msg.SetView(ViewMsg);
            Msg.SetNeutralButton("لغو", delegate { });
            Msg.SetCancelable(false); Msg.Create(); Msg.Show();
        }

        public void RunAppHRMSG()
        {
            AlertDialog.Builder Msg = new AlertDialog.Builder(this);
            Msg.SetTitle("نوع اندازه گیر ضربان قلب");
            Msg.SetItems(new string[] { "لرزش سنج", "سنسور ضربان قلب" }, this);
            Msg.SetPositiveButton("توقف شمارشگر", delegate
            {
                ProNm.Progress = 0;
                ImgAT.ImageAlpha = 255;
                typeHR = TypeHR.CshNull;
                ImgAT.SetImageResource(Resource.Drawable.HRCardiogram);
                ZeroValue();
                System.Threading.Thread.Sleep(500);
                TextCountHR.Text = "تعداد ضربان قلب";
                TextReport.Text = "گزارش از ضربان قلب";
                TextVMHR.Text = "محدوده ضربان قلب";
                TableMain.Visibility = ViewStates.Visible;
                SensorMg.UnregisterListener(this);
            });
            Msg.SetNegativeButton("لغو", delegate { });
            Msg.Create(); Msg.Show();
        }

        public void CustomToast(string text, ToastLength tl, int icon)
        {
            View view = View.Inflate(this, Resource.Layout.HeartRateCSToast, null);
            ImageView ImgToast = (ImageView)view.FindViewById(Resource.Id.HeartRateCSToastImageViewTM);
            ImgToast.SetImageResource(icon);

            TextView TxtMsg = (TextView)view.FindViewById(Resource.Id.HeartRateCSToastTextViewTM);
            TxtMsg.Text = text;

            Toast toast = new Toast(this);
            toast.Duration = tl;
            toast.SetGravity(GravityFlags.Center, 0, -50);
            toast.View = view;
            toast.Show();
        }

        public async void OnClick(IDialogInterface dialog, int which)
        {
            switch (which)
            {
                case 0:
                    List<Sensor> sensorA = new List<Sensor>(SensorMg.GetSensorList(SensorType.Accelerometer));
                    if (sensorA.Count > 0)
                    {
                        if (typeHR == TypeHR.CshNull)
                        {
                            TableMain.Visibility = ViewStates.Invisible;
                            dfSensor = SensorMg.GetDefaultSensor(SensorType.Accelerometer);
                            CustomToast("ضربان گیر فعال شد", ToastLength.Short, Resource.Drawable.Heart);
                            var Isp = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(this);
                            bool TfSound = Isp.GetBoolean("Sound", true);
                            if (TfSound)
                                SoundBeep.Start();
                            await Task.Delay(2000);
                            typeHR = TypeHR.Accelerometer;
                            AnimationImgAccelerometer();
                            SensorMg.RegisterListener(this, dfSensor, SensorDelay.Ui);
                        }
                    }
                    else
                    {
                        var Isp = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(this);
                        bool TfSound = Isp.GetBoolean("Sound", true);
                        if (TfSound)
                            SoundError.Start();
                        CustomToast("دستگاه شما از سنسور شتاب سنج را پشتیبانی نمی کند", ToastLength.Long, Resource.Drawable.Sensor);
                    }

                    break;
                case 1:
                    List<Sensor> sensorHr = new List<Sensor>(SensorMg.GetSensorList(SensorType.HeartRate));
                    if (sensorHr.Count > 0)
                    {
                        if (typeHR == TypeHR.CshNull)
                        {
                            TableMain.Visibility = ViewStates.Invisible;
                            dfSensor = SensorMg.GetDefaultSensor(SensorType.HeartRate);
                            CustomToast("ضربان گیر فعال شد", ToastLength.Short, Resource.Drawable.Heart);
                            var Isp = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(this);
                            bool TfSound = Isp.GetBoolean("Sound", true);
                            if (TfSound)
                                SoundBeep.Start();
                            await Task.Delay(2000);
                            typeHR = TypeHR.HeartRate;
                            AnimationImgHeartRate();
                            SensorMg.RegisterListener(this, dfSensor, SensorDelay.Ui);
                        }
                    }
                    else
                    {
                        var Isp = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(this);
                        bool TfSound = Isp.GetBoolean("Sound", true);
                        if (TfSound)
                            SoundError.Start();
                        CustomToast("دستگاه شما از سنسور ضربان قلب پشتیبانی نمی کند", ToastLength.Long, Resource.Drawable.Sensor);
                    }

                    break;
                default:
                    break;
            }
        }

        public void SensorHR()
        {
            StringBuilder StrBuilder = new StringBuilder();
            // Get Bool Sensor Heart rate
            List<Sensor> Ls = new List<Sensor>(SensorMg.GetSensorList(SensorType.HeartRate));
            if ((Ls.Count > 0))
            {
                Sensor sensor = SensorMg.GetDefaultSensor(SensorType.HeartRate);
                StrBuilder.AppendLine(" نام : " + sensor.Name);
                StrBuilder.AppendLine(" قدرت : " + sensor.Power);
                StrBuilder.AppendLine(" حداکثر برد : " + sensor.MaximumRange);
                StrBuilder.AppendLine(" حداقل تاخیر : " + sensor.MinDelay);
                StrBuilder.AppendLine(" حداکثر تاخیر : " + sensor.MaxDelay);
                StrBuilder.AppendLine(" وضوح : " + sensor.Resolution);
                StrBuilder.AppendLine(" نسخه : " + sensor.Version);
            }
            else
            {
                StrBuilder.AppendLine("دستگاه شما از سنسور ضربان قلب پشتیبانی نمی کند.");
            }
            AlertDialog.Builder Message = new AlertDialog.Builder(this);
            Message.SetTitle("سنسور ضربان قلب");
            Message.SetMessage(StrBuilder.ToString());
            Message.SetNeutralButton("لغو", delegate { return; });
            Message.SetCancelable(false); Message.Create(); Message.Show();
        }

        #endregion

        #region Animation Heart rate

        public void AnimationImgHeartRate()
        {
            ProNm.Progress = 100;
            HRAniImgAlphaSizeSound();
        }

        public async void HRAniImgAlphaSizeSound()
        {
            var Isp = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(this);
            bool TfSound = Isp.GetBoolean("Sound", true);
            bool PfVibrator = PreferenceManager.GetDefaultSharedPreferences(this).GetBoolean("Vibrator", true);

            for (int i = 0; i <= 150; i++)
            {
                if (typeHR == TypeHR.HeartRate)
                {
                    if (CountHR == 0)
                    {
                        await Task.Delay(200);

                        if (TfSound) { SoundHR.Start(); }

                        if (PfVibrator) { vibrator.Vibrate(25); }

                        int Simg = Resource.Drawable.HR520;
                        switch (i % 2)
                        {
                            case 0:
                                Simg = Resource.Drawable.HR520;
                                ImgAT.ImageAlpha = 255;
                                break;
                            case 1:
                                Simg = Resource.Drawable.HR470;
                                ImgAT.ImageAlpha = 128;
                                break;
                            default:
                                break;
                        }
                        ImgAT.SetImageResource(Simg);
                    }
                    else if (CountHR > 0)
                    {
                        ImgAT.SetImageResource(Resource.Drawable.HRCardiogram);
                        ImgAT.ImageAlpha = 255;
                        SensorMg.UnregisterListener(this);
                        ViewHeartRateAccelermeter(CountHR);
                        break;
                    }
                }
                if (i == 150)
                {
                    if (CountHR == 0)
                    {
                        Toast.MakeText(this, "خطایی در سنسور رخ داده لطفا دوباره امتحان کنید", ToastLength.Short).Show();
                    }
                }

            }

        }

        #endregion

        #region Animation Accelermeter

        public void AnimationImgAccelerometer()
        {
            ACAniSizeImg();
            ACAniNumberProgress();
            ACAniAlphaImg();
            ACAniSound();
        }

        public async void ACAniAlphaImg()
        {
            for (int i = 0; i < 256; i++)
            {
                if (typeHR == TypeHR.Accelerometer)
                {
                    await Task.Delay(40);
                    ImgAT.ImageAlpha = (i);
                }
                else
                {
                    ImgAT.ImageAlpha = 255;
                    break;
                }
            }
        }

        public async void ACAniSizeImg()
        {
            for (int i = 0; i < 200; i++)
            {
                if (typeHR == TypeHR.Accelerometer)
                {
                    await Task.Delay(100);
                    int Simg = Resource.Drawable.HR520;

                    switch (i % 4)
                    {
                        case 0:
                            Simg = Resource.Drawable.HR520;
                            break;
                        case 1:
                            Simg = Resource.Drawable.HR490;
                            break;
                        case 2:
                            Simg = Resource.Drawable.HR470;
                            break;
                        case 3:
                            Simg = Resource.Drawable.HR450;
                            break;
                        default:
                            break;
                    }
                    ImgAT.SetImageResource(Simg);
                }
                else
                {
                    ImgAT.SetImageResource(Resource.Drawable.HRCardiogram);
                    break;
                }
            }
        }

        public async void ACAniNumberProgress()
        {
            for (int i = 0; i <= 100; i++)
            {
                if (typeHR == TypeHR.Accelerometer)
                {
                    await Task.Delay(200);
                    ProNm.Progress = i;
                    if (i == 100)
                    {
                        SensorMg.UnregisterListener(this);
                        ViewHeartRateAccelermeter(CountHR);
                    }
                }
                else
                {
                    ProNm.Progress = 0;
                    break;
                }

            }
        }

        public async void ACAniSound()
        {
            var Isp = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(this);
            bool TfSound = Isp.GetBoolean("Sound", true);
            for (int i = 0; i < 12; i++)
            {
                if (typeHR == TypeHR.Accelerometer)
                {
                    await Task.Delay(1500);
                    if (TfSound)
                        SoundHR.Start();
                }
                else
                {
                    SoundHR.Stop();
                    break;
                }
            }
        }

        #endregion

        #region Calculator Sensor

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            if (sensor.Type == SensorType.Accelerometer) { accuracy = SensorStatus.AccuracyHigh; }

            if (sensor.Type == SensorType.HeartRate) { accuracy = SensorStatus.AccuracyHigh; }
        }

        public void OnSensorChanged(SensorEvent e)
        {
            switch (e.Sensor.Type)
            {
                case SensorType.Accelerometer:
                    if (typeHR == TypeHR.Accelerometer)
                    {
                        // Get x,y,z,TimeStamp
                        var x = e.Values[0]; var y = e.Values[1]; var z = e.Values[2];
                        double xyz = Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2);
                        float Fpxyz = (float)((Math.Pow(xyz, 0.5) - SensorManager.StandardGravity));

                        if (TimeUpdate == 0)
                        {
                            TimeUpdate = e.Timestamp;
                        }
                        // Progress x,y,z,TimeStamp
                        if ((e.Timestamp - TimeUpdate > 0))
                        {
                            if (Fpxyz - ValueBefreSA > 0.1)
                            {
                                CountHR++;
                            }
                            ValueBefreSA = Fpxyz;
                            TimeUpdate = e.Timestamp;
                        }
                        // Producer does the design : Mahdi Khayamdar 
                        /*
                        1) FBrodar = (x+y+z)^0.5
                        2) Result = Fbrodar - StdGravity(9.8 m/s^2)
                        3) if (timeStamp) Affect to Result
                        4) End Work Sensor
                        5) Counter ++ 
                        6) View Data 'Hreart rate'
                        */
                        var PfManager = PreferenceManager.GetDefaultSharedPreferences(this);
                        bool PfVibrator = PfManager.GetBoolean("Vibrator", true);

                        if (PfVibrator) { vibrator.Vibrate(20); }
                    }
                    break;
                case SensorType.HeartRate:
                    if (typeHR == TypeHR.HeartRate)
                    {
                        var ValueHR = e.Values[0];
                        CountHR = (int)ValueHR;
                    }
                    break;
                default:
                    break;
            }

        }

        #endregion

        #region View data

        public async void VisibleTable()
        {
            for (int i = 0; i < 26; i++)
            {
                await Task.Delay(100);
                ImgHeartRate.ImageAlpha = (i * 10) + 5;
                ImgReport.ImageAlpha = (i * 10) + 5;
                ImgVMHR.ImageAlpha = (i * 10) + 5;
                if (i == 4)
                {
                    TableMain.Visibility = ViewStates.Visible;
                }
            }
        }

        public void ViewHeartRateAccelermeter(int counthr)
        {
            System.Threading.Thread.Sleep(300);
            if (typeHR == TypeHR.HeartRate)
                SoundHR.Stop();
            VisibleTable();
            // Count Heart rate
            if (counthr >= 55 && counthr <= 130)
            {
                TextCountHR.Text = " تعداد ضربان قلب در دقیقه " + counthr;
                var PfManager = PreferenceManager.GetDefaultSharedPreferences(this);
                bool PfSound = PfManager.GetBoolean("Sound", true);
                bool PfVibrator = PfManager.GetBoolean("Vibrator", true);

                if (PfSound) { SoundError.Start(); }
                if (PfVibrator) { vibrator.Vibrate(250); }
                typeHR = TypeHR.CshNull;
                ZeroValue();
                // Get Data app
                ISharedPreferences isp = GetSharedPreferences("Profile", FileCreationMode.WorldReadable);
                string name = isp.GetString("Name", null);
                string action = isp.GetString("Active", null);
                int age = isp.GetInt("Age", 0);
                int tall = isp.GetInt("Tall", 0);
                int weight = isp.GetInt("Weight", 0);
                int sex = isp.GetInt("Sex", -1);
                // Report HR
                // 1) Max HR  2) Good & Bad
                StringBuilder strb = new StringBuilder();
                strb.AppendLine("مشخصات ضربان قلب");
                int ValueAge = sex == 0 ? 220 : 226;
                int MaxHR = 0;
                if (age <= 40)
                {
                    strb.AppendLine(" حداکثر ضربان قلب شما " + (ValueAge - age)); MaxHR = (ValueAge - age);
                }
                else if (age > 40)
                {
                    strb.AppendLine(" حداکثر ضربان قلب شما " + (210 - age)); MaxHR = (210 - age);
                }
                double Check = (counthr * 100) / (MaxHR);
                if (Check > 31 && Check < 70)
                {
                    strb.AppendLine("با توجه سن شما و حداکثر ضربان قلب شما این ضربان قلب خوب است");
                }
                else
                {
                    strb.AppendLine("با توجه سن شما و حداکثر ضربان قلب شما این ضربان قلب خوب نیست");
                }
                TextReport.Text = strb.ToString();
                // Range HR
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("محدوده تعداد ضربان قلب در فعالیت های مختلف");
                sb.AppendLine(" محدوده سلامت قلب شما " + ((MaxHR * 50) / 100) + " تا " + ((MaxHR * 60) / 100));
                sb.AppendLine(" زیبایی اندام (چربی سوزی) " + ((MaxHR * 60) / 100) + " تا " + ((MaxHR * 70) / 100));
                sb.AppendLine(" تمرینات استقامتی " + ((MaxHR * 70) / 100) + " تا " + ((MaxHR * 80) / 100));
                sb.AppendLine(" تمرینات پیشرفته " + ((MaxHR * 80) / 100) + " تا " + ((MaxHR * 90) / 100));
                sb.AppendLine(" خط قرمز (حداکثر توان) " + ((MaxHR * 90) / 100) + " تا " + ((MaxHR * 100) / 100));
                TextVMHR.Text = sb.ToString();
            }
            else
            {
                Toast.MakeText(this, "خطایی در محاسبه ضربان رخ داده دوباره امتحان کنید", ToastLength.Long).Show();
            }
            // zero value
            typeHR = TypeHR.CshNull; ZeroValue();

        }

        public void ZeroValue()
        {
            ValueBefreSA = 0;
            CountHR = 0;
            TimeUpdate = 0;
        }

        #endregion

        #region Fonts App 

        protected override void AttachBaseContext(Android.Content.Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }

        #endregion

    }
}