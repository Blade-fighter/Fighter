using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenerateRoom
{
    public enum RoomType{
        Start,
        Normal,
        Shop,
        Boss
    }
    public class RoomNode
    {
        public int X;
        public int Y;
        public RoomType Type{ get; private set; } = RoomType.Start;

        public RoomNode(int x, int y, RoomType type = RoomType.Start){
            Type = type;
            X = x;
            Y = y;
        }

        public List<RoomNode> NextRooms = new List<RoomNode>();

        public  RoomNode AddNextRoom(RoomNode node){
            NextRooms.Add(node);
            return this;
        }
    }
    public class RoomConfig
    {
        //��һ��
        public RoomNode StartRoom = new RoomNode(0,0);

        public RoomConfig()
        {
            Init();
        }

        private void Init()
        {
            //�ڶ���
            var level2Room1 = new RoomNode(-1, 1, RoomType.Normal);
            var level2Room2 = new RoomNode(0, 1, RoomType.Normal);
            var level2Room3 = new RoomNode(1, 1, RoomType.Normal);
            StartRoom.AddNextRoom(level2Room1)
                .AddNextRoom(level2Room2)
                .AddNextRoom(level2Room3);
            
            //������
            var level3Room1 = new RoomNode(-1, 2, RoomType.Normal);
            var level3Room2 = new RoomNode(0, 2, RoomType.Normal);
            var level3Room3 = new RoomNode(1, 2, RoomType.Normal);

            level2Room1.AddNextRoom(level3Room2);
            level2Room2.AddNextRoom(level3Room2)
                .AddNextRoom(level3Room3);
            level2Room3.AddNextRoom(level3Room1);
            
            //���Ĳ�
            var level4Room1 = new RoomNode(-2, 3, RoomType.Normal);
            var level4Room2 = new RoomNode(-1, 3, RoomType.Normal);
            var level4Room3 = new RoomNode(1, 3, RoomType.Normal);
            var level4Room4 = new RoomNode(2, 3, RoomType.Normal);

            level3Room1.AddNextRoom(level4Room3)
                .AddNextRoom(level4Room4);
            level3Room2.AddNextRoom(level4Room2);
            level3Room3.AddNextRoom(level4Room1);
            
            //����� �̵��
            var shopRoom = new RoomNode(0, 4, RoomType.Shop);
            
            level4Room1.AddNextRoom(shopRoom);
            level4Room2.AddNextRoom(shopRoom);
            level4Room3.AddNextRoom(shopRoom);
            level4Room4.AddNextRoom(shopRoom);
            
            //������
            var level6Room1 = new RoomNode(-1, 5, RoomType.Normal);
            var level6Room2 = new RoomNode(1, 5, RoomType.Normal);

            shopRoom.AddNextRoom(level6Room1)
                .AddNextRoom(level6Room2);
            
            //���߲�
            var level7Room1 = new RoomNode(-1, 6, RoomType.Normal);
            var level7Room2 = new RoomNode(0, 6, RoomType.Normal);
            var level7Room3 = new RoomNode(1, 6, RoomType.Normal);

            level6Room1.AddNextRoom(level7Room1)
                .AddNextRoom(level7Room3);
            level6Room2.AddNextRoom(level7Room2);
            
            //�ڰ˲�
            var level8Room1 = new RoomNode(-1, 7, RoomType.Normal);
            var level8Room2 = new RoomNode(1, 7, RoomType.Normal);

            level7Room1.AddNextRoom(level8Room1);
            level7Room2.AddNextRoom(level8Room2);
            level7Room3.AddNextRoom(level8Room2);
            
            //�ھŲ�
            var bossRoom = new RoomNode(0, 8, RoomType.Boss);
            level8Room1.AddNextRoom(bossRoom);
            level8Room2.AddNextRoom(bossRoom);
        }
    }
}
