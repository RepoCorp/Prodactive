package prodactivemovil.services;


public class StepServiceBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("ProdactiveMovil.Services.StepServiceBinder, ProdactiveMovil, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", StepServiceBinder.class, __md_methods);
	}


	public StepServiceBinder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == StepServiceBinder.class)
			mono.android.TypeManager.Activate ("ProdactiveMovil.Services.StepServiceBinder, ProdactiveMovil, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public StepServiceBinder (prodactivemovil.services.StepService p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == StepServiceBinder.class)
			mono.android.TypeManager.Activate ("ProdactiveMovil.Services.StepServiceBinder, ProdactiveMovil, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "ProdactiveMovil.Services.StepService, ProdactiveMovil, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}

	java.util.ArrayList refList;
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
