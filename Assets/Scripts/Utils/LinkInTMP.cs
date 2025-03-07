using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CoduckStudio
{
    [RequireComponent(typeof(TMP_Text))]
    public class LinkInTMP : MonoBehaviour, IPointerClickHandler
    {
        private TMP_Text tmpTextBox;
        private RectTransform textBoxRectTransform;
        [HideInInspector] public string originalText;
        private string startTag = "<u>";
        private string endTag = "</u>";
        private bool highlighted = false;

        private void Awake()
        {
            tmpTextBox = GetComponent<TMP_Text>();
            textBoxRectTransform = GetComponent<RectTransform>();
        }

        public void ResetOriginalText(string text)
        {
            Debug.Log($"LinkInTMP::ResetOriginalText(): originalText '{originalText}' has been set to '{text}'");
            originalText = text;
            UnhighlightSelection();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, 0);

            var linkTaggedText = TMP_TextUtilities.FindIntersectingLink(tmpTextBox, mousePosition, null);
            
            if (linkTaggedText != -1)
            {
                TMP_LinkInfo linkInfo = tmpTextBox.textInfo.linkInfo[linkTaggedText];
                Debug.Log($"LinkInTMP::OnPointerClick(): Opening url linkInfo={linkInfo.GetLinkText()} linkID={linkInfo.GetLinkID()}");
                Application.OpenURL(linkInfo.GetLinkID());
            }
        }

        private void Update()
        {
            if (!String.IsNullOrEmpty(originalText)) {
                CheckForLinkAtMousePosition();
            }
        }

        private void CheckForLinkAtMousePosition()
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            
            bool isIntersectingRectTransform = TMP_TextUtilities.IsIntersectingRectTransform(textBoxRectTransform, mousePosition, null);
            
            if (!isIntersectingRectTransform) {
                if (highlighted) {
                    tmpTextBox.SetText(UnhighlightSelection());
                }
                return;
            }

            int intersectingLink = TMP_TextUtilities.FindIntersectingLink(tmpTextBox, mousePosition, null);
            
            if (intersectingLink == -1) {
                if (highlighted) {
                    tmpTextBox.SetText(UnhighlightSelection());
                }
                return;
            }

            TMP_LinkInfo linkInfo = tmpTextBox.textInfo.linkInfo[intersectingLink];
            tmpTextBox.SetText(HighlightSelection(linkInfo));
        }

        public string HighlightSelection(TMP_LinkInfo linkInfo)
        {
            if (highlighted) {
                return tmpTextBox.text;
            }

            Debug.Log($"LinkInTMP::MarkSelection(): Highlighting linkInfo={linkInfo.GetLinkText()} linkID={linkInfo.GetLinkID()}");

            string linkId = linkInfo.GetLinkID();
            
            int startIndex = tmpTextBox.text.IndexOf($"<link=\"{linkId}\">");
            int extraTagCharacters = $"<link=\"{linkId}\">".Length;

            int linkTextFirstCharacterIndex = startIndex + extraTagCharacters;
            int linkTextLastCharacterIndex = linkTextFirstCharacterIndex + linkInfo.linkTextLength;
            
            var markedText = new StringBuilder(tmpTextBox.text);

            markedText.Insert(linkTextFirstCharacterIndex, startTag);
            markedText.Insert(linkTextLastCharacterIndex + startTag.Length, endTag);

            highlighted = true;

            return markedText.ToString();
        }

        public string UnhighlightSelection()
        {
            highlighted = false;
            return originalText;
        }
    }
}
