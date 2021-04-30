package md57a361a59211582a763cb42f9adac1e58;


public class FlipNumbersView
	extends android.widget.LinearLayout
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("FlipNumbers.FlipNumbersView, Xamarin.Controls.FlipNumbers.Android", FlipNumbersView.class, __md_methods);
	}


	public FlipNumbersView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == FlipNumbersView.class)
			mono.android.TypeManager.Activate ("FlipNumbers.FlipNumbersView, Xamarin.Controls.FlipNumbers.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public FlipNumbersView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == FlipNumbersView.class)
			mono.android.TypeManager.Activate ("FlipNumbers.FlipNumbersView, Xamarin.Controls.FlipNumbers.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public FlipNumbersView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == FlipNumbersView.class)
			mono.android.TypeManager.Activate ("FlipNumbers.FlipNumbersView, Xamarin.Controls.FlipNumbers.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
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
