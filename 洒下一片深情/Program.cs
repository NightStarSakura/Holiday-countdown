
using Jint.Runtime.Interop;
using Jint;
using Jint.Runtime;
using Esprima.Ast;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using MoreLinq;

namespace 洒下一片深情
{

    internal static class Program
    {


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            List<int> list = new List<int>() { 2, 1, 4, 3 };
            list.OrderBy(p => p);
            list.ForEach(p => { Console.WriteLine(p); });
            List<string> tst = new List<string>() { "g","2g","3g","4g"};
            tst.OrderBy(x => x).ForEach(p=>
            {
                Console.WriteLine(p);
            }) ;

            List<int> list1 = new List<int> { 1, 2, 3, 4, 5, 6,7,8,9,10,11,12 };
            List<int> list2 = new List<int> { 13,14,10,9,8,15,8,7,6,5,4,16,17,18 };

            List<int> matchingIndices = new List<int>();

            for (int i = 1; i < list2.Count-1; i++)
            {
                if (list1.Contains(list2[i]))
                {
                    int index1 = list1.IndexOf(list2[i]);
                    if(index1==0||index1==list1.Count-1||index1==-1)
                        continue;
                    if (list1[index1 - 1] != list2[i + 1])
                        continue;
                    matchingIndices.Add(i);
                    matchingIndices.Add(i+1);
                    var temp = 2;
                    while (index1-temp!=-1&&i+temp!=list2.Count&& list1[index1 - temp] == list2[i + temp])
                    {
                        matchingIndices.Add(i+temp);
                        temp++;
                    }
                    i += temp;
                }
            }

            List<int> matchingElements = matchingIndices.Select(index => list2[index]).ToList();

            Console.WriteLine("满足条件的元素：");
            foreach (var element in matchingElements)
            {
                Console.WriteLine(element);
            }
            //SnakeGame.Start();
            //四向车心跳报文
            byte[] data = new byte[] { 0x02, 0xFD, 0x00, 0x1A, 0x01, 0x01, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1A };
            List<string> strlist = new List<string>()
            {
              "李","凤","鸣","翟","孙","浩","杰","张","郭","宁","伟","王","赵","松","牛","堃","刘","楠",//18
              "宇","超","劲","洁","晨","晖","嘉","鑫","帅","武","举","良","志","婷","苗","陈","璐","芮",//36
              "琪","亚","薛","姬","杨","祥","辉","丁","哲","经","紫","闫","达","茹","娜","阳","柯","笑",//54
              "宋","团","高","丽","风","翔","娇","漆","乐","乌","鸦","当","瓶","含","峰","旭","日","月",//72
              "星","辰","楼","蚁","苍","生","堂","庙","忧","其","民","亦","未","眠","晓","不","知","道",//90
              "醉","翁","意","欲","何","为","则","林","火","金","铁","车","炮","仕","田","马","庞","涓",//108
              "叶","常","春","秋","冬","夏","雨","威","婶","姨","子","女","父","母","刃","卡","夫","孔",//126
              "孟","庄","墨","泰","韩","晕","杀","兹","卖","钱","江","河","汉","流","水","清","无","鱼",//144
              "困","旋","转","我","会","心","等","有","天","看","蹄","踏","万","勤","毛","葛","宾","昕",//162
              "维","杜","晃","焦","杰","谦","奶","珍","明","涛","政","全","标","杯","易","老","界","宙",//180
              "伍","郝","凡","善","恶","勿","为","烦","虑","好","实","爱","坦","制","人","犬","黑","白",//198
              "匆","命","咽","下","独","甜","短","逢","化","尸","骨","跳","蝇","鹰","瓜","武","哭","箭",//216
              "锋","所","指","永","秦","卢","牛","腰","辣","畅","滚","岗","立","戴","惠","猜","小","阿",//234
              "外","力","何","蓉","瑜","崔","邢","悦","梦","安","师","尊","驷","仨","欢","鼠","鸭","脖",//252
              "碧","航","莉","角","兽","希","丝","桐","蒂","经","如","亦","送","颠","倒","齐","乎","完",//270
              "胜","慧","宗","穆","帅","什","开","桂","榆","次","点","油","鲁","权","仲","噹","咣","府",//288
              "仙","佐","索","樱","狼","渚","汐","冈","沛","韩","弃","辛","病","彻","赢","固","态","汁",//306
              "尬","尴","魑","减","除","乘","洛","斯","麦","玻","羽","羊","配","嘟","昂","娴","琳","潘",//324
              "狄","方","铂","钻","佰","栗","聂","基","蚁","绵","梨","果","睡","困","辑","磁","戳","南",//342
              "霄","曦","震","中","傻","瓢","七","岔","双","妈","红","巾","共","产","孝","忠","义","光" //360
            };
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        static ushort CalculateModbusRTUCRC(byte[] data)
        {
            ushort crc = 0xFFFF;

            for (int i = 0; i < data.Length; i++)
            {
                crc ^= data[i];

                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }

            // 将 CRC 寄存器的高、低字节进行交换
            crc = (ushort)((crc << 8) | (crc >> 8));

            return crc;
        }

    }
}

   