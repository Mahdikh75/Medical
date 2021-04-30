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
using Android.Hardware;
using Android.Locations;
using Android.Preferences;
using FlipNumbers;
using Android.Graphics;

namespace Medical
{
    [Activity(Label = "Health", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Health : Activity, Android.Hardware.ISensorEventListener, Android.Locations.ILocationListener, IDialogInterfaceOnClickListener
    {
        PowerManager PowerMg; PowerManager.WakeLock WLock; Vibrator vibrator;
        Sensor dfSensorStp, dfSensorDec; SensorManager SensorMg;
        Location dfLocation; LocationManager LocationMg;
        //app
        FlipNumbersView FlipNumberVM; Button BtnOnOffSensor; Chronometer TimeHth;
        TextView TxtCaleri, TxtKhMeter, TxtSpeed, TxtHeight;
        double TxtCalSM = 0;
        double TypeCal = 0, ValueCal = 0;
        float HeightKMin, HeightKMax = 0; bool HeightGPS = false, DispalyHGPS = false;
        float Khm = 0; int ValueTime = 0;int ValueEmptySensor = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.ThemeaMain);
            SetContentView(Resource.Layout.Health);
            LoadWidgetApp();

            vibrator = (GetSystemService(VibratorService) as Vibrator);
            LocationMg = GetSystemService(Context.LocationService) as LocationManager;
            SensorMg = (SensorManager)GetSystemService(Context.SensorService);
            PowerMg = GetSystemService(Context.PowerService) as PowerManager;
            WLock = PowerMg.NewWakeLock(WakeLockFlags.Full, "DoNotSleep"); WLock.Acquire();

            List<Sensor> ListSensor = new List<Sensor>(SensorMg.GetSensorList(SensorType.StepCounter));
            if (ListSensor.Count > 0)
            {
                dfSensorStp = SensorMg.GetDefaultSensor(SensorType.StepCounter);
                List<Sensor> LS = new List<Sensor>(SensorMg.GetSensorList(SensorType.StepDetector));
                if (LS.Count > 0)
                    dfSensorDec = SensorMg.GetDefaultSensor(SensorType.StepDetector);
            }
            else
            {
                AlertDialog.Builder Msg = new AlertDialog.Builder(this);
                Msg.SetTitle("خطا سنسور");
                Msg.SetMessage("دستگاه از سنسور گام شمار پشتیبانی نمی کند");
                Msg.SetPositiveButton("ورود به برنامه", delegate { });
                Msg.SetNegativeButton("خروج", delegate { Finish(); });
                Msg.SetCancelable(false);
                Msg.Create(); Msg.Show();
            }

        }

        public void LoadWidgetApp()
        {
            FlipNumberVM = (FlipNumbersView)FindViewById(Resource.Id.HealthFlipNumbersMainV);

            BtnOnOffSensor = (Button)FindViewById(Resource.Id.HealthBtnOnOff);
            BtnOnOffSensor.Background.SetColorFilter(Color.ParseColor("#148264"), PorterDuff.Mode.Multiply);
            BtnOnOffSensor.Click += BtnOnOffSensor_Click;

            TimeHth = (Chronometer)FindViewById(Resource.Id.HealthChronometerMain);
            TimeHth.ChronometerTick += TimeHth_ChronometerTick;

            TxtCaleri = (TextView)FindViewById(Resource.Id.HealthTextViewCaleri);
            TxtHeight = (TextView)FindViewById(Resource.Id.HealthTextViewHeightKL);

            TxtKhMeter = (TextView)FindViewById(Resource.Id.HealthTextViewKiloMeter);
            TxtSpeed = (TextView)FindViewById(Resource.Id.HealthTextViewSpeed);

        }

        private void TimeHth_ChronometerTick(object sender, EventArgs e)
        {
            ValueTime++;
        }

        private void BtnOnOffSensor_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder Msg = new AlertDialog.Builder(this);
            Msg.SetTitle("نوع حرکت را مشخص کنید");
            Msg.SetItems(new string[] { "دویدن", "قدم زدن", "کوه نوردی", "پیاده روی" }, this);
            Msg.SetNeutralButton("توقف سنسور", delegate
            {
                SensorMg.UnregisterListener(this, dfSensorStp);
                SensorMg.UnregisterListener(this, dfSensorDec);
                TimeHth.Stop();
            });
            Msg.SetCancelable(true); Msg.Create(); Msg.Show();
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            if (TxtCaleri.Text == "میزان سوخت کالری")
            {
                TxtCaleri.Text = "";
            }
            else
            {
                try
                {
                    ValueCal = double.Parse(TxtCaleri.Text);
                }
                catch
                {
                    Toast.MakeText(this, "لطفا منتظر باشید", ToastLength.Short).Show();
                }

            }
            switch (which)
            {
                case 0:
                    TypeCal = 0.2917;
                    break;
                case 1:
                    TypeCal = 0.1167;
                    break;
                case 2:
                    TypeCal = 0.1667;
                    break;
                case 3:
                    TypeCal = 0.1133;
                    break;
                default:
                    break;
            }
            TxtCaleri.Text = "";
            SensorMg.RegisterListener(this, dfSensorStp, SensorDelay.Ui);
            SensorMg.RegisterListener(this, dfSensorDec, SensorDelay.Ui);
            TimeHth.Start();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (data.Action == Android.Provider.Settings.ActionLocationSourceSettings || requestCode == 1)
            {
                if ((LocationMg.IsProviderEnabled(LocationManager.GpsProvider)))
                {
                    Toast.MakeText(this, "Location Provider 'GPS' Enabel", ToastLength.Short).Show();
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public void Speedomoter()
        {
            AlertDialog.Builder MsgBox = new AlertDialog.Builder(this);
            MsgBox.SetTitle("داشبورد");
            MsgBox.SetMessage("آیا مایل هستید از سرعت و کیلومتر سنج براساس مکان یابی استفاده کنید ؟");
            MsgBox.SetPositiveButton("اجرا", delegate
            {
                if (!(LocationMg.IsProviderEnabled(LocationManager.GpsProvider)))
                {
                    string ActionSetting = Android.Provider.Settings.ActionLocationSourceSettings;
                    StartActivity(new Intent(ActionSetting));
                    OnActivityResult(1, Result.Ok, new Intent(ActionSetting));
                    // Send to provider 'GPS' if (true) { Run to gps }
                }
                else
                {
                    try
                    {
                        LocationMg.RequestLocationUpdates(LocationManager.GpsProvider, 1000, 1f, this);
                        dfLocation = LocationMg.GetLastKnownLocation(LocationManager.GpsProvider);
                        RunOnUiThread(() => OnLocationChanged(dfLocation));
                    }
                    catch { }
                }

            });
            MsgBox.SetNegativeButton("توقف", delegate
            {
                LocationMg.RemoveUpdates(this);
            });
            MsgBox.SetNeutralButton("تنظیمات مکان یابی", delegate
            {
                string ActionSetting = Android.Provider.Settings.ActionLocationSourceSettings;
                StartActivity(new Intent(ActionSetting));
            });
            MsgBox.SetCancelable(true); MsgBox.Create(); MsgBox.Show();
        }

        public void LevelSeaHeight()
        {
            AlertDialog.Builder Msg = new AlertDialog.Builder(this);
            Msg.SetTitle("کوه نوردی");
            Msg.SetMessage("آیا می خواهید ارتفاع سنج کوه نوردی را فعال کنید ؟");
            Msg.SetPositiveButton("اجرا", delegate
            {
                if (!(LocationMg.IsProviderEnabled(LocationManager.GpsProvider)))
                {
                    string ActionSetting = Android.Provider.Settings.ActionLocationSourceSettings;
                    StartActivity(new Intent(ActionSetting));
                }
                else
                {
                    try
                    {
                        LocationMg.RequestLocationUpdates(LocationManager.GpsProvider, 1000, 1f, this);
                        dfLocation = LocationMg.GetLastKnownLocation(LocationManager.GpsProvider);
                        RunOnUiThread(() => OnLocationChanged(dfLocation));
                        HeightGPS = true; DispalyHGPS = true;
                    }
                    catch { }
                }
            });
            Msg.SetNegativeButton("توقف", delegate
            {
                HeightKMin = 0;
                HeightKMax = 0;
                DispalyHGPS = false;
                HeightGPS = false;
                LocationMg.RemoveUpdates(this);
            });
            Msg.SetNeutralButton("لغو", delegate { });
            Msg.SetCancelable(false); Msg.Create(); Msg.Show();
        }

        public void ZeroValues()
        {
            AlertDialog.Builder Msg = new AlertDialog.Builder(this);
            Msg.SetTitle("بهینه سازی");
            Msg.SetMessage("آیا میخواهید برنامه مقادیر را پاک کند ؟");
            Msg.SetPositiveButton("بله", delegate
            {
                StartActivity(new Intent(this, typeof(Health)).SetFlags(ActivityFlags.ClearTop));
            });
            Msg.SetNegativeButton("خیر", delegate { });
            Msg.SetNeutralButton("تعداد گام ها", delegate
            {
                ISharedPreferences isp = GetSharedPreferences("Health", FileCreationMode.WorldReadable);
                ISharedPreferencesEditor edit = isp.Edit();

                SensorMg.RegisterListener(this, dfSensorStp, SensorDelay.Ui);
                SensorMg.RegisterListener(this, dfSensorDec, SensorDelay.Ui);

                do
                {
                    if (ValueEmptySensor != 0)
                    {
                        edit.PutInt("StepCounter", ValueEmptySensor);
                        edit.Commit();
                        ValueEmptySensor = 0;FlipNumberVM.SetValue(0, true);
                        SensorMg.UnregisterListener(this, dfSensorStp);
                        SensorMg.UnregisterListener(this, dfSensorDec);
                    }
                } while (ValueEmptySensor > 0);

            });
            Msg.SetCancelable(true); Msg.Create(); Msg.Show();
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                // Java Android
                Java.IO.File FileCacheApp = this.CacheDir;
                if (FileCacheApp.IsFile)
                    FileCacheApp.Delete();
                // .Net - C#
                string path = this.CacheDir.Path;
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                try
                {
                    var IsStep = FlipNumberVM.Value != 0 ? true : false;

                    if (IsStep)
                    {
                        AlertDialog.Builder Msg = new AlertDialog.Builder(this);
                        Msg.SetTitle("تندرستی");
                        Msg.SetMessage("آیا دیگر فعالیت تمرینی نمی کنید ؟");
                        Msg.SetPositiveButton("بعدا تمرین میکنم", delegate { Finish(); });
                        Msg.SetNegativeButton("ادامه میدم", delegate { });
                        Msg.Create(); Msg.Show();
                    }
                }
                catch { }

            }
            return base.OnKeyDown(keyCode, e);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 400, 0, "داشبورد");
            menu.Add(0, 401, 0, "صعود ارتفاع کوه نوردی");
            menu.Add(0, 403, 0, "نمایش قدم ها در اعلان ها").SetIcon(Resource.Drawable.StepCounterNoti).SetShowAsAction(ShowAsAction.Always);
            menu.Add(0, 402, 0, "صفر کردن مقادیر").SetIcon(Resource.Drawable.ReLoad).SetShowAsAction(ShowAsAction.Always);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 400:
                    Speedomoter();
                    break;
                case 401:
                    LevelSeaHeight();
                    break;
                case 402:
                    ZeroValues();
                    break;
                case 403:
                    LoadStepCounterNoti();
                    break;
                default:
                    break;
            }
            return base.OnMenuItemSelected(featureId, item);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            ISharedPreferences isp = GetSharedPreferences("Health", FileCreationMode.WorldReadable);
            ISharedPreferencesEditor edit = isp.Edit();
            if (FlipNumberVM.Value != 0)
            {
                int MainVB = isp.GetInt("StepCounter", 0);
                edit.PutInt("StepCounter", (FlipNumberVM.Value + MainVB));
                edit.Commit();
            }
            WLock.Release(); LocationMg.RemoveUpdates(this);
            SensorMg.UnregisterListener(this, dfSensorStp);
            SensorMg.UnregisterListener(this, dfSensorDec);
        }

