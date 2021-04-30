
using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using UK.CO.Chrisjenx.Calligraphy;
using Android.Graphics;
namespace Medical
{
    [Activity(Label = "Massagers", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Massagers : Activity,IDialogInterfaceOnClickListener
    {
        // app 
        Vibrator vibrator;PowerManager PowerMg;PowerManager.WakeLock WakeLock;TextView TxtPanel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.ThemeaMain);
            SetContentView(Resource.Layout.Massagers);
            TxtPanel = (TextView)FindViewById(Resource.Id.MassagersTextViewPanel);

            vibrator = GetSystemService(Context.VibratorService) as Vibrator;
            PowerMg = (PowerManager)GetSystemService(Context.PowerService);
            WakeLock = PowerMg.NewWakeLock(WakeLockFlags.Full, "DoNotSleep");
            WakeLock.Acquire();

            TxtPanel.Text = "";
            Button BtnTouchMS = (Button)FindViewById(Resource.Id.MassagersBtnMSHandel);
            BtnTouchMS.Background.SetColorFilter(Color.ParseColor("#da5145"), PorterDuff.Mode.Multiply);
            BtnTouchMS.SetTextColor(Color.WhiteSmoke);
            BtnTouchMS.Touch += BtnTouchMS_Touch;
        }

        private void BtnTouchMS_Touch(object sender, View.TouchEventArgs e)
        {
            TxtPanel.Text = "حالت ماساژور : دستی";
            vibrator.Vibrate(1000);
        }

        public void MassagerAuto()
        {
            AlertDialog.Builder Msg = new AlertDialog.Builder(this);
            Msg.SetTitle("سبک ماساژ را انتخاب کنید");
            Msg.SetItems(new string[] { "سبک 1","سبک 2","سبک 3","سبک 4" }, this);
            Msg.SetNeutralButton("لغو", delegate { });
            Msg.SetCancelable(false);
            Msg.Create();Msg.Show();
        }

        public void MassagerCustom()
        {
            AlertDialog.Builder MsgBox = new AlertDialog.Builder(this);
            View view = View.Inflate(this, Resource.Layout.MassagersCustom, null);
            MsgBox.SetView(view);
            NumberPicker TimeNP = (NumberPicker)view.FindViewById(Resource.Id.MassagersNumberPickerTime);
            TimeNP.MaxValue = 30;
            ToggleButton RateTB = (ToggleButton)view.FindViewById(Resource.Id.MassagersToggleButtonRate);
            RateTB.SetTextColor(Color.WhiteSmoke);
            RateTB.Background.SetColorFilter(Color.Maroon, PorterDuff.Mode.Multiply);
            MsgBox.SetTitle("زمان را انتخاب کنید");
            MsgBox.SetPositiveButton("اجرا", delegate
            {
                if (RateTB.Checked)
                {
                    long[] VibratorVM = new long[30];
                    for (int i = 0; i < 30; i++)
                    {
                        VibratorVM[i] = (long)(TimeNP.Value * 1000) / 30;
                    }
                    vibrator.Vibrate(VibratorVM, -1);
                }
                else
                {
                    vibrator.Vibrate((TimeNP.Value * 1000));
                }
                TxtPanel.Text = "حالت ماساژور : سفارشی";
            });
            MsgBox.SetNegativeButton("لغو", delegate { });
            MsgBox.SetCancelable(false);
            MsgBox.Create();MsgBox.Show();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 300, 0, "ماساژ اتوماتیک");
            menu.Add(0, 301, 0, "ماساژ سفارشی");
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 300:
                    MassagerAuto();
                    break;
                case 301:
                    MassagerCustom();
                    break;
                default:
                    break;
            }
            return base.OnMenuItemSelected(featureId, item);
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            switch (which)
            {
                case 0:
                    vibrator.Vibrate(new long[] { 100, 110, 120, 130, 140, 150, 160, 200, 250, 250, 500, 750, 750, 1000,
                        400, 300, 150,100, 110, 120, 130, 140, 150, 160, 200, 250, 250, 500 }, -1);
                    break;
                case 1:
                    vibrator.Vibrate(new long[] { 50, 60, 70, 80, 90, 100, 120, 150, 180, 200, 250, 300, 350, 400, 450, 500, 550, 600,
                        650, 700, 750, 800, 900, 1000, 400, 450, 500, 550, 600, 650, 700, 750, 800, 900, 1000}, -1);
                    break;
                case 2:
                    vibrator.Vibrate(new long[] { 100, 200, 300, 400, 500, 500, 1000, 1500, 2000, 2500,
                        3000, 200, 300, 400, 500, 100, 200, 300, 400, 500, 500 }, -1);
                    break;
                case 3:
                    vibrator.Vibrate(new long[] { 1000, 2000, 3000, 4000, 5000, 5000, 1000, 2000, 3000 }, -1);
                    break;
                default:
                    break;
            }
            TxtPanel.Text = "حالت ماساژور : اتوماتیک";
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            vibrator.Cancel();
            WakeLock.Release();
        }

        protected override void AttachBaseContext(Android.Content.Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }

    }
}