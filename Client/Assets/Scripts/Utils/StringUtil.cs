using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography; 
using System.Text.RegularExpressions;

public static class StringUtil
{
	/// <summary>
	/// MD5 String
	/// </summary>
	/// <returns>
	static public string MD5(string str)
	{
		byte[] b = Encoding.Default.GetBytes(str);
		MD5 md5 = new MD5CryptoServiceProvider();
		byte[] c = md5.ComputeHash(b);
		return System.BitConverter.ToString(c).Replace("-", "");
	}

	/// <summary>
	/// 将一个int只转换成英制字符串
	/// </summary>
	/// <param name="arg"></param>
	/// <returns></returns>
	public static string BritishSystem(int arg)
	{
		string british = arg.ToString();
		int i = british.Length;
		while (true)
		{
			i -= 3;
			if (i <= 0)
			{
				break;
			}
			british = british.Insert(i, ",");
		}
		return british;
	}
    /// <summary>
    /// 检测一个字符串是否符合规则，A-Z,a-z0-9
    /// </summary>
    /// <returns><c>true</c>, if mate was regulared, <c>false</c> otherwise.</returns>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    /// <param name="info">Info.</param>
    static public bool RegularMate(this string info,int min,int max){
        string code = string.Format(@"^[A-Za-z0-9]{{0},{1}}$",min,max);
        System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(code);
        bool mat = rx.IsMatch(info);
        return mat;
    }
    /// <summary>
    /// 将一个double数据转换成00：00：00格式
    /// </summary>
    /// <returns>The to time.</returns>
    /// <param name="value">Value.</param>
    static public string SecondToTime(double value){
        if(value < 0)
            return "00:00:00";
        System.DateTime dt = new System.DateTime();
        dt = dt.AddSeconds(value);
        string str = dt.ToString("HH:mm:ss");
        return str;
    }
    /// <summary>
    /// 移除字符串总的/n/r转移符
    /// </summary>
    /// <returns>The terminator.</returns>
    /// <param name="text">Text.</param>
    public static string ReqularTerminator(this string text) {
        return text.Replace("\n", string.Empty).Replace("\r", string.Empty);
        string terminator = @"\\n";
        return Regex.Replace(text, terminator, "");
    }
    /// <summary>
    /// 转换成 int 类型
    /// </summary>
    /// <returns>The int.</returns>
    /// <param name="value">Value.</param>
    static public int ToInt(this string value){
        return int.Parse(value);
    }
    /// <summary>
    /// 转换成 float 类型
    /// </summary>
    /// <returns>The float.</returns>
    /// <param name="value">Value.</param>
    static public float ToFloat(this string value){
        return float.Parse(value);
    }
    /// <summary>
    /// 转换成Enum类型
    /// </summary>
    /// <returns>The enum.</returns>
    /// <param name="value">Value.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    static public T ToEnum<T>(this string value) where T : struct, System.IConvertible {
        return (T)System.Enum.Parse(typeof(T),value);
    }
    /// <summary>
    /// pattern匹配正则
    /// </summary>
    /// <param name="text"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static bool IsMatch(this string text, string pattern)
    {
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        return regex.IsMatch(text);
    }
    /// <summary>
    /// 是否为数字 可以是正负数，也可以带小数点
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsNumber(this string text)
    {
        return IsMatch(text, @"^[-]?\d+([.]\d+)?$");
    }
    
    /// <summary>
    /// 是否为英文字符
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsEnglish(this string text)
    {
        return IsMatch(text, @"^[a-zA-Z]+$");
    }
    
    /// <summary>
    /// 是否为汉字
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsChinese(this string text)
    {
        return IsMatch(text, @"^[\u4e00-\u9fa5]+$");
    }
    
    /// <summary>
    /// 是否为英文单词
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsWord(this string text)
    {
        return IsMatch(text, @"^[a-zA-Z0-9]+$");
    }
    /// <summary>
    /// 是否为电话号码
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsTelphone(this string text)
    {
        return IsMatch(text, @"^(0\d{2,3}(\-)?)?\d{7,10}$");
    }
    
    /// <summary>
    /// 是否为手机
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsMobile(this string text)
    {
        return IsMatch(text, @"^((147)|(13[0-9])|(15[^4])|(18[0,1,2,3,5-9]))\d{8}$");
    }
    
    /// <summary>
    /// 是否为邮件
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsEmail(this string text)
    {
        return IsMatch(text, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
    }
    
    /// <summary>
    /// 是否为长日期类型 yyyy-MM-dd hh:mm:ss
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsLongDate(this string text)
    {
        return IsMatch(text, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$");
    }
    
    /// <summary>
    /// 是否为短日期类型 yyyy-MM-dd
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsShortDate(this string text)
    {
        return IsMatch(text, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
    }


}