        protected override void AttachBaseContext(Android.Content.Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }

        public void LoadStepCounterNoti()
        {
            AlertDialog.Builder Msg = new AlertDialog.Builder(this);
            Msg.SetTitle("گام شمار");
            Msg.SetMessage("آیا میخواهید تعداد قدم ها در قسمت اعلان ها نمایش داده شود ؟");
            Msg.SetNegativeButton("خیر", delegate { });
            Msg.SetPositiveButton("بله", delegate
            {
                StartService(new Intent(this, typeof(StepCounterNotiCm)));
                System.Threading.Thread.Sleep(100);
                Toast.MakeText(this, "نمایش تعداد قدم ها در اعلان ها", ToastLength.Long).Show();
            });
            Msg.Create(); Msg.Show();
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            if (sensor.Type == SensorType.StepCounter)
            {
                accuracy = SensorStatus.AccuracyHigh;
            }
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type == SensorType.StepCounter)
            {
                ISharedPreferences isp = GetSharedPreferences("Health", FileCreationMode.WorldReadable);
                int StepCouter = ((int)e.Values[0]);ValueEmptySensor = StepCouter;

                FlipNumberVM.SetValue((StepCouter - isp.GetInt("StepCounter", 0)), true);

                TxtCalSM += (TypeCal);
                TxtCaleri.Text = Convert.ToSingle((TxtCalSM + ValueCal)).ToString();

                if (vibrator.HasVibrator) { vibrator.Vibrate(40); }
            }
            if (e.Sensor.Type == SensorType.StepDetector)
            {
                if (vibrator.HasVibrator) { vibrator.Vibrate(10); }
            }

        }

        public void OnLocationChanged(Location location)
        {
            // Speed m/s for to Km/h = Speed * 3.6
            TxtSpeed.Text = (float)(location.Speed * 3.6) + " Km/h";
            Khm += (float)(location.Speed);
            TxtKhMeter.Text = (Khm + " m | " + (((float)(Khm / 1000)) + " Km"));

            if (HeightGPS)
            {
                HeightKMin = (float)location.Altitude;
                HeightGPS = false;
            }
            if (DispalyHGPS)
            {
                HeightKMax = (float)location.Altitude;
                TxtHeight.Text = " ارتفاع پایه " + HeightKMin + " | " + " ارتفاع اوج " + (HeightKMax) + " | " + " صعود " + (HeightKMax - HeightKMin);
            }
        }

        public void OnProviderDisabled(string provider)
        {
            if (LocationMg.IsProviderEnabled(provider) == false)
            {
                Toast.MakeText(this, "سرویس مکان یابی غیر فعال شد", ToastLength.Short).Show();
                Android.Util.Log.Error("OnProviderDisabled", "Provider = False");
            }
        }

        public void OnProviderEnabled(string provider)
        {
            if (LocationMg.IsProviderEnabled(provider))
            {
                // Set Metood 
                Toast.MakeText(this, "سرویس مکان یابی فعال شد", ToastLength.Short).Show();
                try
                {
                    LocationMg.RequestLocationUpdates(LocationManager.GpsProvider, 1000, 1f, this);
                    dfLocation = LocationMg.GetLastKnownLocation(LocationManager.GpsProvider);
                    RunOnUiThread(() => OnLocationChanged(dfLocation));
                }
                catch { }
            }
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            if (LocationManager.GpsProvider == provider)
            {
                switch (status)
                {
                    case Availability.Available:
                        Toast.MakeText(this, "جی پی اس در دسترس است", ToastLength.Short).Show();
                        break;
                    case Availability.OutOfService:
                        Toast.MakeText(this, "چی پی اس خراب است", ToastLength.Short).Show();
                        break;
                    case Availability.TemporarilyUnavailable:
                        Toast.MakeText(this, "چی پی اس موقتا در دسترس نیست", ToastLength.Short).Show();
                        break;
                    default:
                        break;
                }

            }

        }

    }

    [Service(Enabled = true, Exported = false, Icon = "@drawable/StepCounterNoti")]
    public class StepCounterNotiCm : Service, ISensorEventListener
    {
        Sensor dfsensor; SensorManager MgSensor; Notification.Builder AelanNoti;
        NotificationManager MgNoti; Vibrator vibrator; int ValueSetST = 0;

        public override IBinder OnBind(Intent intent) { return null; }

        public override void OnCreate()
        {
            //MgNoti = (NotificationManager)GetSystemService(NotificationService);
            //vibrator = (Vibrator)GetSystemService(VibratorService);
            //MgSensor = (SensorManager)GetSystemService(SensorService) as SensorManager;
            //List<Sensor> ListSensor = new List<Sensor>(MgSensor.GetSensorList(SensorType.StepCounter));
            //if (ListSensor.Count > 0)
            //{
            //    dfsensor = MgSensor.GetDefaultSensor(SensorType.StepCounter);
            //    MgSensor.RegisterListener(this, dfsensor, SensorDelay.Ui);
            //}

            base.OnCreate();
        }

        public override void OnDestroy()
        {
            if (ValueSetST != 0)
            {
                ISharedPreferences isp = GetSharedPreferences("Health", FileCreationMode.WorldReadable);
                ISharedPreferencesEditor edit = isp.Edit(); int MainVB = isp.GetInt("StepCounter", 0);
                edit.PutInt("StepCounter", Convert.ToInt16(ValueSetST)); edit.Commit();
            }
            vibrator = (Vibrator)GetSystemService(VibratorService);
            if (vibrator.HasVibrator) { vibrator.Vibrate(1000); }
            MgSensor.UnregisterListener(this);
            base.OnDestroy();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            MgNoti = (NotificationManager)GetSystemService(NotificationService);
            vibrator = (Vibrator)GetSystemService(VibratorService);
            MgSensor = (SensorManager)GetSystemService(SensorService) as SensorManager;
            List<Sensor> ListSensor = new List<Sensor>(MgSensor.GetSensorList(SensorType.StepCounter));
            if (ListSensor.Count > 0)
            {
                dfsensor = MgSensor.GetDefaultSensor(SensorType.StepCounter);
                MgSensor.RegisterListener(this, dfsensor, SensorDelay.Ui);
            }

            return StartCommandResult.Sticky;
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            StartService(new Intent(this, typeof(StepCounterNotiCm)));
            base.OnTaskRemoved(rootIntent);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            if (sensor.Type == SensorType.StepCounter)
                accuracy = SensorStatus.AccuracyMedium;
        }

        public void OnSensorChanged(SensorEvent e)
        {
            ISharedPreferences isp = GetSharedPreferences("Health", FileCreationMode.WorldReadable);
            ISharedPreferencesEditor edit = isp.Edit(); int MainVB = isp.GetInt("StepCounter", 0);

            if (e.Sensor.Type == SensorType.StepCounter)
            {
                float ValueSTC = e.Values[0]; ValueSetST = (int)ValueSTC;
                AelanNoti = new Notification.Builder(this);
                AelanNoti.SetSmallIcon(Resource.Drawable.StepCounterNoti);
                AelanNoti.SetAutoCancel(false);
                AelanNoti.SetLights(Resource.Color.Red, 500, 2000);
                AelanNoti.SetContentText(" کالری : "+ (((int)ValueSTC - MainVB) * 0.1167));
                AelanNoti.SetContentTitle("تعداد گام : " + ((int)ValueSTC - MainVB) + "  مسافت : " + (((int)ValueSTC - MainVB) * 0.7) + " متر ");
                Notification nvm = AelanNoti.Build();
                PendingIntent AMPend = PendingIntent.GetActivity(this, 1, new Intent(this, typeof(Health)), PendingIntentFlags.UpdateCurrent);
                nvm.SetLatestEventInfo(this, "تعداد گام : " + ((int)ValueSTC - MainVB) + "  مسافت : " +  (((int)ValueSTC - MainVB) * 0.7) + " متر " , " کالری : " + (((int)ValueSTC - MainVB) * 0.1167), AMPend);
                nvm.Flags = NotificationFlags.NoClear;
                nvm.LedARGB = Resource.Color.Red;
                nvm.LedOnMS = 200; nvm.LedOffMS = 500;
                MgNoti.Notify(1, nvm);
            }


        }

    } 

}