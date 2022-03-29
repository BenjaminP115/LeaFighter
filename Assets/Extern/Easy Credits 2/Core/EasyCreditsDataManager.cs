using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Onerat.EasyCredits
{
    public class EasyCreditsDataManager : MonoBehaviour
    {
        public string fileName = "Credits";
        private TextAsset CSV;
        private string[] data;
        private bool startingPassComplete = false;

        public float startDelay = 1;
        public Color backgroundColor = Color.black;
        [Range(0,10)] public int scrollSpeed = 2;

        [Range(-1.0f, 1.0f)] public float TextCentering = 0f;
        [Range(-1.0f, 1.0f)] public float TitleLogoCentering = 0f;
        public bool KeepTitleCentered = true;

        public Sprite GameTitleImage;
        public int gameTitleScale = 0;
        public string GameTitle;
        public TMP_FontAsset gameTitleFont;
        [Range(0.0f, 100.0f)] public float GameTitleWordSpacing = 0;
        [Range(0.0f, 100.0f)] public float GameTitleCharacterSpacing = 0;
        [Range(0.0f, 100.0f)] public int GameTitleLineSpacing = 10;
        [Range(0.0f, 50.0f)] public int GameTitlefontSize = 22;
        public Color GameTitleColor = Color.grey;
        [Space]
        [Range(0.0f, 50.0f)] public int SectionLineSpacing = 20;
        [Space]
        public TMP_FontAsset roleFont;
        [Range(0.0f, 100.0f)] public float RoleWordSpacing = 0;
        [Range(0.0f, 100.0f)] public float RoleCharacterSpacing = 0;
        [Range(0.0f, 50.0f)] public int RoleLineSpacing = 10;
        [Range(0.0f, 50.0f)] public int RolefontSize = 22;
        public Color roleColor = Color.grey;
        [Space]
        public TMP_FontAsset titleFont;
        [Range(0.0f, 100.0f)] public float TitleWordSpacing = 0;
        [Range(0.0f, 100.0f)] public float TitleCharacterSpacing = 0;

        public TMP_FontAsset LegalFont;
        [Range(0.0f, 100.0f)] public float LegalWordSpacing = 0;
        [Range(0.0f, 100.0f)] public float LegalCharacterSpacing = 0;
        public Color LegalColor = Color.white;
        [Range(0.0f, 50.0f)] public int LegalFontSize = 18;

        [Range(0.0f, 50.0f)] public int TitleLineSpacing = 5;
        [Range(0.0f, 50.0f)] public int LegalLineSpacing = 5;
        [Range(0.0f, 50.0f)] public int TitlefontSize = 18;
        public Color titleColor = Color.white;
        [Space]
        [Range(-100.0f, 100.0f)] public int startPointOffset = 0;

        public bool ExitToSceneOnEnd = true;
        public float exitTime = 0;
        public int SceneToExitTo;

        public bool KeepLegalLineCentred;
        public string LegalLine = "Credits powered by Easy Credits";

        public UnityEvent OnCreditsStart;
        public UnityEvent OnCreditsEnd;

        public InlayType inlayType;
        public List<InlayCase> Inlays = new List<InlayCase>();
        public List<StationaryInlayCase> StationaryInlays = new List<StationaryInlayCase>();

        [Range(-100.0f, 100.0f)]
        public int cinematicRoleCentering;
        public CreditType creditType;
        public enum CreditType { Inline, Cinematic }
        public enum InlayType { None, ScrollingInline, ScrollingSideBySideRole, ScrollingManual, Stationary, StationaryAndScrollingInline }

        [Range(0, 1f)]
        public float InlayPostionLeftRight, InlayPostionUpDown;

        public TextAlignmentOptions gameTitleAnchor = TextAlignmentOptions.Center;
        public TextAlignmentOptions roleAnchor = TextAlignmentOptions.Center;
        public TextAlignmentOptions titleAnchor = TextAlignmentOptions.Center;
        public TextAlignmentOptions legalAnchor = TextAlignmentOptions.Center;

        [System.Serializable]
        public class InlayCase
        {
            public EasyCreditsInlay EasyCreditsInlays;
            [Range(-1.0f, 1.0f)]
            public float Centering;
            public int Spacing;
            public int Scale;
        }

        [System.Serializable]
        public class StationaryInlayCase
        {
            public EasyCreditsInlay EasyCreditsInlays;
            public float StartTime;
            public int EndTime;
            public float Scale;
        }

        [HideInInspector]
        public bool scroll = true;

        private void Start()
        {
            this.GetComponent<EasyCreditsCanvasManager>().scroll = scroll;
            this.GetComponent<EasyCreditsCanvasManager>().offset = startPointOffset;
            this.GetComponent<EasyCreditsCanvasManager>().backgroundColor = backgroundColor;
            this.GetComponent<EasyCreditsCanvasManager>().startDelay = startDelay;
            this.GetComponent<EasyCreditsCanvasManager>().scrollSpeed = scrollSpeed;
            this.GetComponent<EasyCreditsCanvasManager>().CreateCanvas();
            this.GetComponent<EasyCreditsCanvasManager>().CreatePannel();

            if (OnCreditsStart == null)
                OnCreditsStart = new UnityEvent();
            if (OnCreditsEnd == null)
                OnCreditsEnd = new UnityEvent();

            CreateData();
        }

        private int inlayCount = 0;
        private int stationaryInlayCount = 0;
        private bool endOfInlays = false;
        private bool endOfStationaryInlays = false;

        public void CreateData()
        {
            OnCreditsStart.Invoke();

            if (GameTitleImage)
            {
                this.GetComponent<EasyCreditsCanvasManager>().PlaceLogo(GameTitleImage, gameTitleScale);
                this.GetComponent<EasyCreditsCanvasManager>().PopulatePannel(GameTitle, GameTitleLineSpacing, gameTitleFont, gameTitleAnchor, GameTitlefontSize, GameTitleColor, false, GameTitleWordSpacing, TitleCharacterSpacing, false, true);
            }
            else
            if (GameTitle != "")
                this.GetComponent<EasyCreditsCanvasManager>().PopulatePannel(GameTitle, GameTitleLineSpacing, gameTitleFont, gameTitleAnchor, GameTitlefontSize, GameTitleColor, false, GameTitleWordSpacing, TitleCharacterSpacing, false, true);

            CSV = Resources.Load<TextAsset>(fileName); // load credits file
            data = CSV.text.Split(new char[] { '\n' }); // create array of rows

            string[] topRowData = data[0].Split(new char[] { ',' }); // create string with role data
                                                                     // loop through roles
            for (int i = 0; i < topRowData.Length; i++)
            {
                // if role isnt null
                if (topRowData[i] != "")
                {
                    if (startingPassComplete == false)
                    {
                        //TriggerInlayPlacement();
                        startingPassComplete = true;
                    }
                    else
                    {
                        this.GetComponent<EasyCreditsCanvasManager>().PopulatePannel("", SectionLineSpacing, roleFont, roleAnchor, RolefontSize, roleColor, false, RoleWordSpacing, RoleCharacterSpacing);
                        TriggerInlayPlacement();
                    }
                    if(creditType == CreditType.Inline){
                        this.GetComponent<EasyCreditsCanvasManager>().PopulatePannel(topRowData[i], RoleLineSpacing, roleFont, roleAnchor, RolefontSize, roleColor, false, RoleWordSpacing, RoleCharacterSpacing);
                    }
                    else if (creditType == CreditType.Cinematic){
                        this.GetComponent<EasyCreditsCanvasManager>().PopulatePannel(topRowData[i], RoleLineSpacing, roleFont, roleAnchor, RolefontSize, roleColor, false, RoleWordSpacing, RoleCharacterSpacing, true);
                    }
                    for (int x = 1; x < data.Length; x++)
                    {
                        string[] RowData = data[x].Split(new char[] { ',' }); // create string with role data
                        if (RowData[i] != "")
                        {
                            this.GetComponent<EasyCreditsCanvasManager>().PopulatePannel(RowData[i], TitleLineSpacing, titleFont, titleAnchor, TitlefontSize, titleColor, false, TitleWordSpacing, TitleCharacterSpacing);
                        }
                    }
                }
            }
            this.GetComponent<EasyCreditsCanvasManager>().PopulatePannel(LegalLine, LegalLineSpacing, LegalFont, legalAnchor, LegalFontSize, LegalColor, true, LegalWordSpacing, LegalCharacterSpacing);
        }
        private bool isVis = false;
        private void Update()
        {
            if (this.GetComponent<EasyCreditsCanvasManager>().LastElement.GetComponent<Renderer>().isVisible)
            {
                isVis = true;
            }
            if (isVis)
            {
                if (this.GetComponent<EasyCreditsCanvasManager>().LastElement.GetComponent<Renderer>().isVisible == false)
                {
                    StartCoroutine(Exit());
                }
            }
            if (!scroll)
            {
                StartCoroutine(Exit());
            }
        }

        IEnumerator Exit()
        {
            yield return new WaitForSeconds(exitTime);
            OnCreditsEnd.Invoke();
            if(ExitToSceneOnEnd) { Application.LoadLevel(SceneToExitTo); }
        }

        void TriggerInlayPlacement()
        {
            if (Inlays.Count < 1)
            {
                inlayType = InlayType.None;
            }
            //if not stationary
            else if (inlayType != InlayType.None && inlayType != InlayType.Stationary && inlayType != InlayType.StationaryAndScrollingInline && endOfInlays == false)
            {
                EasyCreditsInlay inlay = Inlays[inlayCount].EasyCreditsInlays;
                this.GetComponent<EasyCreditsCanvasManager>().AddInlay(inlay, inlayType, inlayCount);

                if (inlayCount < Inlays.Count - 1)
                    inlayCount++;
                else endOfInlays = true;
            }
            //if statationary
            else if (inlayType == InlayType.Stationary && endOfStationaryInlays == false)
            {
                EasyCreditsInlay inlay = StationaryInlays[stationaryInlayCount].EasyCreditsInlays;
                this.GetComponent<EasyCreditsCanvasManager>().AddInlay(inlay, inlayType, stationaryInlayCount);

                if (stationaryInlayCount < StationaryInlays.Count - 1)
                    stationaryInlayCount++;
                else
                    endOfStationaryInlays = true;
            }
            //if startionary and inline
            else if (inlayType == InlayType.StationaryAndScrollingInline)
            {
                if (endOfInlays == false)
                {
                    EasyCreditsInlay inlay = Inlays[inlayCount].EasyCreditsInlays;
                    this.GetComponent<EasyCreditsCanvasManager>().AddInlay(inlay, InlayType.ScrollingInline, inlayCount);
                
                    if (inlayCount < Inlays.Count - 1)
                        inlayCount++;
                    else endOfInlays = true;
                }
                if (endOfStationaryInlays == false)
                {
                    EasyCreditsInlay inlay = StationaryInlays[stationaryInlayCount].EasyCreditsInlays;
                    this.GetComponent<EasyCreditsCanvasManager>().AddInlay(inlay, InlayType.Stationary, stationaryInlayCount);

                    if (stationaryInlayCount < StationaryInlays.Count - 1)
                        stationaryInlayCount++;
                    else
                        endOfStationaryInlays = true;
                }
            }
        }
    }
}


