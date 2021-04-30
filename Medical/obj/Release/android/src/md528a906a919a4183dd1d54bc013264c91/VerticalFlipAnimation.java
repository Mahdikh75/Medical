package md528a906a919a4183dd1d54bc013264c91;


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
		mono.android.Runtime.register ("FlipNumbers.VerticalFlipAnimation, Xamarin.Controls.FlipNumbers.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", VerticalFlipAnimation.class, __md_methods);
	}


	public VerticalFlipAnimation () throws java.lang.Throwable
	{
		super ();
		if (getClass () == VerticalFlipAnimation.class)
			mono.android.TypeManager.Activate ("FlipNumbers.VerticalFlipAnimation, Xamarin.Controls.FlipNumbers.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public VerticalFlipAnimation (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == VerticalFlipAnimation.class)
			mono.android.TypeManager.Activate ("FlipNumbers.VerticalFlipAnimation, Xamarin.Controls.FlipNumbers.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}

	public VerticalFlipAnimation (float p0, float p1, float p2, float p3) throws java.lang.Throwable
	{
		super ();
		if (getClass () == VerticalFlipAnimation.class)
			mono.android.TypeManager.Activate ("FlipNumbers.VerticalFlipAnimation, Xamarin.Controls.FlipNumbers.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "System.Single, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Single, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Single, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Single, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3 });
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
