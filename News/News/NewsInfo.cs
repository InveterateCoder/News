namespace News
{
    public struct NewsInfo
    {
        public string Description { set; get; }
        public string Address { set; get; }
        public static bool operator == (NewsInfo left, NewsInfo right)
            => string.Compare(left.Description, right.Description, false) == 0;
        public static bool operator !=(NewsInfo left, NewsInfo right)
            => !(string.Compare(left.Description, right.Description, false) == 0);
        public static string Serialize(NewsInfo info)
        {
            string ret = "";
            string length = info.Description.Length.ToString();
            ret += length.Length;
            ret += length;
            ret += info.Description;
            length = info.Address.Length.ToString();
            ret += length.Length;
            ret += length;
            ret += info.Address;
            return ret;
        }
        public static NewsInfo Deserialize(string str)
        {
            NewsInfo ret = new NewsInfo();
            int index = 0;
            int length = int.Parse(str.Substring(index, 1));
            index += length + 1;
            length = int.Parse(str.Substring(index - length, length));
            ret.Description = str.Substring(index, length);
            index += length;
            length = int.Parse(str.Substring(index, 1));
            index += length + 1;
            length = int.Parse(str.Substring(index - length, length));
            ret.Address = str.Substring(index, length);
            return ret;
        }
        public override string ToString()
        {
            return Description;
        }
    }
}
