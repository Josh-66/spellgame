using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BookInfo", menuName = "BookInfo", order = 1)]
public class BookInfo:ScriptableObject{
    public List<BookPage> pages;
}