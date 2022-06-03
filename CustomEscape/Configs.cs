namespace CustomEscape
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;

    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;

    using Points;
    using Points.DataTypes;
    using Points.Tools;

    using UnityEngine;

    public class Configs : IConfig
    {
        // who said nested?
        public Dictionary<string, EscapePointConfig> EscapePoints { get; set; } =
            new Dictionary<string, EscapePointConfig>
            {
                {
                    "escape0", new EscapePointConfig
                    {
                        EscapeRadius = 1000f,
                        RoleConversions = new Dictionary<RoleType, PrettyCuffedConfig>
                        {
                            {
                                RoleType.ClassD,
                                new PrettyCuffedConfig
                                    { CuffedRole = RoleType.NtfPrivate, UnCuffedRole = RoleType.ChaosConscript }
                            },
                            {
                                RoleType.Scientist,
                                new PrettyCuffedConfig
                                    { CuffedRole = RoleType.ChaosConscript, UnCuffedRole = RoleType.NtfSpecialist }
                            },
                        },
                    }
                },
            };


        [Description("Points file.  它以ID和原始XYZ数据的形式包含所有撤离点位置")]
        public string PointsFileName { get; set; } = "EscapePoints";

        public bool Debug { get; set; } = false;
        [Description("启用自定义撤离点")] public bool IsEnabled { get; set; } = true;

        public void TryCreateFile()
        {
            PointList pointList = Points.GetPointList(CustomEscape.Singleton.Config.PointsFileName);
            if (FileManager.FileExists(Path.Combine(PointIO.FolderPath, PointsFileName) + ".txt"))
                return;
            Log.Info("Creating new EscapePoint file using default spawn points.");

            pointList.RawPoints.Add(
                new RawPoint("escape0", RoomType.Surface,
                    new Vector3(170f, -16.6f, 25f), new Vector3(0f, 0f, 0f))
            );
            PointIO.Save(pointList, Path.Combine(PointIO.FolderPath, PointsFileName) + ".txt");
            pointList.FixData();
        }
    }

    public class EscapePointConfig
    {
        [Description("撤离点有多大")]
        public float EscapeRadius { get; set; } = 1000f;

        [Description(
            "不言而喻。错误的配置将导致角色更改为Scp173。 " +
            "您可以传递None以不更改角色。 " +
            "确保遵循示例格式，否则可能会中断")]
        public Dictionary<RoleType, PrettyCuffedConfig> RoleConversions { get; set; } =
            new Dictionary<RoleType, PrettyCuffedConfig>
            {
                {
                    RoleType.ClassD,
                    new PrettyCuffedConfig
                        { CuffedRole = RoleType.NtfPrivate, UnCuffedRole = RoleType.ChaosConscript }
                },
                {
                    RoleType.Scientist,
                    new PrettyCuffedConfig
                        { CuffedRole = RoleType.ChaosConscript, UnCuffedRole = RoleType.NtfSpecialist }
                },
            };
    }

    public class
        PrettyCuffedConfig // Because of this there will be "cuffed_role" and "un_cuffed_role" config entries instead of just dictionaries
    {
        public RoleType CuffedRole { get; set; } = RoleType.ChaosConscript;

        [Description("玩家在逃跑时失去库存")]
        public bool CuffedClearInventory { get; set; } = false;

        public RoleType UnCuffedRole { get; set; } = RoleType.NtfPrivate;

        [Description("玩家在逃跑时失去库存")]
        public bool UnCuffedClearInventory { get; set; } = false;
    }
}
