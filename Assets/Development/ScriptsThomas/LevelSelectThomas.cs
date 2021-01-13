using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectThomas : MonoBehaviour{
    public GameObject levelHolder;
    public GameObject levelIcon;
    public GameObject thisCanvas;
    public PageManagerThomas PageManager;
    public int numberOfLevels = 50;
    public Vector2 iconSpacing;
    private Rect panelDimensions;
    private Rect iconDimensions;
    private int amountPerPage;
    private int currentLevelCount;
    private GameObject text;
    [SerializeField] private Animator musicAnimator;
    [SerializeField] private float waitTime;
    [SerializeField] private AudioManagement audioManagement;

    // Start is called before the first frame update
    void Start()
    {
        musicAnimator = GameObject.Find("MusicAudioObject").GetComponent<Animator>();
        panelDimensions = levelHolder.GetComponent<RectTransform>().rect;
        iconDimensions = levelIcon.GetComponent<RectTransform>().rect;
        int maxInARow = Mathf.FloorToInt((panelDimensions.width + iconSpacing.x) / (iconDimensions.width + iconSpacing.x));
        int maxInACol = Mathf.FloorToInt((panelDimensions.height + iconSpacing.y) / (iconDimensions.height + iconSpacing.y));
        amountPerPage = maxInARow * maxInACol;
        int totalPages = Mathf.CeilToInt((float)numberOfLevels / amountPerPage);
        LoadPanels(totalPages);
        AddButtonListeners();
    }
    void LoadPanels(int numberOfPanels){
        GameObject panelClone = Instantiate(levelHolder) as GameObject;
        //PageSwiper swiper = levelHolder.AddComponent<PageSwiper>();
        //swiper.totalPages = numberOfPanels;

        for(int i = 1; i <= numberOfPanels; i++)
        {
            GameObject panel = Instantiate(panelClone) as GameObject;
            panel.transform.SetParent(thisCanvas.transform, false);
            panel.transform.SetParent(levelHolder.transform);
            panel.name = "Page-" + i;
            panel.SetActive(false);
            //panel.GetComponent<RectTransform>().localPosition = new Vector2(panelDimensions.width * (i - 1), 0);
            panel.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            SetUpGrid(panel);
            PageManager.AddPage(panel);
            int numberOfIcons = i == numberOfPanels ? numberOfLevels - currentLevelCount : amountPerPage;
            LoadIcons(numberOfIcons, panel);
        }

        PageManager.ShowFirst();

        Destroy(panelClone);
    }
    void SetUpGrid(GameObject panel){
        GridLayoutGroup grid = panel.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(iconDimensions.width, iconDimensions.height);
        grid.childAlignment = TextAnchor.UpperCenter;
        grid.spacing = iconSpacing;
    }
    void LoadIcons(int numberOfIcons, GameObject parentObject){
        for(int i = 1; i <= numberOfIcons; i++){
            currentLevelCount++;
            GameObject icon = Instantiate(levelIcon) as GameObject;
            icon.transform.SetParent(thisCanvas.transform, false);
            icon.transform.SetParent(parentObject.transform);
            icon.name = "Level " + i;
            icon.GetComponentInChildren<Text>().text = "" + currentLevelCount;
            levelSelectButtons.Add(icon.GetComponent<Button>());
            //icon.GetComponent<Button>().onClick.AddListener(() => LoadScene("Level" + i));
        }
    }

    public List<Button> levelSelectButtons = new List<Button>();
    private void AddButtonListeners() {
        for ( int i = 0; i < levelSelectButtons.Count; i++ ) {
            Button button = levelSelectButtons[i];
            button.onClick.RemoveAllListeners();
            int j = i + 1;
            button.onClick.AddListener(() => LoadScene(j));
        }
    }

    public void LoadScene(int index)
    {
        StartCoroutine(ChangeScene(index));
        audioManagement.DisableMusicLowPass();
        Time.timeScale = 1;
    }

    private IEnumerator ChangeScene(int index)
    {
        musicAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("Level" + index, LoadSceneMode.Single);
        musicAnimator.SetTrigger("FadeIn");
    }
}
