using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class BookController : WindowController
{
    public static BookController instance;
    public static bool isOpen {get{return instance.gameObject.activeSelf;}}
    public AudioSource pageSource;
    public AudioClip pageTurn;

    public int pageNumber{
        get{
            return _pageNumber;
        }
        set{
            if (_pageNumber!=value){
                pageHistory.Add(_pageNumber);
                if (pageHistory.Count>20)
                    pageHistory.RemoveAt(0);
            }
            _pageNumber=value;
            LoadPage(_pageNumber);
        }
    }
    int _pageNumber;
    public BookInfo bookInfo;
    public List<int> pageHistory = new List<int>();

    public MonoBehaviour activePageController{
        set{
            if(_activePageController!=null)
                _activePageController.gameObject.SetActive(false);
            _activePageController=value;
            _activePageController.gameObject.SetActive(true);
        }
    }
    MonoBehaviour _activePageController;
    public TextPageBookController textPageBookController;
    public QuickRefBookController quickRefBookController;
    public IndexPageBookController indexPageBookController;
    public SettingsPageController settingsPageController;
    public GameObject backButton;
    public static void OpenBook(){
        instance.Open();
    }
    public static void CloseBook(bool silent = false){
        instance.Close(silent);
    }
    public static void ToggleBook(){
        instance.Toggle();
    }
    public override void Activate(){
        
        instance=this;
        pageNumber=0;
        CloseBook(true);
    }
    
    void LoadPage(int number){
        bookInfo.pages[number].Activate(this);
        if (pageSource.isActiveAndEnabled){
            pageSource.clip=pageTurn;
            pageSource.Play();
        }
    }
    public void CloseButton(){
        if (!MyInput.click)
            return;
        CloseBook();
    }
    public void PageLeft(){
        if (!MyInput.click)
            return;
        if (pageNumber>0)
            pageNumber--;
    }
    public void PageRight(){
        if (!MyInput.click)
            return;
        if (pageNumber<bookInfo.pages.Count-1){
            pageNumber++;
        }
    }
    public void GoToPage(int page){
        // if (!MyInput.click)
        //     return;
        pageNumber=page;
    }

    public void GoToPageByName(BookInfo.PageTabs page){
        GoToPage(bookInfo.PageOf(page));
    }
    public void GoToPageByName(string page){
        GoToPage(bookInfo.PageOf(System.Enum.Parse<BookInfo.PageTabs>(page)));
    }
    public void GoToIndexPage(int position){
        GlyphType type = position switch{
            0=>quickRefBookController.type1,
            1=>quickRefBookController.type2,
            2=>quickRefBookController.type3,
            3=>quickRefBookController.type4,
            _=>GlyphType.Gag,
        };
        for (int i = bookInfo.PageOf(BookInfo.PageTabs.Index); i < bookInfo.pages.Count;i++){
            IndexPageBookPage page = bookInfo.pages[i] as IndexPageBookPage;
            if (page!=null && page.type==type){
                GoToPage(i);
                break;
            }
        }
    }
    public void GoBack(){
        if (pageHistory.Count==0)
            return;

        int toGoTo = pageHistory[pageHistory.Count-1];
        pageHistory.RemoveAt(pageHistory.Count-1);
        pageNumber=toGoTo;
        pageHistory.RemoveAt(pageHistory.Count-1);//Remove new added page
    }

    public override void Update(){
        base.Update();
        backButton.SetActive(pageHistory.Count>0);
    }
    public static void OpenOptions(){
        OpenBook();
        instance.GoToPageByName(BookInfo.PageTabs.Options);
    }
}
