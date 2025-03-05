using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoduckStudio
{
    public class GenericTooltip : Utils.Singleton<GenericTooltip>
    {
        public class ConfigDescription
        {
            public string name;
            public string description;
            public Color color = Color.white;
            public string amount;

            public ConfigDescription(string name, Color color, string amount = "")
            {
                this.name = name;
                this.color = color;
                this.amount = amount;
            }

            public ConfigDescription(string name, string description, Color color, string amount = "")
            {
                this.name = name;
                this.description = description;
                this.color = color;
                this.amount = amount;
            }
        }

        public class Config
        {
            public string name;
            public Color titleColor = Color.white;
            public List<ConfigDescription> descriptions;
            public Vector2 positionOffset;
            public int upgradeLevel = -1;
            public Color backgroundColor;

            public Config(string name, Color titleColor, Color backgroundColor, Vector2 positionOffset = default(Vector2), List<ConfigDescription> descriptions = null)
            {
                this.name = name;
                this.titleColor = titleColor;
                this.descriptions = descriptions ?? new List<ConfigDescription>();
                this.positionOffset = positionOffset;
                this.backgroundColor = backgroundColor;
            }
        }

        [Header("References")]
        public RectTransform rectTransform;
        public RectTransform canvasRectTransform;
        public CanvasGroup canvasGroup;

        public TMP_Text title;

        public RectTransform descriptionContent;
        public GameObject descriptionPrefab;
        public GameObject DescriptionAndAmountPrefab;
        public Image backgroundImage;

        public Config currentConfig;
        [Header("Runtime")]
        public GameObject target;
        public RectTransform targetRT;

        private void Awake() {
            Hide(true);
        }

        public void Show(Config config = null, GameObject target = null)
        {
            currentConfig = config;
            if (config == null) {
                Debug.LogWarning($"GenericTooltip::Show(): Config was null");
                Hide();
                return;
            }

            this.target = target;
            targetRT = target?.GetComponent<RectTransform>() ?? null;

            backgroundImage.color = config.backgroundColor;

            canvasGroup.DOKill();
            canvasGroup.DOFade(1, 0.2f);

            DeleteAllDescription();

            title.text = config.name;
            title.GetComponent<TMP_Text>().color = config.titleColor;

            foreach (var description in config.descriptions) {
                AddDescription(description);
            }

            Utils.Async.Instance.WaitForEndOfFrame(() => {
                descriptionContent.GetComponent<VerticalLayoutGroup>().enabled = false;
                Utils.Async.Instance.WaitForEndOfFrame(() => {
                    descriptionContent.GetComponent<VerticalLayoutGroup>().enabled = true;
                });
            });
        }

        private string IntToRomain(int number)
        {
            if (number == 1) {
                return "I";
            }
            else if (number == 2) {
                return "II";
            }
            else if (number == 3) {
                return "III";
            }

            return "";
        }

        public void Hide(bool instant = false)
        {
            currentConfig = null;

            canvasGroup.DOKill();
            
            if (instant) {
                canvasGroup.alpha = 0;
                return;
            }

            canvasGroup.DOFade(0, 0.1f).OnComplete(() => {
                DeleteAllDescription();
            });
        }

        protected void AddDescription(ConfigDescription configDescription)
        {
            if (String.IsNullOrEmpty(configDescription.amount)) {
                GameObject descriptionObj = Instantiate(descriptionPrefab, descriptionContent);
                descriptionObj.name = "Description_";
                descriptionObj.GetComponent<TMP_Text>().color = configDescription.color;
                descriptionObj.GetComponent<TMP_Text>().text = configDescription.name;
            }
            else {
                GameObject descriptionAndAmountObj = Instantiate(DescriptionAndAmountPrefab, descriptionContent);
                descriptionAndAmountObj.name = "descriptionAndAmount_";
                foreach (Transform child in descriptionAndAmountObj.transform) {
                    if (child.name == "Text") {
                        child.GetComponent<TMP_Text>().text = configDescription.name;
                    }
                    else if (child.name == "Number") {
                        child.GetComponent<TMP_Text>().text = configDescription.amount;
                        child.GetComponent<TMP_Text>().color = configDescription.color;
                    }
                }
            }
        }

        protected void DeleteAllDescription()
        {
            foreach (Transform child in descriptionContent) {
                if (child.name != title.name) {
                    Destroy(child.gameObject);
                }
            }
        }

        private void Update()
        {
            if (currentConfig == null) {
                return;
            }

            Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
            if (targetRT != null) {
                anchoredPosition.x = (targetRT.position.x + 137.5f) / transform.parent.localScale.x;

                anchoredPosition.y = (targetRT.position.y + (targetRT.pivot.y * targetRT.sizeDelta.y)) / transform.parent.localScale.y - (GetTooltipSize().y / 2);
                if (anchoredPosition.x + GetTooltipSize().x > Screen.currentResolution.width / transform.parent.localScale.x) {
                    anchoredPosition.x = (targetRT.position.x - 137.5f) / transform.parent.localScale.x - GetTooltipSize().x;
                }
            }
            else if (target != null) {
                Vector2 posInScreen = Camera.main.WorldToScreenPoint(target.transform.position);

                anchoredPosition.x = (posInScreen.x + 137.5f) / transform.parent.localScale.x;
                
                anchoredPosition.y = posInScreen.y / transform.parent.localScale.y - (GetTooltipSize().y / 2);
                if (anchoredPosition.x + GetTooltipSize().x > Screen.currentResolution.width / transform.parent.localScale.x) {
                    anchoredPosition.x = (posInScreen.x - 137.5f) / transform.parent.localScale.x - GetTooltipSize().x;
                }
            }

            rectTransform.anchoredPosition = anchoredPosition;
        }

        private Vector2 GetTooltipSize()
        {
            Vector2 size = Vector2.zero;
            size.x = descriptionContent.rect.width;
            size.y = descriptionContent.rect.height;
            return size;
        }
    }
}
