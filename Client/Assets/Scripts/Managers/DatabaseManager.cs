using UnityEngine;
using System.Collections;

public class DatabaseManager  {

    #region Map Database

    private static MapDatabase mapDatabase;
    public static MapDatabase MapDatabase{
        get{
            if(mapDatabase == null){
                mapDatabase = new MapDatabase();
            }
            return mapDatabase;
        }
    }

    #endregion

    #region HeroDatabase

    static private HeroDatabase heroDatabase;
    static public HeroDatabase HeroDatabase{
        get{
            if(heroDatabase == null){
                heroDatabase = new HeroDatabase();
            }
            return heroDatabase;
        }
    }

    #endregion 
}
