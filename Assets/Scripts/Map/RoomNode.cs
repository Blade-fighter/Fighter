using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenerateRoom
{
    public enum RoomType
    {
        Start,
        Normal,
        Shop,
        Boss
    }

    public class RoomNode
    {
        public int X;
        public int Y;
        public RoomType Type { get;  set; } = RoomType.Start;

        public RoomNode(int x, int y, RoomType type = RoomType.Start)
        {
            Type = type;
            X = x;
            Y = y;
        }

        public List<RoomNode> NextRooms = new List<RoomNode>();

        public RoomNode AddNextRoom(RoomNode node)
        {
            NextRooms.Add(node);
            return this;
        }
    }

    public class RoomConfig
    {
        public RoomNode StartRoom = new RoomNode(0, 0);

        public Dictionary<RoomType, int> RoomCounts = new Dictionary<RoomType, int>
        {
            { RoomType.Start, 0 },
            { RoomType.Normal, 10 },
            { RoomType.Shop, 3 },
            { RoomType.Boss, 0 }
        };

        public RoomConfig()
        {
            Init();
        }

        private void Init()
        {
            StartRoom.Type = RoomType.Start;

            // Generate rooms based on counts
            foreach (var roomType in RoomCounts)
            {
                for (int i = 0; i < roomType.Value; i++)
                {
                    var room = new RoomNode(Random.Range(-5, 5), Random.Range(1, 10), roomType.Key);
                    var bossRoom = new RoomNode(0, 10, RoomType.Boss);
                    StartRoom.AddNextRoom(room);
                    StartRoom.AddNextRoom(bossRoom);
                }
            }
        }
    }
}
