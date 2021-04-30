package md57a361a59211582a763cb42f9adac1e58;


public class VerticalFlipAnimation
	extends android.view.animation.Animation
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_initialize:(IIII)V:GetInitialize_IIIIHandler\n" +
			"n_applyTransformation:(FLandroid/view/animation/Transformation;)V:GetApplyTransformation_FLandroid_view_animation_Transformation_Handler\n" +
			"";
		mono.android.Runtime.register ("FlipNumbers.VerticalFlipAnimation, Xamarin.Controls.FlipNumbers.Android", VerticalFlipAnimation.class, __md_methods);
	}


	public VerticalFlipAnimation ()
	{
		super ();
		if (getClass () == VerticalFlipAnimation.class)
			mono.android.TypeManager.Activate ("FlipNumbers.VerticalFlipAnimation, Xamarin.Controls.FlipNumbers.Android", "", this, new java.lang.Object[] {  });
	}


	public VerticalFlipAnimation (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == VerticalFlipAnimation.class)
			mono.android.TypeManager.Activate ("FlipNumbers.VerticalFlipAnimation, Xamarin.Controls.FlipNumbers.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}

	public VerticalFlipAnimation (float p0, float p1, float p2, float p3)
	{
		super ();
		if (getClass () == VerticalFlipAnimation.class)
			mono.android.TypeManager.Activate ("FlipNumbers.VerticalFlipAnimation, Xamarin.Controls.FlipNumbers.Android", "System.Single, mscorlib:System.Single, mscorlib:System.Single, mscorlib:System.Single, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void initialize (int p0, int p1, int p2, int p3)
	{
		n_initialize (p0, p1, p2, p3);
	}

	private native void n_initialize (int p0, int p1, int p2, int p3);


	public void applyTransformation (float p0, android.view.animation.Transformation p1)
	{
		n_applyTransformation (p0, p1);
	}

	private native void n_applyTransformation (float p0, android.view.animation.Transformation p1);

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
