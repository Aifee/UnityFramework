using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Inventory
{
    /// <summary>
    /// 玩家拥有的所有建筑
    /// </summary>
    public BuildingDatabase WholeBuildings;
    /// <summary>
    /// 正在建造的建筑
    /// </summary>
    public BuildingDatabase BuiltBuildings;
    /// <summary>
    /// 存在场景中的建筑
    /// </summary>
    public BuildingDatabase InSceneBuildings;
    /// <summary>
    /// 未存在场景中的建筑
    /// </summary>
    public BuildingDatabase OutSceneBuildings;
    /// <summary>
    /// 玩家拥有的所有英雄
    /// </summary>
    private HeroDatabase WholeHeros;
    //正在招募的英雄
    private HeroDatabase RecruitingHeros;
    /// <summary>
    /// 所携带的英雄，指上阵的，可以战斗的英雄
    /// </summary>
    private HeroDatabase CarryHeros;

    private Dictionary<int, Item> Items = new Dictionary<int, Item>();
    private Dictionary<int, Equip> Equips = new Dictionary<int, Equip>();

    public Player(){
        WholeBuildings = new BuildingDatabase();
        BuiltBuildings = new BuildingDatabase();
        InSceneBuildings = new BuildingDatabase();
        OutSceneBuildings = new BuildingDatabase();

        WholeHeros = new HeroDatabase();
        RecruitingHeros = new HeroDatabase();
        CarryHeros = new HeroDatabase();

    }

    #region Building data manipulation logic

    #endregion

    #region Hero data manipulation logic
    
    #endregion
}
