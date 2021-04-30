package md528a906a919a4183dd1d54bc013264c91;


public class FlipDigitView
	extends android.widget.RelativeLayout
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("FlipNumbers.FlipDigitView, Xamarin.Controls.FlipNumbers.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", FlipDigitView.class, __md_methods);
	}


	public FlipDigitView (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == FlipDigitView.class)
			mono.android.TypeManager.Activate ("FlipNumbers.FlipDigitView, Xamarin.Controls.FlipNumbers.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public FlipDigitView (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == FlipDigitView.class)
			mono.android.TypeManager.Activate ("FlipNumbers.FlipDigitView, Xamarin.Controls.FlipNumbers.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public FlipDigitView (android.content.Context p0, android.util.AttributeSet p1, int p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == FlipDigitView.class)
			mono.android.TypeManager.Activate ("FlipNumbers.FlipDigitView, Xamarin.Controls.FlipNumbers.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
