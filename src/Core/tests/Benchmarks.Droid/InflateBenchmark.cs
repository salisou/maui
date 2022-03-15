using Android.Content;
using Android.Views;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui;

namespace Benchmarks.Droid;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class InflateBenchmark
{
	static readonly Context context = new ContextThemeWrapper(Application.Context!, Resource.Style.Theme_AppCompat);
	static readonly LayoutInflater inflater = LayoutInflater.FromContext(context)!;

	[Benchmark]
	public void InflateXml()
	{
		var layout = inflater.Inflate(Resource.Layout.bottomtablayout, null)!;
		var bottom = layout.FindViewById<BottomNavigationView>(Resource.Id.bottomtab_tabbar);
		var navigation = layout.FindViewById<FrameLayout>(Resource.Id.bottomtab_navarea);
	}

	[Benchmark]
	public void CSharp()
	{
		var layout = new LinearLayout(context)
		{
			Orientation = Orientation.Vertical,
			LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent),
		};

		var bottom = new FrameLayout(context)
		{
			Id = View.GenerateViewId(),
			LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 0)
			{
				Gravity = GravityFlags.Fill,
				Weight = 1,
			}
		};

		var navigation = new BottomNavigationView(new ContextThemeWrapper(context, Resource.Style.Widget_Design_BottomNavigationView))
		{
			LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent),
		};

		bottom.AddView(navigation);
		layout.AddView(bottom);
	}

	[Benchmark]
	public void JavaList()
	{
		var list = ViewHelper.CreateBottomTabLayout(context, Resource.Style.Widget_Design_BottomNavigationView);
		var layout = (LinearLayout)list[0];
		var bottom = (FrameLayout)list[1];
		var navigation = (BottomNavigationView)list[2];
	}

	[Benchmark]
	public void JavaThreeCalls()
	{
		var layout = ViewHelper.CreateLinearLayout(context);
		var bottom = ViewHelper.CreateFrameLayout(context, layout);
		var navigation = ViewHelper.CreateNavigationBar(context, Resource.Style.Widget_Design_BottomNavigationView, bottom);
	}
}
