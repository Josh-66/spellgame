using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BookInfo", menuName = "BookInfo", order = 1)]
public class BookInfo:ScriptableObject{
    public List<BookPage> pages;
    public enum PageTabs{
        Rules,Element,Form,Strength,Style,Index,Options,None
    }
    public Dictionary<PageTabs,int> tabToPageNumber;
    public Dictionary<GlyphType,int> glyphToIndexPage;

    public int PageOf(PageTabs tab){
        if (tabToPageNumber==null)
            MakeDicts();
        if (tabToPageNumber.ContainsKey(tab))
            return tabToPageNumber[tab];
        else return 0;
    }

    public void MakeDicts(){
        tabToPageNumber=new Dictionary<PageTabs, int>();
        for (int i = 0 ; i < pages.Count;i++){
            if (pages[i].tab!=PageTabs.None){
                tabToPageNumber[pages[i].tab]=i;
            }
        }
    }
}