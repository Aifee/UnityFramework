using UnityEngine;
using System.Collections;

public class VersionManager : Singleton<VersionManager> {

    private string verion = "";
    public int MajorVersion;
    public int MinorVersion;
    public int Revision;
    public int MonthVersion;
    public int DailyVersion;

    public string FullVersion(){
        if(verion == null || verion == "" ){
            string reversion = Revision < 10 ? "0" + Revision : Revision.ToString();
            string month = MonthVersion < 10 ? "0" + MonthVersion : MonthVersion.ToString();
            string daily = DailyVersion < 10 ? "0" + DailyVersion : DailyVersion.ToString();
            verion = string.Format("{0}.{1}.{2}.{3}.{4}",MajorVersion,MinorVersion,reversion,month,daily);
        }
        return verion;
    }
}
