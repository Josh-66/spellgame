using UnityEngine;


public class BookPage:ScriptableObject{
    public BookInfo.PageTabs tab = BookInfo.PageTabs.None;
    public virtual void Activate(BookController bc){

    }
}