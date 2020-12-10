using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour //IUnityAdsListener
{
    string placement = "rewardedVideo";

    IEnumerator Start()
    {
        // Advertisement.AddListener(this);
        Advertisement.Initialize("3351412", true);

        while (!Advertisement.IsReady(placement))
            yield return null;

        Advertisement.Show(placement);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(showResult == ShowResult.Finished)
        {
            //continue button weer availible 
        }
        else if (showResult == ShowResult.Failed)
        {
            //ad is niet gekeken of onderbroken
        }
    }

    public void OnUnityAdsStart(string placementId)
    {

    }
}
