using GoogleMobileAds.Api;

public class AdMob{
	
	private BannerView bannerViewTop;
	private BannerView bannerViewBottom;
	private InterstitialAd interstitial;
	private string adUnitIdBanner        = "ca-app-pub-6904186947817626/6737471590";
	private string adUnitIdInterstitial = "ca-app-pub-6904186947817626/8284077196";	
	
	public void requestBanner()
	{
		#if UNITY_ANDROID
		string adUnitId = this.adUnitIdBanner;
		#elif UNITY_IPHONE
		string adUnitId = this.adUnitIdBanner;
		#else
		string adUnitId = this.adUnitIdBanner;
		#endif
		
		bannerViewTop = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
		bannerViewBottom = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
		
		AdRequest request = new AdRequest.Builder().Build();
		bannerViewTop.LoadAd(request);
		bannerViewBottom.LoadAd(request);
	}
	
	public void requestBannerInterstitial()
	{	
		#if UNITY_ANDROID
		string adUnitId = this.adUnitIdInterstitial;
		#elif UNITY_IPHONE
		string adUnitId = this.adUnitIdInterstitial;
		#else
		string adUnitId = this.adUnitIdInterstitial;
		#endif
		
		interstitial = new InterstitialAd(adUnitId);
		AdRequest request = new AdRequest.Builder().Build();
		interstitial.LoadAd(request);
	}
	
	public void showInterstitial() { 
		if (interstitial.IsLoaded()) { 
			interstitial.Show(); 
		}	
	}
	
	public void hideBanners() { 
		bannerViewTop.Hide();
		bannerViewBottom.Hide ();
	}
	
	public void showBanners() { 
		bannerViewTop.Show();
		bannerViewBottom.Show();
	}
}