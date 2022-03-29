using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Onerat.EasyCredits
{
    public class EasyCreditsCanvasManager : MonoBehaviour
    {
        private Canvas canvas;
        private int DistanceToMove = 0;
        private int DistanceToMoveInlayManual;

        private GameObject panel;

        [HideInInspector]
        public float startDelay = 1;
        [HideInInspector]
        public Color backgroundColor = Color.black;
        [HideInInspector]
        public int scrollSpeed = 2;

        [HideInInspector]
        public bool scroll = true;
        [HideInInspector]
        public int offset = 0;
        [HideInInspector]
        public GameObject LastElement;
        private EasyCreditsDataManager dataManager;

        private void Start()
        {
            this.transform.position = new Vector3(this.transform.position.x + (this.gameObject.GetComponent<EasyCreditsDataManager>().TextCentering * 10), this.transform.position.y, this.transform.position.z);
            dataManager = this.GetComponent<EasyCreditsDataManager>();
        }

        private void Update()
        {
            startDelay -= Time.deltaTime;
            if (startDelay < 0)
            {
                if (scroll)
                    panel.transform.Translate(transform.up * Time.deltaTime * scrollSpeed);
            }
        }

        public void PlaceLogo(Sprite image, int scale)
        {
            GameObject TitleHolder = new GameObject("Title");
            TitleHolder.transform.parent = panel.transform;
            TitleHolder.AddComponent<SpriteRenderer>();
            TitleHolder.GetComponent<SpriteRenderer>().sprite = image;
            if(this.GetComponent<EasyCreditsDataManager>().KeepTitleCentered == false)
            {
                TitleHolder.transform.position += new Vector3(
                TitleHolder.transform.position.x
                + this.gameObject.GetComponent<EasyCreditsDataManager>().TitleLogoCentering * 100,
                TitleHolder.transform.position.y + offset,
                0);
            }
            else
            {
                TitleHolder.transform.position += new Vector3(
                TitleHolder.transform.position.x,
                TitleHolder.transform.position.y + offset,
                0);
            }

            TitleHolder.transform.localScale += new Vector3(scale, scale, scale);

        }

        public void CreateCanvas()
        {
            canvas = this.gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            this.gameObject.AddComponent<CanvasScaler>();
            this.gameObject.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

            Camera cam = Camera.main;
            canvas.worldCamera = cam;
            cam.orthographic = true;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = backgroundColor;
        }

        public void CreatePannel()
        {
            panel = new GameObject();
            panel.name = "Panel";
            panel.transform.parent = this.transform;
            panel.transform.position = new Vector3(this.gameObject.GetComponent<EasyCreditsDataManager>().TextCentering * 100, offset, 0);
        }

        public void AddInlay(EasyCreditsInlay inlay, EasyCreditsDataManager.InlayType inlayType, int count)
        {
            //account for manual controls offset
            if (DistanceToMoveInlayManual == 0) DistanceToMoveInlayManual = DistanceToMove;

            //set ref to easy credits data manager
            if (!dataManager)
                dataManager = this.GetComponent<EasyCreditsDataManager>();

            //create panel centering overwrite offset
            Vector3 panelPosWithOffset = panel.transform.position - new Vector3(this.gameObject.GetComponent<EasyCreditsDataManager>().TextCentering * 100, 0, 0);
            
            //scale
            float scale = 0;
            if (inlayType != EasyCreditsDataManager.InlayType.Stationary)
            scale = dataManager.Inlays[count].Scale;
            else{
                scale = dataManager.StationaryInlays[count].Scale;
            }

            if (inlay.Sprite)
            {
                GameObject Element = new GameObject();

                if(inlayType != EasyCreditsDataManager.InlayType.Stationary)
                    Element.transform.parent = panel.transform;
                //else
                //    Element.transform.parent = this.transform;

                Element.AddComponent<SpriteRenderer>(); Element.GetComponent<SpriteRenderer>().sprite = inlay.Sprite;

                Element.transform.localScale = new Vector3(scale, scale, scale);

                if (inlayType == EasyCreditsDataManager.InlayType.ScrollingInline || inlayType == EasyCreditsDataManager.InlayType.ScrollingSideBySideRole){
                    Element.transform.position = panelPosWithOffset
                        + new Vector3(dataManager.Inlays[count].Centering * 100, DistanceToMove, 0)
                        - new Vector3(0, Element.transform.localScale.y, 0);
                }
                else if(inlayType == EasyCreditsDataManager.InlayType.ScrollingManual){
                    Element.transform.position = panelPosWithOffset
                        + new Vector3(dataManager.Inlays[count].Centering * 100, DistanceToMoveInlayManual, 0)
                        - new Vector3(0, Element.transform.localScale.y, 0);
                }
                else if(inlayType == EasyCreditsDataManager.InlayType.Stationary)
                {
                    Element.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(
                        dataManager.InlayPostionLeftRight,
                        dataManager.InlayPostionUpDown,
                        Camera.main.nearClipPlane + 10
                        ));

                    //Element.transform.position = Camera.main.ScreenToWorldPoint(
                    //    new Vector3((Screen.width / 2) + dataManager.InlayPostionLeftRight * 50,
                    //    (Screen.height / 2) + dataManager.InlayPostionUpDown * 25,
                    //    Camera.main.nearClipPlane + 10)
                    //    );

                    //add stationary component
                    GameObject stationaryTimer = new GameObject("Stationary Timer");
                    stationaryTimer.transform.parent = this.transform;
                    stationaryTimer.AddComponent<EasyCreditsStationaryTimer>();
                    stationaryTimer.GetComponent<EasyCreditsStationaryTimer>().destoryTime = (dataManager.StationaryInlays[count].EndTime);
                    stationaryTimer.GetComponent<EasyCreditsStationaryTimer>().showTime = (dataManager.StationaryInlays[count].StartTime);
                    stationaryTimer.GetComponent<EasyCreditsStationaryTimer>().element = Element;
                    Element.SetActive(false);
                    StartCoroutine(stationaryTimer.GetComponent<EasyCreditsStationaryTimer>().DestroyTimer());
                }
            }
            else if (inlay.AnimatedObject)
            {
                GameObject Element;
                if (inlayType == EasyCreditsDataManager.InlayType.ScrollingInline || inlayType == EasyCreditsDataManager.InlayType.ScrollingSideBySideRole)
                {
                    Element = Instantiate(inlay.AnimatedObject, panelPosWithOffset 
                        + new Vector3(dataManager.Inlays[count].Centering * 100, DistanceToMove, 0), Quaternion.identity, panel.transform);
                    Element.transform.localScale += new Vector3(scale, scale, scale);
                }
                else if (inlayType == EasyCreditsDataManager.InlayType.ScrollingManual)
                {
                    Element = Instantiate(inlay.AnimatedObject, panelPosWithOffset + new Vector3(dataManager.Inlays[count].Centering * 100, DistanceToMoveInlayManual, 0), Quaternion.identity, panel.transform);
                    Element.transform.localScale += new Vector3(scale, scale, scale);
                }
            }
            else if (inlay.AudioClip)
            {
                if (!this.GetComponent<AudioSource>())
                    this.gameObject.AddComponent<AudioSource>();
                this.GetComponent<AudioSource>().clip = inlay.AudioClip;
                this.GetComponent<AudioSource>().Play();
            }

            if (inlayType == EasyCreditsDataManager.InlayType.ScrollingInline)
                DistanceToMove = DistanceToMove - dataManager.Inlays[count].Spacing;
            else if(inlayType == EasyCreditsDataManager.InlayType.ScrollingManual)
                DistanceToMoveInlayManual = DistanceToMoveInlayManual - dataManager.Inlays[count].Spacing;
        }

        public void PopulatePannel(string wordToWrite, int distanceToMove, TMP_FontAsset font, TextAlignmentOptions textAnchor, int fontSize, Color color, bool last, float wordSpacing, float characterSpacing, bool cinematic = false, bool first = false)
        {
            GameObject Element = new GameObject();
            Element.name = "Text Element";
            Element.transform.parent = panel.transform;
            if(cinematic == false)
            Element.transform.position = panel.transform.position + new Vector3(0, DistanceToMove, 0);
            else{
                if (!dataManager)
                    dataManager = this.GetComponent<EasyCreditsDataManager>();
                Element.transform.position = panel.transform.position + new Vector3(dataManager.cinematicRoleCentering, DistanceToMove, 0);
            }

            TextMeshProUGUI elementText = Element.AddComponent<TextMeshProUGUI>();
            elementText.text = wordToWrite;
            elementText.font = font;
            elementText.color = color;
            elementText.alignment = textAnchor;
            elementText.fontSize = fontSize;
            elementText.overflowMode = TextOverflowModes.Overflow;
            elementText.horizontalMapping = TextureMappingOptions.Line;
            elementText.wordSpacing = wordSpacing;
            elementText.characterSpacing = characterSpacing;
            elementText.enableWordWrapping = false;

            Element.GetComponent<RectTransform>().localScale = new Vector3(.2f, .2f, .2f);

            if(cinematic == false)
            DistanceToMove = DistanceToMove - distanceToMove;

            if (first)
            {
                Element.transform.position -= new Vector3(
                    Element.transform.position.x  
                    + -this.gameObject.GetComponent<EasyCreditsDataManager>().TitleLogoCentering * 100,
                    Element.transform.position.y + offset,
                    0);
                if (this.GetComponent<EasyCreditsDataManager>().KeepTitleCentered)
                {
                    Element.transform.position = panel.transform.position + new Vector3(-this.gameObject.GetComponent<EasyCreditsDataManager>().TextCentering * 100, 0, 0);
                }
            }

            if (last)
            {
                LastElement = Element;
                Element.AddComponent<SpriteRenderer>();

                if (!dataManager)
                    dataManager = this.GetComponent<EasyCreditsDataManager>();

                if (dataManager.KeepLegalLineCentred)
                    Element.transform.position = panel.transform.position + new Vector3(-this.gameObject.GetComponent<EasyCreditsDataManager>().TextCentering * 100, DistanceToMove, 0);
            }
        }
    }

}
