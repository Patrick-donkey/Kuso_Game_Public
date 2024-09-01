//using System;
//using UnityEditor;
using UnityEngine;

//卡牌的各種參數(名字,類型,使用目標,稀有度tag,稀有度顏色,卡牌外型,卡牌效果 ScriptableObj) 
namespace RiverCrab
{
    [System.Serializable]
    public class Card
    {
        public int TimesOfUse;
        public string Name;
        public string Type;
        public string ObjTag;
        public string Rarity;
        public Color RarityColor;
        public Sprite Image;
        public CardEffect Effect;       

        public Card(int timeofuse,string name, string type, string rarity, Color raritycolor, Sprite image, CardEffect effect, string objtag)
        {
            TimesOfUse = timeofuse;
            Name = name;
            Type = type;
            Rarity = rarity;
            RarityColor = raritycolor;
            Image = image;
            Effect = effect;
            ObjTag = objtag;            
        }

        //public void DisplayRare(List<Card> cards)
        //{
        //    CardRare CR = GameObject.Find("CardDraw").GetComponent<CardRare>();
        //    Color color = cards[0].Rarity;
        //    CR.Rare = color;
        //}
        //public void DisplayType(List<Card> cards)
        //{
        //    CardRare CR = GameObject.Find("CardDraw").GetComponent<CardRare>();
        //    string name = cards[0].Type;
        //    CR.Name = name;
        //}
        //public void DisplayName(List<Card> cards)
        //{
        //    CardRare CR = GameObject.Find("CardDraw").GetComponent<CardRare>();
        //    string type = cards[0].Name;
        //    CR.Type = type;
        //}
        //public void DisplayImage(List<Card> cards)
        //{
        //    CardRare CR = GameObject.Find("CardDraw").GetComponent<CardRare>();
        //    Sprite sprite = cards[0].Image;
        //    CR.CardSprite = sprite;        
        //}
        //public void DisplayEffect(List<Card> cards)
        //{
        //    CardRare CR = GameObject.Find("CardDraw").GetComponent<CardRare>();
        //    CardEffect cardeff = cards[0].Effect;
        //    CR.cardeffect = cardeff;
        //}
        //public void DisplayObjTag(List<Card> cards)
        //{
        //    CardRare CR = GameObject.Find("CardDraw").GetComponent<CardRare>();
        //    string objtag = cards[0].ObjTag;
        //    CR.ObjTag = objtag;
        //}
    }
}

//public enum Type
//{
//    Attack, Defend, Buff, Debuff
//}
//public enum Name
//{
//    Name1 = 0, Name2, Name3, Name4, Name5, Name6, Name7, Name8, Name9, Name10
//}
//public enum ImageCard
//{
//    Image1, Image2, Image3, Image4, Image5, Image6
//}

//    public Rare rare { get; private set; }
//    public Type type { get; private set; }
//    public Name Cardname { get; private set; }
//    public ImageCard Cardimage { get; private set; }
//    public Card(Rare rare, Type type, Name name, ImageCard cardimage)
//    {
//        this.rare = rare;
//        this.type = type;
//        this.Cardname = name;
//        this.Cardimage = cardimage;        
//    }
//    public static Name[] Names = (Name[])Enum.GetValues(typeof(Name));
//    public static Dictionary<Name, string> CardName = new Dictionary<Name, string>
//    {
//        {Names[0],"Card1"},
//        {Names[1],"Card2"},
//        {Names[2],"Card3"},
//        {Names[3],"Card4"},
//        {Names[4],"Card5"},
//        {Names[5],"Card6"},
//        {Names[6],"Card7"},
//        {Names[7],"Card8"},
//        {Names[8],"Card9"},
//        {Names[9],"Card10"}
//    };
//    public static ImageCard[] Images = (ImageCard[])Enum.GetValues(typeof(ImageCard));
//    public static Dictionary<ImageCard, string> CardImage = new Dictionary<ImageCard, string>
//    {
//        {Images[0],"Free Chicken Sprites_0"},//{ImageCard索引數,Sprite名稱},
//        {Images[1],"Free Chicken Sprites_1"},
//        {Images[2],"Free Chicken Sprites_2"},
//        {Images[3],"Free Chicken Sprites_3"},
//        {Images[4],"Free Chicken Sprites_4"},
//        {Images[5],"Free Chicken Sprites_5"},
//    };
//    public void LoadAndDisplayImage(ImageCard tag,List<Sprite> sprites)
//    {
//#if UNITY_EDITOR
//        if (CardImage.TryGetValue(tag, out string path))
//        {
//            // 切割紋理
//            //Sprite image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
//            // 顯示圖片
//            CardRare CR = GameObject.Find("CardDraw").GetComponent<CardRare>();
//            Sprite sprite = sprites.Find(s => s.name == path);
//            CR.CardSprite = sprite;
//            Debug.Log($"Loaded image for {tag}: {path}");
//        }
//        else
//        {
//            Debug.LogError($"No image found for {tag}");
//        }
//#else
//        Debug.LogError("AssetDatabase can only be used in the Unity Editor.");
//#endif
//    }
//    // Start is called before the first frame update
