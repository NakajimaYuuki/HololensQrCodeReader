using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public class Utility : MonoBehaviour{

    public static int IsUrl(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return 9;
        }

        bool isUrl = Regex.IsMatch(input, @"^s?https://esap.herokuapp.com/[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$");

        if (isUrl)
        {
            return 0;
        }

        return 9;
    }
}