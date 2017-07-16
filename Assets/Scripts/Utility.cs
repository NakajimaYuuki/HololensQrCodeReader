using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public class Utility : MonoBehaviour{

    public static bool IsUrl(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        bool isUrl = Regex.IsMatch(input, @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$");

        if (!isUrl)
        {
            return Regex.IsMatch( input, @"^s?http?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$");

        }
        return isUrl;
    }
}